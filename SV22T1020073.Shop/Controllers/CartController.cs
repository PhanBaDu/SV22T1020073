using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1020073.Models.Sales;
using SV22T1020073.Models.Common;
using SV22T1020073.Models.Catalog;
using SV22T1020073.Shop.Models;
using System.Security.Claims;
using SV22T1020073.Shop.AppCodes;
using SV22T1020073.Shop.Services;

namespace SV22T1020073.Shop.Controllers
{
    /// <summary>
    /// Điều khiển xử lý các yêu cầu liên quan đến giỏ hàng và thanh toán
    /// </summary>
    public class CartController : Controller
    {
        private List<CartItem> GetCart() => ShoppingCartService.GetShoppingCart();
        private void SaveCart(List<CartItem> cart) => ShoppingCartService.SaveShoppingCart(cart);
        private int GetCartItemCount() => ShoppingCartService.GetCartCount();

        /// <summary>
        /// Chỉ cho phép đường dẫn nội bộ tương đối (tránh open redirect).
        /// </summary>
        private static string SafeReturnUrl(string? candidate, string fallback = "/Cart")
        {
            if (string.IsNullOrWhiteSpace(candidate)) return fallback;
            var s = candidate.Trim();
            if (s.StartsWith('/') && !s.StartsWith("//", StringComparison.Ordinal)) return s;
            return fallback;
        }

        private List<CartItem> GetSelectedItems()
        {
            var cart = GetCart();
            if (HttpContext.Request.Cookies.TryGetValue("selectedCartItems", out string? cookieVal) && !string.IsNullOrWhiteSpace(cookieVal))
            {
                var ids = new HashSet<int>();
                foreach (var idStr in cookieVal.Split(','))
                {
                    if (int.TryParse(idStr.Trim(), out int parsed)) ids.Add(parsed);
                }
                if (ids.Count > 0)
                {
                    return cart.Where(i => ids.Contains(i.ProductID)).ToList();
                }
            }
            return cart;
        }

        /// <summary>
        /// Hiển thị trang giỏ hàng
        /// </summary>
        /// <returns>View chứa danh sách sản phẩm trong giỏ</returns>
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng (Ajax)
        /// TC-B3: Chưa đăng nhập -> trả về JSON redirect về Login.
        /// TC-B1 (phần Add): Kiểm tra IsSelling, số lượng >= 1.
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <param name="quantity">Số lượng muốn thêm</param>
        /// <param name="returnUrl">Url quay lại sau khi đăng nhập</param>
        /// <returns>JSON kết quả thao tác</returns>
        [HttpPost]
        public async Task<IActionResult> Add(int id, int quantity = 1, string? returnUrl = null)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                var ret = SafeReturnUrl(returnUrl, "/Cart");
                return Json(new
                {
                    success = false,
                    requireLogin = true,
                    redirectUrl = Url.Action("Login", "Account", new { returnUrl = ret }),
                    message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng."
                });
            }

            if (quantity < 1) quantity = 1;

            var cart = GetCart();
            var existing = cart.FirstOrDefault(m => m.ProductID == id);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                var product = await ShopCatalogService.GetProductAsync(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }
                if (product.IsSelling != true)
                {
                    return Json(new { success = false, message = "Sản phẩm hiện không còn được bán." });
                }

                cart.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Photo = product.Photo ?? "",
                    Price = product.Price,
                    Unit = product.Unit,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
            var itemCount = GetCartItemCount();
            return Json(new { success = true, itemCount, message = $"Đã thêm \"{cart.Last().ProductName}\" vào giỏ hàng." });
        }

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong giỏ (Ajax)
        /// TC-B3: Chưa đăng nhập -> trả về JSON redirect về Login.
        /// TC-B1 (phần Update): Số lượng nguyên dương.
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <param name="quantity">Số lượng mới</param>
        /// <returns>JSON kết quả cập nhật</returns>
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Json(new
                {
                    success = false,
                    requireLogin = true,
                    redirectUrl = Url.Action("Login", "Account", new { returnUrl = "/Cart" }),
                    message = "Vui lòng đăng nhập."
                });
            }

            if (quantity < 1) quantity = 1;

            var cart = GetCart();
            var item = cart.FirstOrDefault(m => m.ProductID == id);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
                return Json(new
                {
                    success = true,
                    subtotal = item.TotalPrice.ToString("N0"),
                    total = cart.Sum(c => c.TotalPrice).ToString("N0")
                });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// Xóa sản phẩm khỏi giỏ hàng (Ajax)
        /// TC-B3: Chưa đăng nhập -> trả về JSON redirect về Login.
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        /// <returns>JSON kết quả xóa</returns>
        [HttpPost]
        public IActionResult Remove(int id)
        {
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Json(new
                {
                    success = false,
                    requireLogin = true,
                    redirectUrl = Url.Action("Login", "Account", new { returnUrl = "/Cart" }),
                    message = "Vui lòng đăng nhập."
                });
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(m => m.ProductID == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                return Json(new
                {
                    success = true,
                    total = cart.Sum(c => c.TotalPrice).ToString("N0"),
                    itemCount = cart.Count
                });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// Xóa toàn bộ giỏ hàng
        /// </summary>
        /// <returns>Redirect về trang giỏ hàng</returns>
        [HttpPost]
        public IActionResult Clear()
        {
            ShoppingCartService.ClearCart();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Hiển thị trang thanh toán
        /// TC-B3: Checkout yêu cầu đăng nhập (dùng [Authorize] attribute).
        /// TC-B2: Giỏ trống -> redirect về Index.
        /// </summary>
        /// <returns>View thanh toán với thông tin khách hàng và đơn vị vận chuyển</returns>
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetSelectedItems();
            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.";
                return RedirectToAction("Index");
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return RedirectToAction("Login", "Account");

            var customer = await ShopPartnerService.GetCustomerAsync(userId);
            ViewBag.Cart = cart;
            ViewBag.Shippers = await ShopPartnerService.ListShippersAsync(new PaginationSearchInput { PageSize = 100 });

            return View(customer);
        }

        /// <summary>
        /// Xác nhận và tạo đơn hàng
        /// TC-B3: Checkout khi chưa đăng nhập -> redirect Login -> quay lại.
        /// TC-B1: Lưu Orders + OrderDetails thành công.
        /// TC-B3: Phải chọn Shipper + địa chỉ nhận.
        /// </summary>
        /// <param name="recipientName">Tên người nhận</param>
        /// <param name="recipientPhone">Số điện thoại người nhận</param>
        /// <param name="deliveryAddress">Địa chỉ giao hàng</param>
        /// <param name="deliveryProvince">Tỉnh/Thành phố giao hàng</param>
        /// <param name="shipperID">Mã đơn vị vận chuyển</param>
        /// <param name="note">Ghi chú đơn hàng</param>
        /// <returns>Redirect về trang trạng thái đơn hàng</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Confirm(
            string recipientName,
            string recipientPhone,
            string deliveryAddress,
            string deliveryProvince,
            int? shipperID,
            string note = "")
        {
            var cart = GetSelectedItems();
            if (cart.Count == 0) return RedirectToAction("Index");

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(recipientName))
                ModelState.AddModelError("recipientName", "Vui lòng nhập tên người nhận.");
            if (string.IsNullOrWhiteSpace(recipientPhone))
                ModelState.AddModelError("recipientPhone", "Vui lòng nhập số điện thoại người nhận.");
            if (string.IsNullOrWhiteSpace(deliveryAddress))
                ModelState.AddModelError("deliveryAddress", "Vui lòng nhập địa chỉ giao hàng.");
            if (string.IsNullOrWhiteSpace(deliveryProvince))
                ModelState.AddModelError("deliveryProvince", "Vui lòng chọn tỉnh/thành phố giao hàng.");
            if (!shipperID.HasValue || shipperID.Value <= 0)
                ModelState.AddModelError("shipperID", "Vui lòng chọn đơn vị vận chuyển.");

            if (!ModelState.IsValid)
            {
                var customer = await ShopPartnerService.GetCustomerAsync(userId);
                ViewBag.Cart = cart;
                ViewBag.Shippers = await ShopPartnerService.ListShippersAsync(new PaginationSearchInput { PageSize = 100 });
                ViewData["ValidationErrors"] = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );
                return View("Checkout", customer);
            }

            var fullDeliveryAddress = $"{recipientName} — {recipientPhone} — {deliveryAddress}";

            var order = new Order
            {
                CustomerID = userId,
                DeliveryAddress = fullDeliveryAddress,
                DeliveryProvince = deliveryProvince,
                ShipperID = shipperID,
                OrderTime = DateTime.Now,
                Status = OrderStatusEnum.New
            };

            int orderID = await ShopSalesService.AddOrderAsync(order);

            foreach (var item in cart)
            {
                await ShopSalesService.AddDetailAsync(new OrderDetail
                {
                    OrderID = orderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    SalePrice = item.Price
                });
            }

            var fullCart = GetCart();
            foreach (var item in cart)
            {
                var c = fullCart.FirstOrDefault(x => x.ProductID == item.ProductID);
                if (c != null) fullCart.Remove(c);
            }
            SaveCart(fullCart);
            Response.Cookies.Delete("selectedCartItems");

            TempData["SuccessMessage"] = $"Đặt hàng thành công! Mã đơn hàng #{orderID}. Đơn hàng đang được chờ duyệt.";
            return RedirectToAction("Status", "Order", new { id = orderID });
        }

        /// <summary>
        /// Trả về số lượng sản phẩm trong giỏ (Ajax) cho client cập nhật badge
        /// </summary>
        /// <returns>JSON chứa số lượng sản phẩm</returns>
        public IActionResult GetCartCount()
        {
            if (User?.Identity?.IsAuthenticated != true)
                return Json(new { count = 0 });
            return Json(new { count = GetCartItemCount() });
        }
    }
}
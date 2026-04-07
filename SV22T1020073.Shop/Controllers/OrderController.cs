using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV22T1020073.Models.Sales;
using System.Security.Claims;
using SV22T1020073.Shop.Models;
using SV22T1020073.Models.Catalog;
using SV22T1020073.Shop.AppCodes;
using SV22T1020073.Shop.Services;

namespace SV22T1020073.Shop.Controllers
{
    /// <summary>
    /// Điều khiển xử lý các yêu cầu liên quan đến đơn hàng
    /// </summary>
    [Authorize]
    public class OrderController : Controller
    {
        private const int PAGE_SIZE = 10;

        private static List<CartItem> GetCart() => ShoppingCartService.GetShoppingCart();
        private static void SaveCart(List<CartItem> cart) => ShoppingCartService.SaveShoppingCart(cart);

        /// <summary>
        /// Hiển thị trang lịch sử đơn hàng của khách hàng
        /// </summary>
        /// <param name="status">Trạng thái đơn hàng cần lọc</param>
        /// <param name="page">Số trang hiện tại</param>
        /// <param name="searchValue">Từ khóa tìm kiếm</param>
        /// <returns>View lịch sử đơn hàng với danh sách và phân trang</returns>
        public async Task<IActionResult> History(int status = 0, int page = 1, string searchValue = "")
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");

            var input = new OrderSearchInput
            {
                Page = page,
                PageSize = PAGE_SIZE,
                Status = (OrderStatusEnum)status,
                SearchValue = searchValue,
                CustomerID = userId
            };

            var data = await ShopSalesService.ListOrdersAsync(input);
            ViewBag.Status = status;
            ViewBag.SearchValue = searchValue;

            var detailsDict = new Dictionary<int, List<OrderDetailViewInfo>>();
            foreach (var order in data.DataItems)
            {
                var details = await ShopSalesService.ListDetailsAsync(order.OrderID);
                detailsDict[order.OrderID] = details;
            }
            ViewBag.OrderDetails = detailsDict;

            return View(data);
        }

        /// <summary>
        /// Hiển thị trang trạng thái chi tiết của một đơn hàng
        /// </summary>
        /// <param name="id">Mã đơn h��ng</param>
        /// <returns>View chi tiết đơn hàng hoặc redirect về History nếu không có quyền</returns>
        public async Task<IActionResult> Status(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");

            var order = await ShopSalesService.GetOrderAsync(id);
            if (order == null || order.CustomerID != userId)
            {
                return RedirectToAction("History");
            }

            ViewBag.Details = await ShopSalesService.ListDetailsAsync(id);
            return View(order);
        }

        /// <summary>
        /// Hủy đơn hàng của khách hàng (chỉ áp dụng cho đơn hàng mới hoặc đã chấp nhận)
        /// </summary>
        /// <param name="id">Mã đơn hàng cần hủy</param>
        /// <returns>Redirect về trang lịch sử với thông báo kết quả</returns>
        public async Task<IActionResult> Cancel(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");

            var order = await ShopSalesService.GetOrderAsync(id);
            if (order == null || order.CustomerID != userId)
            {
                return RedirectToAction("History");
            }

            if (order.Status != OrderStatusEnum.New && order.Status != OrderStatusEnum.Accepted)
            {
                TempData["ErrorMessage"] = "Đơn hàng đã được giao cho đơn vị vận chuyển, không thể hủy.";
                return RedirectToAction("Status", new { id });
            }

            bool ok = await ShopSalesService.CancelOrderAsync(id);
            if (ok)
            {
                TempData["SuccessMessage"] = "Đơn hàng đã được hủy thành công.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể hủy đơn hàng. Vui lòng thử lại.";
            }

            return RedirectToAction("History");
        }

        /// <summary>
        /// Đặt lại sản phẩm từ đơn hàng cũ vào giỏ hàng hiện tại
        /// </summary>
        /// <param name="id">Mã đơn hàng cần đặt lại</param>
        /// <returns>Redirect về trang giỏ hàng với thông báo kết quả</returns>
        public async Task<IActionResult> Reorder(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");

            var order = await ShopSalesService.GetOrderAsync(id);
            if (order == null || order.CustomerID != userId)
            {
                return RedirectToAction("History");
            }

            var details = await ShopSalesService.ListDetailsAsync(id);
            if (details.Count == 0)
            {
                TempData["ErrorMessage"] = "Đơn hàng này không có sản phẩm nào.";
                return RedirectToAction("History");
            }

            var cart = GetCart();
            int addedCount = 0;

            foreach (var d in details)
            {
                var product = await ShopCatalogService.GetProductAsync(d.ProductID);
                if (product != null && product.IsSelling == true)
                {
                    var existing = cart.FirstOrDefault(c => c.ProductID == d.ProductID);
                    if (existing != null)
                        existing.Quantity += d.Quantity;
                    else
                        cart.Add(new CartItem
                        {
                            ProductID = product.ProductID,
                            ProductName = product.ProductName,
                            Photo = product.Photo ?? "",
                            Price = product.Price,
                            Unit = product.Unit,
                            Quantity = d.Quantity
                        });
                    addedCount++;
                }
            }

            SaveCart(cart);

            if (addedCount > 0)
            {
                TempData["SuccessMessage"] = $"Đã thêm {addedCount} sản phẩm vào giỏ hàng từ đơn #{id}.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không có sản phẩm nào có thể thêm vào giỏ (sản phẩm có thể đã ngừng bán).";
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}
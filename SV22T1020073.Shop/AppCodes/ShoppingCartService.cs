using SV22T1020073.Shop.Models;

namespace SV22T1020073.Shop.AppCodes
{
    /// <summary>
    /// Cung cấp các chức năng xử lý trên giỏ hàng (Giỏ hàng lưu trong session)
    /// </summary>
    public static class ShoppingCartService
    {
        private const string CART_SESSION_KEY = "UserCart";

        /// <summary>
        /// Lấy giỏ hàng từ session
        /// </summary>
        public static List<CartItem> GetShoppingCart()
        {
            var cart = ApplicationContext.GetSessionData<List<CartItem>>(CART_SESSION_KEY);
            if (cart == null)
            {
                cart = new List<CartItem>();
                ApplicationContext.SetSessionData(CART_SESSION_KEY, cart);
            }
            return cart;
        }

        /// <summary>
        /// Lưu giỏ hàng vào session
        /// </summary>
        public static void SaveShoppingCart(List<CartItem> cart)
        {
            ApplicationContext.SetSessionData(CART_SESSION_KEY, cart);
        }

        /// <summary>
        /// Lấy số lượng sản phẩm trong giỏ
        /// </summary>
        public static int GetCartCount()
        {
            return GetShoppingCart().Count;
        }

        /// <summary>
        /// Xóa giỏ hàng
        /// </summary>
        public static void ClearCart()
        {
            ApplicationContext.SetSessionData(CART_SESSION_KEY, new List<CartItem>());
        }
    }
}

using SV22T1020073.Models.Common;
using SV22T1020073.Models.Sales;

namespace SV22T1020073.Shop.Services
{
    /// <summary>
    /// Lớp xử lý các nghiệp vụ liên quan đến bán hàng trong Shop
    /// </summary>
    public static class ShopSalesService
    {
        /// <summary>
        /// Tạo mới một đơn hàng
        /// </summary>
        /// <param name="data">Thông tin đơn hàng</param>
        /// <returns>Mã đơn hàng mới được tạo</returns>
        public static async Task<int> AddOrderAsync(Order data)
        {
            return await SV22T1020073.BusinessLayers.SalesDataService.AddOrderAsync(data);
        }

        /// <summary>
        /// Thêm chi tiết đơn hàng
        /// </summary>
        /// <param name="data">Thông tin chi tiết đơn hàng</param>
        public static async Task AddDetailAsync(OrderDetail data)
        {
            await SV22T1020073.BusinessLayers.SalesDataService.AddDetailAsync(data);
        }

        /// <summary>
        /// Lấy danh sách đơn hàng theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="input">Thông tin tìm kiếm và phân trang</param>
        /// <returns>Danh sách đơn hàng</returns>
        public static async Task<PagedResult<OrderViewInfo>> ListOrdersAsync(OrderSearchInput input)
        {
            return await SV22T1020073.BusinessLayers.SalesDataService.ListOrdersAsync(input);
        }

        /// <summary>
        /// Lấy thông tin đơn hàng theo mã
        /// </summary>
        /// <param name="orderId">Mã đơn hàng</param>
        /// <returns>Thông tin đơn hàng hoặc null</returns>
        public static async Task<Order?> GetOrderAsync(int orderId)
        {
            return await SV22T1020073.BusinessLayers.SalesDataService.GetOrderAsync(orderId);
        }

        /// <summary>
        /// Lấy danh sách chi tiết đơn hàng
        /// </summary>
        /// <param name="orderId">Mã đơn hàng</param>
        /// <returns>Danh sách chi tiết đơn hàng</returns>
        public static async Task<List<OrderDetailViewInfo>> ListDetailsAsync(int orderId)
        {
            return await SV22T1020073.BusinessLayers.SalesDataService.ListDetailsAsync(orderId);
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        /// <param name="orderId">Mã đơn hàng cần hủy</param>
        /// <returns>true nếu hủy thành công, false nếu thất bại</returns>
        public static async Task<bool> CancelOrderAsync(int orderId)
        {
            return await SV22T1020073.BusinessLayers.SalesDataService.CancelOrderAsync(orderId);
        }
    }
}
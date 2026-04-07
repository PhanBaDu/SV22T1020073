using SV22T1020073.Models.Common;
using SV22T1020073.Models.Partner;

namespace SV22T1020073.Shop.Services
{
    /// <summary>
    /// Lớp xử lý các nghiệp vụ liên quan đến đối tác trong Shop
    /// </summary>
    public static class ShopPartnerService
    {
        /// <summary>
        /// Lấy thông tin khách hàng theo mã
        /// </summary>
        /// <param name="customerId">Mã khách hàng</param>
        /// <returns>Thông tin khách hàng hoặc null</returns>
        public static async Task<Customer?> GetCustomerAsync(int customerId)
        {
            return await SV22T1020073.BusinessLayers.PartnerDataService.GetCustomerAsync(customerId);
        }

        /// <summary>
        /// Lấy danh sách khách hàng theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="input">Thông tin tìm kiếm và phân trang</param>
        /// <returns>Danh sách khách hàng</returns>
        public static async Task<PagedResult<Customer>> ListCustomersAsync(PaginationSearchInput input)
        {
            return await SV22T1020073.BusinessLayers.PartnerDataService.ListCustomersAsync(input);
        }

        /// <summary>
        /// Kiểm tra email khách hàng đã tồn tại chưa
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>true nếu email hợp lệ (chưa tồn tại), false nếu đã tồn tại</returns>
        public static async Task<bool> ValidateCustomerEmailAsync(string email)
        {
            return await SV22T1020073.BusinessLayers.PartnerDataService.ValidateCustomerEmailAsync(email);
        }

        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="data">Thông tin khách hàng cần thêm</param>
        /// <returns>Mã khách hàng mới được tạo</returns>
        public static async Task<int> AddCustomerAsync(Customer data)
        {
            return await SV22T1020073.BusinessLayers.PartnerDataService.AddCustomerAsync(data);
        }

        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="data">Thông tin khách hàng cần cập nhật</param>
        /// <returns>true nếu cập nhật thành công, false nếu thất bại</returns>
        public static async Task<bool> UpdateCustomerAsync(Customer data)
        {
            return await SV22T1020073.BusinessLayers.PartnerDataService.UpdateCustomerAsync(data);
        }

        /// <summary>
        /// Lấy danh sách đơn vị vận chuyển
        /// </summary>
        /// <param name="input">Thông tin tìm kiếm và phân trang</param>
        /// <returns>Danh sách đơn vị vận chuyển</returns>
        public static async Task<PagedResult<Shipper>> ListShippersAsync(PaginationSearchInput input)
        {
            return await SV22T1020073.BusinessLayers.PartnerDataService.ListShippersAsync(input);
        }

        /// <summary>
        /// Lấy thông tin nhà cung cấp theo mã
        /// </summary>
        /// <param name="supplierId">Mã nhà cung cấp</param>
        /// <returns>Thông tin nhà cung cấp hoặc null</returns>
        public static async Task<Supplier?> GetSupplierAsync(int supplierId)
        {
            return await SV22T1020073.BusinessLayers.PartnerDataService.GetSupplierAsync(supplierId);
        }
    }
}
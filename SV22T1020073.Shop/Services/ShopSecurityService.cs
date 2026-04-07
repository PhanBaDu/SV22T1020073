using SV22T1020073.Models.Security;

namespace SV22T1020073.Shop.Services
{
    /// <summary>
    /// Lớp xử lý các nghiệp vụ bảo mật trong Shop
    /// </summary>
    public static class ShopSecurityService
    {
        /// <summary>
        /// Xác thực tài khoản người dùng
        /// </summary>
        /// <param name="userName">Tên đăng nhập (email)</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="userType">Loại tài khoản</param>
        /// <returns>Thông tin tài khoản hoặc null nếu xác thực thất bại</returns>
        public static async Task<UserAccount?> AuthorizeAsync(string userName, string password, UserTypes userType)
        {
            return await SV22T1020073.BusinessLayers.SecurityDataService.AuthorizeAsync(userName, password, userType);
        }

        /// <summary>
        /// Thay đổi mật khẩu người dùng
        /// </summary>
        /// <param name="userName">Tên đăng nhập (email)</param>
        /// <param name="password">Mật khẩu mới</param>
        /// <param name="userType">Loại tài khoản</param>
        /// <returns>true nếu đổi thành công, false nếu thất bại</returns>
        public static async Task<bool> ChangePasswordAsync(string userName, string password, UserTypes userType)
        {
            return await SV22T1020073.BusinessLayers.SecurityDataService.ChangePasswordAsync(userName, password, userType);
        }
    }
}
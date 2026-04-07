using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SV22T1020073.Shop
{
    /// <summary>
    /// Thông tin tài khoản người dùng được lưu trong phiên đăng nhập (cookie) của Shop
    /// </summary>
    public class WebUserData
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Photo { get; set; }
        public List<string>? Roles { get; set; }

        /// <summary>
        /// Tạo Principal dựa trên thông tin người dùng
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal CreatePrincipal()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserId ?? ""),
                new Claim(ClaimTypes.Name, UserName ?? ""),
                new Claim(nameof(DisplayName), DisplayName ?? ""),
                new Claim(nameof(Photo), Photo ?? "")
            };
            if (Roles != null)
                foreach (var role in Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }

    /// <summary>
    /// Định nghĩa tên vai trò cho người dùng Shop
    /// </summary>
    public class WebUserRoles
    {
        /// <summary>
        /// Khách hàng
        /// </summary>
        public const string Customer = "customer";
    }

    /// <summary>
    /// Extension methods cho context bảo mật của Shop
    /// </summary>
    public static class WebUserExtensions
    {
        /// <summary>
        /// Lấy thông tin user hiện tại từ Principal
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static WebUserData? GetUserData(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                    return null;

                var userData = new WebUserData();

                userData.UserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                userData.UserName = principal.FindFirstValue(ClaimTypes.Name);
                userData.DisplayName = principal.FindFirstValue(nameof(userData.DisplayName));
                userData.Photo = principal.FindFirstValue(nameof(userData.Photo));

                userData.Roles = new List<string>();
                foreach (var claim in principal.FindAll(ClaimTypes.Role))
                {
                    userData.Roles.Add(claim.Value);
                }

                return userData;
            }
            catch
            {
                return null;
            }
        }
    }
}

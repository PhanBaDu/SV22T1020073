using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace SV22T1020073.Admin
{
    /// <summary>
    /// Thông tin tài khoản người dùng được lưu trong phiên đăng nhập (cookie)
    /// </summary>
    public class WebUserData
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Photo { get; set; }
        public List<string>? Roles { get; set; }

        /// <summary>
        /// Lấy danh sách các Claim chứa thông tin của user
        /// </summary>
        /// <returns></returns>
        private List<Claim> Claims
        {
            get
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(nameof(UserId), UserId ?? ""),
                    new Claim(nameof(UserName), UserName ?? ""),
                    new Claim(nameof(DisplayName), DisplayName ?? ""),
                    new Claim(nameof(Email), Email ?? ""),
                    new Claim(nameof(Photo), Photo ?? "")
                };
                if (Roles != null)
                    foreach (var role in Roles)
                        claims.Add(new Claim(ClaimTypes.Role, role));
                return claims;
            }
        }

        /// <summary>
        /// Tạo Principal dựa trên thông tin của người dùng
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal CreatePrincipal()
        {
            var claimIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            return claimPrincipal;
        }
    }

    /// <summary>
    /// Định nghĩa tên của các role sử dụng trong phân quyền chức năng cho nhân viên
    /// </summary>
    public class WebUserRoles
    {
        /// <summary>
        /// Quản trị
        /// </summary>
        public const string Administrator = "admin";
        /// <summary>
        /// Quản lý dữ liệu
        /// </summary>
        public const string DataManager = "datamanager";
        /// <summary>
        /// Quản lý bán hàng
        /// </summary>
        public const string Sales = "sales";
    }

    /// <summary>
    /// Extension các phương thức cho các đối tượng liên quan đến xác thực tài khoản người dùng
    /// </summary>
    public static class WebUserExtensions
    {
        /// <summary>
        /// Đọc thông tin của user từ principal
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

                userData.UserId = principal.FindFirstValue(nameof(userData.UserId));
                userData.UserName = principal.FindFirstValue(nameof(userData.UserName));
                userData.DisplayName = principal.FindFirstValue(nameof(userData.DisplayName));
                userData.Email = principal.FindFirstValue(nameof(userData.Email));
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

namespace SV22T1020073.Admin.AppCodes
{
    /// <summary>
    /// Yêu cầu kiểm tra quyền: người dùng cần có ít nhất một trong danh sách quyền (OR logic)
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Danh sách quyền cần kiểm tra
        /// </summary>
        public IReadOnlyList<string> Permissions { get; }

        /// <summary>
        /// Khởi tạo PermissionRequirement với danh sách quyền
        /// </summary>
        /// <param name="permissions">Danh sách mã quyền</param>
        public PermissionRequirement(params string[] permissions)
        {
            Permissions = permissions.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Handler xử lý kiểm tra quyền: người dùng cần có ít nhất một quyền trong danh sách (OR logic)
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        /// <summary>
        /// Xử lý kiểm tra quyền theo OR logic
        /// </summary>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            foreach (var perm in requirement.Permissions)
            {
                if (context.User.HasClaim("Permission", perm))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Attribute yêu cầu người dùng có ít nhất một quyền trong danh sách (OR logic).
    /// Sử dụng cho các action cần kiểm tra quyền theo OR logic (ví dụ: create HOẶC edit).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizePermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Khởi tạo với danh sách quyền cần kiểm tra (OR logic)
        /// </summary>
        /// <param name="permissions">Danh sách mã quyền</param>
        public AuthorizePermissionAttribute(params string[] permissions)
        {
            var sortedPerms = permissions.OrderBy(p => p).ToList();
            var combinedKey = string.Join("_", sortedPerms).Replace(":", "_");
            Policy = "Permission_" + combinedKey;
        }
    }

    /// <summary>
    /// Attribute yêu cầu người dùng có tất cả quyền trong danh sách (AND logic).
    /// Sử dụng cho các action cần kiểm tra quyền theo AND logic.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAllPermissionsAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Khởi tạo với danh sách quyền cần kiểm tra (AND logic)
        /// </summary>
        /// <param name="permissions">Danh sách mã quyền</param>
        public AuthorizeAllPermissionsAttribute(params string[] permissions)
        {
            var sortedPerms = permissions.OrderBy(p => p).ToList();
            var combinedKey = string.Join("_", sortedPerms).Replace(":", "_");
            Policy = "AllPermissions_" + combinedKey;
        }
    }

    /// <summary>
    /// Cung cấp Authorization Policy động cho các quyền.
    /// Hỗ trợ policy đơn lẻ ("Permission_x") và policy kết hợp ("Permission_x_y")
    /// mà không cần đăng ký trước từng tổ hợp.
    /// </summary>
    public class DynamicPermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string PermissionPrefix = "Permission_";

        private readonly DefaultAuthorizationPolicyProvider _fallback;

        /// <summary>
        /// Khởi tạo DynamicPermissionPolicyProvider
        /// </summary>
        /// <param name="options">AuthorizationOptions từ DI</param>
        public DynamicPermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallback = new DefaultAuthorizationPolicyProvider(options);
        }

        /// <summary>
        /// Lấy policy mặc định
        /// </summary>
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => _fallback.GetDefaultPolicyAsync();

        /// <summary>
        /// Lấy policy fallback
        /// </summary>
        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => _fallback.GetFallbackPolicyAsync();

        /// <summary>
        /// Lấy policy theo tên, tạo động nếu tên bắt đầu bằng "Permission_"
        /// </summary>
        /// <param name="policyName">Tên policy</param>
        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PermissionPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var combinedKey = policyName[PermissionPrefix.Length..];
                var permissions = ParsePermissionsFromKey(combinedKey);

                if (permissions.Length > 0)
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new PermissionRequirement(permissions))
                        .Build();
                    return Task.FromResult<AuthorizationPolicy?>(policy);
                }
            }

            return _fallback.GetPolicyAsync(policyName);
        }

        /// <summary>
        /// Phân tích combinedKey thành danh sách chuỗi quyền.
        /// Ví dụ: "employee_create_employee_edit" → ["employee:create", "employee:edit"]
        /// </summary>
        /// <param name="combinedKey">Chuỗi key kết hợp</param>
        /// <returns>Mảng các mã quyền</returns>
        private static string[] ParsePermissionsFromKey(string combinedKey)
        {
            var segments = combinedKey.Split('_');
            if (segments.Length % 2 != 0)
            {
                return new[] { combinedKey.Replace('_', ':') };
            }

            var result = new List<string>();
            for (int i = 0; i < segments.Length; i += 2)
            {
                result.Add($"{segments[i]}:{segments[i + 1]}");
            }
            return result.ToArray();
        }
    }
}
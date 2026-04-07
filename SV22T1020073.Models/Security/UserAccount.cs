namespace SV22T1020073.Models.Security
{
    /// <summary>
    /// Tài kho?n ngu?i dùng
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// Tên dang nh?p
        /// </summary>
        public string UserID { get; set; } = "";
        /// <summary>
        /// Tên hi?n th?
        /// </summary>
        public string FullName { get; set; } = "";
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = "";
        /// <summary>
        /// ?nh d?i di?n
        /// </summary>
        public string? Photo { get; set; }
    }
}
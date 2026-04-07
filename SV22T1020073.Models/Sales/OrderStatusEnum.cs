namespace SV22T1020073.Models.Sales
{
    /// <summary>
    /// Ð?nh nghia các tr?ng thái c?a don hàng
    /// </summary>
    public enum OrderStatusEnum
    {
        /// <summary>
        /// Ðon hàng b? t? ch?i
        /// </summary>
        Rejected = -2,
        /// <summary>
        /// Ðon hàng b? h?y
        /// </summary>
        Cancelled = -1,
        /// <summary>
        /// Ðon hàng v?a du?c t?o, chua du?c x? lý
        /// </summary>
        New = 1,
        /// <summary>
        /// Ðon hàng dã du?c duy?t ch?p nh?n
        /// </summary>
        Accepted = 2,
        /// <summary>
        /// Ðon hàng dang du?c giao cho ngu?i giao hàng d? v?n chuy?n d?n khách hàng
        /// </summary>
        Shipping = 3,
        /// <summary>
        /// Ðon hàng dã hoàn t?t (thành công)
        /// </summary>
        Completed = 4
    }
}
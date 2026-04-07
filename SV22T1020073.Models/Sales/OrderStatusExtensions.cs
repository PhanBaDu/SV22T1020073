namespace SV22T1020073.Models.Sales
{
    /// <summary>
    /// M? r?ng các phuong th?c cho enum OrderStatusEnum
    /// </summary>
    public static class OrderStatusExtensions
    {
        /// <summary>
        /// L?y chu?i mô t? cho t?ng tr?ng thái c?a don hàng
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetDescription(this OrderStatusEnum status)
        {
            return status switch
            {
                OrderStatusEnum.Rejected => "Ðon hàng b? t? ch?i",
                OrderStatusEnum.Cancelled => "Ðon hàng dã b? h?y",
                OrderStatusEnum.New => "Ðon hàng v?a t?o",
                OrderStatusEnum.Accepted => "Ðon hàng dã du?c duy?t",
                OrderStatusEnum.Shipping => "Ðon hàng dang du?c v?n chuy?n",
                OrderStatusEnum.Completed => "Ðon hàng dã hoàn t?t",
                _ => "Không xác d?nh"
            };
        }
    }
}
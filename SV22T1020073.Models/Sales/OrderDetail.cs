namespace SV22T1020073.Models.Sales
{
    /// <summary>
    /// Thông tin chi ti?t c?a m?t hàng du?c bán trong don hàng
    /// </summary>
    public class OrderDetail
    {
        /// <summary>
        /// Mã don hàng
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// Mã m?t hàng
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// S? lu?ng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Giá bán
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// T?ng s? ti?n
        /// </summary>
        public decimal TotalPrice => Quantity * SalePrice;        
    }
}
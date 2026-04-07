namespace SV22T1020073.Models.Partner
{
    /// <summary>
    /// Ngu?i giao hàng
    /// </summary>
    public class Shipper
    {
        /// <summary>
        /// Mã ngu?i giao hàng
        /// </summary>
        public int ShipperID { get; set; }
        /// <summary>
        /// Tên ngu?i giao hàng
        /// </summary>
        public string ShipperName { get; set; } = string.Empty;
        /// <summary>
        /// Ði?n tho?i
        /// </summary>
        public string? Phone { get; set; }
    }
}
namespace SV22T1020073.Models.Sales
{
    /// <summary>
    /// Ðon hàng
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Mã don hàng
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public int? CustomerID { get; set; }
        /// <summary>
        /// Th?i di?m d?t hàng (th?i di?m t?o don hàng)
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// T?nh/Thành giao hàng
        /// </summary>
        public string? DeliveryProvince { get; set; }
        /// <summary>
        /// Ð?a ch? giao hàng
        /// </summary>
        public string? DeliveryAddress { get; set; }
        /// <summary>
        /// Mã nhân viên x? lý don hàng (ngu?i nh?n/duy?t don hàng)
        /// </summary>
        public int? EmployeeID { get; set; }
        /// <summary>
        /// Th?i di?m duy?t don hàng (th?i di?m nhân viên nh?n/duy?t don hàng)
        /// </summary>
        public DateTime? AcceptTime { get; set; }
        /// <summary>
        /// Mã ngu?i giao hàng
        /// </summary>
        public int? ShipperID { get; set; }
        /// <summary>
        /// Th?i di?m ngu?i giao hàng nh?n don hàng d? giao
        /// </summary>
        public DateTime? ShippedTime { get; set; }
        /// <summary>
        /// Th?i di?m k?t thúc don hàng
        /// </summary>
        public DateTime? FinishedTime { get; set; }
        /// <summary>
        /// Tr?ng thái hi?n t?i c?a don hàng
        /// </summary>
        public OrderStatusEnum Status { get; set; }
    }
}
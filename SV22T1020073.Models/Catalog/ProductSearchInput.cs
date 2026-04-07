using SV22T1020073.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV22T1020073.Models.Catalog
{
    /// <summary>
    /// Bi?u di?n d? li?u d?u vào tìm ki?m, phân trang d?i v?i m?t hàng
    /// </summary>
    public class ProductSearchInput : PaginationSearchInput
    {
        /// <summary>
        /// Mã lo?i hàng (0 n?u b? qua)
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Mã nhà cung c?p (0 n?u b? qua)
        /// </summary>
        public int SupplierID { get; set; }
        /// <summary>
        /// Giá t?i thi?u (0 n?u b? qua)
        /// </summary>
        public decimal MinPrice { get; set; }
        /// <summary>
        /// M?c giá t?i da (0 n?u b? qua)
        /// </summary>
        public decimal MaxPrice { get; set; }
    }
}
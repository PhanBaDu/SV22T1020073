namespace SV22T1020073.Models.Common
{
    /// <summary>
    /// L?p dùng d? bi?u di?n thông tin d?u vào c?a m?t truy v?n/tìm ki?m 
    /// d? li?u don gi?n du?i d?ng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        private const int MaxPageSize = 100; //Gi?i h?n t?i da 100 dòng m?i trang
        private int _page = 1;
        private int _pageSize = 20;
        private string _searchValue = "";
        
        /// <summary>
        /// Trang c?n du?c hi?n th? (b?t d?u t? 1)
        /// </summary>
        public int Page 
        { 
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }
        /// <summary>
        /// S? dòng du?c hi?n th? trên m?i trang
        /// (0 có nghia là hi?n th? t?t c? các dòng trên m?t trang, t?c là không phân trang)
        /// </summary>
        public int PageSize 
        { 
            get => _pageSize; 
            set
            {
                if (value < 0)
                    _pageSize = 0;
                else if (value > MaxPageSize)
                    _pageSize = MaxPageSize;
                else
                    _pageSize = value;
            }
        }
        /// <summary>
        /// Giá tr? tìm ki?m (n?u có) du?c s? d?ng d? l?c d? li?u 
        /// (N?u không có giá tr? tìm ki?m, thì d? r?ng)
        /// </summary>
        public string SearchValue
        { 
            get => _searchValue; 
            set => _searchValue = value?.Trim() ?? ""; 
        }        
        /// <summary>
        /// S? dòng c?n b? qua (tính t? dòng d?u tiên c?a t?p d? li?u) 
        /// d? l?y d? li?u cho trang hi?n t?i
        /// </summary>
        public int Offset => PageSize > 0 ? (Page - 1) * PageSize : 0;
    }
}
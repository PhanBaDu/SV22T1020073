using SV22T1020073.Models.Common;
using SV22T1020073.Models.Catalog;
using SV22T1020073.Models.Sales;
using SV22T1020073.Models.Partner;
using SV22T1020073.Models.Security;

namespace SV22T1020073.Shop.Services
{
    /// <summary>
    /// Lớp xử lý các nghiệp vụ liên quan đến danh mục sản phẩm trong Shop
    /// </summary>
    public static class ShopCatalogService
    {
        /// <summary>
        /// Lấy danh sách sản phẩm theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="input">Thông tin tìm kiếm và phân trang</param>
        /// <returns>Danh sách sản phẩm</returns>
        public static async Task<PagedResult<Product>> ListProductsAsync(ProductSearchInput input)
        {
            return await SV22T1020073.BusinessLayers.CatalogDataService.ListProductsAsync(input);
        }

        /// <summary>
        /// Lấy thông tin một sản phẩm theo mã
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>Thông tin sản phẩm hoặc null nếu không tìm thấy</returns>
        public static async Task<Product?> GetProductAsync(int productId)
        {
            return await SV22T1020073.BusinessLayers.CatalogDataService.GetProductAsync(productId);
        }

        /// <summary>
        /// Lấy danh sách danh mục sản phẩm
        /// </summary>
        /// <param name="input">Thông tin tìm kiếm và phân trang</param>
        /// <returns>Danh sách danh mục</returns>
        public static async Task<PagedResult<Category>> ListCategoriesAsync(PaginationSearchInput input)
        {
            return await SV22T1020073.BusinessLayers.CatalogDataService.ListCategoriesAsync(input);
        }

        /// <summary>
        /// Lấy thông tin một danh mục theo mã
        /// </summary>
        /// <param name="categoryId">Mã danh mục</param>
        /// <returns>Thông tin danh mục hoặc null nếu không tìm thấy</returns>
        public static async Task<Category?> GetCategoryAsync(int categoryId)
        {
            return await SV22T1020073.BusinessLayers.CatalogDataService.GetCategoryAsync(categoryId);
        }

        /// <summary>
        /// Lấy danh sách hình ảnh của sản phẩm
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>Danh sách hình ảnh</returns>
        public static async Task<List<ProductPhoto>> ListPhotosAsync(int productId)
        {
            return await SV22T1020073.BusinessLayers.CatalogDataService.ListPhotosAsync(productId);
        }

        /// <summary>
        /// Lấy danh sách thuộc tính của sản phẩm
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>Danh sách thuộc tính</returns>
        public static async Task<List<ProductAttribute>> ListAttributesAsync(int productId)
        {
            return await SV22T1020073.BusinessLayers.CatalogDataService.ListAttributesAsync(productId);
        }
    }
}
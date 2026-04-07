using Microsoft.AspNetCore.Mvc;
using SV22T1020073.Models.Common;
using SV22T1020073.Models.Catalog;
using SV22T1020073.Shop.Services;

namespace SV22T1020073.Shop.Controllers;

/// <summary>
/// Điều khiển xử lý các yêu cầu liên quan đến sản phẩm
/// </summary>
public class ProductController : Controller
{
    private const int PAGE_SIZE = 12;

    /// <summary>
    /// Hiển thị trang danh sách sản phẩm với bộ lọc
    /// </summary>
    /// <param name="page">Số trang hiện tại</param>
    /// <param name="categoryID">Mã danh mục để lọc</param>
    /// <param name="searchValue">Từ khóa tìm kiếm</param>
    /// <param name="minPrice">Giá tối thiểu</param>
    /// <param name="maxPrice">Giá tối đa</param>
    /// <returns>View danh sách sản phẩm</returns>
    public async Task<IActionResult> Index(int page = 1, int categoryID = 0, string searchValue = "", decimal minPrice = 0, decimal maxPrice = 0)
    {
        var input = new ProductSearchInput()
        {
            Page = page,
            PageSize = PAGE_SIZE,
            SearchValue = searchValue ?? "",
            CategoryID = categoryID,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };
        ViewBag.Categories = await ShopCatalogService.ListCategoriesAsync(new PaginationSearchInput { Page = 1, PageSize = 100 });
        return View(input);
    }

    /// <summary>
    /// Trả về danh sách sản phẩm dạng partial view (Ajax)
    /// TC-C3: Kiểm tra MaxPrice >= MinPrice.
    /// </summary>
    /// <param name="input">Thông tin tìm kiếm và phân trang</param>
    /// <returns>Partial view chứa danh sách sản phẩm</returns>
    public async Task<IActionResult> Search(ProductSearchInput input)
    {
        if (input.MaxPrice > 0 && input.MinPrice > 0 && input.MaxPrice < input.MinPrice)
        {
            input.MaxPrice = 0;
        }
        if (input.MinPrice < 0) input.MinPrice = 0;
        if (input.MaxPrice < 0) input.MaxPrice = 0;

        input.PageSize = PAGE_SIZE;
        var data = await ShopCatalogService.ListProductsAsync(input);
        return PartialView("_ProductGrid", data);
    }

    /// <summary>
    /// Hiển thị trang chi tiết sản phẩm
    /// </summary>
    /// <param name="id">Mã sản phẩm</param>
    /// <returns>View chi tiết sản phẩm hoặc redirect về Index nếu không tìm thấy</returns>
    public async Task<IActionResult> Detail(int id)
    {
        var product = await ShopCatalogService.GetProductAsync(id);
        if (product == null) return RedirectToAction("Index");

        ViewBag.Photos = await ShopCatalogService.ListPhotosAsync(id);
        ViewBag.Attributes = await ShopCatalogService.ListAttributesAsync(id);
        
        if (product.SupplierID.HasValue && product.SupplierID.Value > 0)
            ViewBag.Supplier = await ShopPartnerService.GetSupplierAsync(product.SupplierID.Value);
            
        if (product.CategoryID.HasValue && product.CategoryID.Value > 0)
            ViewBag.Category = await ShopCatalogService.GetCategoryAsync(product.CategoryID.Value);

        return View(product);
    }
}
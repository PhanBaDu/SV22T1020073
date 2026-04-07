using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SV22T1020073.Shop.Models;
using SV22T1020073.Models.Common;
using SV22T1020073.Models.Catalog;
using SV22T1020073.Shop.Services;

namespace SV22T1020073.Shop.Controllers;

/// <summary>
/// Điều khiển xử lý các yêu cầu liên quan đến trang chủ và tìm kiếm sản phẩm
/// </summary>
public class HomeController : Controller
{
    private const int PAGE_SIZE = 12;

    /// <summary>
    /// Hiển thị trang chủ với danh sách sản phẩm được phân trang
    /// </summary>
    /// <param name="page">Số trang hiện tại</param>
    /// <param name="categoryID">Mã danh mục để lọc sản phẩm</param>
    /// <param name="searchValue">Từ khóa tìm kiếm sản phẩm</param>
    /// <param name="minPrice">Giá tối thiểu</param>
    /// <param name="maxPrice">Giá tối đa</param>
    /// <returns>View trang chủ với danh sách sản phẩm</returns>
    public async Task<IActionResult> Index(
        int page = 1,
        int categoryID = 0,
        string searchValue = "",
        decimal minPrice = 0,
        decimal maxPrice = 0)
    {
        var categories = await ShopCatalogService.ListCategoriesAsync(
            new PaginationSearchInput { Page = 1, PageSize = 100 });

        var input = new ProductSearchInput
        {
            Page = page,
            PageSize = PAGE_SIZE,
            SearchValue = searchValue ?? "",
            CategoryID = categoryID,
            MinPrice = minPrice,
            MaxPrice = maxPrice
        };
        var products = await ShopCatalogService.ListProductsAsync(input);

        ViewBag.Categories = categories;
        ViewBag.ProductResult = products;
        ViewBag.CurrentCategoryID = categoryID;
        ViewBag.CurrentSearchValue = searchValue;
        ViewBag.CurrentMinPrice = minPrice;
        ViewBag.CurrentMaxPrice = maxPrice;
        ViewBag.FilterInput = input;

        return View();
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
    /// Hiển thị trang thông tin quyền riêng tư
    /// </summary>
    /// <returns>View trang quyền riêng tư</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Hiển thị trang xử lý lỗi hệ thống
    /// </summary>
    /// <returns>View trang lỗi với mã yêu cầu</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
using Microsoft.AspNetCore.Mvc;
using SV22T1020073.Models.Common;
using SV22T1020073.Shop.Services;

namespace SV22T1020073.Shop.ViewComponents;

/// <summary>
/// Component hiển thị danh mục sản phẩm trên thanh điều hướng
/// </summary>
public class CategoryNavViewComponent : ViewComponent
{
    /// <summary>
    /// Lấy danh sách danh mục để hiển thị trên navigation
    /// </summary>
    /// <returns>Danh sách danh mục dưới dạng ViewComponentResult</returns>
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await ShopCatalogService.ListCategoriesAsync(
            new PaginationSearchInput { Page = 1, PageSize = 500 });

        return View(result.DataItems);
    }
}
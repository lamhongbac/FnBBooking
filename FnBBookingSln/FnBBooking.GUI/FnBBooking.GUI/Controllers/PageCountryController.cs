using System;
using System.Collections.Generic;
using System.Linq;
using FnBBooking.GUI.Helpers;
using FnBBooking.GUI.ViewModels;

namespace FnBBooking.GUI.Controllers
{
    public class PageCountryController : BasePageController<CountryViewModel>
    {
        public PageCountryController(List<CountryViewModel> allItems, SortModel sortModel,
            Dictionary<string, object> filterObject, int pageIndex = 1, int pageSize = 10)
            : base(allItems, sortModel, filterObject, pageIndex, pageSize)
        {
        }

        public override void Filter()
        {
            var query = AllItems.Where(b => !b.IsDeleted);

            if (FilterObject != null && FilterObject.Count > 0)
            {
                // 1. Search keyword
                if (FilterObject.TryGetValue(FilterKey.SearchKeyword, out var keywordObj) && keywordObj != null)
                {
                    string keyword = keywordObj.ToString()!.Trim();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        query = query.Where(b =>
                            (b.Number != null && b.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (b.Name != null && b.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (b.Description != null && b.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        );
                    }
                }

                // 2. Status filter
                if (FilterObject.TryGetValue(FilterKey.Status, out var statusObj) && statusObj != null)
                {
                    string status = statusObj.ToString()!;
                    if (status == "Active")
                    {
                        query = query.Where(b => b.IsActive);
                    }
                    else if (status == "Inactive")
                    {
                        query = query.Where(b => !b.IsActive);
                    }
                }
            }

            FilterItems = query.ToList();
            DoSort();
        }

        public override void DoSort()
        {
            if (PageSortModel == null || string.IsNullOrEmpty(PageSortModel.SortedProperty))
            {
                FilterItems = FilterItems.OrderBy(b => b.Number).ToList();
                return;
            }

            bool isAsc = PageSortModel.SortedOrder == SortOrder.Ascending;
            string prop = PageSortModel.SortedProperty;

            FilterItems = prop switch
            {
                "Number" => isAsc ? FilterItems.OrderBy(b => b.Number).ToList() : FilterItems.OrderByDescending(b => b.Number).ToList(),
                "Name" => isAsc ? FilterItems.OrderBy(b => b.Name).ToList() : FilterItems.OrderByDescending(b => b.Name).ToList(),
                "IsActive" => isAsc ? FilterItems.OrderBy(b => b.IsActive).ToList() : FilterItems.OrderByDescending(b => b.IsActive).ToList(),
                _ => isAsc ? FilterItems.OrderBy(b => b.Number).ToList() : FilterItems.OrderByDescending(b => b.Number).ToList(),
            };
        }
    }
}

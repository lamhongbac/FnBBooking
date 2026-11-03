using System;
using System.Collections.Generic;
using System.Linq;
using FnBBooking.GUI.Helpers;
using FnBBooking.GUI.ViewModels;

namespace FnBBooking.GUI.Controllers
{
    public class PageAreaController : BasePageController<AreaViewModel>
    {
        public PageAreaController(List<AreaViewModel> allItems, SortModel sortModel,
            Dictionary<string, object> filterObject, int pageIndex = 1, int pageSize = 10)
            : base(allItems, sortModel, filterObject, pageIndex, pageSize)
        {
        }

        public override void Filter()
        {
            var query = AllItems.Where(a => !a.IsDeleted);

            if (FilterObject != null && FilterObject.Count > 0)
            {
                // 1. Search keyword
                if (FilterObject.TryGetValue(FilterKey.SearchKeyword, out var keywordObj) && keywordObj != null)
                {
                    string keyword = keywordObj.ToString()!.Trim();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        query = query.Where(a =>
                            (a.Number != null && a.Number.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (a.Name != null && a.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (a.Description != null && a.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        );
                    }
                }

                // 2. Status filter
                if (FilterObject.TryGetValue(FilterKey.Status, out var statusObj) && statusObj != null)
                {
                    string status = statusObj.ToString()!;
                    if (status == "Active")
                    {
                        query = query.Where(a => a.IsActive);
                    }
                    else if (status == "Inactive")
                    {
                        query = query.Where(a => !a.IsActive);
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
                FilterItems = FilterItems.OrderBy(a => a.Number).ToList();
                return;
            }

            bool isAsc = PageSortModel.SortedOrder == SortOrder.Ascending;
            string prop = PageSortModel.SortedProperty;

            FilterItems = prop switch
            {
                "Number" => isAsc ? FilterItems.OrderBy(a => a.Number).ToList() : FilterItems.OrderByDescending(a => a.Number).ToList(),
                "Name" => isAsc ? FilterItems.OrderBy(a => a.Name).ToList() : FilterItems.OrderByDescending(a => a.Name).ToList(),
                "TableCount" => isAsc ? FilterItems.OrderBy(a => a.TableCount).ToList() : FilterItems.OrderByDescending(a => a.TableCount).ToList(),
                "ModifiedOn" => isAsc ? FilterItems.OrderBy(a => a.ModifiedOn).ToList() : FilterItems.OrderByDescending(a => a.ModifiedOn).ToList(),
                _ => isAsc ? FilterItems.OrderBy(a => a.Number).ToList() : FilterItems.OrderByDescending(a => a.Number).ToList(),
            };
        }
    }
}


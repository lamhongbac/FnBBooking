using System.Collections.Generic;
using System.Linq;

namespace FnBBooking.GUI.Helpers
{
    public abstract class BasePageController<T>
    {
        private IEnumerable<T> _allItems = new List<T>();

        public IEnumerable<T> AllItems
        {
            get => _allItems;
            set
            {
                _allItems = value ?? new List<T>();
                FilterItems = _allItems;
            }
        }

        public IEnumerable<T> FilterItems { get; set; } = new List<T>();

        public Dictionary<string, object> FilterObject { get; set; } = new();

        public PaginatedList<T> CurrentPageData { get; set; }

        public PagerModel PagingInfo { get; set; }

        public SortModel PageSortModel { get; set; }

        public BasePageController(List<T> allItems, SortModel sortModel,
            Dictionary<string, object> filterObject, int pageIndex = 1, int pageSize = 10)
        {
            PageSortModel = sortModel;
            FilterObject = filterObject ?? new();
            AllItems = allItems ?? new();

            Filter();
            DoSort();

            PagingInfo = new PagerModel(FilterItems.Count(), pageIndex, pageSize);
            CurrentPageData = new PaginatedList<T>(FilterItems.ToList(), PagingInfo.CurrentPage, PagingInfo.PageSize);
        }

        public void SetupPage(int pageIndex = 1)
        {
            PagingInfo.SetPaging(FilterItems.Count(), pageIndex, PagingInfo.PageSize);
            CurrentPageData = new PaginatedList<T>(FilterItems.ToList(), PagingInfo.CurrentPage, PagingInfo.PageSize);
        }

        public void ChangePageSize(int pageSize)
        {
            PagingInfo.SetPageSize(pageSize);
            SetupPage(1);
        }

        public void RefreshPage()
        {
            Filter();
            DoSort();
            SetupPage(PagingInfo.CurrentPage);
        }

        public void GoToPage(int pageIndex)
        {
            PagingInfo.SetPage(pageIndex);
            CurrentPageData.ChangePage(pageIndex, PagingInfo.PageSize);
        }

        public abstract void Filter();

        public abstract void DoSort();
    }
}


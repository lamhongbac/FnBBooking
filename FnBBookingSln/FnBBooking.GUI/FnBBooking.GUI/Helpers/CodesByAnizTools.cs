using System;
using System.Collections.Generic;
using System.Linq;

namespace FnBBooking.GUI.Helpers
{
    public enum SortOrder
    {
        Ascending = 0,
        Descending = 1
    }

    public class SortModel
    {
        public string SortedProperty { get; set; } = string.Empty;
        public SortOrder SortedOrder { get; set; } = SortOrder.Ascending;
        public List<string> SortableColumns { get; set; } = new();

        public void AddColumn(string colname, bool isDefaultColumn = false)
        {
            var existing = SortableColumns.FirstOrDefault(c => string.Equals(c, colname, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                SortableColumns.Add(colname);
            }

            if (isDefaultColumn || SortableColumns.Count == 1)
            {
                SortedProperty = colname;
                SortedOrder = SortOrder.Ascending;
            }
        }

        public void ApplySort(string column)
        {
            if (string.Equals(column, SortedProperty, StringComparison.OrdinalIgnoreCase))
            {
                SortedOrder = (SortedOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                SortedProperty = column;
                SortedOrder = SortOrder.Ascending;
            }
        }
    }

    public class PagerModel
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }

        public PagerModel(int totalItems, int pageIndex, int pageSize = 10)
        {
            SetPaging(totalItems, pageIndex, pageSize);
        }

        public void SetPaging(int totalItems, int pageIndex, int pageSize)
        {
            TotalItems = totalItems;
            PageSize = pageSize <= 0 ? 10 : pageSize;
            TotalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)PageSize);
            CurrentPage = pageIndex <= 0 ? 1 : (pageIndex > TotalPages && TotalPages > 0 ? TotalPages : pageIndex);

            int startPage = CurrentPage - 2;
            int endPage = CurrentPage + 2;

            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }

            if (endPage > TotalPages)
            {
                endPage = TotalPages;
                if (endPage > 5)
                {
                    startPage = endPage - 4;
                }
            }

            StartPage = Math.Max(1, startPage);
            EndPage = Math.Max(1, endPage);
        }

        public void SetPage(int page)
        {
            CurrentPage = page;
        }

        public void SetPageSize(int pageSize)
        {
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((decimal)TotalItems / (decimal)PageSize);
            if (CurrentPage > TotalPages && TotalPages > 0)
            {
                CurrentPage = TotalPages;
            }
        }
    }

    public class PaginatedList<T> : List<T>
    {
        private List<T> _source = new();
        public int TotalRecords { get; private set; }

        public PaginatedList(List<T> source, int pageIndex, int pageSize)
        {
            _source = source ?? new List<T>();
            TotalRecords = _source.Count;
            var items = _source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            AddRange(items);
        }

        public void ChangePage(int pageIndex, int pageSize)
        {
            Clear();
            var items = _source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            AddRange(items);
        }
    }
}


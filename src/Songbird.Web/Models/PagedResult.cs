using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Songbird.Web.Models {
    public class PagedResult<T> where T : class {
        public PagedResult() {
            Items = new ReadOnlyCollection<T>(Array.Empty<T>());
        }

        public PagedResult(IEnumerable<T> items, Int32 pageIndex, Int32 pageSize, Int32 total) {
            Items = new ReadOnlyCollection<T>(items.ToList());
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
        }

        public IReadOnlyCollection<T> Items { get; }
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 Total { get; set; }
    }
}

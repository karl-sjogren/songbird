using System;

namespace Songbird.Web.Queries {
    public abstract class SearchSettingsBase {
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; } = 50;
        public string SortField { get; set; }
        public bool SortAscending { get; set; }
    }
}

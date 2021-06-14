using System;

namespace Songbird.Web.DataObjects {
    public class FacetValue {
        public FacetValue(string name, string value, Int32 count) {
            Name = name;
            Value = value;
            Count = count;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public Int32 Count { get; }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Songbird.Web.DataObjects {
    public class Facet {
        public Facet(string name, string type, ICollection<FacetValue> values) {
            Name = name;
            Type = type;
            Items = new ReadOnlyCollection<FacetValue>(values.ToList());
        }

        public string Name { get; }
        public string Type { get; }
        public IReadOnlyCollection<FacetValue> Items { get; }
    }
}

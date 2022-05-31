using System;

namespace Songbird.Web.DataObjects;

public class CustomerDTO : DataObjectBase {
    public string Name { get; set; }
    public string ShortName { get; set; }
    public byte[] Timestamp { get; set; }

    public Guid? IconId { get; set; }
}

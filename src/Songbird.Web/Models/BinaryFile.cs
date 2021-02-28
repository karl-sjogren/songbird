namespace Songbird.Web.Models {
    public class BinaryFile : ModelBase {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string Checksum { get; set; }
    }
}

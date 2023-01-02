using Newtonsoft.Json;

namespace ReceiptParser.Entities
{
    public class ReceiptDetail
    {
        public string Locale { get; set; }
        public string Description { get; set; }
        public BoundingPoly BoundingPoly { get; set; }
    }
}

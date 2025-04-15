using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop.Models
{
    public class ProductImages
    {
        public int id { get; set; }
        public byte[] img { get; set; }
        public byte[] thumbnailimg { get; set; }
        public int productsid { get; set; }
        [ForeignKey("productsid")]
        public Products products { get; set; }
    }
}

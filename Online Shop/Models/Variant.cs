using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public string? size { get; set; }
        public string? color { get; set; }
        public int count { get; set; }
        public string category { get; set; }
        public int categoryId { get; set; }
        public int productsId { get; set; }
        [ForeignKey("productsId")]
        public Products products { get; set; }
   
    }
}

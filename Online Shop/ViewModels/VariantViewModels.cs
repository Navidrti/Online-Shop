using Online_Shop.Models;

namespace Online_Shop.ViewModels
{
    public class VariantViewModels
    {
        public int Id { get; set; }
        public string? size { get; set; }
        public string? color { get; set; }
        public int count { get; set; }
        public string category { get; set; }
        public int categoryId { get; set; }
        public int productsId { get; set; }
    }
}

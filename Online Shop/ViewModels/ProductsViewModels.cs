namespace Online_Shop.ViewModels
{
    public class ProductsViewModels
    {
        public int id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public string description { get; set; }
        public IFormFile? img { get; set; }
        public byte[]? imgBytes { get; set; }
        public List<IFormFile> image { get; set; }
      
    }
}

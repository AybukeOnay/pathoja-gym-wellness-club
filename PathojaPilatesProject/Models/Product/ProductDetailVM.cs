namespace PathojaPilatesProject.Models.Product
{
    public class ProductDetailVM
    {
        public int CategoryId { get; set; }
        public int? ProductId { get; set; }
        public string? CategoryName { get; set; }
        public string? FullName { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool KvkkApproved { get; set; }

    }
}

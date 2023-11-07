using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmazonWebAppMVC.Models
{
    [Table("Products")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public float Price { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        [NotMapped]
        public IFormFile Image { get; set; } = default!;

        public int CategoryId { get; set; }
        public virtual Category Categorie { get; set; }

        public Product()
        {
        }
    }
}

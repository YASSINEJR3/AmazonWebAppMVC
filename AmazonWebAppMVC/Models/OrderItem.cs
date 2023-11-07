using System.ComponentModel.DataAnnotations;

namespace AmazonWebAppMVC.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int QuantityOrdered { get; set; }
        public float Price { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }

        public OrderItem()
        {
        }

    }
}

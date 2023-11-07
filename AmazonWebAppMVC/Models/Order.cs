using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazonWebAppMVC.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        [NotMapped]
        public int Total { get; set; }
        public virtual List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public Order()
        {
        }

        public void AddItem(int productId, float price,int quantity = 1)
        {
            var existingItem = OrderItems.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.QuantityOrdered += quantity;
            }
            else
            {
                OrderItems.Add(new OrderItem
                {
                    ProductId = productId,
                    QuantityOrdered = quantity,
                    Price = price
                });
            }
        }
    }
}
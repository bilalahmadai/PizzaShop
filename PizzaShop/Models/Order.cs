using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public OrderState Status { get; set; }
        public DateTime LastUpdated { get; set; }

        // Book
        [ForeignKey("Pizzas")]
        public int PizzaId { get; set; }
        public virtual Pizza Pizzas { get; set; }
        // User
        [ForeignKey("Users")]
        public string UserId { get; set; }
        public virtual IdentityUser Users { get; set; }
    }
    public enum OrderState
    {
        InCart,
        OrderPlaced,
        Verifying,
        Inprocess,
        Delivered
    }
}

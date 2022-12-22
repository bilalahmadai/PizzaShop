using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaShop.Models
{
    public class ToppingPizza
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("Pizzas")]
        public int PizzaId { get; set; }
        public Pizza  Pizzas { get; set; }

        [ForeignKey("Toppings")]
        public int ToppingsId { get; set; }
        public Topping Toppings { get; set; }
    }
}

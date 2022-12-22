using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PizzaShop.Models
{
    public class Topping
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        

        // Pizzas
        public List<Pizza> Pizzas { get; set; }
    }
}

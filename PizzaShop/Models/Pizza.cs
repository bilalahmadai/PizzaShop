using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaShop.Models
{
    public class Pizza
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public int Price { get; set; }
        

        // Chef
        [ForeignKey("Chefs")]
        public int ChefId { get; set; }
        public virtual Chef Chefs { get; set; }

        // Topping
        public List<Topping> Toppings { get; set; }
        public List<Order> Orders { get; set; }
    }
}

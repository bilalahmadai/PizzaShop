﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Models
{
    public class Chef
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
 
        public string Address { get; set; }
        public string URL { get; set; }

        // pizzas
        public List<Pizza> Pizzas { get; set; }
    }
}

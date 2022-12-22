using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Models;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace PizzaShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
         }
        public DbSet<Pizza> Pizza { get; set; }
        public DbSet<Topping> Topping { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<ToppingPizza> ToppingPizza { get; set; }
        public DbSet<Chef> Chef { get; set; }
    }
}

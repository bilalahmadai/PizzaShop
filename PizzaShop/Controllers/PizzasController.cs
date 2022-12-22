using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;
using PizzaShop.Models;


/*
@foreach(var chef in ViewData["Chefs"] as List<Chef>)
                    {
    if (chef.Id == item.Id)
    {

                            < img style = "width:100px;" src = "@chef.URL" alt = "@chef.Name" />
                        }
}*/


namespace PizzaShop.Controllers
{
    public class PizzasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PizzasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pizzas
        public IActionResult SearchPizza()
        {
            return View();
        }
        public async Task<IActionResult> SearchResult(string Name)
        {
           
            // show index view  and Use Fileter Where(a=>a.Name.Contains(Name))
            return View("Index", await _context.Pizza.Where(a => a.Title.Contains(Name)).ToListAsync());
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Chefs"] = await _context.Chef.ToListAsync<Chef>();
            ViewData["Toppings"] = await _context.Topping.ToListAsync();
            ViewData["ToppingPizza"] = await _context.ToppingPizza.ToListAsync();
            var applicationDbContext = _context.Pizza.Include(p => p.Chefs);
            return View(await applicationDbContext.ToListAsync());
        }



        public async Task<IActionResult> PizzaDetails(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            ViewData["ToppingPizza"] = await _context.ToppingPizza.ToListAsync();
            ViewData["Toppings"] = await _context.Topping.ToListAsync();
            var applicationDbContext = _context.Pizza.Include(b => b.Chefs).Where(b => b.Id == Id);
            return View("Index", await applicationDbContext.ToListAsync());
        }
        // GET: Pizzas/Details/5
        /* public async Task<IActionResult> Details(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var pizza = await _context.Pizza
                 .Include(p => p.Chefs)
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (pizza == null)
             {
                 return NotFound();
             }

             return View(pizza);
         }*/

        // GET: Pizzas/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ChefId"] = new SelectList(_context.Chef, "Id", "Name");
            ViewData["ToppingsId"] = new SelectList(_context.Topping, "Id", "Name");

            return View();
        }

        // POST: Pizzas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,URL,Price,ChefId")] Pizza pizza, List<int> Toppings)
        {
            if (ModelState.IsValid)
            {
               // add in pizza tbl
                _context.Add(pizza);
                await _context.SaveChangesAsync();
                //add in toppingPizaa tbl
                List<ToppingPizza> toppingPizza = new List<ToppingPizza>();
                foreach (int topping in Toppings)
                {
                    toppingPizza.Add(new ToppingPizza { ToppingsId = topping, PizzaId = pizza.Id });
                }
                _context.ToppingPizza.AddRange(toppingPizza);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ChefId"] = new SelectList(_context.Chef, "Id", "Id", pizza.ChefId);
            return View(pizza);
        }

        // GET: Pizzas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizza.FindAsync(id);
            if (pizza == null)
            {
                return NotFound();
            }
            IList<ToppingPizza> toppingPizzas = await _context.ToppingPizza.Where<ToppingPizza>(a => a.PizzaId == pizza.Id).ToListAsync();
            IList<int> listToppings = new List<int>();
            foreach (ToppingPizza toppingPizza in toppingPizzas)
            {
                listToppings.Add(toppingPizza.ToppingsId);
            }

            ViewData["ToppingsId"] = new MultiSelectList(_context.Topping, "Id", "Name", listToppings.ToArray());
            ViewData["ChefId"] = new SelectList(_context.Chef, "Id", "Name", pizza.ChefId);
            return View(pizza);
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,URL,Price,ChefId")] Pizza pizza, List<int> Toppings)
        {
            if (id != pizza.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizza);
                    _context.SaveChanges();
                    List<ToppingPizza> toppingPizza = new List<ToppingPizza>();
                    foreach (int topping in Toppings)
                    {
                        toppingPizza.Add(new ToppingPizza { ToppingsId = topping, PizzaId = pizza.Id });
                    }
                    List<ToppingPizza> deleteToppingPizzas = await _context.ToppingPizza.Where<ToppingPizza>(a => a.PizzaId == pizza.Id).ToListAsync();
                    _context.ToppingPizza.RemoveRange(deleteToppingPizzas);
                    _context.SaveChanges();

                    _context.ToppingPizza.UpdateRange(toppingPizza);
                    _context.SaveChanges();

                    //await transaction.CommitAsync();
                    //_context.Update(pizza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaExists(pizza.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChefId"] = new SelectList(_context.Chef, "Id", "Name", pizza.ChefId);
            return View(pizza);
        }

        // GET: Pizzas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _context.Pizza
                .Include(p => p.Chefs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizza = await _context.Pizza.FindAsync(id);
            _context.Pizza.Remove(pizza);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaExists(int id)
        {
            return _context.Pizza.Any(e => e.Id == id);
        }
    }
}

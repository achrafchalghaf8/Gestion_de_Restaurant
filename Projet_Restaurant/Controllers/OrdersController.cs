using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_Restaurant.Data;
using Projet_Restaurant.DTO;
using Projet_Restaurant.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Restaurant.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Article)  // Inclure les articles associés
                .ToListAsync();

            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Article)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        // GET: Orders/Create
        public IActionResult Create()
        {
            var orderDTO = new OrderDTO();
            return View(orderDTO);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDTO orderDTO)
        {
            if (ModelState.IsValid)
            {
                // Mapper le DTO vers l'entité
                var order = new Order
                {
                    OrderDate = orderDTO.OrderDate,
                    TotalPrice = orderDTO.TotalPrice,
                    
                };

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(orderDTO);
        }


        // GET: Orders/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // If the order object is not null, proceed with creating DTO
            var orderDTO = new OrderDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                // Add additional fields here if necessary
            };

            // Check if related collections (like OrderItems) are null or empty
          

            return View(orderDTO);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDTO orderDTO)
        {
            if (id != orderDTO.OrderId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mapping the DTO back to entity
                    var order = new Order
                    {
                        OrderId = orderDTO.OrderId,
                        OrderDate = orderDTO.OrderDate,
                        TotalPrice = orderDTO.TotalPrice,
                        // Map additional fields
                    };

                  

                    _context.Update(order);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency issues
                    throw;
                }
            }

            return View(orderDTO);
        }


        // GET: Orders/Delete/5
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Article)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // Mapper vers DTO si nécessaire
            var orderDTO = new OrderDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
                {
                    // Propriétés de OrderDetailDTO
                }).ToList()
            };

            return View(orderDTO); // Passez le DTO ou le modèle approprié
        }
        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails) // Inclure les relations si nécessaire
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            // Si vous avez une relation avec suppression en cascade, la suppression de l'ordre suffira.
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}

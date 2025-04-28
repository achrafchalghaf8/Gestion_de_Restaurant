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
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            return await _context.Details.Include(od => od.Order).Include(od => od.Article).ToListAsync();
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> Details()
        {
            var details = await _context.Details.Include(od => od.Article).ToListAsync();

            var dto = details.Select(od => new OrderDetailDTO
            {
                OrderDetailId = od.OrderDetailId,
                ArticleId = od.Article.ArticleId
            });

            return Ok(dto);
        }

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
            // Vérification si la commande existe
            var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == orderDetail.OrderId);
            if (!orderExists)
            {
                return BadRequest("La commande spécifiée n'existe pas.");
            }

            // Vérification si l'article existe
            var articleExists = await _context.Articles.AnyAsync(a => a.ArticleId == orderDetail.ArticleId);
            if (!articleExists)
            {
                return BadRequest("L'article spécifié n'existe pas.");
            }

            _context.Details.Add(orderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDetails", new { id = orderDetail.OrderDetailId }, orderDetail);
        }


        // PUT: api/OrderDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderDetailId)
            {
                return BadRequest("L'ID du détail de commande ne correspond pas.");
            }

            // Vérification si la commande existe
            var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == orderDetail.OrderId);
            if (!orderExists)
            {
                return BadRequest("La commande spécifiée n'existe pas.");
            }

            // Vérification si l'article existe
            var articleExists = await _context.Articles.AnyAsync(a => a.ArticleId == orderDetail.ArticleId);
            if (!articleExists)
            {
                return BadRequest("L'article spécifié n'existe pas.");
            }

            var existingOrderDetail = await _context.Details.FindAsync(id);
            if (existingOrderDetail == null)
            {
                return NotFound("Le détail de commande spécifié n'existe pas.");
            }

            // Mise à jour des propriétés
            existingOrderDetail.ArticleId = orderDetail.ArticleId;
            existingOrderDetail.Quantity = orderDetail.Quantity;
            existingOrderDetail.Price = orderDetail.Price;
            existingOrderDetail.OrderId = orderDetail.OrderId;

            _context.Entry(existingOrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }


        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.Details.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound("Le détail de commande spécifié n'existe pas.");
            }

            try
            {
                _context.Details.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erreur lors de la suppression. Vérifiez les dépendances : {ex.Message}");
            }

            return NoContent();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_Restaurant.Data;
using Projet_Restaurant.DTO;
using Projet_Restaurant.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Restaurant.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // Afficher la liste des catégories dans une vue Razor
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();

            // Mapper les entités Category à CategoryDTO
            var categoryDtos = categories.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToList();

            return View(categoryDtos); // Passer les DTO de catégorie à la vue
        }



        // Afficher les détails d'une catégorie dans une vue Razor
        [HttpGet("Categories/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories.Include(c => c.Articles).FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
                return NotFound();

            // Mapper l'entité Category à CategoryDTO
            var categoryDto = new CategoryDTO
            {
                Name = category.Name
            };

            // Retourner la vue Razor avec les détails de la catégorie
            return View(categoryDto);
        }

        // GET: Create Category
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            // Vérifier si une catégorie avec le même nom existe (insensible à la casse)
            if (await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Une catégorie avec ce nom existe déjà.");
                return View(categoryDto);
            }

            // Créer et sauvegarder la catégorie
            var category = new Category
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Afficher le formulaire pour modifier une catégorie
        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            // Mapper le modèle vers le DTO
            var categoryDto = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };

            return View(categoryDto); // Passer le bon modèle à la vue
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
                return View(categoryDto);

            // Recherche de la catégorie existante
            var existingCategory = await _context.Categories.FindAsync(categoryDto.CategoryId);
            if (existingCategory == null)
                return NotFound();

            // Mise à jour des valeurs
            existingCategory.Name = categoryDto.Name;

            // Sauvegarde dans la base de données
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Erreur lors de la mise à jour de la catégorie.");
                return View(categoryDto);
            }

            return RedirectToAction("Index");
        }


        // Supprimer une catégorie
        // GET : Formulaire de confirmation de suppression
        [HttpGet("Categories/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category); // Passez l'entité directement à la vue
        }

        // POST : Effectuer la suppression d'une catégorie
        [HttpPost("Categories/Delete/{id:int}")]
        [ActionName("Delete")] // Cette annotation permet de faire correspondre l'action avec le formulaire POST
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

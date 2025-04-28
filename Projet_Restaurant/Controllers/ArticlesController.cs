using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Restaurant.Data;
using Projet_Restaurant.DTO;
using Projet_Restaurant.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Restaurant.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        // Afficher la liste des articles dans une vue Razor
        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                                         .Include(a => a.Category) // Inclut les informations de la catégorie
                                         .Select(a => new ArticleDTO
                                         {
                                             ArticleID = a.ArticleId,
                                             Name = a.Name,
                                             Price = a.Price,
                                             Description = a.Description,
                                             ImageUrl = a.ImageUrl,
                                             CategoryId = a.CategoryId,      // Utilisé pour gérer les relations
                                             CategoryName = a.Category.Name // Nom de la catégorie
                                         })
                                         .ToListAsync();

            return View(articles);
        }


        [HttpGet("Articles/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _context.Articles
                                        .Include(a => a.Category)
                                        .FirstOrDefaultAsync(a => a.ArticleId == id);

            if (article == null)
                return NotFound();

            // Mapper Article vers ArticleDTO
            var articleDto = new ArticleDTO
            {
                Name = article.Name,
                Price = article.Price,
                Description = article.Description,
                ImageUrl = article.ImageUrl,
                ArticleID = article.ArticleId,
                CategoryName = article.Category?.Name ?? "No Category"
            };

            return View(articleDto); // Passer un ArticleDTO à la vue
        }
        // GET: Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View(new ArticleDTO());
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleDTO articleDto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(articleDto);
            }

            // Créer un nouvel article à partir du DTO
            var article = new Article
            {
                Name = articleDto.Name,
                Price = articleDto.Price,
                Description = articleDto.Description,
                ImageUrl = articleDto.ImageUrl,
                CategoryId = articleDto.CategoryId
            };

            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Erreur lors de la création de l'article.");
                await LoadCategoriesAsync();
                return View(articleDto);
            }
        }

        // Charger les catégories
        private async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories
                                           .Select(c => new { c.CategoryId, c.Name })
                                           .ToListAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _context.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            // Charger les catégories pour le dropdown
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");

            var articleDto = new ArticleDTO
            {
                ArticleID = article.ArticleId,
                Name = article.Name,
                Price = article.Price,
                Description = article.Description,
                ImageUrl = article.ImageUrl,
                CategoryId = article.CategoryId
            };

            return View(articleDto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleDTO articleDto)
        {
            if (!ModelState.IsValid)
            {
                // Recharger les catégories si la validation échoue
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "Name");
                return View(articleDto);
            }

            var article = await _context.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
                return NotFound();

            article.Name = articleDto.Name;
            article.Price = articleDto.Price;
            article.Description = articleDto.Description;
            article.ImageUrl = articleDto.ImageUrl;
            article.CategoryId = articleDto.CategoryId; // Mettre à jour l'ID de la catégorie

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET : Afficher la confirmation pour supprimer un article
        [HttpGet("Articles/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleId == id);
            if (article == null)
                return NotFound();

            // Mapper l'article vers ArticleDTO
            var articleDTO = new ArticleDTO
            {
                ArticleID = article.ArticleId,
                Name = article.Name,
                Price = article.Price,
                Description = article.Description,
                ImageUrl = article.ImageUrl,
                CategoryId = article.CategoryId,
                CategoryName = article.Category?.Name // Assurez-vous que la catégorie est incluse
            };

            return View(articleDTO);
        }
        [HttpPost("Articles/Delete/{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}

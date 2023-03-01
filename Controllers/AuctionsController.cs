using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_project.Data;
using web_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace web_project.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AuctionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Auctions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Auction.ToListAsync());
        }

        // GET: Auctions/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Auction == null)
            {
                return NotFound();
            }

            var auction = await _context.Auction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // GET: Auctions/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auctions/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ImageUrl,StartingPrice,EndDate,Category,Condition")] Auction auction)
        {
            ModelState.Clear();

            // Set the start date
            auction.StartDate = DateTime.Now;

            // Set the user id
            auction.UserId = _userManager.GetUserId(User);

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                _context.Add(auction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auction);
        }


        // GET: Auctions/Edit
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auction.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,ImageUrl,StartingPrice,EndDate,Category,Condition")] Auction auction)
        {
            if (id != auction.Id)
            {
                return NotFound();
            }

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(auction.Id))
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
            return View(auction);
        }

        // GET: Auctions/Delete
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // POST: Auctions/Delete
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auction = await _context.Auction.FindAsync(id);
            _context.Auction.Remove(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Auction Search
        private bool AuctionExists(int id)
        {
            return _context.Auction.Any(e => e.Id == id);
        }

        // GET: Auctions/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: Auctions/SearchResults
        [HttpPost]
        public IActionResult SearchResults(string SearchQuery)
        {
            var auctions = _context.Auction.Where(a => a.Name.Contains(SearchQuery) || a.Description.Contains(SearchQuery)).ToList();
            return View("Index", auctions);
        }

        // GET: Auctions/MyAuctions
        [Authorize]
        public IActionResult MyAuctions()
        {
            var auctions = _context.Auction.Where(a => a.UserId == _userManager.GetUserId(User)).ToList();
            return View("Index", auctions);
        }
    }
}

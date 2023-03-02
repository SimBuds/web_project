using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_project.Data;
using web_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        public async Task<IActionResult> Create([Bind("Name,Description,ImageUrl,StartingPrice,EndDate,Category,Condition,UserId")] Auction auction)
        {
            auction.StartDate = DateTime.Now;
            auction.UserId = _userManager.GetUserId(User);
            
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = _context.Auction.Find(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,StartingPrice,StartDate,EndDate,Category,Condition,UserId")] Auction auction)
        {
            if (id != auction.Id)
            {
                return NotFound();
            }
            
            auction.UserId = _userManager.GetUserId(User);

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

        // Built-in Search
        private bool AuctionExists(int id)
        {
            return _context.Auction.Any(e => e.Id == id);
        }

        // POST: Auctions/SearchBar
        [HttpPost]
        public async Task<IActionResult> SearchBar(string SearchString)
        {
            var auctions = from m in _context.Auction
                           select m;

            if (!String.IsNullOrEmpty(SearchString))
            {
                auctions = auctions.Where(s => s.Name.Contains(SearchString));
            }

            return View("Index", await auctions.ToListAsync());
        }

        // POST: Auctions/Category
        // Display Specific Categories
        [HttpPost]
        public async Task<IActionResult> CategorySort(int CategorySelection)
        {
            var auctions = from m in _context.Auction
                           select m;

            if (CategorySelection == 0)
            {
                auctions = auctions.Where(s => s.Category == Category.Electronics);
            }
            else if (CategorySelection == 1)
            {
                auctions = auctions.Where(s => s.Category == Category.Fashion);
            }
            else if (CategorySelection == 2)
            {
                auctions = auctions.Where(s => s.Category == Category.Home);
            }
            else if (CategorySelection == 3)
            {
                auctions = auctions.Where(s => s.Category == Category.Sports);
            }
            else if (CategorySelection == 4)
            {
                auctions = auctions.Where(s => s.Category == Category.Books);
            }
            else if (CategorySelection == 5)
            {
                auctions = auctions.Where(s => s.Category == Category.Other);
            }

            return View("Index", await auctions.ToListAsync());
        }

        // POST: Auctions/Condition
        // Display Specific Conditions
        [HttpPost]
        public async Task<IActionResult> ConditionSort(int ConditionSelection)
        {
            var auctions = from m in _context.Auction
                           select m;

            if (ConditionSelection == 0)
            {
                auctions = auctions.Where(s => s.Condition == Condition.New);

            }
            else
            {
                auctions = auctions.Where(s => s.Condition == Condition.Used);
            }

            return View("Index", await auctions.ToListAsync());
        }

        // POST: Auctions/Price
        // Display Specific Price Ranges high to low
        [HttpPost]
        public async Task<IActionResult> PriceSort(int PriceSelection)
        {
            var auctions = from m in _context.Auction
                           select m;

            if (PriceSelection == 1)
            {
                auctions = auctions.OrderByDescending(s => s.StartingPrice);
            }
            else
            {
                auctions = auctions.OrderBy(s => s.StartingPrice);
            }

            return View("Index", await auctions.ToListAsync());
        }
    }
}

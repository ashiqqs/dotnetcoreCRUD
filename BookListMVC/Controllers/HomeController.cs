using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using BookListMVC.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [BindProperty]
        public Book Book { get; set; }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if (id == null)
            {
                return View(Book);
            }
            Book =  _db.Books.Find(id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    _db.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                 _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }

        #region API calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book is null)
            {
                return Json(new { success = false, message = "Book not found" });
            }
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successfull" });
        }
        #endregion
    }
}
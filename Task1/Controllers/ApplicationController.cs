using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Task1.Data;
using Task1.Models;
using System.IO;

namespace Task1.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Application obj)
        {
            obj.SubmissionTimestamp = DateTime.Now;

            if (obj.PdfFile != null && obj.PdfFile.Length > 0)
            {
                string originalFileName = Path.GetFileName(obj.PdfFile.FileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
                string fileExtension = Path.GetExtension(originalFileName);

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string uniqueFileName = $"{fileNameWithoutExtension}_{timestamp}{fileExtension}";

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await obj.PdfFile.CopyToAsync(stream);
                }

                obj.FileName = uniqueFileName;
                _db.Application.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("PdfFile", "Please choose a PDF file.");
                return View(obj);
            }
        }


    }
}

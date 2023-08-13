using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Task1.Data;
using Task1.Models;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Task1.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public ApplicationController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
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

                // Check the email using the stored procedure
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("CheckEmailForApplication", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@EmailAddress", obj.Email);
                        SqlParameter canApplyParameter = command.Parameters.Add("@CanApply", SqlDbType.Bit);
                        canApplyParameter.Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        bool canApply = (bool)canApplyParameter.Value;
                        if (!canApply)
                        {
                            ModelState.AddModelError("", "You can't apply within the same address in less than 24 hours.");
                            Console.WriteLine("Email validation: Can't apply within 24 hours");
                            return View(obj);
                        }
                        else
                        {
                            _db.Application.Add(obj);
                            _db.SaveChanges();
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            // Return a default view if the file condition is not met
            ModelState.AddModelError("", "Please choose a PDF file.");
            return View(obj);
        }

    }
}


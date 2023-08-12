using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Task1.Models;
using Task1.Data;
using System;
using System.Net.Mail;
using System.Globalization;
using Newtonsoft.Json;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Task1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

    
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Admin model)
        {
            if (ModelState.IsValid)
            {
                var admin = _db.Admin.FirstOrDefault(a => a.Username == model.Username);
                if (admin != null && model.PasswordHash == admin.PasswordHash)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username),
            };

                    var identity = new ClaimsIdentity(claims, "LoginAuthentication");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("LoginAuthentication", principal);

                    

                    return RedirectToAction("ViewApplications", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials.");
                }
            }

            return View(model);
        }



        [Authorize]
        public IActionResult ViewApplications()
        {
            var applications = _db.Application.ToList();
            return View(applications);
        }



        [HttpPost]
        public IActionResult ApproveAndRedirect(int id, DateTime? scheduledDateTime)
        {
            var application = _db.Application.Find(id);
            if (application != null)
            {
                application.ApplicationStatus = true;
                application.ActionDate = DateTime.Now;
                _db.SaveChanges();

                if (scheduledDateTime.HasValue)
                {
                    SendEmailToApplicant(application.Email, scheduledDateTime.Value, application);

                    TempData["EmailSent"] = "Email sent successfully!";
                }

                return RedirectToAction("SendEmail", new { id });
            }
            return RedirectToAction("SendEmail", new { id });
        }


        private void SendEmailToApplicant(string recipientEmail, DateTime scheduledDateTime, Task1.Models.Application application)
        {
            // Configure MIME message

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("USAL", "your@email.com")); // Sender's information
            message.To.Add(new MailboxAddress("", recipientEmail)); // Recipient's email
            message.Subject = "Application Approved";

            // Customize email body
            var emailBody = $"Dear {application.FullName},\n\n" +
                            "We are pleased to inform you that your application has been reviewed and accepted by our admissions committee.\n" +
                            $"You are invited to come to our campus on {scheduledDateTime:dd/MM/yyyy} at {scheduledDateTime:HH:mm}.\n\n" +
                            "Best regards,\n" +
                            "University of Science & Arts";

            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                // Connect to Gmail's SMTP server on port 587 with STARTTLS
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Authenticate with your Gmail app password
                client.Authenticate("aliridasiblani313@gmail.com", "retyhvftpcnovosi");

                // Send the email
                client.Send(message);

                // Disconnect from the server
                client.Disconnect(true);
            }


        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        public IActionResult ViewDetails(int id)
        {
            var application = _db.Application.FirstOrDefault(a => a.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }
        [Authorize]
        public IActionResult SendEmail(int id)
        {
            var application = _db.Application.FirstOrDefault(a => a.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }
    }
}

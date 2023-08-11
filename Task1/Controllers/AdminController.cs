using Microsoft.AspNetCore.Mvc;
using Task1.Models;
using Task1.Data;
using System;
using System.Net.Mail;  // Add this line
using System.Globalization;
using Newtonsoft.Json;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using static System.Net.Mime.MediaTypeNames;

namespace Task1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Admin model)
        {
            if (ModelState.IsValid)
            {
                if (IsValidCredentials(model.Username, model.PasswordHash))
                {
                    return RedirectToAction("ViewApplications", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials.");
                }
            }

            return View(model);
        }

        private bool IsValidCredentials(string username, string password)
        {
            var admin = _db.Admin.FirstOrDefault(a => a.Username == username);
            return admin != null && admin.PasswordHash == password;
        }

        public IActionResult ViewApplications()
        {
            var applications = _db.Application.ToList();
            return View(applications);
        }



        [HttpPost]
        public IActionResult ApproveAndRedirect(int id, DateTime scheduledDateTime)
        {
            var application = _db.Application.Find(id);
            if (application != null)
            {
                application.ApplicationStatus = true;
                application.ActionDate = DateTime.Now;
                _db.SaveChanges();

               
                SendEmailToApplicant(application.Email, scheduledDateTime, application);

                TempData["EmailSent"] = "Email sent successfully!";

                return RedirectToAction("SendEmail", new { id });
            }
            return RedirectToAction("ViewDetails", new { id });
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





        public IActionResult Disapprove(int id)
        {
            var application = _db.Application.Find(id);
            if (application != null)
            {
                application.ApplicationStatus = false;
                application.ActionDate = DateTime.Now;
                _db.SaveChanges();
            }
            return RedirectToAction("ViewApplications", new { id });
        }
        public IActionResult SendEmail(int id)
        {
            var application = _db.Application.FirstOrDefault(a => a.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

           
            return View(application);
        }

        public IActionResult ViewDetails(int id)
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

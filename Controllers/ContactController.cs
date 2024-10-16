﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SakwithiWebApp.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace SakwithiWebApp.Controllers
{

    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        private readonly IConfiguration _configuration;
        public ContactController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await SendEmailAsync(model);
                    TempData["SubmissionSuccessful"] = true;
                    return RedirectToAction("ThankYou", model);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "An error occurred while sending the email.");
                    Console.WriteLine($"An error occurred while sending the email. Please try again later.{ex.Message}");
                }
            }
            return View("Index", model);
        }

        public IActionResult ThankYou(ContactModel model)
        {
            if (TempData["SubmissionSuccessful"] == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        private async Task SendEmailAsync(ContactModel model)
        {
            var smtpServer = _configuration["SmtpSettings:Server"];
            var smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
            var smtpUsername = _configuration["SmtpSettings:Username"];
            var smtpPassword = _configuration["SmtpSettings:Password"];
            var toEmail = _configuration["SmtpSettings:ToEmail"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = "New Contact Form Submission",
                    Body = $"Name: {model.FirstName} {model.LastName}\nEmail: {model.Email}\nMessage: {model.Message}",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}

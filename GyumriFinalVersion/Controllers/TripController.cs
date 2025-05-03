using Gyumri.Application.Interfaces;
using Gyumri.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using DinkToPdf.Contracts;
using DinkToPdf;

namespace GyumriFinalVersion.Controllers
{
    public class TripController : Controller
    {
        private const string CultureCookieName = "UserCulture";
        private readonly ApplicationContext _context;
        private readonly ICategory _categoryService;
        private readonly IConverter _converter;

        public TripController(ICategory categoryService, ApplicationContext context)
        {
            _categoryService = categoryService;
            _context = context;        
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> FirstStep()
        {

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> SecondStep(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> ThirdStep(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> ForthStep(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }
        public async Task<IActionResult> Final(int categoryId)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();

            var currentCulture = Request.Cookies["UserCulture"] ?? "en";
            var cultureInfo = new System.Globalization.CultureInfo(currentCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View();
        }

        // ______________________Send email ____________________
        private readonly string fromEmail = "inesamkrtchyan9999@gmail.com";
        private readonly string password = "lhwd zttk xque sajp";

        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(fromEmail, "Your Name");
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        [HttpPost]
        public IActionResult SendWelcomeEmail(string email)
        {
            var subject = AppRes.WelcomeGyumri;
            var body = $"<h1>{AppRes.TripPlan}</h1>" +
                $"<h3>Այստեղ կարող է լինել ձեր գովազդը :D </h3>";

            SendEmail(email, subject, body);

            return RedirectToAction("Index", "Home");
        }

        // ______________________Send email ____________________

        //________________Download PDF____________________

        //public IActionResult DownloadTripPlanPdf()
        //{
        //    var body = $"<h1>Ճանապարհորդության պլան</h1>" +
        //               $"<h3>Այստեղ կարող է լինել ձեր գովազդը :D </h3>";

        //    var doc = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = new GlobalSettings
        //        {
        //            PaperSize = PaperKind.A4,
        //            Orientation = Orientation.Portrait,
        //        },
        //        Objects = {
        //        new ObjectSettings
        //        {
        //            HtmlContent = body,
        //            WebSettings = { DefaultEncoding = "utf-8" }
        //        }
        //    }
        //    };

        //    var pdf = _converter.Convert(doc);

        //    return File(pdf, "application/pdf", "TripPlan.pdf");
        //}

        //________________Download PDF____________________
    }
}

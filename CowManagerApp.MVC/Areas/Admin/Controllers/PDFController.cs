using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using CowManager.Models.Models;
using CowManager.Data;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;



namespace CowManagerApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PdfController(IConverter converter) : Controller
    {
        private readonly IConverter _converter = converter;

        public IActionResult GeneratePDF()
        {
            var htmlContent = "<h1>Witaj w PDF</h1><p>To jest testowy dokument PDF wygenerowany w ASP.NET MVC</p>";

            var pdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
                Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                }
            }
            };

            var file = _converter.Convert(pdfDocument);
            return File(file, "application/pdf", "testowy.pdf");
        }
    }
}

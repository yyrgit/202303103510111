using Microsoft.AspNetCore.Mvc;
using SeoOptimizerTool.Services;
using System;
using System.Threading.Tasks;

namespace SeoOptimizerTool.Controllers
{
    public class SeoController : Controller
    {
        private readonly SeoService _seoService;

        public SeoController(SeoService seoService)
        {
            _seoService = seoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Analyze(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                ModelState.AddModelError("", "Please enter a valid URL (including http:// or https://).");
                return View("Index");
            }

            var result = await _seoService.AnalyzeUrlAsync(url);
            return View("Result", result);
        }
    }
}

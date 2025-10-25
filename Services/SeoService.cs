using HtmlAgilityPack;
using SeoOptimizerTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeoOptimizerTool.Services
{
    public class SeoService
    {
        private readonly HttpClient _http;

        public SeoService()
        {
            _http = new HttpClient();
        }

        public async Task<SeoResult> AnalyzeUrlAsync(string url)
        {
            var result = new SeoResult { Url = url };

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; SeoOptimizerTool/1.0)");

                var response = await _http.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Basic SEO tags
                result.Title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText.Trim() ?? "";
                result.MetaDescription = doc.DocumentNode.SelectSingleNode("//meta[@name='description']")?.GetAttributeValue("content", "").Trim() ?? "";
                result.MetaKeywords = doc.DocumentNode.SelectSingleNode("//meta[@name='keywords']")?.GetAttributeValue("content", "").Trim() ?? "";

                result.HeadingCountH1 = doc.DocumentNode.SelectNodes("//h1")?.Count ?? 0;
                result.HeadingCountH2 = doc.DocumentNode.SelectNodes("//h2")?.Count ?? 0;

                var bodyText = doc.DocumentNode.SelectSingleNode("//body")?.InnerText ?? "";
                result.WordCount = bodyText.Split(new char[] {' ', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Length;

                // Canonical URL
                result.CanonicalUrl = doc.DocumentNode.SelectSingleNode("//link[@rel='canonical']")?.GetAttributeValue("href", "") ?? "";

                // Robots meta tag
                result.RobotsMeta = doc.DocumentNode.SelectSingleNode("//meta[@name='robots']")?.GetAttributeValue("content", "") ?? "";

                // Images alt attribute count
                var images = doc.DocumentNode.SelectNodes("//img");
                int missingAlt = 0;
                if (images != null)
                {
                    missingAlt = images.Count(img => string.IsNullOrEmpty(img.GetAttributeValue("alt", "").Trim()));
                }
                result.ImagesMissingAlt = missingAlt;

                // Links count (internal vs external)
                var linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
                int internalLinks = 0, externalLinks = 0;
                if(linkNodes != null)
                {
                    Uri baseUri = new Uri(url);
                    foreach (var link in linkNodes)
                    {
                        var href = link.GetAttributeValue("href", "");
                        if (string.IsNullOrEmpty(href))
                            continue;

                        // Normalize relative URLs
                        if (Uri.TryCreate(baseUri, href, out Uri linkUri))
                        {
                            if (linkUri.Host == baseUri.Host)
                                internalLinks++;
                            else
                                externalLinks++;
                        }
                    }
                }
                result.InternalLinksCount = internalLinks;
                result.ExternalLinksCount = externalLinks;

                // Warnings
                if (string.IsNullOrEmpty(result.Title) || result.Title.Length < 10)
                    result.Warnings.Add("Title is too short or missing.");

                if (string.IsNullOrEmpty(result.MetaDescription) || result.MetaDescription.Length < 50)
                    result.Warnings.Add("Meta description is too short or missing.");

                if (result.HeadingCountH1 == 0)
                    result.Warnings.Add("Page has no H1 headers.");

                if (string.IsNullOrEmpty(result.CanonicalUrl))
                    result.Warnings.Add("Canonical URL tag is missing.");

                if (result.RobotsMeta.Contains("noindex"))
                    result.Warnings.Add("Page uses 'noindex' in robots meta tag, page will not be indexed.");

                if (result.ImagesMissingAlt > 0)
                    result.Warnings.Add($"{result.ImagesMissingAlt} images are missing ALT attributes.");
            }
            catch (HttpRequestException ex)
            {
                result.Warnings.Add("Could not fetch the URL content: " + ex.Message);
            }
            catch (Exception ex)
            {
                result.Warnings.Add("An error occurred: " + ex.Message);
            }

            return result;
        }
    }
}

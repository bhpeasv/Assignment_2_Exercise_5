using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Exercise_5
{
    class WebCrawler
    {

        private Queue<Uri> frontier;
        private Dictionary<string, bool> visitedUrls;
        private int maxLinks;

        private WebClient wc;

        public WebCrawler(Queue<Uri> initialUrls, int maxLinksToFind)
        {
            frontier = initialUrls;
            visitedUrls = new Dictionary<string, bool>();
            maxLinks = maxLinksToFind;

            wc = new WebClient();
            crawl();
        }

        private void crawl()
        {
            while (frontier.Count > 0 && visitedUrls.Count < maxLinks)
            {
                Uri url = frontier.Dequeue();
                if (!visitedUrls.ContainsKey(url.ToString()))
                {
                    resolvePage(url);
                }
            }
        }


        private void resolvePage(Uri page)
        {
            string urlStr = page.ToString();

            visitedUrls[urlStr] = true;
            Console.WriteLine(visitedUrls.Count);
            try
            {
                // try to download the webpage. Throws exception if the url is bad.
                string webPage = wc.DownloadString(urlStr);

                // look for links in the webpage
                var hrefPattern = new Regex("href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|(?<url>\\S+))", RegexOptions.IgnoreCase);

                var urls = hrefPattern.Matches(webPage);

                Uri baseUrl = new UriBuilder(page.Host).Uri;
                foreach (Match url in urls)
                {
                    string newUrl = url.Groups["url"].Value.ToLower();

                    Uri absoluteUrl = normalizedUrl(baseUrl, newUrl);

                    if (absoluteUrl != null && !visitedUrls.ContainsKey(absoluteUrl.ToString()))
                    {
                        frontier.Enqueue(absoluteUrl);
                    }
                }
            }
            catch
            {
                // unable to load page
                visitedUrls[page.ToString()] = false;
            }
        }

        private Uri normalizedUrl(Uri baseUrl, string newUrl)
        {
            newUrl = newUrl.ToLower();
            if (Uri.TryCreate(newUrl, UriKind.RelativeOrAbsolute, out var url))
            {
                return Uri.TryCreate(baseUrl, url, out var normalizedUrl) ? normalizedUrl : null;
            }
            return null;
        }


        public Dictionary<string, bool> GetVisitedUrls()
        {
            return new Dictionary<string, bool>(visitedUrls);
        }
    }
}

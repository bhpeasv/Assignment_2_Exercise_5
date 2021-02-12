using System;
using System.Collections.Generic;

namespace Exercise_5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter url: ");
            string urlStr = Console.ReadLine();

            UriBuilder ub = new UriBuilder(urlStr);

            Queue<Uri> initialUrls = new Queue<Uri>();
            initialUrls.Enqueue(ub.Uri);

            WebCrawler webCrawler = new WebCrawler(initialUrls, 50);

            Console.WriteLine("\n\nValid links: ");
            printLinks(webCrawler.GetVisitedUrls(), true);

            Console.WriteLine("\n\nInvalid links: ");
            printLinks(webCrawler.GetVisitedUrls(), false);

            Console.ReadKey();
        }

        private static void printLinks(Dictionary<string, bool> links, bool isValid)
        {
            foreach (KeyValuePair<string, bool> kv in links)
            {
                if (kv.Value == isValid)
                {
                    Console.WriteLine("    {0}", kv.Key);
                }
            }
        }
    }
}

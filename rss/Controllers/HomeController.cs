using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rss.Models;
using rss.Models.VievModels;

namespace rss.Controllers
{
    public class HomeController : Controller
    {
        private string furl;
        private NameValueCollection appSettings;
        private Configuration config;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            appSettings = ConfigurationManager.AppSettings;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
        public IActionResult Index(string Url)
        {
            var settings = new Settings
            {
                UpdateFrequency = Int32.Parse(appSettings["updateFrequency"]),
                Urls = appSettings["urls"].Split(';').ToList()
            };
            WebClient wclient = new WebClient();
            var feed = new FeedViewModel
            {
                Urls = settings.Urls,
                Data = new List<Feed>()
            };
            if (!String.IsNullOrEmpty(Url)) {
                string data = wclient.DownloadString(Url);
                XDocument xml = XDocument.Parse(data);
                feed.Data = (from x in xml.Descendants("item")
                             select new Feed
                             {
                                 Title = ((string)x.Element("title")),
                                 Link = ((string)x.Element("link")),
                                 Description = ((string)x.Element("description")),
                                 PubDate = ((string)x.Element("pubDate"))
                             });
            }
            else
            {
                foreach (var url in settings.Urls)
                {
                    string data = wclient.DownloadString(url);
                    XDocument xml = XDocument.Parse(data);
                    var feedData = (from x in xml.Descendants("item")
                                    select new Feed
                                    {
                                        Title = ((string)x.Element("title")),
                                        Link = ((string)x.Element("link")),
                                        Description = ((string)x.Element("description")),
                                        PubDate = ((string)x.Element("pubDate"))
                                    });
                    ((List<Feed>)feed.Data).AddRange(feedData);
                }
            }
            return View(feed);
        }
        [HttpGet]
        public IActionResult Settings()
        {
            var settings = new SettingsViewModel
            {
                UpdateFrequency = Int32.Parse(appSettings["updateFrequency"]),
                Urls = appSettings["urls"]
            };
            return View(settings);
        }
        [HttpPost]
        public IActionResult Settings(SettingsViewModel model)
        {
            config.AppSettings.Settings["updateFrequency"].Value = model.UpdateFrequency.ToString();
            config.AppSettings.Settings["urls"].Value = model.Urls;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            return RedirectToAction("Settings", "Home");
        }
        [HttpPost]
        public IActionResult Feed(string Url)
        {
            var settings = new Settings
            {
                UpdateFrequency = Int32.Parse(appSettings["updateFrequency"]),
                Urls = appSettings["urls"].Split(';').ToList()
            };
            WebClient wclient = new WebClient();
            string data = wclient.DownloadString(Url);
            XDocument xml = XDocument.Parse(data);
            var feedData = (from x in xml.Descendants("item")
                            select new Feed
                            {
                                Title = ((string)x.Element("title")),
                                Link = ((string)x.Element("link")),
                                Description = ((string)x.Element("description")),
                                PubDate = ((string)x.Element("pubDate"))
                            });
            return View(feedData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using selenium_test.Core.Entities;
using selenium_test.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace selenium_test.Core
{
    public class Application
    {
        public Application()
        {
            configuration = FileHelper.LoadConfiguration();
            cities = FileHelper.ReadUrls(configuration);
            Console.WriteLine($"Всего найдено {cities.Count} ссылок.");
        }
        public Config configuration { get; set; }
        public List<string> cities = null;
        WebClient web = null;
        Random rand = new Random();
        public void Start()
        {
            while (web == null)
            {
                Console.WriteLine("Попытка соединится с https://youcontrol.com.ua/...");
                web = CloudflareEvader.CreateBypassedWebClient($"https://youcontrol.com.ua/");
                Console.WriteLine("Успех!");
            }
            Console.WriteLine($"Начинаю парсить");
            if (configuration.CurrentCity.Length == 0)
            {
                configuration.CurrentCity = cities[0];
            }

            while (true)
            {
                try
                {
                    string data = "";
                    string page = web.DownloadString($"{configuration.CurrentCity}{configuration.Counter}/");
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(page);
                    if (document.DocumentNode.InnerHtml.Contains("403 Forbidden"))
                    {
                        throw new Exception("PROXY::NEED_CHANGE");
                    }

                    foreach (var item in document.DocumentNode.QuerySelectorAll("a.link-td"))
                    {
                        Console.WriteLine(item.Attributes["title"].Value);
                        data += item.Attributes["title"].Value + '\n';
                    }
                    FileHelper.AppendAllFops(configuration, data);
                    Console.WriteLine($"Данные страницы {configuration.Counter++} записаны в {configuration.FilePathOutput}");
                    FileHelper.SaveConfiguration(configuration);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    if (e.Message.Contains("404"))
                    {
                        configuration.CurrentCity = cities[cities.IndexOf(configuration.CurrentCity) + 1];
                        Console.WriteLine($"Новая ссылка для парсинга {configuration.CurrentCity}");
                        configuration.Counter = 1;
                        FileHelper.SaveConfiguration(configuration);
                    }
                    else
                    {
                        if (configuration.IsProxy)
                        {
                            Console.WriteLine("Заблокирован. Меняю прокси...");
                            web.Proxy = new WebProxy(configuration.ProxyList[rand.Next(0, configuration.ProxyList.Count - 1)]);
                            Console.WriteLine("Новый прокси применён!");
                        }
                        else
                        {
                            Console.WriteLine("Заблокирован. Использование прокси отключено. Ожидаю 30 минут.");
                            Thread.Sleep(1800000);
                        }
                    }
                }
                Thread.Sleep(3000);
            }
        }
    }
}




//Слава Україні

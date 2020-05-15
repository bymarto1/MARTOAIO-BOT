using System;
using DiscordWebhook;
using System.Threading.Tasks;
using System.Threading;

namespace MARTOAIO
{
    public class Program

    {


        //private static WebClient legitClient = new WebClient();
        //private static CookieContainer cookies = new CookieContainer();



        static async Task Main()
        {

            /* Console.WriteLine(@" /$$      /$$                       /$$                /$$$$$$  /$$$$$$  /$$$$$$ 
     | $$$    /$$$                      | $$               /$$__  $$|_  $$_/ /$$__  $$
     | $$$$  /$$$$  /$$$$$$   /$$$$$$  /$$$$$$    /$$$$$$ | $$  \ $$  | $$  | $$  \ $$
     | $$ $$/$$ $$ |____  $$ /$$__  $$|_  $$_/   /$$__  $$| $$$$$$$$  | $$  | $$  | $$
     | $$  $$$| $$  /$$$$$$$| $$  \__/  | $$    | $$  \ $$| $$__  $$  | $$  | $$  | $$
     | $$\  $ | $$ /$$__  $$| $$        | $$ /$$| $$  | $$| $$  | $$  | $$  | $$  | $$
     | $$ \/  | $$|  $$$$$$$| $$        |  $$$$/|  $$$$$$/| $$  | $$ /$$$$$$|  $$$$$$/
     |__/     |__/ \_______/|__/         \___/   \______/ |__/  |__/|______/ \______/ ");



                 */
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"          .--.  ,---.  _______  .---.    .--.  ,-. .---.   
|\    /| / /\ \ | .-.\|__   __|/ .-. )  / /\ \ |(|/ .-. )  
|(\  / |/ /__\ \| `-'/  )| |   | | |(_)/ /__\ \(_)| | |(_) 
(_)\/  ||  __  ||   (  (_) |   | | | | |  __  || || | | |  
| \  / || |  |)|| |\ \   | |   \ `-' / | |  |)|| |\ `-' /  
| |\/| ||_|  (_)|_| \)\  `-'    )---'  |_|  (_)`-' )---'   
'-'  '-'            (__)       (_)                (_)      ");


            Console.WriteLine("AUTOMATION AT ITS FINEST");
            Console.WriteLine("@by_martori");

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("---------------------------------------------------------------------");

            Console.WriteLine("Wich website u want to cook?");
            Console.WriteLine("Snipes ----------> type '1'");
            Console.WriteLine("Holypopstore ----> type '2'");
            Console.WriteLine("Solebox ---------> type '3'");

            string store = Console.ReadLine();


            if (store == "1")
            {

              await Snipes.snipesAsync();

               
            }
            else if (store == "2")
            {

                //Holypop.holypop();


            }
            else if(store == "3")
            {
               await Solebox.soleboxAsync();
           
            }
           
                
                // Display the total count for the downloaded websites.  
            }





        public struct Proxy
        {
            public bool proxyless;
            public string ip;
            public int port;
            public string username;
            public string password;
            
            public Proxy(string proxy)
            {

                if (proxy=="") {
                    ip = "";
                    port = 0;
                    username = "";
                    password = "";
                    proxyless = true;

                }
                else
                {
                    string[] proxyparams = proxy.Split(":");
                    ip = proxyparams[0];
                    port = Int32.Parse(proxyparams[1]);
                    username = proxyparams[2];
                    password = proxyparams[3];
                    proxyless = false;
                    /*
                    Console.WriteLine(ip);
                    Console.WriteLine(port);
                    Console.WriteLine(username);
                    Console.WriteLine(password);
                    */
                }



            }
        }


















        public static void sendCheckout(string checkouturl, string productname , string size, string imageurl,string site ,long time = 0, string mode ="idk", bool proxyless = true  , string id = "?")
        {
            //Console.WriteLine("SENDING LINK TO DISCORD ...");

            //ChromeDriver driver = new ChromeDriver();
            //driver.Navigate().GoToUrl(checkouturl);
            Random r = new Random();
            string proxy = "proxyless";
          
            if (proxyless == false) proxy = "using proxies";
            WebhookObject obj = new WebhookObject()
            {
                embeds = new Embed[]
                  {
                      new Embed()
                      {

                        title= site,

                        fields = new Field[]
                          {
                            new Field()
                            {
                              name = "Product",
                              value = productname
                            } ,
                            new Field()
                            {
                              name = "Task",
                              value = id
                            } ,
                            new Field()
                            {
                              name = "Time",
                              value = time.ToString()+"ms"
                            } ,
                             new Field()
                            {
                              name = "Size",
                              value = size
                            },
                            new Field()
                            {
                              name = "Mode",
                              value = mode
                            },
                            new Field()
                            {
                              name = "Proxy",
                              value = proxy
                            },
                            new Field()
                            {
                              name = "Checkout Url",
                              value = checkouturl
                            }
                          },

                          thumbnail = new Thumbnail()
                          {
                              url = imageurl
                          },
                          footer = new Footer()
                          {
                              text = "Developed by by_martori",
                              icon_url = "https://scontent-mad1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/84499267_414609729333279_1770961900918411388_n.jpg?_nc_ht=scontent-mad1-1.cdninstagram.com&_nc_cat=103&_nc_ohc=7p5PC0hB8MUAX_XXS7x&oh=60e78f8b53d29db7e3a12a998641d6e1&oe=5EB67BDB"

                          }


                      }
                  },
                username = "MARTOAIO LINKS",

                avatar_url = "https://scontent-mad1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/84499267_414609729333279_1770961900918411388_n.jpg?_nc_ht=scontent-mad1-1.cdninstagram.com&_nc_cat=103&_nc_ohc=7p5PC0hB8MUAX_XXS7x&oh=60e78f8b53d29db7e3a12a998641d6e1&oe=5EB67BDB"
            };

            bool sent = false;
            while (!sent)
            {
                try
                {

                    if (site == "SNIPES")
                    {
                        Webhook webhook = new Webhook("https://discordapp.com/api/webhooks/708314996114718740/sZEwplNHFu2aEh3lff7O8bSqrZKA-b0qLYWMsIAkAVujNxBFPcof5PqJaXfetZECn3ls");
                        webhook.PostData(obj);
                        sent = true;

                    }
                    else if (site == "SOLEBOX")
                    {
                        Webhook webhook = new Webhook("https://discordapp.com/api/webhooks/709001948807823373/ocIxvkiOBO7T0vDxn1J_AHi9uQv8vkTUUtlH3OF5Iu9N9RHHyMwcY8gHrKSen_3hxTaf");
                        webhook.PostData(obj);
                        sent = true;
                    }
                    else
                    {
                        Webhook webhook = new Webhook("https://discordapp.com/api/webhooks/709002401688059914/TdvY1kwuQU2DpVYui_KqR0_bml4qqYUT19hUyND48EJu7z9rhIlLwm-JuQVFMDKSvfGY");
                        Thread.Sleep(r.Next(1000));
                        webhook.PostData(obj);
                        sent = true;

                    }
                    printMsgAsync("Task " + id + "---CHECK DISCORD!!!--------------", "Green");


                }
                catch (Exception e)
                {
                    Thread.Sleep(1500);
                }
            }


     





        }

        public static async Task printMsgAsync(string msg, string c = "White")
        {
            DateTime now = DateTime.Now;
            switch (c)
            {
                case "Green":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[{0}]-", now.ToString("T"));
                    Console.ForegroundColor = ConsoleColor.Green;
                    await Console.Out.WriteLineAsync(msg);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "Red":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[{0}]-", now.ToString("T"));
                    Console.ForegroundColor = ConsoleColor.Red;
                    await Console.Out.WriteLineAsync(msg);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[{0}]-", now.ToString("T"));
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    await Console.Out.WriteLineAsync(msg);
                    break;

            }

        }









    }
}
           




           
    





    






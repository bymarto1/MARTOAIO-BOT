﻿using System;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static MARTOAIO.Program;
using System.Collections.Generic;
using System.Net.Http;
using CloudflareSolverRe;
using System.Reflection;

namespace MARTOAIO
{

    public class Solebox
    {

        public static async Task soleboxAsync()
        {
            try { 
            System.Net.ServicePointManager.DefaultConnectionLimit = 50;

            string url = "", size = "", variant = "", method , dataatc = "", mode ,pid;
            int ntasks;


            Console.WriteLine("Do you want to use url or variant? (u) (v)  (ONLY WORKS URL AT THE MOMENT)");
            method = Console.ReadLine();

            if (method != "v")
            {
                Console.WriteLine("Insert the url here:");
                url = Console.ReadLine();

            //    Console.WriteLine("Insert the size (eu) :");
             //   size = Console.ReadLine();
                pid = url.Substring(url.Length - 13 );
                pid = pid.Substring(0, 8);
                await printMsgAsync("pid:" + pid, "Green");

            }
            else
            {
                Console.WriteLine("Insert variant here:");
                variant = Console.ReadLine();
                pid = variant.Substring(0, 8);
                await printMsgAsync("pid: " + pid, "Green");
                // Console.WriteLine("Insert the size (eu) :");
                //size = Console.ReadLine();

                StringBuilder data = new StringBuilder();
                data.Append("pid=");
                data.Append(variant);
                data.Append("&options=%5B%7B%22optionId%22%3A%22212%22%2C%22selectedValueId%22%3A%22");
                data.Append(size);
                data.Append("%22%7D%5D&quantity=1");
                dataatc = data.ToString();

            }


           // Console.WriteLine("How many tasks do you want to run?");
           // ntasks = Convert.ToInt32(Console.ReadLine());

            // Console.WriteLine("Do u want paypal or cc link?   (pp) (cc)");
            //string payment = Console.ReadLine();
            string payment = "pp";


           // Console.WriteLine("Wich mode u want Fast or Safe?   (f) (s)");
           // mode = Console.ReadLine();



           // Console.WriteLine("Do u want to use proxies? (y) (n)");
           // string usingproxy = Console.ReadLine();

           // if (usingproxy == "n") proxyless = true;

    


                var path = Path.Combine(Directory.GetCurrentDirectory(), "tasks.csv");

                string[] readlinestasks = System.IO.File.ReadAllLines(path);
                Console.WriteLine(readlinestasks.Length);
                string[][] tasks = new string[readlinestasks.Length-1][];
                for(int i = 1; i < readlinestasks.Length; ++i)
                {
                    tasks[i-1] = readlinestasks[i].Split(";");
                }

                
                for (int i = 0; i < tasks.Length; ++i)
                {   for(int j = 0; j<3; ++j)
                    {
                        Console.WriteLine(tasks[i][j]);
                    }
                    

                }


                path = Path.Combine(Directory.GetCurrentDirectory(), "profile.json");
                var myJsonString = File.ReadAllText("profile.json");
                Profile Info = JsonConvert.DeserializeObject<Profile>(myJsonString);

                Console.WriteLine("perfil llegit");


                


                string[] proxys;

                 path = Path.Combine(Directory.GetCurrentDirectory(), "proxies.txt");
                 proxys = System.IO.File.ReadAllLines(path);

                 System.Console.WriteLine("Contents of proxies.txt = ");

                /*   foreach (string proxy in proxys)
                   {
                       // Use a tab to indent each line of the file.
                       Console.WriteLine("\t" + proxy);
                   }
                   */

                ntasks = tasks.Length ;
                Task[] taskList = new Task[ntasks];

                //Console.WriteLine(pid);
                //Console.WriteLine(variant);

                int nproxies = proxys.Length;

                Random r = new Random();
                Proxy proxytask = new Program.Proxy("");
                List<Task> TaskList = new List<Task>(ntasks);
                for (int i = 0; i < ntasks; ++i)
                {
                    mode = tasks[i][0];
                    if (tasks[i][1] == "y")
                    {
                        proxytask = new Program.Proxy(proxys[r.Next(nproxies)]);
                    }
                    size = tasks[i][2];
                    Console.WriteLine("Creating task {0}", i);
                    var LastTask = SoleboxtaskAsync(i.ToString(), url, pid, size, payment, dataatc, Info, proxytask, mode);
                    TaskList.Add(LastTask);
                }

                
                await Task.WhenAll(TaskList);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} First exception caught.", e);

            }



        }


        public class Profile
        {
            public string Mail { get; set; }
            public string Name { get; set; }
            public string Midname { get; set; }
            public string CP { get; set; }
            public string Adress { get; set; }
            public string Adress2 { get; set; }
            public string City { get; set; }
            public string HouseNumber { get; set; }
            public string Phone { get; set; }
            public string Country { get; set; }


        }








        public static string GenShipping(string token, string uuid, ref Profile Info)
        {

            Info.Adress = Info.Adress.Replace(" ", "+");
            StringBuilder postString = new StringBuilder();
            postString.Append("originalShipmentUUID=");
            postString.Append(Uri.EscapeDataString(uuid));
            postString.Append("&shipmentUUID=");
            postString.Append(Uri.EscapeDataString(uuid));
            postString.Append("&dwfrm_shipping_shippingAddress_shippingMethodID=home-delivery_europe&address-selector=new&dwfrm_shipping_shippingAddress_addressFields_title=Mr&dwfrm_shipping_shippingAddress_addressFields_firstName=");
            postString.Append(Info.Name);
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_lastName=");
            postString.Append(Info.Midname);
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_postalCode=");
            postString.Append(Info.CP);
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_city=");
            postString.Append(Info.City);
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_street=");
            postString.Append(Info.Adress);
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_suite=");
            postString.Append(Info.HouseNumber);
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_address1=");
            postString.Append(Info.Adress);
            postString.Append("+%2C+");
            postString.Append(Uri.EscapeDataString(Info.HouseNumber));
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_address2=");
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_phone=");
            postString.Append("12345678910");
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_countryCode=");
            postString.Append(Info.Country);
            postString.Append("&serviceShippingMethod=ups-express-saver&dwfrm_shipping_shippingAddress_shippingAddressUseAsBillingAddress=true&dwfrm_billing_billingAddress_addressFields_title=Mr&dwfrm_billing_billingAddress_addressFields_firstName=");
            postString.Append(Info.Name);
            postString.Append("&dwfrm_billing_billingAddress_addressFields_lastName=");
            postString.Append(Info.Midname);
            postString.Append("&dwfrm_billing_billingAddress_addressFields_postalCode=");
            postString.Append(Info.CP);
            postString.Append("&dwfrm_billing_billingAddress_addressFields_city=");
            postString.Append(Info.City);
            postString.Append("&dwfrm_billing_billingAddress_addressFields_street=");
            postString.Append(Info.Adress);
            postString.Append("&dwfrm_billing_billingAddress_addressFields_suite=");
            postString.Append(Uri.EscapeDataString(Info.HouseNumber));
            postString.Append("&dwfrm_billing_billingAddress_addressFields_address1=");
            postString.Append(Info.Adress);
            postString.Append("+%2C+");
            postString.Append(Uri.EscapeDataString(Info.HouseNumber));
            postString.Append("&dwfrm_billing_billingAddress_addressFields_address2=");
            postString.Append("&dwfrm_billing_billingAddress_addressFields_countryCode=");
            postString.Append(Info.Country);
            postString.Append("&dwfrm_billing_billingAddress_addressFields_phone=");
            postString.Append("12345678910");
            postString.Append("&dwfrm_contact_email=");
            postString.Append("klkmarto@martoaio.club");
            postString.Append("&dwfrm_contact_phone=");
            postString.Append("12345678910");
            postString.Append("&csrf_token=");
            postString.Append(Uri.EscapeDataString(token));
            //Console.WriteLine(postString.ToString());
            return postString.ToString();

        }


        public static string GenPaymentSelec(string token)
        {
            StringBuilder postString = new StringBuilder();
            postString.Append("dwfrm_billing_paymentMethod=Paypal&csrf_token=");
            postString.Append(Uri.EscapeDataString(token));
            return postString.ToString();

        }

        public static async Task<int> SoleboxtaskAsync(string id, string url, string pidd, string size, string payment, string dataatc, Profile Info, Proxy proxy, string mode)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            bool atc = false;
            string productname = "";
            string imageurl = "";
            int retry = 0;
            Random r = new Random();



            try
            {
                CookieContainer cookies = new CookieContainer();

                HttpClientHandler req = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli,
                    CookieContainer = cookies,
                    UseCookies = true
                };

                HttpClient client = new HttpClient(req);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
                client.DefaultRequestHeaders.Add("authority", "www.solebox.com");
                client.DefaultRequestHeaders.Add("origin", "https://www.solebox.com");
                client.DefaultRequestHeaders.Add("scheme", "https");
                client.DefaultRequestHeaders.Add("cache-control", "max-age=0");
                client.DefaultRequestHeaders.Add("accept", "application/json, text/javascript, */*; q=0.01");
                //client.DefaultRequestHeaders.Add("content-type", "application/json");
                client.Timeout = TimeSpan.FromSeconds(30);
                //  cloudflare(url, new WebProxy(), client, handler);
                if (mode == "s")
                {
                    if (proxy.proxyless == false)
                    {
                        WebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
                        string username = proxy.username;
                        string password = proxy.password;
                        myProxy.Credentials = new NetworkCredential(username, password);
                        req.Proxy = myProxy;

                        CookieContainer cfcookies = cloudflare("https://www.solebox.com/en_ES", myProxy, client , req , false);
                        //req.CookieContainer = cfcookies;

                        
                        Console.WriteLine("USING:");
                        Console.WriteLine(proxy.ip);
                        Console.WriteLine(proxy.port);
                        Console.WriteLine(proxy.username);
                        Console.WriteLine(proxy.password);
                        Console.WriteLine("-----------------------");
                        

                    }
                    else
                    {
                        req.UseDefaultCredentials = true;
                        CookieContainer cfcookies = cloudflare("https://www.solebox.com/en_ES", new WebProxy(), client, req, true);
                       // req.CookieContainer = cfcookies;

                    }


                    watch.Stop();
                    Console.WriteLine($"CF time: {watch.ElapsedMilliseconds} ms");
                    watch.Start();

                }
                else if (proxy.proxyless == false)
                {
                    WebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
                    string username = proxy.username;
                    string password = proxy.password;
                    myProxy.Credentials = new NetworkCredential(username, password);
                    req.Proxy = myProxy;


                    Console.WriteLine("USING:");
                    Console.WriteLine(proxy.ip);
                    Console.WriteLine(proxy.port);
                    Console.WriteLine(proxy.username);
                    Console.WriteLine(proxy.password);
                    Console.WriteLine("-----------------------");
                    


                }

                //  await Login(id, client, account);

                 string loginpage = url;
                 string klk = await solerequests.Get(loginpage, client,loginpage);
                //if (id == "0") Console.WriteLine(klk);
                //watch.Stop();
                //await printMsg("First login to get px time:" + watch.ElapsedMilliseconds + " ms");
                //watch.Restart();
                   bool monitor = true;
                    while(monitor == true) { 
                //Console.WriteLine(login);
                    if (dataatc == "")
                    {
                        dataatc = await GenatcAsync(id, url, pidd, size, productname, client);
                        //watch.Stop();
                       // await printMsgAsync("Gen ATC TIME:" + watch.ElapsedMilliseconds + " ms");
                      //  watch.Start();
                        if (dataatc == "") Console.WriteLine("Task {0}---CAN'T GEN ATC!", id);
                    }
                    if (dataatc == "OOS") await printMsgAsync("Task " + id + "---SOLD OUT!", "Red");
                    else if (dataatc == "NOSIZE") await printMsgAsync("Task " + id + "---SIZE NOT VALID", "Red");
                    else if (dataatc == "ERROR") await printMsgAsync("Task " + id + "---CAN'T GEN ATC INFO!", "Red");
                    else
                    {
                        //Console.WriteLine(dataatc);


                        var atcOut = await Atc(id, dataatc, url, imageurl, client);

                        atc = atcOut.added;

                        while (atc == false && retry < 5)
                        {
                            await Task.Delay(1000);
                            atcOut = await Atc(id, dataatc, url, imageurl, client);
                            atc = atcOut.added;
                            await printMsgAsync("Task " + id + "---ERROR ADDING TO CART!", "Red");
                            retry++;
                        }
                        if (atc == true)
                        {
                            monitor = false;
                            watch.Stop();
                            await printMsgAsync("Task " + id + "---ADDED TO CART!", "Green");
                            await printMsgAsync("Task " + id + ": ATC TIME :" + watch.ElapsedMilliseconds + " ms", "Green");
                            watch.Start();

                            await printMsgAsync("Task " + id + "---STARTING CHECKOUT ... ");

                            string checkouturl = await checkoutAsync(id, Info, payment, client);

                            watch.Stop();

                            await printMsgAsync("Task " + id + " Execution time: " + watch.ElapsedMilliseconds + " ms", "Green");

                            if (checkouturl != "")
                            {
                                Program.sendCheckout(checkouturl, atcOut.productname, size, atcOut.image, "SOLEBOX", watch.ElapsedMilliseconds, mode, proxy.proxyless, id);
                            }
                            else
                            {
                                await printMsgAsync("Task " + id + "---Something Went wrong trying to checkout");

                            }
                        }

                    }
                    //else await printMsgAsync("Task " + id + "---Too many atc attempts");
                    dataatc = "";
                    if (r.Next(10) == 1)
                    {
                        await solerequests.Get(url, client, loginpage, false);
                        await printMsgAsync("Task " + id + "---MONITORING...");
                    }
                    await Task.Delay(2500);


                }
                return 1;
      
                     
            }
            catch (Exception e)
            {
                await printMsgAsync("Task " + id + "  " + e, "Red");

                return 0;
            }




        }


        public static CookieContainer cloudflare(string url, WebProxy myProxy , HttpClient client, HttpClientHandler handler ,bool proxyless = false)
        {


 
            var cf = new CloudflareSolver
            {
                MaxTries = 10,
                ClearanceDelay = 3000
            };
            if(url =="") url = "https://www.solebox.com/en_ES";
            var target = new Uri(url);

            CloudflareSolverRe.Types.SolveResult result = new CloudflareSolverRe.Types.SolveResult();
            if (proxyless == false)
            {
               // result = cf.Solve(target, userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36", myProxy).Result;
                result = cf.Solve(client, handler , target).Result;
            }
            else
            {
               // result = cf.Solve(target, userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36").Result;
                // result = cf.Solve(client, handler, target).Result;
                result = cf.Solve(client, handler, target).Result;


            }

            /*
            Console.WriteLine("-------------------");

            Console.WriteLine(result.FailReason);

            Console.WriteLine("-------------------");
            Console.WriteLine(result.Cookies.AsHeaderString());
            Console.WriteLine("-------------------");
            Console.WriteLine(result.DetectResult.ToString());

            Console.WriteLine(result.Cookies.AsHeaderString());

            Console.WriteLine("-------------------");
            */


            if (!result.Success)
            {
                Console.WriteLine("-------------------");
                Console.WriteLine($"[Failed] Details: {result.FailReason}");
                Console.WriteLine("-------------------");

                Console.WriteLine(result.FailReason);

                Console.WriteLine("-------------------");
                // Console.WriteLine(result.Cookies.AsHeaderString());
                Console.WriteLine("-------------------");
                Console.WriteLine(result.DetectResult.ToString());
                Console.WriteLine("-------------------");

                Console.WriteLine(result.Cookies.AsHeaderString());

                Console.WriteLine("-------------------");
                return new CookieContainer();
            }
            else
            {
                /* Console.WriteLine("-------------------");
                 Console.WriteLine(result.Cookies.AsHeaderString());
                 Console.WriteLine("-------------------");
                 */
                Console.WriteLine("-------------------");
                Console.WriteLine(result.DetectResult.ToString());

                if (result.Cookies.AsHeaderString() != "")
                {
                   Console.WriteLine("Cookies not found!");
                    return result.Cookies.AsCookieContainer();
                }
                else return new CookieContainer();

            }



        }



                public static async Task<string> GenatcAsync(string id, string url, string pid, string size, string productname, HttpClient client)
                {

                    var watch = System.Diagnostics.Stopwatch.StartNew();

                    StringBuilder sizeUrl = new StringBuilder();
                    sizeUrl.Append(url);
                    sizeUrl.Append("?chosen=size&dwvar_");
                    sizeUrl.Append(pid);
                    sizeUrl.Append("_212=");
                    sizeUrl.Append(size);
                    sizeUrl.Append("&format=ajax");
                    string sizeUrldone = sizeUrl.ToString();

                    //Console.WriteLine(sizeUrldone);

            
                  /*  watch.Stop();
                    await printMsgAsync("Task " + id + "Stringbuilder time: " + watch.ElapsedMilliseconds + " ms", "Red");
                    watch.Restart();
                    */
                    string srcsize = await solerequests.Get(sizeUrldone, client, url);
                   /* watch.Stop();
                    await printMsgAsync("Task " + id + "First get time: " + watch.ElapsedMilliseconds + " ms", "Red");
                    watch.Restart();
                    */
                    //Console.WriteLine(srcsize);
                    productname = new Regex("\"productName\": \"(.+?)\"").Match(srcsize).Groups[1].Value;

                   /* watch.Stop();
                    await printMsgAsync("Task " + id + "First regex time: " + watch.ElapsedMilliseconds + " ms", "Red");
                    watch.Restart();
                    */

                    if (srcsize.Contains("\"statusCode\": \"outofstock\""))
                    {
                        //Console.WriteLine("Task {0}---ITEM IS SOLD OUT!", id);
                        return "OOS";
                    }
                    else if (srcsize.Contains("//captcha.px-cdn.ne"))
                    {
                        Console.WriteLine("Task {0}---", id);
                        Console.WriteLine("Task {0}---", srcsize);

                        await printMsgAsync("Task " + id + "--FAILED PX3", "Red");
                        return "ERROR";
                    }

                    string variant = new Regex("\"id\": \"(.+?)\"").Match(srcsize).Groups[1].Value;

                    /*watch.Stop();
                    await printMsgAsync("Task " + id + "Find variant time: " + watch.ElapsedMilliseconds + " ms", "Red");
                    watch.Restart();
                    */

                    //Console.WriteLine(variant);
                    if (variant == pid)
                    {
                        return "NOSIZE";
                    }
                    else if (variant == "")
                    {
                       // Console.WriteLine("Task {0}---", id);
                       // Console.WriteLine("Task {0}---", srcsize);
                        return "ERROR";
                    }
                    else
                    {

                        await printMsgAsync("Task " + id + "---VARIANT: " + variant);
                        StringBuilder data = new StringBuilder();
                        data.Append("pid=");
                        data.Append(variant);
                        data.Append("&options=%5B%7B%22optionId%22%3A%22212%22%2C%22selectedValueId%22%3A%22");
                        data.Append(size);
                        data.Append("%22%7D%5D&quantity=1");
                        return data.ToString();
                    }


                    }




                public static async Task<(bool added, string image, string productname)> Atc(string id, string postData, string url, string imageurl, HttpClient client)
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                  
                    bool atc = false;

                    string referer;
                    //Console.WriteLine(postData);
                    string target = "https://www.solebox.com/en_ES/add-product?format=ajax";
                    if (url == "") referer = "https://www.solebox.com/en_ES";
                    else referer = url;

                    //string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                      // string page = requests.Get(url, referer, cookies, false, accept, proxy);

                    string added = await solerequests.Post(target, postData, false, client, referer);
                    //Console.WriteLine(added);

                     /*watch.Stop();
                    await printMsgAsync("Task " + id + "ATC POST time: " + watch.ElapsedMilliseconds + " ms", "Red");
                    watch.Restart();
                    */

                    if (added.Contains("Product added to cart"))
                    {
                        atc = true;
                    }
                    else
                    {
                        await printMsgAsync("Task " + id + "---ATC FAILED RESPONSE :" , "Red");
                        Console.WriteLine(added);
                        return (false, "", "");

                     }

                    string productname = new Regex("\"productName\": \"(.+?)\"").Match(added).Groups[1].Value;
                    imageurl = new Regex("\"srcTRetina\": \"(.+?)\"").Match(added).Groups[1].Value;
                    if (imageurl == "")
                    {
                        imageurl = "https://scontent-mad1-1.cdninstagram.com/v/t51.2885-15/e35/p1080x1080/84499267_414609729333279_1770961900918411388_n.jpg?_nc_ht=scontent-mad1-1.cdninstagram.com&_nc_cat=103&_nc_ohc=7p5PC0hB8MUAX_XXS7x&oh=60e78f8b53d29db7e3a12a998641d6e1&oe=5EB67BDB";

                    }
                    //Console.WriteLine("imageurl{0}", imageurl);

                    //string cart = "https://www.snipes.es/cart";
                    //page = requests.Get(cart, url, cookies, true, accept, proxy);
                    watch.Stop();

                    return (atc, imageurl, productname);

                }


        public static async Task<string> checkoutAsync(string id, Profile Info, string payment, HttpClient client)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            string checkoutlink = "https://www.solebox.com/en_ES/checkout";
            String source = await solerequests.Get(checkoutlink, client);

          /*  watch.Stop();
            await printMsgAsync("Task " + id + "first get checkout: " + watch.ElapsedMilliseconds + " ms", "Red");
            watch.Restart();
            */

            String token = new Regex("<input type=\"hidden\" name=\"csrf_token\" value=\"(.+?)\"").Match(source).Groups[1].Value;

            await printMsgAsync("Task " + id + "---CRSF TOKEN FOUND!");
            //Console.WriteLine($"csrf_token ={token}");

            // string method = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/CheckoutShippingServices-SelectShippingMethod?format=ajax";
            //string referer = checkoutPage;

            string UUID = new Regex("name=\"shipmentUUID\" type=\"hidden\" value=\"(.+?)\" />").Match(source).Groups[1].Value;
            //Console.WriteLine(source);
           /* watch.Stop();
            await printMsgAsync("Task " + id + "find UUI: " + watch.ElapsedMilliseconds + " ms", "Red");
            watch.Restart();
            */
            //await printMsgAsync("Task " + id + "---UUID FOUND!");
            // Console.WriteLine("UUID : ");
            // Console.WriteLine(UUID);

            //VALIDATION OF SHIPPING, ITS A POST REQUEST WITH THE BASIC ADRESS , DOESN'T NEED IT TO CHECKOUT

            // Console.WriteLine("Task {0}---Checking shipping...", id);


            //  string shipping = GenShippingValid(token, ref Info);


            //  await requests.POST(targetValidate, shipping, false, client);
            // Console.WriteLine("Task {0}---SHIPPING VALID!", id);

            await printMsgAsync("Task " + id + "---SUBMITTING SHIPPING...");



            //string shipid = new Regex("\"address- selector\" value = \"(.+?)\"").Match(source).Groups[1].Value;

            /*  StringBuilder data = new StringBuilder();
              data.Append("https://www.solebox.com/on/demandware.store/Sites-solebox-Site/en_ES/CheckoutShippingServices-ShippingRates?selected=true&id=");
              data.Append(shipid);
              data.Append("&addressType=home-delivery&snipesStore=&postOfficeNumber=&packstationNumber=&postNumber=&postalCode=");
              data.Append(Info.CP);
              data.Append("&countryCode=");
              data.Append(Info.Country);
              data.Append("&suite=");
              data.Append("Info.HouseNumber");
              data.Append("&street=");
              data.Append("Info.Adress");
              data.Append("&city=");
              data.Append("Info.City");
              data.Append("&address2=");
              data.Append(Uri.EscapeDataString(Info.Adress2));
              data.Append("&lastName=");
              data.Append(Info.Midname);
              data.Append("&firstName=");
              data.Append(Info.Name);
              data.Append("&title=Mr&format=ajax");
              string selectShipping=  data.ToString();

      */
            StringBuilder data = new StringBuilder();
            data.Append("https://www.solebox.com/on/demandware.store/Sites-solebox-Site/en_ES/CheckoutShippingServices-ShippingRates?title=Mr&firstName");
            data.Append(Info.Name);
            data.Append("&lastName");
            data.Append(Info.Midname);
            data.Append("&postalCode=");
            data.Append(Info.CP);
            data.Append("&city=");
            data.Append(Info.City);
            data.Append("&street=");
            data.Append(Uri.EscapeDataString(Info.Adress));
            data.Append("&suite=");
            data.Append(Info.HouseNumber);
            data.Append("address1=&address2=");
            data.Append(Uri.EscapeDataString(Info.Adress2));
            data.Append("&phone=");
            data.Append(Info.Phone);
            data.Append("&countryCode=");
            data.Append(Info.Country);
            data.Append("&useAsBilling=true&format=ajax");  //maybe false 
            string selectShipping = data.ToString();

            string referer = "https://www.solebox.com/en_ES/checkout?stage=shipping";
            await solerequests.Post(selectShipping, "", false, client, referer, false);

            
            string shipp =  GenShipping(token, UUID, ref Info);

            string targetship = "https://www.solebox.com/on/demandware.store/Sites-solebox-Site/en_ES/CheckoutShippingServices-SubmitShipping?region=europe&country=" + Info.Country + "&format=ajax";
            await solerequests.Post(targetship, shipp, false, client, referer, false);

            await printMsgAsync("Task " + id + "---SHIPPING SUBMITTED!!!");


            string target = "https://www.solebox.com/on/demandware.store/Sites-solebox-Site/en_ES/CheckoutServices-SubmitPayment?format=ajax";
            string postData = GenPaymentSelec(token);

            await solerequests.Post(target, postData, false, client,referer, false);

            await printMsgAsync("Task " + id + "---PAYMENT SELECTED... ");


            target = "https://www.solebox.com/on/demandware.store/Sites-solebox-Site/en_ES/CheckoutServices-PlaceOrder?format=ajax";

            string src = await solerequests.Post(target, "", false, client, referer);
           // Console.WriteLine(src);
            target = new Regex("continueUrl\": \"(.+?)\",").Match(src).Groups[1].Value;
            string cancel = new Regex("cancelUrl\": \"(.+?)\"").Match(src).Groups[1].Value;


            await printMsgAsync("Task " + id + "---CHECKOUT URL FOUND!!");


            if (target == "")
            {
                await printMsgAsync("Task " + id + "---TARGET NOT FOUND!!!...", "Red");
                Console.Write(src);
                return "";

            }

            else
            {
                if (payment == "pp")
                {

                    return target;

                }

                else
                {
                    var cookiespay = new CookieContainer();

                   StringBuilder refe = new StringBuilder();
                   refe.Append("https://www.snipes.es");
                   refe.Append(cancel);
                    referer = refe.ToString();


                   Console.WriteLine("Task {0}---STARTING CHECKOUT...", id);

                   //string paySource = requests.Getpay(target, referer, ref cookiespay, true, accept, true);

                   //Console.Write(source);
                  // string session = new Regex("name=\"SessionId\" type=\"hidden\" value=\"(.+?)\" />").Match(paySource).Groups[1].Value;


                   //   Console.Write("SCRAPPING CHECKOUT URL...");
                     // Console.WriteLine(session);

                    // Console.WriteLine("FOUND!");
                      

                    StringBuilder payselec = new StringBuilder();
                    payselec.Append("https://www.saferpay.com/VT2/mpp/PaymentSelection/Index/");
                    //  payselec.Append(Uri.EscapeDataString(session));
                    string payselecurl = payselec.ToString();

                    //finishCheckout(payselecurl, referer, session);
                    return payselecurl;

                }

            }
            
            


        }

        public static async Task Login(string id, HttpClient client, string account)
        {
            try
            {
                //   string mainpage = "https://www.solebox.com/en_ES";

                //   await solerequests.Get(mainpage, client);

                string referer = "https://www.solebox.com/en_ES/login";
                await printMsgAsync("Task " + id + "---Loggin in...");
                string loginpage = "https://www.solebox.com/en_ES/login?rurl=1";

                string loginsrc = await solerequests.Get(loginpage, client);
                // string loginsrc = "klk";

                // Console.WriteLine(loginsrc);
                String token = new Regex("<input type=\"hidden\" name=\"csrf_token\" value=\"(.+?)\"").Match(loginsrc).Groups[1].Value;
                if (token == "")
                {
                    Console.WriteLine("CSRF TOKEN NOT FOUND ");
                }

                string[] accountInfo = account.Split(":");

                string email = accountInfo[0];
                string pw = accountInfo[1];


                await printMsgAsync("Task " + id + "---Entering account : " + email);

                StringBuilder loginInfo = new StringBuilder();
                loginInfo.Append("dwfrm_profile_customer_email=");
                loginInfo.Append(email);
                loginInfo.Append("&dwfrm_profile_login_password=");
                loginInfo.Append(pw);
                loginInfo.Append("&csrf_token=");
                loginInfo.Append(Uri.EscapeDataString(token));
                string postData = loginInfo.ToString();
                string submitLoginUrl = "https://www.solebox.com/en_ES/authentication?rurl=1&format=ajax";
                string logged = await solerequests.Post(submitLoginUrl, postData, false, client, referer);
                //Console.WriteLine(logged);
                if (logged.Contains("\"userLoginStatus\": true"))
                {
                    await printMsgAsync("Task " + id + "---Logged in!!!", "Green");

                }
                else await printMsgAsync("Task " + id + "---CAN'T LOG IN", "Red");



            }
            catch (Exception e)
            {
                Console.WriteLine("{0} First exception caught.", e);
            }

        }








    }











}


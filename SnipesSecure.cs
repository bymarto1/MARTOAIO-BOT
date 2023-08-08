using System;
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

namespace MARTOAIO
{
    public class Snipes
    {

        public static async Task snipesAsync()
        {

            string url = "", variant = "", method, dataatc = "", mode;
            int ntasks;
            string pid="";


            await printMsgAsync("Do you want to use size or variant? (u) (v)");
            //Console.WriteLine("Do you want to use size or variant? (u) (v)");
            method = Console.ReadLine();
            string size;
            if (method == "u")

            {
                await printMsgAsync("Insert the url here:");
                url = Console.ReadLine();

               // await printMsgAsync("Insert the size (eu) :");
                //size = Console.ReadLine();

                pid = url.Substring(url.Length - 19);
                pid = pid.Substring(0, 14);
            }

            

            try
            {

                var path = Path.Combine(Directory.GetCurrentDirectory(), "tasks.csv");
                Console.WriteLine(path);
                string[] readlinestasks = System.IO.File.ReadAllLines(path);
                Console.WriteLine(readlinestasks.Length);
                string[][] tasks = new string[readlinestasks.Length - 1][];
                for (int i = 1; i < readlinestasks.Length; ++i)
                {
                    tasks[i - 1] = readlinestasks[i].Split(";");
                }


                for (int i = 0; i < tasks.Length; ++i)
                {
                    for (int j = 0; j < tasks[i].Length; ++j)
                    {
                        Console.WriteLine(tasks[i][j]);
                    }


                }
                path = Path.Combine(Directory.GetCurrentDirectory(), "profile.json");
                var myJsonString = File.ReadAllText("profile.json");
                Profile Info = JsonConvert.DeserializeObject<Profile>(myJsonString);

                //Console.WriteLine("perfil llegit");

                path = Path.Combine(Directory.GetCurrentDirectory(), "proxies.txt");
                string[] proxys = System.IO.File.ReadAllLines(path);

                // System.Console.WriteLine("Contents of proxies.txt = ");
                /*
                   foreach (string proxy in proxys)
                   {
                       // Use a tab to indent each line of the file.
                       Console.WriteLine("\t" + proxy);
                   }
                   */

                ntasks = tasks.Length;
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
                    if (tasks[i][3] == "v")
                    {
                            StringBuilder data = new StringBuilder();
                            data.Append("pid=");
                            data.Append(tasks[i][4]);
                            data.Append("&options=%5B%7B%22optionId%22%3A%22212%22%2C%22selectedValueId%22%3A%22");
                            data.Append(size);
                            data.Append("%22%7D%5D&quantity=1");
                            dataatc = data.ToString();
                
                    }
                    Console.WriteLine("Creating task {0}", i);
                    var LastTask = SnipestaskAsync(i.ToString(), url, pid, size,dataatc, Info, proxytask, mode);
                    TaskList.Add(LastTask);
                    dataatc = "";
                }

                await Task.WhenAll(TaskList);
            }
            catch (Exception e)
            {

                await printMsgAsync(e+"Fist exception caught", "Red");


            }



        }

        public static async Task<CookieContainer> cloudflareAsync(string url, WebProxy myProxy, bool proxyless = true)
        {

            url = "https://www.snipes.es/";
            var cf = new CloudflareSolver
            {
                MaxTries = 10,
                ClearanceDelay = 3000,
            };

            var target = new Uri(url);

            CloudflareSolverRe.Types.SolveResult result = new CloudflareSolverRe.Types.SolveResult();
            if (proxyless == false)
            {
                result = cf.Solve(target, userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36", myProxy).Result;
            }
            else result = cf.Solve(target, userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36").Result;

            Console.WriteLine("----------");
            Console.WriteLine(result.Cookies.AsHeaderString());

            Console.WriteLine("----------");



            if (!result.Success)
            {
                await printMsgAsync("Failed Cloudflare", "Red");
               // Console.WriteLine($"[Failed] Details: {result.FailReason}");
                return new CookieContainer();
            }
            else
            {
                if (result.Cookies.AsHeaderString() != "")
                {
                    return result.Cookies.AsCookieContainer();
                }
                return new CookieContainer();

            }



        }



        public static async Task<int> SnipestaskAsync(string id, string url, string pidd, string size, string dataatc,Profile Info, Proxy proxy, string mode)
        {

            var watch = System.Diagnostics.Stopwatch.StartNew();
            bool atc = false;
            //CookieContainer cookiespay = new CookieContainer();
            string productname = "";
            string imageurl = "";
            int state = -1;
            int retry = 0;



            try
            {
                CookieContainer cookies = new CookieContainer();

                HttpClientHandler req = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli,
                    CookieContainer = cookies,
                    UseCookies = true
                };

                if (mode == "s")
                {
                    if (proxy.proxyless == false)
                    {
                        WebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
                        string username = proxy.username;
                        string password = proxy.password;
                        myProxy.Credentials = new NetworkCredential(username, password);
                        req.Proxy = myProxy;
                        CookieContainer cfcookies = await cloudflareAsync(url, myProxy, false);
                        req.CookieContainer = cfcookies;
                        
                        watch.Stop();
                        await printMsgAsync("CF time:" + watch.ElapsedMilliseconds + " ms");
                        watch.Start();

                        /*
                        Console.WriteLine("USING:");
                        Console.WriteLine(proxy.ip);
                        Console.WriteLine(proxy.port);
                        Console.WriteLine(proxy.username);
                        Console.WriteLine(proxy.password);
                        Console.WriteLine("-----------------------");
                        */

                    }
                    else
                    {
                        req.UseDefaultCredentials = true;
                        CookieContainer cfcookies = await cloudflareAsync(url, new WebProxy());
                        req.CookieContainer = cfcookies;
                        watch.Stop();
                        await printMsgAsync("CF time:" + watch.ElapsedMilliseconds + " ms");
                        watch.Start();
                    }
                }
                else if (proxy.proxyless == false)
                {
                    WebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
                    string username = proxy.username;
                    string password = proxy.password;
                    myProxy.Credentials = new NetworkCredential(username, password);
                    req.Proxy = myProxy;


                    /*
                    Console.WriteLine("USING:");
                    Console.WriteLine(proxy.ip);
                    Console.WriteLine(proxy.port);
                    Console.WriteLine(proxy.username);
                    Console.WriteLine(proxy.password);
                    Console.WriteLine("-----------------------");
                    */


                }


                HttpClient  client = new HttpClient(req);
                //client.DefaultRequestHeaders.ExpectContinue = false;
                client.Timeout = TimeSpan.FromSeconds(30);

                //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                client.DefaultRequestHeaders.Add("authority", "www.snipes.es");
                client.DefaultRequestHeaders.Add("origin", "https://www.snipes.es");
                client.DefaultRequestHeaders.Add("scheme", "https");
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                client.DefaultRequestHeaders.Add("accept", "application/json, text/javascript, */*; q=0.01");

                //Console.WriteLine(dataatc);
                bool monitor = true;

                while (monitor == true)
                {


                    if (dataatc == "")
                    {
                        dataatc = await GenatcAsync(id, url, pidd, size, productname, client);
                        watch.Stop();
                        await printMsgAsync("Task " + id + "---Gen ATC TIME:" + watch.ElapsedMilliseconds + " ms");
                        watch.Start();
                    }

                    if (dataatc == "OOS")
                    {
                        await printMsgAsync("Task " + id + "---SOLD OUT!", "Red");

                    }
                    else if (dataatc == "NOSIZE")
                    {
                        await printMsgAsync("Task " + id + "---SIZE NOT VALID", "Red");

                    }
                    else if (dataatc == "ERROR")
                    {
                        await printMsgAsync("Task " + id + "---CAN'T GEN ATC INFO!", "Red");

                    }

                    else
                    {
                        
                        var atcOut = await Atc(id, dataatc, url, imageurl, client);
                        atc = atcOut.added;
                        while (atc == false)
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

                            string checkouturl = await checkoutAsync(id, Info, client);

                            watch.Stop();

                            await printMsgAsync("Task " + id + " Execution time: " + watch.ElapsedMilliseconds + " ms", "Green");

                            if (checkouturl != "")
                            {
                                Program.sendCheckout(checkouturl, atcOut.productname, size, atcOut.image, "SNIPES", watch.ElapsedMilliseconds, mode , proxy.proxyless, id);
                            }
                            else
                            {
                                await printMsgAsync("Task " + id + "---Something Went wrong trying to checkout");

                            }
                        }
                    }
                    await Task.Delay(1500);

                    //else await printMsgAsync("Task " + id + "---Too many atc attempts");

                    state = 1;
                }
                return state;
            }
            catch (Exception e)
            {
               await printMsgAsync("Task " + id + "  " + e );
                return 0;
            }



        }







        public class Profile
        {
            public string Mail { get; set; }
            public string Name { get; set; }
            public string Midname { get; set; }
            public string CP { get; set; }
            public string Adress { get; set; }
            public string City { get; set; }
            public string HouseNumber { get; set; }
            public string Phone { get; set; }
            public string Country { get; set; }


        }




        public static string GenShippingValid(string token, Profile Info)
        {
            StringBuilder postString = new StringBuilder();
            postString.Append("street=");
            postString.Append("Info.Adress");
            postString.Append("&houseNo=");
            postString.Append(Info.HouseNumber);
            postString.Append("&postalCode=");
            postString.Append(Info.CP);
            postString.Append("&city=");
            postString.Append(Info.City);
            postString.Append("&country=");
            postString.Append(Info.Country);
            postString.Append("&csrf_token=");
            postString.Append(Uri.EscapeDataString(token));
            return postString.ToString();

        }





        public static async Task<string> GenShipping(string token, string uuid,  Profile Info)
        {

            Info.Adress = Info.Adress.Replace(" ", "+");
            StringBuilder postString = new StringBuilder();
            postString.Append("originalShipmentUUID=");
            postString.Append(Uri.EscapeDataString(uuid));
            postString.Append("&shipmentUUID=");
            postString.Append(Uri.EscapeDataString(uuid));
            postString.Append("&dwfrm_shipping_shippingAddress_shippingMethodID=home-delivery_es&address-selector=new&dwfrm_shipping_shippingAddress_addressFields_title=Herr&dwfrm_shipping_shippingAddress_addressFields_firstName=");
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
            postString.Append(Info.Phone);
            postString.Append("&dwfrm_shipping_shippingAddress_addressFields_countryCode=");
            postString.Append(Info.Country);
            postString.Append("&dwfrm_shipping_shippingAddress_shippingAddressUseAsBillingAddress=true&dwfrm_billing_billingAddress_addressFields_title=Herr&dwfrm_billing_billingAddress_addressFields_firstName=");
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
            postString.Append(Info.Phone);
            postString.Append("&dwfrm_contact_email=");
            postString.Append(Uri.EscapeDataString(Info.Mail));
            postString.Append("&dwfrm_contact_phone=p");
            postString.Append(Info.Phone);
            postString.Append("&csrf_token=");
            postString.Append(Uri.EscapeDataString(token));
            return postString.ToString();


        }

        public static async Task<string> GenPaymentSelec(string token)
        {
            StringBuilder postString = new StringBuilder();
            postString.Append("dwfrm_billing_paymentMethod=");
            postString.Append("Paypal");
            postString.Append("&dwfrm_giftCard_cardNumber=&dwfrm_giftCard_pin=&csrf_token=");
            postString.Append(Uri.EscapeDataString(token));
            postString.Append("&csrf_token=");
            postString.Append(Uri.EscapeDataString(token));
            return postString.ToString();

        }

        


        public static async Task<string> GenatcAsync(string id, string url, string pid, string size, string productname, HttpClient client)
        {

            StringBuilder sizeUrl = new StringBuilder();
            sizeUrl.Append(url);
            sizeUrl.Append("?chosen=size&dwvar_");
            sizeUrl.Append(pid);
            sizeUrl.Append("_212=");
            sizeUrl.Append(size);
            sizeUrl.Append("&format=ajax");
            string sizeUrldone = sizeUrl.ToString();

            string srcsize = await requests.Get(sizeUrldone, false, client);
            //Console.WriteLine(srcsize);
            
            productname = new Regex("\"productName\": \"(.+?)\"").Match(srcsize).Groups[1].Value;

            if (srcsize.Contains("\"statusCode\": \"outofstock\""))
            {
                return "OOS";
            };

            string variant = new Regex("\"id\": \"(.+?)\"").Match(srcsize).Groups[1].Value;
            await printMsgAsync("Task " + id + "---VARIANT: " + variant);
            //Console.WriteLine(variant);
            if (variant == pid)
            {
                return "NOSIZE";
            }
            else if (variant == "")
            {
                await printMsgAsync("error gen atc =" + srcsize, "Red");
                return "ERROR";
            }
            else
            {
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


            bool atc = false;
            string target = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/Cart-AddProduct?format=ajax";
            //Console.WriteLine(postData);
            //string referer = "https://www.snipes.es/";

            //string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            // string page = requests.Get(url, referer, cookies, false, accept, proxy);
            string added = await requests.POST(target, postData, false, client);

            //Console.WriteLine(added);

            if (added.Contains("Agregado"))
            {
                atc = true;
            }
            else
            {
                await printMsgAsync("Task " + id + "---ATC FAILED RESONSE :" + added, "Red");
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

            return (atc, imageurl, productname);

        }



        public static async Task<string> checkoutAsync(string id, Profile Info, HttpClient client)
        {
            // var watch = System.Diagnostics.Stopwatch.StartNew();


            // string checkoutLogin = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/Checkout-Login";
            //string referer = "https://www.snipes.es/cart";
            // string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            // await requests.Get(checkoutLogin, referer, true, accept, client ,req);


            string checkoutPage = "https://www.snipes.es/checkout";
            string source = await requests.Get(checkoutPage, true, client);


            String token = new Regex("<input type=\"hidden\" name=\"csrf_token\" value=\"(.+?)\" />").Match(source).Groups[1].Value;


            await printMsgAsync("Task " + id + "---CRSF TOKEN FOUND!");
            //Console.WriteLine($"csrf_token ={token}");
            /* watch.Stop();
             Console.WriteLine($"Execution Time : {watch.ElapsedMilliseconds} ms");
             watch.Restart();
             */


            // string method = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/CheckoutShippingServices-SelectShippingMethod?format=ajax";
            //string referer = checkoutPage;

            string UUID = new Regex("name=\"shipmentUUID\" type=\"hidden\" value=\"(.+?)\" />").Match(source).Groups[1].Value;
           
            string shipData = await GenShipping(token, UUID, Info);
            string paymentData = await GenPaymentSelec(token);

            await printMsgAsync("Task " + id + "---UUID FOUND!");
            //Console.WriteLine("UUID : ");
            //Console.WriteLine(UUID);

            /*  watch.Stop();
              Console.WriteLine($"Execution Time : {watch.ElapsedMilliseconds} ms");
              watch.Restart();
              */
            /*   StringBuilder postString = new StringBuilder();
               postString.Append("methodID=home-delivery_es&shipmentUUID=");
               postString.Append(Uri.EscapeDataString(UUID));
               string postData = postString.ToString();



              await requests.POST(method, postData, referer, false , client );
              Console.WriteLine("Task {0}---SELECTED HOME DELIVERY...", id);
              */



            //VALIDATION OF SHIPPING, ITS A POST REQUEST WITH THE BASIC ADRESS , DOESN'T NEED IT TO CHECKOUT


            //await printMsgAsync("Task " + id + "---Checking shipping...");

           // string targetValidate = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/CheckoutAddressServices-Validate?format=ajax";

           // string shipping = GenShippingValid(token, ref Info);

            // string referer = "https://www.snipes.es/checkout?stage=shipping";

            // await requests.POST(targetValidate, shipping, false, client);

            /*  watch.Stop();
              Console.WriteLine($"Execution Time : {watch.ElapsedMilliseconds} ms");
              watch.Restart();
              */

           // await printMsgAsync("Task " + id + "---SHIPPING VALID!");

            await printMsgAsync("Task " + id + "---SUBMITTING SHIPPING...");


            string targetCheckout = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/CheckoutShippingServices-SubmitShipping?format=ajax";
            //referer = "https://www.snipes.es/checkout?stage=shipping";

            //Console.WriteLine(shipp);

            await requests.POST(targetCheckout, shipData, false, client, false);

            /* watch.Stop();
             Console.WriteLine($"Execution Time : {watch.ElapsedMilliseconds} ms");
             watch.Restart();
             */
            await printMsgAsync("Task " + id + "---SHIPPING SUBMITTED!!!");


            string target = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/CheckoutServices-SubmitPayment?format=ajax";
            //referer = "https://www.snipes.es/checkout?stage=payment";

            await requests.POST(target, paymentData, false, client,false);

            /*    watch.Stop();
                Console.WriteLine($"Execution Time : {watch.ElapsedMilliseconds} ms");
                watch.Restart();
                */
            await printMsgAsync("Task " + id + "---PAYMENT SELECTED... ");

            target = "https://www.snipes.es/on/demandware.store/Sites-snse-SOUTH-Site/es_ES/CheckoutServices-PlaceOrder?format=ajax";
            //referer = "https://www.snipes.es/checkout?stage=placeOrder";

            string src = await requests.POST(target, "", false, client);
            //Console.WriteLine(src);
            target = new Regex("continueUrl\": \"(.+?)\",").Match(src).Groups[1].Value;
            string cancel = new Regex("cancelUrl\": \"(.+?)\"").Match(src).Groups[1].Value;

            /*  watch.Stop();
              Console.WriteLine($"Execution Time : {watch.ElapsedMilliseconds} ms");
              watch.Restart();

              */



            /* Console.Write("TARGET:");


             Console.WriteLine(target);
             Console.WriteLine("-------------------------------------------");

             Console.Write("CANCEL:");
             Console.WriteLine(cancel);
             Console.WriteLine("-------------------------------------------");

             */

            if (target != "")
            {
                await printMsgAsync("Task " + id + "---CHECKOUT URL FOUND!!");
                return target;

            }

            else
            {
                await printMsgAsync("Task " + id + "---TARGET NOT FOUND!!!...", "Red");
                return "";

                
                /*
                else
                {
                    var cookiespay = new CookieContainer();

                    /*
                   StringBuilder refe = new StringBuilder();
                   refe.Append("https://www.snipes.es");
                   refe.Append(cancel);
                   referer = refe.ToString();

                   /*

                   Console.WriteLine("Task {0}---STARTING CHECKOUT...", id);

                   //string paySource = requests.Getpay(target, referer, ref cookiespay, true, accept, true);

                   //Console.Write(source);
                  // string session = new Regex("name=\"SessionId\" type=\"hidden\" value=\"(.+?)\" />").Match(paySource).Groups[1].Value;


                      Console.Write("SCRAPPING CHECKOUT URL...");
                     // Console.WriteLine(session);

                      Console.WriteLine("FOUND!");
                      

                    StringBuilder payselec = new StringBuilder();
                    payselec.Append("https://www.saferpay.com/VT2/mpp/PaymentSelection/Index/");
                    //  payselec.Append(Uri.EscapeDataString(session));
                    string payselecurl = payselec.ToString();

                    //finishCheckout(payselecurl, referer, session);
                    return payselecurl;

                }
                    */

            }


        }





        public static void finishCheckout(string payselecurl, string referer, string session, CookieContainer cookiespay, Proxy proxy)
        {
            Console.Write("NEXT:");
            Console.WriteLine(payselecurl);
            Console.WriteLine("-------------------------------------------");

            Console.WriteLine($"--------------------GETREQUEST----------------------- ={payselecurl}");

            string paysource = requests.Getpay(payselecurl, referer, ref cookiespay, true, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9", false, proxy);

            Console.WriteLine("SELECTING");

            Console.WriteLine($"--------------------POSTREQUEST----------------------- ={payselecurl}");

            string cardData = "1_Card&1090";

            string selected = requests.POSTpay(payselecurl, cardData, payselecurl, ref cookiespay, true, proxy);

            //Console.WriteLine(selected);
            Console.WriteLine("SELECTED!!!!!!");
            Console.WriteLine("-----------------------------------------");




            StringBuilder postpay = new StringBuilder();
            postpay.Append("https://www.saferpay.com/VT2/mpp/PaymentDataEntry/Index/");
            postpay.Append(Uri.EscapeDataString(session));
            string payUrl = postpay.ToString();
            //Console.Write(payUrl);

            string postData = "CardNumber=1111+1111+1111+1111+&ExpMonth=1&ExpYear=1111&HolderName=FIRSTNAME+SECONDNAME&VerificationCode=123&SubmitToNext=";

            Console.WriteLine($"--------------------GETREQUEST----------------------- ={payUrl}");

            string pay = requests.Getpay(payUrl, payselecurl, ref cookiespay, true, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9", false, proxy);
            //Console.WriteLine(pay);


            Console.WriteLine($"--------------------POSTREQUEST----------------------- ={payUrl}");

            string afterpay = requests.POSTpay(payUrl, postData, payUrl, ref cookiespay, true, proxy);
            //Console.WriteLine(afterpay);   



            /* StringBuilder final = new StringBuilder();
             final.Append("https://www.saferpay.com/VT2/mpp/ThreeDSv2PP/Result/");
             final.Append(Uri.EscapeDataString(session));
             string finalurl = postpay.ToString();
             Console.Write(finalurl);
             */

            //string finall = POSTpay(finalurl, "", "", true);
            //Console.WriteLine(finall);
            Console.Write("CHECKED OUT!!!!");

        }









    }

}

using System;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using static MARTOAIO.Program;
using static MARTOAIO.Holyreq;


namespace MARTOAIO
{
    public class Holypop
    {
        
        public struct Cloudflare
        {
            public static bool solved;
            public static string user;

        }

        public static void holypop()
        {
           // WebClient legitClient = new WebClient();
           CookieContainer cookiesholy = new CookieContainer();

            Console.WriteLine("Insert holypop product url:");
            string url = Console.ReadLine();
            Console.WriteLine("Insert size (US) : ");
            string size = Console.ReadLine();
            size = size + " US";
            Console.WriteLine(size);




           // string folderLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "proxies.txt");
            string[] proxys = System.IO.File.ReadAllLines(path);

            Program.Proxy proxytask = new Program.Proxy(proxys[0]);

            if (atc(url, size,  ref cookiesholy, proxytask))
            {
                string checkouturl = checkout(url,  ref cookiesholy, proxytask);
                //finishcheckout(checkouturl, proxytask, ref cookiesholy);
            }

        }


        public static bool login(ref CookieContainer cookiesholy, Proxy proxy, bool checkout=false)
        {
            
            bool logged = false;
            int count = 0;

            string referer, target;
            if (checkout == true)
            {
                referer = "https://www.holypopstore.com/en/login?redirectTo=https://www.holypopstore.com/en/orders/review";
            }
            else
            {
                 referer = "https://www.holypopstore.com/en/account";
            }

             target = "https://www.holypopstore.com/index.php";

            string pstD = "controller=auth&action=authenticate&type=standard&extension=holypop&credential=EMAIL%40gmail.com&password=PASSWORD&language=EN&version=228&cookieVersion=204";
            
            while (logged == false && count != 10) {
                string responseLogin = Holyreq.POST(target, pstD, referer, ref cookiesholy, false, proxy);

                string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                string accounturl = "https://www.holypopstore.com/en/account";
                string src = Holyreq.Get(accounturl, referer , ref cookiesholy, true, accept, false, proxy);
                logged = src.Contains("Your account");
                ++count;
                Console.WriteLine("Error login in...");
                System.Threading.Thread.Sleep(2000);
                



            }
            if (logged) Console.WriteLine("Logged in!!");
            else Console.WriteLine("Not logged in!!");
            return logged;


        }




        public static bool atc(string url, string size,  ref CookieContainer cookiesholy , Proxy proxy)
        {

            Console.WriteLine("STARTING TASK!");

            string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            string accounturl = "https://www.holypopstore.com/en/account";
            string src = Holyreq.Get(accounturl, url, ref cookiesholy, true, accept, false, proxy, false);

            /*        string tgt = "https://www.holypopstore.com/index.php";
                    string rfr = "https://www.holypopstore.com/en/account";
                    string responseLogin = Holyreq.POST(tgt, pstD, rfr, ref cookiesholy, false , proxy );

                    */
            Console.WriteLine("ENTERING ACCOUNT!");

            if (!login(ref cookiesholy, proxy)) return false;
            //Console.WriteLine(responseLogin);
             
            //accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
         /*   accounturl = "https://www.holypopstore.com/en/account";
            src = Holyreq.Get(accounturl, url, ref cookiesholy, true, accept, false, proxy );
            bool logged = src.Contains("Your account");
            while (!logged)
            {
                Console.WriteLine("ERROR LOGIN IN");
               // Console.WriteLine(src);
                System.Threading.Thread.Sleep(2000);
                tgt = "https://www.holypopstore.com/index.php";
                rfr = "https://www.holypopstore.com/en/account";
                responseLogin = Holyreq.POST(tgt, pstD, rfr, ref cookiesholy, false, proxy);
                src = Holyreq.Get(accounturl, url, ref cookiesholy, true, accept, false, proxy);
                logged = src.Contains("Your account");


            }
            */
            accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            src = Holyreq.Get(url, url, ref cookiesholy, true, accept, false, proxy);
            
           // Console.WriteLine(src);

            string cartSubtotal = new Regex("\"basket-subtotal\">(.*?)<").Match(src).Groups[1].Value;
            while (cartSubtotal != "0.00")
            {
                Console.WriteLine("THERES SOMETHING IN UR CART!!!");
                Console.WriteLine("CART SUBTOTAL:");
                Console.WriteLine(cartSubtotal);

                
                string cartitemid = new Regex("\"stockId\":\"(.*?)\"").Match(src).Groups[1].Value;
                string basketitemid = new Regex("preloadedBasket = [[]{\"id\":\"(.*?)\"").Match(src).Groups[1].Value;
                //Console.WriteLine("itemid");
                //Console.WriteLine(cartitemid);
                //Console.WriteLine("basketitemid");
                //Console.WriteLine(basketitemid);

                string index = "https://www.holypopstore.com/index.php";

                StringBuilder cart = new StringBuilder();
                cart.Append("controller=orders&action=removeStockItemFromBasket&stockItemId=");
                cart.Append(cartitemid);
                cart.Append("&basketItemId=");
                cart.Append(basketitemid);
                cart.Append("&extension=holypop&language=EN&version=228&cookieVersion=204");
                string cartdata = cart.ToString();

                string responsecart = Holyreq.POST(index, cartdata, url, ref cookiesholy, false, proxy);
                //Console.WriteLine(responsecart);

                Console.WriteLine("CLEARING CART!!!");

                src = Get(url, url, ref  cookiesholy, true, accept, false , proxy);

                cartSubtotal = new Regex("\"basket-subtotal\">(.*?)<").Match(src).Groups[1].Value;

            }


            Console.WriteLine("CART CLEARED!!!");






            // string id = new Regex("\"stockItemId\"(.*?)10.5 US").Match(src).Groups[1].Value;



            login(ref cookiesholy, proxy);




            string pid = "-1";
            Regex ItemRegex = new Regex("\"stockItemId\"(.*?)}}}", RegexOptions.Compiled);
            foreach (Match ItemMatch in ItemRegex.Matches(src))
            {
                string line = ItemMatch.Value;
                if (line.Contains(size))
                {
                    pid = line.Substring(15, 15);
                    pid = pid.Substring(0, 5);

                }


            }

            StringBuilder post = new StringBuilder();
            post.Append("controller=orders&action=addStockItemToBasket&stockItemId=");
            post.Append(pid);
            post.Append("&quantity=1&extension=holypop&language=EN&version=265&cookieVersion=239");
            string postData = post.ToString();
            Console.WriteLine(postData);
            string target = "https://www.holypopstore.com/index.php";
            System.Threading.Thread.Sleep(2000);

            string response = Holyreq.POST(target, postData, target, ref cookiesholy, false, proxy);

            Console.WriteLine(response);
            Console.WriteLine("ADDING TO CART...-------------------");

            // Console.WriteLine(src);


            accounturl = "https://www.holypopstore.com/en/account";
            src = Holyreq.Get(accounturl, url, ref cookiesholy, true, accept, false, proxy);
            Console.WriteLine(src);

            cartSubtotal = new Regex("\"basket-subtotal\">(.*?)<").Match(src).Groups[1].Value;
             if(cartSubtotal == "0.00")
            {
                Console.WriteLine("NOT ADDED TO CART!!!");

                return false;

            }
            else
            {
                if(src.Contains("Your account")) Console.WriteLine("ADDED TO CARTA AND LOGGED IN");
                else Console.WriteLine("ADDED TO CART without login :(((");
                return true;
            }

          

            
        }

        public static string checkout(string url, ref CookieContainer cookiesholy , Proxy proxy)
        {
            Console.WriteLine("STARTING CHECKOUT!!!");

            login(ref cookiesholy, proxy, true);

            string orderpage = "https://www.holypopstore.com/en/orders/review";
            string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            Holyreq.Get(orderpage, url,   ref cookiesholy, true, accept, false, proxy, false);
            string orderpage2 = "https://www.holypopstore.com/en/login?redirectTo=https://www.holypopstore.com/en/orders/review";
            //Holyreq.Get(orderpage2, orderpage , ref cookies, true, accept);


            string target = "https://www.holypopstore.com/";
            string postData = "controller=firewall&action=trackRealVisitors&extension=holypop&language=EN&version=228&cookieVersion=204";
            string referer = url;
            string responsesecure = Holyreq.POST(target, postData, referer, ref cookiesholy, false, proxy);
            //Console.WriteLine(responsesecure);

            string src = Holyreq.Get(orderpage, orderpage2,ref cookiesholy, true, accept,false , proxy);

            //Console.WriteLine(src);

            string shipperId =  new Regex("\"shipperId\":\"(.+?)\"").Match(src).Groups[1].Value;
            Console.WriteLine(shipperId);
            string shipperAccountId = new Regex("\"shipperAccountId\":\"(.+?)\"").Match(src).Groups[1].Value;
            Console.WriteLine(shipperAccountId);

            string adressId = new Regex("preloadedAddresses = [[]{\"id\":\"(.+?)\"").Match(src).Groups[1].Value;
            Console.WriteLine(adressId);

            StringBuilder post = new StringBuilder();
            post.Append("secretly=false&hardErrorize=true&billingAddressId=");
            post.Append(adressId);
            post.Append("&shippingAddressId=");
            post.Append(adressId);
            post.Append("&shipmentFlow=DELIVERY&newAddresses=0&requestInvoice=0&notes=&paymentMethodId=12&paymentMethodAccountId=4&shipments%5B0%5D%5BshipmentFlow%5D=DELIVERY&shipments%5B0%5D%5BaddressId%5D=");
            post.Append(adressId);
            post.Append("&shipments%5B0%5D%5BshipperId%5D=");
            post.Append(shipperId);
            post.Append("&shipments%5B0%5D%5BshipperAccountId%5D=");
            post.Append(shipperAccountId);
            post.Append("&toDisplay=0&extension=holypop&controller=orders&action=save&language=EN&version=228&cookieVersion=204");
            postData = post.ToString();
            Console.WriteLine(postData);

            var uri = new Uri("https://www.holypopstore.com/index.php");

            foreach (Cookie cook in cookiesholy.GetCookies(uri))
            {
                Console.WriteLine("Cookie:");
                Console.WriteLine($"{cook.Name} = {cook.Value}");
            }


            target = "https://www.holypopstore.com/index.php";
            string startcheckout = Holyreq.POST(target, postData, orderpage, ref cookiesholy, false, proxy);

            string orderId = new Regex("\"orderId\":\"(.+?)\"").Match(startcheckout).Groups[1].Value;
            Console.WriteLine(startcheckout);

            StringBuilder check = new StringBuilder();
            check.Append("https://www.holypopstore.com/en/orders/checkout/");
            check.Append(orderId);
            check.Append("?paymentMethodId=12&paymentMethodAccountId=4&repay=0");
            string checkurl = check.ToString();
            Console.WriteLine(checkurl);

            accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            string redirect = Holyreq.Get(checkurl, orderpage, ref cookiesholy, true, accept, false , proxy);
           // Console.WriteLine(redirect);

            string atoken =  new Regex("aspx\\?a=(.+?)\\&").Match(redirect).Groups[1].Value;
            string btoken = new Regex("&amp;b=(.+?)\"").Match(redirect).Groups[1].Value;
            Console.WriteLine(atoken);
            Console.WriteLine(btoken);

            StringBuilder redirected = new StringBuilder();
            redirected.Append("https://ecomm.sella.it/pagam/pagam.aspx?a=");
            redirected.Append(atoken);
            redirected.Append("&b=");
            redirected.Append(btoken);
            string firsturl = redirected .ToString();
            Console.WriteLine(firsturl);


            //Program.sendCheckout(firsturl, "", "", "");
            return firsturl;

        }


        public static void finishcheckout(string checkouturl, Proxy proxy, ref CookieContainer cookies)
        {
            CookieContainer cookiespay = new CookieContainer();

            string referer = "https://www.holypopstore.com/it/orders/review";
            string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            string src = Holyreq.Get(checkouturl, referer, ref  cookies, true,accept , false, proxy);

            //Console.WriteLine(src);
            string target = "https://ecomm.sella.it/pagam/swiftPayment.aspx?sesStep=InsD&reqTypeBuy=RCC";
            src = Holyreq.Get(target, checkouturl, ref cookiespay, true, accept, false , proxy);
            //Console.WriteLine(src);


            string viewstate = new Regex("id=\"__VIEWSTATE\" value=\"(.+?)\"").Match(src).Groups[1].Value;
     //       Console.WriteLine("VIEWSTATE =");
    //        Console.WriteLine(viewstate);

            string viewstategenerator = new Regex("id=\"__VIEWSTATEGENERATOR\" value=\"(.+?)\"").Match(src).Groups[1].Value;

   //         Console.WriteLine("VIEWSTATEGENERATOR =");
   //         Console.WriteLine(viewstategenerator);

            string eventvalidator = new Regex("id=\"__EVENTVALIDATION\" value=\"(.+?)\"").Match(src).Groups[1].Value;

       //     Console.WriteLine("eventvalidator =");
       //     Console.WriteLine(eventvalidator);








            string cardnumber = "";
            string cardmonth = "";
            string cardyear = "";
            string cardcvv = "";

            StringBuilder post = new StringBuilder();
            post.Append("__EVENTTARGET=ctl00%24ContentPlaceHolder1%24btnProcedi&__EVENTARGUMENT=&__VIEWSTATE=");
            post.Append(Uri.EscapeDataString(viewstate));
            post.Append("&__VIEWSTATEGENERATOR=");
            post.Append(Uri.EscapeDataString(viewstategenerator));
            post.Append("&__SCROLLPOSITIONX=0&__SCROLLPOSITIONY=0&__EVENTVALIDATION=");
            post.Append(Uri.EscapeDataString(eventvalidator));
            post.Append("&ctl00%24ContentPlaceHolder1%24textPAY1_CHNAME=&ctl00%24ContentPlaceHolder1%24textPAY1_CARDNUMBER=");
            post.Append(Uri.EscapeDataString(cardnumber));
            post.Append("&ctl00%24ContentPlaceHolder1%24txtPAY1_EXPMONTH=");
            post.Append(cardmonth);
            post.Append("&ctl00%24ContentPlaceHolder1%24txtPAY1_EXPYEAR=");
            post.Append(cardyear);
            post.Append("&ctl00%24ContentPlaceHolder1%24textPAY1_CVV=");
            post.Append(cardcvv);
            post.Append("&ctl00%24ContentPlaceHolder1%24textPAY1_CHEMAIL=&ctl00%24ContentPlaceHolder1%24hdCookieCheck=FromPagam&ctl00%24ContentPlaceHolder1%24hidErrorCode=&ctl00%24ContentPlaceHolder1%24IsDCCFeatureEnabled=False&ctl00%24hdnTLSDisposal=TRUE");
            string PostData = post.ToString();

    //        Console.WriteLine("FIRST POSTDATA --------------------------");


            //Console.WriteLine(PostData);

            src = Holyreq.POSTpay(target, PostData, target, cookiespay, true, false, false , proxy);

         //   Console.WriteLine("SOURCE CODE --------------------------");

            //Console.WriteLine(src);


            string previouspage = new Regex("id=\"__PREVIOUSPAGE\" value=\"(.+?)\"").Match(src).Groups[1].Value;

          //  Console.WriteLine("previous =");
       //     Console.WriteLine(previouspage);



             viewstate = new Regex("id=\"__VIEWSTATE\" value=\"(.+?)\"").Match(src).Groups[1].Value;
       //     Console.WriteLine("VIEWSTATE2 = -------------------------");
     //       Console.WriteLine(viewstate);

             viewstategenerator = new Regex("id=\"__VIEWSTATEGENERATOR\" value=\"(.+?)\"").Match(src).Groups[1].Value;

          //  Console.WriteLine("VIEWSTATEGENERATOR2 =--------------------");
         //   Console.WriteLine(viewstategenerator);

             eventvalidator = new Regex("id=\"__EVENTVALIDATION\" value=\"(.+?)\"").Match(src).Groups[1].Value;
         //   Console.WriteLine("eventvalidator2 = ---------------------------");
        //    Console.WriteLine(eventvalidator);

            StringBuilder post1 = new StringBuilder();
            post1.Append("__EVENTTARGET=ctl00%24ContentPlaceHolder1%24btnProceedi&__EVENTARGUMENT=&__VIEWSTATE=");
            post1.Append(Uri.EscapeDataString(viewstate));
            post1.Append("&__VIEWSTATEGENERATOR=");
            post1.Append(Uri.EscapeDataString(viewstategenerator));
            post1.Append("&__SCROLLPOSITIONX=0&__SCROLLPOSITIONY=0&__PREVIOUSPAGE=");
            post1.Append(Uri.EscapeDataString(previouspage));
            post1.Append("&__EVENTVALIDATION=");
            post1.Append(Uri.EscapeDataString(eventvalidator));
            post1.Append("&ctl00%24hdnTLSDisposal=TRUE");
            string PostData1 = post1.ToString();

           // Console.WriteLine("PostData1--------------------------");
            //Console.WriteLine(PostData1);
            //referer = 
            target = "https://ecomm.sella.it/pagam/DataSummay.aspx?sesStep=Riep&reqTypeBuy=RCC";
            src = Holyreq.POSTpay(target, PostData1, target,  cookiespay, true, false, false , proxy);
           // Console.WriteLine(src);




            viewstate = new Regex("id=\"__VIEWSTATE\" value=\"(.+?)\"").Match(src).Groups[1].Value;
           //Console.WriteLine("VIEWSTATE3 = -------------------------");
          // Console.WriteLine(viewstate);

            viewstategenerator = new Regex("id=\"__VIEWSTATEGENERATOR\" value=\"(.+?)\"").Match(src).Groups[1].Value;
          //  Console.WriteLine("VIEWSTATEGENERATOR3 =--------------------");
          //Console.WriteLine(viewstategenerator);



            eventvalidator = new Regex("id=\"__EVENTVALIDATION\" value=\"(.+?)\"").Match(src).Groups[1].Value;

            Console.WriteLine("eventvalidator3 = ---------------------------");
            Console.WriteLine(eventvalidator);


            string pareq = new Regex("id=\"PaReq\" value=\"(.+?)\"", RegexOptions.Singleline ).Match(src).Groups[1].Value;

           // Console.WriteLine("pareq = ---------------------------");
            //Console.WriteLine(pareq);

            string md = new Regex("id=\"MD\" value=\"(.+?)\"").Match(src).Groups[1].Value;

            //Console.WriteLine("md = ---------------------------");
            //Console.WriteLine(md);

            StringBuilder post2 = new StringBuilder();
            post2.Append("__VIEWSTATE=");
            post2.Append(Uri.EscapeDataString(viewstate));
            post2.Append("&__VIEWSTATEGENERATOR=");
            post2.Append(Uri.EscapeDataString(viewstategenerator));
            post2.Append("&__SCROLLPOSITIONX=0&__SCROLLPOSITIONY=0&__EVENTTARGET=&__EVENTARGUMENT=&__EVENTVALIDATION=");
            post2.Append(Uri.EscapeDataString(eventvalidator));
            post2.Append("&PaReq=");
            post2.Append(Uri.EscapeDataString(pareq));
            post2.Append("&MD=");
            post2.Append(Uri.EscapeDataString(md));
            post2.Append("&TermUrl=https%3A%2F%2Fecomm.sella.it%2Fpagam%2Fpagam3Do.aspx&PAY1_TRANSKEY=");
            string PostData2 = post2.ToString();
            Console.WriteLine(PostData2);

            referer = "https://ecomm.sella.it/pagam/DataSummay.aspx?sesStep=Riep&reqTypeBuy=RCC";
            target = "https://idcheck.acs.touchtechpayments.com/v1/payerAuthentication";

            src = Holyreq.POSTpay(target, PostData2 ,referer  ,  cookiespay, false, true, false , proxy);
            Console.WriteLine(src);




             string pares = new Regex("var pares = \"(.+?)\"", RegexOptions.Singleline).Match(src).Groups[1].Value;

            Console.WriteLine("pares = ---------------------------");
            Console.WriteLine(pares);

             //string md = new Regex("id=\"MD\" value=\"(.+?)\"").Match(src).Groups[1].Value;

            //Console.WriteLine("md2 = ---------------------------");
            //Console.WriteLine(md2);

            StringBuilder post3 = new StringBuilder();
            post3.Append("MD=");
            post3.Append(Uri.EscapeDataString(md));
            post3.Append("&PaRes=");
            post3.Append(Uri.EscapeDataString(pares));

            string PostData3 = post3.ToString();
            Console.WriteLine(PostData3);


            target = "https://ecomm.sella.it/pagam/pagam3Do.aspx";
            referer = "https://idcheck.acs.touchtechpayments.com/v1/payerAuthentication";
            src = Holyreq.POSTpay(target, PostData2, referer,  cookiespay, false, false, true , proxy);
            Console.WriteLine(src);














        }




















    }
}

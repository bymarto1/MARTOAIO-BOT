using System;
using System.Net;
using System.IO;
using CloudflareSolverRe;
using static MARTOAIO.Program;
using System.Linq;

namespace MARTOAIO
{
    public class Holyreq
    {

            public static string Get(string url, string referer,  ref CookieContainer cookies, bool upgrade, string accept, bool redirect , Proxy proxy, bool cfsolved = true)
            {



            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            IWebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
            string username = proxy.username;
            string password = proxy.password;
            myProxy.Credentials = new NetworkCredential(username, password);
            req.Proxy = myProxy;


            req.Credentials = CredentialCache.DefaultCredentials;

            /*Console.WriteLine(proxy.ip);
            Console.WriteLine(proxy.port);
            Console.WriteLine(proxy.username);
            Console.WriteLine(proxy.password);*/

            if (cfsolved == false)
            {
                var cf = new CloudflareSolver
                {
                    MaxTries = 10,
                    ClearanceDelay = 3000
                };

                var target = new Uri("https://www.holypopstore.com/en/footwear-men/nike/nike-react-vision/10386");
                
                var result = cf.Solve(target, userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36" , myProxy).Result;

                if (!result.Success)
                {
                    Console.WriteLine($"[Failed] Details: {result.FailReason}");
                    return "failed";
                }
                else
                {
                        cfsolved = true;
                    if (result.Cookies.AsHeaderString()!="") {
                        CookieCollection cookieCollection = result.Cookies.AsCookieCollection();
                        cookies.Add(cookieCollection);
                    }
                    Console.WriteLine("solved cf");
                }
                 

                req.Headers.Add(HttpRequestHeader.Cookie, result.Cookies.AsHeaderString());
                req.Headers.Add(HttpRequestHeader.UserAgent, result.UserAgent);
                Console.WriteLine(result.Cookies.AsHeaderString());
            }

            else
            {
                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";

                req.CookieContainer = cookies;


            }


            req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli; 

            req.Method = "GET";

            req.Headers["authority"] = "www.holypopstore.com";
            req.Headers["origin"] = "https://www.holypopstore.com";

            //req.Headers["path"] = "";  AFEGIR SI FA FALTA!
            req.Headers["scheme"] = "https";

            req.Accept = accept;
            req.Headers["accept-encoding"] = "gzip, deflate, br";
            req.Headers["accept-language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";


            req.Referer = referer;
            req.Headers["cache-control"] = "max-age=0";

            req.Headers["sec-fetch-dest"] = "document";
            req.Headers["sec-fetch-mode"] = "navigate";
            req.Headers["sec-fetch-site"] = "none";
            req.Headers["sec-fetch-user"] = "?1";

            if (upgrade)
            {
                req.Headers["upgrade-insecure-requests"] = "1";
            }



            //req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";


            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            cookies.Add(resp.Cookies);

            string returnUrl = "";

            if (redirect == true)
            {
                Console.Write((int)resp.StatusCode);

                returnUrl = resp.GetResponseHeader("location");
                return returnUrl;
            }

            /*  if (resp.StatusCode == HttpStatusCode.OK)
              {
                  // Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                  //                    resp.StatusDescription);
              }*/
            else
            {
                string pageSrc;
                using (StreamReader sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    pageSrc = sr.ReadToEnd();
                }
                return pageSrc;
            }

        }







         


        public static string POST(String _target, string postData, String referer,  ref CookieContainer cookies, bool redirect , Proxy proxy , bool cfsolved = true)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_target);

            IWebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
            string username = proxy.username;
            string password = proxy.password;
            myProxy.Credentials = new NetworkCredential(username, password);
            req.Proxy = myProxy;

            //req.CookieContainer = cookies;

            if (cfsolved == false)
            {
                var cf = new CloudflareSolver
                {
                    MaxTries = 3,
                    ClearanceDelay = 3000
                };

                var target = new Uri("https://www.holypopstore.com/en/footwear-men/nike/nike-react-vision/10386");

                var result = cf.Solve(target, userAgent: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36", myProxy).Result;
                if (!result.Success)
                {
                    Console.WriteLine($"[Failed] Details: {result.FailReason}");
                    return "failed";
                }

                var cookieCollection = result.Cookies.AsCookieCollection();
                cookies.Add(cookieCollection);

                req.Headers.Add(HttpRequestHeader.Cookie, result.Cookies.AsHeaderString());
                req.Headers.Add(HttpRequestHeader.UserAgent, result.UserAgent);

                Console.WriteLine(result.Cookies.AsHeaderString());
            }
            else
            {
                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";

                req.CookieContainer = cookies;


            }




            req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli;

            req.Method = "POST";
            req.Headers["authority"] = "www.holypopstore.com";
            req.Headers["origin"] = "https://www.holypopstore.com";
            req.Headers["scheme"] = "https";

            req.Accept = "application/json, text/javascript, */*; q=0.01";
            req.Headers["accept-encoding"] = "gzip, deflate, br";
            req.Headers["accept-language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";
            req.ContentLength = postData.Length;
            // Console.WriteLine(postData.Length);
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            //req.CookieContainer = cookies;
            req.Referer = referer;

            req.Headers["sec-fetch-dest"] = "empty";
            req.Headers["sec-fetch-site"] = "same-origin";
            req.Headers["sec-fetch-mode"] = "cors";



            //req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";


            req.Headers["x-requested-with"] = "XMLHttpRequest";

            if (redirect)
            {
                req.AllowAutoRedirect = true;
            }
            else req.AllowAutoRedirect = false;

            StreamWriter writePost = new StreamWriter(req.GetRequestStream());
            writePost.Write(postData);
            writePost.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            if (res.StatusCode == HttpStatusCode.OK)
            {
                // Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                //    res.StatusDescription);
            }
            StreamReader readResponse = new StreamReader(res.GetResponseStream());
            String finalResponse = readResponse.ReadToEnd();

            readResponse.Close();
            //Console.WriteLine(finalResponse);

            return finalResponse;

        }











        public static string Getpay(string url, string referer,  CookieContainer cookies, bool upgrade, string accept, bool redirect, Proxy proxy)
        {



            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);


            IWebProxy myProxy = new WebProxy("51.89.177.98", 33128);
            string username = "yQauvecu!a85";
            string password = "XinwcSFK";
            myProxy.Credentials = new NetworkCredential(username, password);
            req.Proxy = myProxy;



            req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli;

            req.Method = "GET";
            req.CookieContainer = cookies;

            req.Headers["Host"] = "ecomm.sella.it";
            //req.Headers["origin"] = "https://www.holypopstore.com";

            //req.Headers["path"] = "";  AFEGIR SI FA FALTA!
            //req.Headers["scheme"] = "https";

            req.Accept = accept;
            req.Headers["Accept-encoding"] = "gzip, deflate, br";
            req.Headers["Accept-language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";
            req.Headers["Connection"] = "keep-alive";


            req.Referer = referer;
            req.Headers["Sec-Fetch-Dest"] = "document";
            req.Headers["Sec-Fetch-Mode"] = "navigate";
            req.Headers["Sec-Fetch-Site"] = "same-origin";
            req.Headers["Sec-Fetch-User"] = "?1";

            if (upgrade)
            {
                req.Headers["Upgrade-Insecure-Requests"] = "1";
            }



            req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";


            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            cookies.Add(resp.Cookies);

            string returnUrl = "";

            if (redirect == true)
            {
                Console.Write((int)resp.StatusCode);

                returnUrl = resp.GetResponseHeader("location");
                return returnUrl;
            }

            /*  if (resp.StatusCode == HttpStatusCode.OK)
              {
                  // Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                  //                    resp.StatusDescription);
              }*/
            else
            {
                string pageSrc;
                using (StreamReader sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    pageSrc = sr.ReadToEnd();
                }
                return pageSrc;
            }

        }










        public static string POSTpay(String _target, string postData, String referer,  CookieContainer cookies, bool redirect, bool last, bool lastlast , Proxy proxy)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_target);

            IWebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
            string username = proxy.username;
            string password = proxy.password;
            myProxy.Credentials = new NetworkCredential(username, password);
            req.Proxy = myProxy;









            req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli;

            req.Method = "POST";
            ;

            if (last == false && lastlast==false)
            {
                req.Headers["Host"] = "ecomm.sella.it";
                req.Headers["Origin"] = "https://ecomm.sella.it";
                //req.Headers["scheme"] = "https";

                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                req.Headers["Accept-Encoding"] = "gzip, deflate, br";
                req.Headers["Accept-Language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";
                req.ContentLength = postData.Length;
                // Console.WriteLine(postData.Length);
                req.ContentType = "application/x-www-form-urlencoded";

                req.CookieContainer = cookies;
                req.Referer = referer;
                req.Headers["Cache-Control"] = "max-age=0";

                req.Headers["Sec-Fetch-Dest"] = "document";
                req.Headers["Sec-Fetch-Site"] = "same-origin";
                req.Headers["Sec-Fetch-Mode"] = "navigate";

                req.Headers["Connection"] = "keep-alive";

                req.Headers["Upgrade-Insecure-Requests"] = "1";



                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";
            }

            
            else if  (last == true){ 
                req.Headers["authority"] = "idcheck.acs.touchtechpayments.com";
                req.Headers["origin"] = "https://ecomm.sella.it";

                req.Headers["scheme"] = "https";

                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                req.Headers["accept-encoding"] = "gzip, deflate, br";
                req.Headers["accept-language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";
                req.ContentLength = postData.Length;
                // Console.WriteLine(postData.Length);
                req.ContentType = "application/x-www-form-urlencoded";

                //req.CookieContainer = cookies;
                req.Referer = referer;
                req.Headers["Cache-Control"] = "max-age=0";

                req.Headers["sec-fetch-dest"] = "document";
                req.Headers["sec-fetch-site"] = "cross-site";
                req.Headers["sec-fetch-mode"] = "navigate";

                //req.Headers["Connection"] = "keep-alive";

                req.Headers["upgrade-insecure-requests"] = "1";



                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";


            }
            else if(lastlast == true)
            {
                req.Headers["Host"] = "ecomm.sella.it";
                req.Headers["Origin"] = "https://idcheck.acs.touchtechpayments.com";
                //req.Headers["scheme"] = "https";

                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
                req.Headers["Accept-Encoding"] = "gzip, deflate, br";
                req.Headers["Accept-Language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";
                req.ContentLength = postData.Length;
                // Console.WriteLine(postData.Length);
                req.ContentType = "application/x-www-form-urlencoded";

                req.CookieContainer = cookies;
                req.Referer = referer;
                req.Headers["Cache-Control"] = "max-age=0";

                req.Headers["Sec-Fetch-Dest"] = "document";
                req.Headers["Sec-Fetch-Site"] = "cross-site";
                req.Headers["Sec-Fetch-Mode"] = "navigate";

                req.Headers["Connection"] = "keep-alive";

                req.Headers["Upgrade-Insecure-Requests"] = "1";



                req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";


            }





            //req.Headers["x-requested-with"] = "XMLHttpRequest";

            if (redirect)
            {
                req.AllowAutoRedirect = true;
            }
            else req.AllowAutoRedirect = false;

            StreamWriter writePost = new StreamWriter(req.GetRequestStream());
            writePost.Write(postData);
            writePost.Close();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            if (res.StatusCode == HttpStatusCode.OK)
            {
                // Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                //    res.StatusDescription);
            }
            StreamReader readResponse = new StreamReader(res.GetResponseStream());
            String finalResponse = readResponse.ReadToEnd();

            readResponse.Close();
            //Console.WriteLine(finalResponse);

            return finalResponse;

        }








    }
}

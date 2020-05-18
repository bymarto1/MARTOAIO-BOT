using System;
using System.Net;
using System.IO;
using static MARTOAIO.Program;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using CloudflareSolverRe;

public class requests
{

    public static async Task<string> Get(string url, bool upgrade, HttpClient client)
    {

        try
        {
            using (var reqmes = new HttpRequestMessage(HttpMethod.Get, url))
            {

                /* 
                reqmes.Headers.Add("authority", "www.snipes.es");
                reqmes.Headers.Add("origin", "https://www.snipes.es");
                reqmes.Headers.Add("scheme", "https");
                */
                //reqmes.Headers.Add("accept", accept);
                reqmes.Headers.Add("accept-encoding", "gzip, deflate, br");

                reqmes.Headers.Add("accept-language", "es,ca;q=0.9,en;q=0.8,de;q=0.7");
                // reqmes.Headers.Add("referer", referer);
                reqmes.Headers.Add("sec-fetch-dest", "document");

                reqmes.Headers.Add("sec-fetch-mode", "navigate");
                reqmes.Headers.Add("sec-fetch-site", "same-origin");
                reqmes.Headers.Add("sec-fetch-user", "?1");
                //  reqmes.Headers.Add("Cache-Control", "no-cache");
                //reqmes.Headers.Add("UserAgent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");


                HttpResponseMessage resp = await client.SendAsync(reqmes);
                // string responseBody = await resp.Content.ReadAsStringAsync();
                // return responseBody;
                using (var streamReader = new StreamReader(await resp.Content.ReadAsStreamAsync()))
                {
                    return await streamReader.ReadToEndAsync();
                }
       

            }

        }/*
        catch (WebException webExcp)
        {
            Console.WriteLine("A WebException has been caught.");
            Console.WriteLine(webExcp.ToString());
            WebExceptionStatus status = webExcp.Status;

            if (status == WebExceptionStatus.ProtocolError)
            {
                Console.Write("The server returned protocol error ");
                HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                Console.WriteLine((int)httpResponse.StatusCode + " - "
                   + httpResponse.StatusCode);
                Console.Write(url);

            }
            return "";
        }*/
        catch (Exception e)
        {
            Console.WriteLine("{0} Exception caught.", e);
            return "";
        }


    }



    public static async Task<string> POST(String _target, string postData, bool redirect, HttpClient client, bool read = true)
    {
        try
        {



            using (var request = new HttpRequestMessage(HttpMethod.Post, _target))
            {
                /*   request.Headers.Add("authority", "www.snipes.es");
                   request.Headers.Add("origin", "https://www.snipes.es");
                   request.Headers.Add("scheme", "https");
                   */
                //request.Headers.Add("accept", "application/json, text/javascript, */*; q=0.01");
                request.Headers.Add("accept-encoding", "gzip, deflate, br");
                request.Headers.Add("accept-language", "es,ca;q=0.9,en;q=0.8,de;q=0.7");
                request.Headers.Add("ContentLength", postData.Length.ToString());
                request.Headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
                request.Headers.Add("sec-fetch-dest", "empty");
                request.Headers.Add("sec-fetch-site", "same-origin");
                request.Headers.Add("sec-fetch-mode", "cors");
                //request.Headers.Add("UserAgent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                request.Headers.Add("x-requested-with", "XMLHttpRequest");
                //request.Headers.Add("Cache-Control", "no-cache");


                using (var stringContent = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"))
                {
                    request.Content = stringContent;
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (read == true)
                    {
                        using (var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }
                    else return "";
                }
            }

            //Console.


        }
        /*    catch (WebException webExcp)
          {
              // If you reach this point, an exception has been caught.  
              Console.WriteLine("A WebException has been caught.");
              // Write out the WebException message.  
              Console.WriteLine(webExcp.ToString());
              // Get the WebException status code.  
              WebExceptionStatus status = webExcp.Status;
              // If status is WebExceptionStatus.ProtocolError,
              //   there has been a protocol error and a WebResponse
              //   should exist. Display the protocol error.  
              if (status == WebExceptionStatus.ProtocolError)
              {
                  Console.Write("The server returned protocol error ");
                  // Get HttpWebResponse so that you can check the HTTP status code.  
                  HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                  Console.WriteLine((int)httpResponse.StatusCode + " - "
                     + httpResponse.StatusCode);
                  Console.Write(_target);
              }
              return "";
          }
          */
        catch (Exception e)
        {
            Console.WriteLine("{0} Exception caught.", e);

            return "";
        }
    }










    public static string Getpay(string url, string referer, ref CookieContainer cookiespay, bool upgrade, string accept, bool redirect, Proxy proxy)
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        if (proxy.proxyless == false)
        {
            IWebProxy myProxy = new WebProxy(proxy.ip, proxy.port);
            string username = proxy.username;
            string password = proxy.password;
            myProxy.Credentials = new NetworkCredential(username, password);
            req.Proxy = myProxy;
        }



        req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

        req.Method = "GET";
        req.CookieContainer = cookiespay;

        req.Headers["Host"] = "www.saferpay.com";
        req.Headers["Cache-Control"] = "max-age=0";
        req.Headers["Connection"] = "keep-alive";





        req.Accept = accept;
        req.Headers["Accept-Encoding"] = "gzip, deflate, br";
        req.Headers["Accept-Language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";


        req.Referer = referer;
        req.Headers["Sec-Fetch-Dest"] = "document";
        req.Headers["Sec-Fetch-Mode"] = "navigate";
        req.Headers["Sec-Fetch-Site"] = "cross-site";
        req.Headers["Sec-Fetch-User"] = "?1";


        req.Headers["Upgrade-Insecure-Requests"] = "1";

        if (redirect)
        {
            req.AllowAutoRedirect = true;
        }



        req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        cookiespay.Add(resp.Cookies);


        if (resp.StatusCode == HttpStatusCode.OK)
        {
            // Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
            //            resp.StatusDescription);
        }
        else Console.Write("ERROR ON GET REQUEST");

        string pageSrc;
        using (StreamReader sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8))
        {
            pageSrc = sr.ReadToEnd();
        }

        return pageSrc;


    }




    public static string POSTpay(String _target, string postData, String referer, ref CookieContainer cookiespay, bool redirect, Proxy proxy)
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_target);
        req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

        req.Method = "POST";
        req.Headers["Host"] = " www.saferpay.com";
        req.Headers["Origin"] = "https://www.saferpay.com";

        req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
        req.Headers["Accept-Encoding"] = "gzip, deflate, br";
        req.Headers["Accept-Language"] = "es,ca;q=0.9,en;q=0.8,de;q=0.7";
        req.Headers["Cache-Control"] = "max-age=0";
        req.Headers["Connection"] = "keep-alive";


        req.ContentLength = postData.Length;
        // Console.WriteLine(postData.Length);
        req.ContentType = "application/x-www-form-urlencoded";

        req.CookieContainer = cookiespay;
        req.Referer = referer;

        req.Headers["Sec-Fetch-Dest"] = "document";
        req.Headers["Sec-Fetch-Mode"] = "navigate";
        req.Headers["Sec-Fetch-Site"] = "same-origin";
        req.Headers["Upgrade-Insecure-Requests"] = "1";





        req.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";



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


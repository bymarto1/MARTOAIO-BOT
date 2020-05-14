
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
namespace MARTOAIO
{
    public class solerequests
    {



        public static async Task<string> Get(string url, HttpClient client,string referer = "" ,bool read = true)
        {

            try
            {
                using (var reqmes = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    /*   reqmes.Headers.Add("authority", "www.snipes.es");
                       reqmes.Headers.Add("origin", "https://www.snipes.es");
                       reqmes.Headers.Add("scheme", "https");
                       */
                    //reqmes.Headers.Add("accept", accept);
                    reqmes.Headers.Add("accept-encoding", "gzip, deflate, br");
                    reqmes.Headers.Add("accept-language", "es,ca;q=0.9,en;q=0.8,de;q=0.7");
                    reqmes.Headers.Add("sec-fetch-dest", "document");
                    reqmes.Headers.Add("sec-fetch-mode", "navigate");
                    reqmes.Headers.Add("sec-fetch-site", "same-origin");
                    if(referer !="") reqmes.Headers.Add("referer", referer);
                   
                    //  reqmes.Headers.TryAddWithoutValidation("content-type", "application/json");
                    //reqmes.Headers.Add("sec-fetch-user", "?1");
                    //  reqmes.Headers.Add("Cache-Control", "no-cache");
                    //reqmes.Headers.Add("UserAgent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");

                    var resp =  await client.SendAsync(reqmes, HttpCompletionOption.ResponseHeadersRead);
                    if (read == true)
                    {
                        var responseBody = await resp.Content.ReadAsStreamAsync();
                        var sr = new StreamReader(responseBody);
                        return await sr.ReadToEndAsync();


                    }
                    else return "";

                }
 




            }
           /* catch (WebException webExcp)
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











        public static async Task<string> Post(String _target, string postData, bool redirect, HttpClient client, string referer = "", bool read = true)
        {
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                //response = new HttpResponseMessage();
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
                    //request.Headers.Add("content-length", postData.Length.ToString());
                    //request.Headers.Add("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
                    if (referer != "")
                    {
                        request.Headers.Add("referer", referer);
                    }
                    request.Headers.Add("sec-fetch-dest", "empty");
                    request.Headers.Add("sec-fetch-mode", "cors");
                    request.Headers.Add("sec-fetch-site", "same-origin");
                    //request.Headers.Add("UserAgent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
                    request.Headers.Add("x-requested-with", "XMLHttpRequest");

                    using (var stringContent = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded"))
                    {
                        request.Content = stringContent;
                        var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                        if (read == true)
                        {
                            /*var responseBody = await response.Content.ReadAsStreamAsync();
                            var sr = new StreamReader(responseBody);
                            return await sr.ReadToEndAsync();
                            */
                            return await response.Content.ReadAsStringAsync();
                        }
                        else return "";
                    }
                }


            }


           /* catch (WebException webExcp)
            {
                Console.WriteLine("A WebException has been caught.");
                Console.WriteLine(webExcp.ToString());
                WebExceptionStatus status = webExcp.Status;
                // If status is WebExceptionStatus.ProtocolError,
                //   there has been a protocol error and a WebResponse
                //   should exist. Display the protocol error.  
                if (status == WebExceptionStatus.ProtocolError)
                {
                    Console.Write("The server returned protocol error ");
                    HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                    Console.WriteLine((int)httpResponse.StatusCode + " - "
                       + httpResponse.StatusCode);
                    Console.Write(_target);
                }
                return "";
            }*/
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);

                return "";
            }
        }






        
    }
}

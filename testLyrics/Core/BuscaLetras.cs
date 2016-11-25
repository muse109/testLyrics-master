using HtmlAgilityPack;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testLyrics.Core
{
    class BuscaLetras
    {


        public  async Task<HtmlDocument> PlyricsAsync(String url, String artist, String song)
        {
            String html;


            var Url = "http://www.plyrics.com/lyrics/armorforsleep/basementghostsinging.html";



          var  htmlArtist = rgx(artist, "[\\s'\"-]", "[^A-Za-z0-9]");
            var htmlSong = rgx(song, "[\\s'\"-]", "[^A-Za-z0-9]");
            

            if (htmlArtist.ToLower().StartsWith("the"))
                htmlArtist = htmlArtist.Substring(3);

            var urlString = String.Format("http://www.plyrics.com/lyrics/{0}/{1}.html",
                    htmlArtist.ToLower(), htmlSong.ToLower());

            var paginavalida = false;

            using (var client = new ClienteWeb())
            {
                client.HeadOnly = true;
                try
                {
                    string s1 = client.DownloadString(urlString);

                    paginavalida = true;
                }
                catch (Exception ex)
                {
                    paginavalida = false;
                  
                }
              
               
            }


            if (paginavalida)
            {

                var web = new HtmlWeb();
                var page = web.LoadFromWebAsync(urlString);


                return await page;
            }
            else {
                return null;
            }

        
        }


        public string rgx(string texto, string patron, string patron2)
        {


            var rgx1 = new Regex(patron);
            var result = rgx1.Replace(texto, "");
            result = result.Replace("&", "and");

            rgx1 = new Regex(patron2);
            result = rgx1.Replace(result, "");

            return result;

        }


        class ClienteWeb : WebClient
        {
            public bool HeadOnly { get; set; }
            protected override WebRequest GetWebRequest(Uri address)
            {
                var req = base.GetWebRequest(address);
                if (HeadOnly && req.Method == "GET")
                {
                    req.Method = "HEAD";
                }
                return req;
            }
        }



    }
}
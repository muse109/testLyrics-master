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


       



          var  htmlArtist = rgx(artist, "[\\s'\"-]", "[^A-Za-z0-9]");
            var htmlSong = rgx(song, "[\\s'\"-]", "[^A-Za-z0-9]");
            

            if (htmlArtist.ToLower().StartsWith("the"))
                htmlArtist = htmlArtist.Substring(3);

            var urlvalida = ValidaUrl(htmlArtist,htmlSong);


            if (urlvalida != "-1")
            {

                var web = new HtmlWeb();
                var page = web.LoadFromWebAsync(urlvalida);


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


        public string ValidaUrl(string artista, string cancion) {

            var urlString = "";
            var pages = new string[] 
            { "http://www.plyrics.com/lyrics/{0}/{1}.html",
                "http://www.alivelyrics.com/{2}/{0}/{1}.html" };


            foreach (var item in pages)
            {
              urlString = String.Format(item,artista.ToLower(), cancion.ToLower(), artista[0].ToString().ToLower());

                using (var client = new ClienteWeb())
                {
                    client.HeadOnly = true;
                    try
                    {
                        var s1 = client.DownloadString(urlString);

                        break;

                    }
                    catch (Exception ex)
                    {
                        urlString = "-1";

                    }


                }

            }
            return urlString;

        }

    }
}
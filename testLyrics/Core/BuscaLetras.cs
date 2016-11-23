using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace testLyrics.Core
{
    class BuscaLetras
    {


        public string Plyrics(String url, String artist, String song)
        {
            String html;


            var Url = "http://www.plyrics.com/lyrics/armorforsleep/basementghostsinging.html";
            var web = new HtmlWeb();


            var htmlArtist = artist.Replace("[\\s'\"-]", "").Replace("&", "and").Replace("[^A-Za-z0-9]", "");
            var htmlSong = song.Replace("[\\s'\"-]", "").Replace("&", "and").Replace("[^A-Za-z0-9]", "");


            var pattern = @"(\w+)'(\w+)";
            var replacement = "$1''$2";
            var input = "EXECUTE PROCEDURE sp_procedurename ('1', 'test', 'tester's code', '0')";
            var result = Regex.Replace(input, pattern, replacement);

            if (htmlArtist.ToLower().StartsWith("the"))
                htmlArtist = htmlArtist.Substring(3);

            var urlString = String.Format("http://www.plyrics.com/lyrics/{0}/{1}.html",
                    htmlArtist.ToLower(),htmlSong.ToLower());



            var page = web.LoadFromWebAsync(Url);

            
            var e= page.Result;
            //try
            //{
            //    Document document = Jsoup.connect(url).userAgent(Net.USER_AGENT).get();
            //    if (document.location().contains(domain))
            //        html = document.html();
            //    else
            //        throw new IOException("Redirected to wrong domain " + document.location());
            //}
            //catch (HttpStatusException e)
            //{
            //    return new Lyrics(Lyrics.NO_RESULT);
            //}
            //catch (IOException e)
            //{
            //    e.printStackTrace();
            //    return new Lyrics(Lyrics.ERROR);
            //}
            //Pattern p = Pattern.Compile(
            //        "<!-- start of lyrics -->(.*)<!-- end of lyrics -->",
            //        Pattern.DOTALL);
            //Matcher matcher = p.matcher(html);

            //if (artist == null || song == null)
            //{
            //    Pattern metaPattern = Pattern.compile(
            //            "ArtistName = \"(.*)\";\r\nSongName = \"(.*)\";\r\n",
            //            Pattern.DOTALL);
            //    Matcher metaMatcher = metaPattern.matcher(html);
            //    if (metaMatcher.find())
            //    {
            //        artist = metaMatcher.group(1);
            //        song = metaMatcher.group(2);
            //        song = song.substring(0, song.indexOf('"'));
            //    }
            //    else
            //        artist = song = "";
            //}

            //if (matcher.find())
            //{
            //    Lyrics l = new Lyrics(Lyrics.POSITIVE_RESULT);
            //    l.setArtist(artist);
            //    String text = matcher.group(1);
            //    text = text.replaceAll("\\[[^\\[]*\\]", "");
            //    l.setText(text);
            //    l.setTitle(song);
            //    l.setURL(url);
            //    l.setSource("PLyrics");
            //    return l;
            //}
            //else
            //    return new Lyrics(Lyrics.NEGATIVE_RESULT);

            return "";
        }
    }
}
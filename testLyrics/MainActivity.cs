using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Util;
using Android.Widget;
using testLyrics.Core;
using System;
using System.Text.RegularExpressions;

namespace testLyrics
{
    [Activity(Label = "testLyrics", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static bool pausa { get; set; }
        public static bool receptorRegistrado { get; set; }
        mReceiver Receptor = new mReceiver();

        public static bool DatosSeteados { get; set; }

        protected override void OnPause()
        {
            base.OnPause();

            pausa = true;


            if (receptorRegistrado)
            {
                UnregisterReceiver(Receptor);
                Receptor = null;
                receptorRegistrado = false;
            }


        }

        protected override void OnResume()
        {
            base.OnResume();
            pausa = false;

            if (!receptorRegistrado)
            {
                if (Receptor == null)
                    Receptor = new mReceiver();
                RegistrarReceptor();
                receptorRegistrado = true;
            }

        }

        public void RegistrarReceptor() {

            var IntentFilter = new IntentFilter();
            IntentFilter.AddAction("com.android.music.metachanged");
            IntentFilter.AddAction("com.htc.music.metachanged");
            IntentFilter.AddAction("com.miui.player.metachanged");
            IntentFilter.AddAction("com.real.IMP.metachanged");
            IntentFilter.AddAction("com.sonyericsson.music.metachanged");
            IntentFilter.AddAction("com.rdio.android.playstatechanged");
            IntentFilter.AddAction("com.samsung.sec.android.MusicPlayer.metachanged");
            IntentFilter.AddAction("com.sec.android.app.music.metachanged");
            IntentFilter.AddAction("com.nullsoft.winamp.metachanged");
            IntentFilter.AddAction("com.amazon.mp3.metachanged");
            IntentFilter.AddAction("com.rhapsody.metachanged");
            IntentFilter.AddAction("com.maxmpz.audioplayer.metachanged");
            IntentFilter.AddAction("com.real.IMP.metachanged");
            IntentFilter.AddAction("com.andrew.apollo.metachanged");
            IntentFilter.AddAction("fm.last.android.metachanged");
           // IntentFilter.AddAction("com.adam.aslfms.notify.playstatechanged");
            IntentFilter.AddAction("com.tbig.playerpro.metachanged");
            IntentFilter.AddAction("net.jjc1138.android.scrobbler.action.MUSIC_STATUS");
            IntentFilter.AddAction("com.spotify.music.metadatachanged");
            var registro = RegisterReceiver(Receptor, IntentFilter);
          

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            RegistrarReceptor();           


            //AudioManager Manager = (AudioManager)this.GetSystemService(Context.AudioService);
            //if (Manager.IsMusicActive)
            //{

            //    // do something - or do it not
            //}
            //http://stackoverflow.com/questions/25215878/how-to-update-the-ui-of-activity-from-broadcastreceiver




        }

       

        [BroadcastReceiver]
        public class mReceiver : BroadcastReceiver
        {
            public override async void OnReceive(Context context, Intent intent)
            {
                if (!pausa)
                {

                    var action = intent.Action;
                    var cmd = intent.GetStringExtra("command");
                    Log.Verbose("mIntentReceiver.onReceive ", action + " / " + cmd);
                    var lyric = intent.GetStringExtra("lyric");
                    var artist1 = intent.GetStringExtra(MediaStore.Audio.AudioColumns.Artist);
                    var album1 = intent.GetStringExtra(MediaStore.Audio.AlbumColumns.Album);
                    var track1 = intent.GetStringExtra(MediaStore.Audio.AudioColumns.Track);
                    var duration1 = intent.GetLongExtra(MediaStore.Audio.AudioColumns.Duration, 0);

                    var duracion = "";

                    var time = duration1;
                    var seconds = time / 1000;
                    var minutes = seconds / 60;
                    seconds = seconds % 60;

                    if (seconds < 10)
                    {
                        var csongs_duration = minutes.ToString() + ":0" + seconds.ToString();
                        duracion = csongs_duration;
                    }
                    else
                    {
                        var csongs_duration = minutes.ToString() + ":" + seconds.ToString();
                        duracion = csongs_duration;
                    }

                    var txtArtista = ((Activity)context).FindViewById<TextView>(Resource.Id.txtArtista);
                    var txtCancion = ((Activity)context).FindViewById<TextView>(Resource.Id.txtCancion);
                    var txtAlbum = ((Activity)context).FindViewById<TextView>(Resource.Id.txtAlbum);
                    var txtDuracion = ((Activity)context).FindViewById<TextView>(Resource.Id.txtDuracion);

                    txtArtista.Text = artist1;
                    txtCancion.Text = track1;
                    txtAlbum.Text = album1;
                    txtDuracion.Text = duracion;


                    var obj = new BuscaLetras();

                   ;

                    try
                    {
                        var html = await obj.PlyricsAsync("", artist1, track1);

                        if (html != null)
                        {

                            var letra = ExtractString(html.DocumentNode.InnerHtml, "!-- start of lyrics --", "!-- end of lyrics --");

                            
                            Toast.MakeText(context,  letra, ToastLength.Long).Show();

                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                   




                    }

                }


            public string ExtractString(string texto, string stag, string ftag)
            {
                // You should check for errors in real-world code, omitted for brevity
                var startTag = "<" + stag + ">";
                var startIndex = texto.IndexOf(startTag) + startTag.Length;
                var endIndex = texto.IndexOf("<" + ftag + ">", startIndex);

              var resultado=  texto.Substring(startIndex, endIndex - startIndex);

                return resultado.Replace("<br>", "\n");


            }
        }




        

       

        public void Dispose()
        {
            Receptor.Dispose();
         
        }








    };
}




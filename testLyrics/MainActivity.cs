using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Util;
using Android.Widget;
using System;

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
            //Google Android player
            IntentFilter.AddAction("com.android.music.playstatechanged");
            IntentFilter.AddAction("com.android.music.playbackcomplete");
            IntentFilter.AddAction("com.android.music.metachanged");
            //HTC Music
            IntentFilter.AddAction("com.htc.music.playstatechanged");
            IntentFilter.AddAction("com.htc.music.playbackcomplete");
            IntentFilter.AddAction("com.htc.music.metachanged");
            //MIUI Player
            IntentFilter.AddAction("com.miui.player.playstatechanged");
            IntentFilter.AddAction("com.miui.player.playbackcomplete");
            IntentFilter.AddAction("com.miui.player.metachanged");
            //Real
            IntentFilter.AddAction("com.real.IMP.playstatechanged");
            IntentFilter.AddAction("com.real.IMP.playbackcomplete");
            IntentFilter.AddAction("com.real.IMP.metachanged");
            //SEMC Music Player
            IntentFilter.AddAction("com.sonyericsson.music.playbackcontrol.ACTION_TRACK_STARTED");
            IntentFilter.AddAction("com.sonyericsson.music.playbackcontrol.ACTION_PAUSED");
            IntentFilter.AddAction("com.sonyericsson.music.TRACK_COMPLETED");
            IntentFilter.AddAction("com.sonyericsson.music.metachanged");
            //rdio
            IntentFilter.AddAction("com.rdio.android.metachanged");
            IntentFilter.AddAction("com.rdio.android.playstatechanged");
            //Samsung Music Player
            IntentFilter.AddAction("com.samsung.sec.android.MusicPlayer.playstatechanged");
            IntentFilter.AddAction("com.samsung.sec.android.MusicPlayer.playbackcomplete");
            IntentFilter.AddAction("com.samsung.sec.android.MusicPlayer.metachanged");
            IntentFilter.AddAction("com.sec.android.app.music.playstatechanged");
            IntentFilter.AddAction("com.sec.android.app.music.playbackcomplete");
            IntentFilter.AddAction("com.sec.android.app.music.metachanged");
            //Winamp
            IntentFilter.AddAction("com.nullsoft.winamp.playstatechanged");
            IntentFilter.AddAction("com.nullsoft.winamp.metachanged");
            //Amazon
            IntentFilter.AddAction("com.amazon.mp3.playstatechanged");
            IntentFilter.AddAction("com.amazon.mp3.metachanged");
            //Rhapsody
            IntentFilter.AddAction("com.rhapsody.playstatechanged");
            IntentFilter.AddAction("com.rhapsody.metachanged");
            //PlayerPro 
            // IntentFilter.AddAction("com.tbig.playerpro.playstatechanged");
            IntentFilter.AddAction("com.tbig.playerpro.metachanged");
            //IntentFilter.AddAction("com.tbig.playerpro.shufflechanged");
            //IntentFilter.AddAction("com.tbig.playerpro.repeatchanged");
            //IntentFilter.AddAction("com.tbig.playerpro.albumartchanged");
            //IntentFilter.AddAction("com.tbig.playerpro.queuechanged");
            //IntentFilter.AddAction("com.tbig.playerpro.ratingchanged");
            //IntentFilter.AddAction("com.tbig.playerpro.playbackcomplete");
            //PowerAmp
            IntentFilter.AddAction("com.maxmpz.audioplayer.playstatechanged");
            IntentFilter.AddAction("com.maxmpz.audioplayer.metachanged");
            // MyTouch4G
            IntentFilter.AddAction("com.real.IMP.metachanged");
            //appollo
            IntentFilter.AddAction("com.andrew.apollo.metachanged");

            //scrobblers detect for players (poweramp for example)
            //Last.fm
            IntentFilter.AddAction("fm.last.android.metachanged");
            IntentFilter.AddAction("fm.last.android.playbackpaused");
            IntentFilter.AddAction("fm.last.android.playbackcomplete");
            //A simple last.fm scrobbler
            //  IntentFilter.AddAction("com.adam.aslfms.notify.playstatechanged");
            //Scrobble Droid
            IntentFilter.AddAction("net.jjc1138.android.scrobbler.action.MUSIC_STATUS");
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
            public override void OnReceive(Context context, Intent intent)
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
                    var duration1 = intent.GetLongExtra(MediaStore.Audio.AudioColumns.Duration,0);

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
       
                }
            }




        }

        public void Dispose()
        {
            Receptor.Dispose();
         
        }
    };
}




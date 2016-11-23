using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Android.Util;
using Android.Provider;
using Android.Media;

namespace testLyrics
{
    [Activity(Label = "testLyrics", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static bool pausa { get; set; }

        protected override void OnPause()
        {
            base.OnPause();

            pausa = true;
        }

        protected override void OnResume()
        {
            base.OnResume();
            pausa = false;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);



            #region Broadcast

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
            IntentFilter.AddAction("com.tbig.playerpro.playstatechanged");
            //IntentFilter.AddAction("com.tbig.playerpro.metachanged");
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




            //  Toast.MakeText(this, registro.Extras.GetString("track").ToString(), ToastLength.Long).Show();

            #endregion
            mReceiver lbr = new mReceiver();
            var registro = RegisterReceiver(lbr, IntentFilter);

            //AudioManager Manager = (AudioManager)this.GetSystemService(Context.AudioService);
            //if (Manager.IsMusicActive)
            //{

            //    // do something - or do it not
            //}

            string cancion = Intent.GetStringExtra("Cancion") ?? "-1";
            string Artista = Intent.GetStringExtra("Artista") ?? "-1";
            string Album = Intent.GetStringExtra("Album") ?? "-1";
            if (cancion != "-1")
            {
                SetInfo(Artista, cancion, Album,Intent);
                UnregisterReceiver(lbr);
            }



            //http://stackoverflow.com/questions/25215878/how-to-update-the-ui-of-activity-from-broadcastreceiver




        }

        public void SetInfo(string Artista, string Cancion, string Album, Intent intent) {

            var txtArtista = FindViewById<TextView>(Resource.Id.txtArtista);
            var txtCancion = FindViewById<TextView>(Resource.Id.txtCancion);
            var txtAlbum = FindViewById<TextView>(Resource.Id.txtAlbum);

            txtArtista.Text = Artista;
            txtCancion.Text = Cancion;
            txtAlbum.Text = Album;

            intent.PutExtra("Cancion", "-1");
          

        }

        [BroadcastReceiver]
        public class mReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (!pausa) {

                    string action = intent.Action;
                    string cmd = intent.GetStringExtra("command");
                    Log.Verbose("mIntentReceiver.onReceive ", action + " / " + cmd);
                    string lyric = intent.GetStringExtra("lyric");
                    string artist1 = intent.GetStringExtra(MediaStore.Audio.AudioColumns.Artist);
                    string album1 = intent.GetStringExtra(MediaStore.Audio.AlbumColumns.Album);
                    string track1 = intent.GetStringExtra(MediaStore.Audio.AudioColumns.Track);
                    long duration1 = intent.GetLongExtra(MediaStore.Audio.AudioColumns.Duration, 0);


                    //Toast.MakeText(context, "Command : " + action + "\n Artist : " + artist1 + "\n Album :" + album1 + "\n Track : " + track1 + "\n Lyric : " + lyric, ToastLength.Long).Show();



                    Intent it = new Intent(context, typeof(MainActivity));

                    it.PutExtra("Artista", artist1);
                    it.PutExtra("Album", album1);
                    it.PutExtra("Cancion", track1);
                    context.StartActivity(it);
                }
                


                // //String action = intent.Getac();
                // string cmd = intent.GetStringExtra("command");
                //// Log.v("tag ", action + " / " + cmd);
                // string artist = intent.GetStringExtra("artist");
                // string album = intent.GetStringExtra("album");
                // string track = intent.GetStringExtra("track");
                // //Log.v("tag", artist + ":" + album + ":" + track);
                // Toast.MakeText(context, track + " " + artist + " ", ToastLength.Long).Show();
            }


       

        }





    };
}




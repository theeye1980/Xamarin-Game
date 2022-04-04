using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamarin_Game
{
    internal class GameView : SurfaceView, ISurfaceHolderCallback
    {
        public GameView(Context context) : base(context)
        {
            //зададим переменную для получения размеров экрана
            var metrics = Resources.DisplayMetrics;
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            //throw new NotImplementedException();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            //throw new NotImplementedException();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            //throw new NotImplementedException();
        }
        public void Resume() { }
        public void Pause() { }

       
    }
}
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
using System.Threading;

namespace Xamarin_Game
{
    class GameView : SurfaceView, ISurfaceHolderCallback
    {
        Thread gameThread, renderThread;  //рендер и игра в разных потоках
        ISurfaceHolder surfacholder; // для добавления колбека
        bool isRunning;
        int displayX, displayY;
        float rX, rY; //величина соотношения экрана

        public GameView(Context context) : base(context)
        {
            //зададим переменную для получения размеров экрана
            var metrics = Resources.DisplayMetrics;
            displayX = metrics.WidthPixels;
            displayY = metrics.HeightPixels;
            rX = displayX / 1920f; // если ширина экрана будет больше 1920, то это множитель
            rY = displayY / 1080f;

            surfacholder = Holder;
            surfacholder.AddCallback(this);
        }

        override
        public void Draw(Canvas canvas) { 
            canvas.DrawColor(Color.Yellow);
        }
        public void Run() {
            Canvas canvas = null;
            while (isRunning) {
                if (surfacholder.Surface.IsValid) {

                    canvas = surfacholder.LockCanvas(); //Блокируем для начала изменений
                    Draw(canvas);
                    surfacholder.UnlockCanvasAndPost(canvas);
                }
                Thread.Sleep(17); //После отрисовки приостанавливаем поток на 17 мс
            }
        }
         public void Update()
        {

        }



        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            //throw new NotImplementedException();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            //throw new NotImplementedException();
            Resume();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            //throw new NotImplementedException();
            Pause();    
        }
        public void Resume() {
            isRunning = true;
            gameThread = new Thread(new ThreadStart(Update)); //Инициализируем потоки
            renderThread = new Thread(new ThreadStart(Run));

            gameThread.Start();
            renderThread.Start();   
        }
        public void Pause() {

            bool retry = true;
            while (retry) {
                try
                {
                    isRunning = false;
                    gameThread.Join();
                    renderThread.Join();
                    retry = false;
                }
                catch(Exception e) { Console.WriteLine(e.Message); }
                
            }
        
        }

       
    }
}
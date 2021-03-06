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
    class GameView : SurfaceView, ISurfaceHolderCallback // Наследуемся от класса, который позволяет управлять канвасом
    {
        Context context;
        Thread gameThread, renderThread;  //рендер и игра в разных потоках
        ISurfaceHolder surfacholder; // для добавления колбека
        bool isRunning;
        int displayX, displayY;
        float rX, rY; //величина соотношения экрана
        Background background;
        //Slightshot slightshot;
        //Bird bird;
        List<Bird> birds = new List<Bird>();
        List<Stone> stones = new List<Stone>();
        int BIRDS_MAX_COUNT = 5;

        Hero hero;
        

        public GameView(Context context) : base(context)

        {
            this.context = context;
            //зададим переменную для получения размеров экрана
            var metrics = Resources.DisplayMetrics;
            displayX = metrics.WidthPixels;
            displayY = metrics.HeightPixels;
            rX = displayX / 1920f; // если ширина экрана будет больше 1920, то это множитель
            rY = displayY / 1080f;

            surfacholder = Holder;
            surfacholder.AddCallback(this);

            background = new Background(context);
            //bird = new Bird(context);
            for (int i = 0; i < BIRDS_MAX_COUNT; i++) {
                birds.Add(new Bird(context,i));
            }

            hero = new Hero(context);
            
        }

        override
        public void Draw(Canvas canvas) {
            //canvas.DrawColor(Color.Yellow);
            canvas.DrawBitmap(background.Bitmap, background.X, background.Y, null);
            //canvas.DrawBitmap(bird.Bitmap, bird.X, bird.Y, null);
            for (int i = 0; i < BIRDS_MAX_COUNT; i++)
            {
                Bird bird = birds.ElementAt(i);
                canvas.DrawBitmap(bird.Bitmap, bird.X, bird.Y, null);
                
            }
            Paint paint = new Paint();
            paint.Color=Color.White;
            canvas.DrawBitmap(hero.Bitmap, hero.X, hero.Y, null);
            canvas.DrawText("Привет",30,30, paint);



            if (stones.Count > 0)
            {

                for (int i = 0; i < stones.Count; i++)
                {

                    Stone stone = stones.ElementAt(i);
                    canvas.DrawBitmap(stone.Bitmap, stone.X, stone.Y, null);

                }

            }
  

        }
        public void Run() {
            Canvas canvas = null;
            while (isRunning) {
                if (surfacholder.Surface.IsValid) {

                    canvas = surfacholder.LockCanvas(); //Блокируем для начала изменений
                    Draw(canvas);
                    surfacholder.UnlockCanvasAndPost(canvas);
                }
                Thread.Sleep(10); //После отрисовки приостанавливаем поток на 17 мс
            }
        }
         public void Update()
        {
            while (isRunning)
            {
                
                for (int i = 0; i < BIRDS_MAX_COUNT; i++)
                {
                    Bird bird = birds.ElementAt(i);
                    bird.MoveObject();

                }
                hero.MoveObject();

                //Пишем обработку камней

                if (stones.Count > 0) {

                    //Перебираем активные камни и разбираемся с этим
                    for (int i = 0; i < stones.Count; i++) {  
                    
                        Stone stone = stones.ElementAt(i);
                        stone.MoveObject();
                        // Удаляем камень из массива, если он вышел за пределы экрана
                        if (stone.Y + Height < 0) { 
                        stones.Remove(stone);
                        
                        }
                    }
                
                }


                Thread.Sleep(17); //После отрисовки приостанавливаем поток на 17 мс
            }
         
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

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.ActionMasked == MotionEventActions.Down)
            {

                if (e.GetX() < displayX / 3)
                {
                    hero.IsMoveLeft = true;
                    hero.IsMoveRight = false;
                    Console.WriteLine("Едем влево");
                }
                else if (e.GetX() < displayX & e.GetX() > 2 * displayX / 3)
                {

                    hero.IsMoveLeft = false;
                    hero.IsMoveRight = true;
                    Console.WriteLine("Едем вправо");
                }
                else {

                    Stone stone = new Stone(context, hero);
                    stones.Add(stone);
                    Console.WriteLine("камень пошел");
                }

            }
            else if (e.ActionMasked == MotionEventActions.Up) {

                hero.IsMoveLeft = false;
                hero.IsMoveRight = false;
            }
            //if (e.ActionMasked == MotionEventActions.)
                return true;
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
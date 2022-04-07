using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;

namespace Xamarin_Game
{
    class Bird : GameObject
    {

        int[] ducksId = { Resource.Drawable.duck0, Resource.Drawable.duck1, Resource.Drawable.duck2 };
        //Bitmap bitmap;
        //int x, y, width, height, speed;
        public Bird(Context context, int i) : base(context)
        {
            Random random = new Random();
            int index = random.Next(ducksId.Length);


            Bitmap = BitmapFactory.DecodeResource(context.Resources, ducksId[index]);
            //var metrics = context.Resources.DisplayMetrics;
            Width = Metrics.WidthPixels / 16; //задаем ширину 1/8 от ширины экрана
            Height = Width * Bitmap.Height/Bitmap.Width;
            Bitmap = Bitmap.CreateScaledBitmap(Bitmap, Width, Height, true);
            
            X = random.Next(0,DisplayX - Width);
            Y = random.Next(0,DisplayY / 3 - Height / 3);
            //X = (metrics.WidthPixels - Width* index) / 2; //задаем появление элемента строго по центру
            //Y = (metrics.HeightPixels - Height * index) / 2;

            Speed = -(int) (4 + 2*(index+1)*(Metrics.WidthPixels / 1920f));
        }

        public void MoveBird() {

            X += Speed;
            if (X + Width > DisplayX)
            {

                Speed *= -1;
                Bitmap = createFlippedBitmap(Bitmap,true,false);
            }
            else if (X <= 0)
            {

                Speed *= -1;
                Bitmap = createFlippedBitmap(Bitmap, true, false);
            }

        }

        public Bitmap createFlippedBitmap(Bitmap source, bool xFlip, bool yFlip) { //зеркально отражаем картинку
            Matrix matrix = new Matrix();  
            matrix.PostScale(xFlip ? -1:1, yFlip ? -1 : 1, source.Width / 2, source.Height / 2);
            return Bitmap.CreateBitmap(source, 0,0,Width, Height, matrix, true);
        }

        //public int Height { get => height; set => height = value; }
        //public int Width { get => width; set => width = value; }
        //public int X { get => x; set => x = value; }
        //public int Y { get => y; set => y = value; }
        //public Bitmap Bitmap { get => bitmap; set => bitmap = value; }
        //public int Speed { get => speed; set => speed = value; }
    }
}
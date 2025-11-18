using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Clicker3Laba
{
    public class CObject
    {
        private Point position { get; set; }
        private Size size { get; set; }
        private double lifetime { get; set; }
        private double pointsValue { get; set; }
        private Ellipse sprite { get; set; }

        public CObject(Point position, double size, double lifetime)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            //Создание кружка
            sprite = new Ellipse();

            //Внешний вид кружка
            sprite.Fill = Brushes.BlueViolet;
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;

            //Центрирование
            sprite.HorizontalAlignment = HorizontalAlignment.Center;
            sprite.VerticalAlignment = VerticalAlignment.Center;

            //Размеры
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;

            //Расположение на канвасе
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);

            //Расчет очков
            pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }

        //Проверка наведения мыши
        public bool isMouseOnObject(Point mousePosition)
        {
            return mousePosition.X >= position.X &&
                   mousePosition.X <= position.X + size.Width &&
                   mousePosition.Y >= position.Y &&
                   mousePosition.Y <= position.Y + size.Height;
        }

        //Получение спрайта
        public Ellipse getSprite()
        {
            return sprite;
        }

        //Получение значения очков
        public double getPointsValue()
        {
            return pointsValue;
        }

        //Обновление времени
        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime <= 0;
        }
    }
}

using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Clicker3Laba
{
    public abstract class CCollectable
    {
        private Point position; //Позиция собираемого объекта в сцене
        protected Size size; //Размер собираемого объекта
        private double lifetime; //Время жизни собираемого объекта
        protected Ellipse sprite; //Визуальное отображение собираемого объекта

        public CCollectable(Point position, double size, double lifetime) //Конструктор
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;

            sprite = new Ellipse();

            sprite.Fill = Brushes.BlueViolet;
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;

            sprite.HorizontalAlignment = HorizontalAlignment.Center;
            sprite.VerticalAlignment = VerticalAlignment.Center;

            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);
        }

        //Методы для работы с объектом
        public bool isMouseOnObject(Point mousePosition)
        {
            return mousePosition.X >= position.X &&
                   mousePosition.X <= position.X + size.Width &&
                   mousePosition.Y >= position.Y &&
                   mousePosition.Y <= position.Y + size.Height;
        }

        public Ellipse getSprite()
        {
            return sprite;
        }

        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            return lifetime <= 0;
        }

        //Абстрактная функция отработки нажатия на объект
        public abstract bool onClick(CPlayer player, CController controller, Point mousePosition);
    }
}

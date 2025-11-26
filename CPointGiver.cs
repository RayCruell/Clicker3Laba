using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Clicker3Laba
{
    public class CPointGiver : CCollectable
    {
        private double pointsValue;
        //Конструктор класса реализует конструктор родителя
        public CPointGiver(Point position, double size, double lifetime, double pointValue) : base(position, size, lifetime)
        {
            sprite.Fill = Brushes.BlueViolet;
            pointsValue = ((1 / this.size.Width) / lifetime) * 1000;
        }
        //Переопределение функции нажатия
        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (isMouseOnObject(mousePosition) == false)
                return false;

            controller.pointsIncrease(pointsValue);
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Clicker3Laba
{
    public class CLifetimeChanger : CCollectable
    {
        private double lifetimeModifier;

        public CLifetimeChanger(Point position, double size, double lifetime, double lifetimeModifier)
            : base(position, size, lifetime)
        {
            this.lifetimeModifier = lifetimeModifier;
            sprite.Fill = Brushes.Orange;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (!isMouseOnObject(mousePosition))
                return false;
            return true;
        }

        public double GetLifetimeModifier() => lifetimeModifier;
    }
}

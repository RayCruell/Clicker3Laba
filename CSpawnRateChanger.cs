using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Clicker3Laba
{
    public class CSpawnRateChanger : CCollectable
    {
        private double spawnRateModifier;

        public CSpawnRateChanger(Point position, double size, double lifetime, double spawnRateModifier)
            : base(position, size, lifetime)
        {
            this.spawnRateModifier = spawnRateModifier;
            sprite.Fill = Brushes.Red;
        }

        public override bool onClick(CPlayer player, CController controller, Point mousePosition)
        {
            if (!isMouseOnObject(mousePosition))
                return false;

            // Просто возвращаем true - логика будет в MainWindow
            return true;
        }

        public double GetSpawnRateModifier() => spawnRateModifier;
    }
}

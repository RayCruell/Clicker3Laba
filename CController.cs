using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clicker3Laba
{
    public class CController
    {
        private List<CCollectable> objects;
        private double points;

        public CController()
        {
            objects = new List<CCollectable>();
            points = 0;
        }

        public List<CCollectable> getObjects() => objects;
        public void addObject(CCollectable obj) => objects.Add(obj);
        public void removeObject(CCollectable obj) => objects.Remove(obj);
        public void pointsIncrease(double value) => points += value;
        public double getPoints() => points;
    }
}

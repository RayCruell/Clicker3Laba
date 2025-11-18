using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Clicker3Laba
{
    public class Controller
    {
        private List<CObject> objects; //Список собираемых объектов

        private double spawnRate; //Время между созданием собираемых объектов
        private double time; //Время  момента создания последнего собираемого объекта

        private Random rng;
        //Минимальное и максимальное время жизни собираемых объектов
        private double minLifetime;
        private double maxLifetime;
        //Минимальный и максимальный размер собираемых объектов
        private double minSpriteSize;
        private double maxSpriteSize;
        private Size sceneSize; //Размер сцены
        private double points; //Очки

        public Controller(double spawnRate, double startTime, Size sceneSize)
        {
            rng = new Random();
            objects = new List<CObject>();
            this.spawnRate = spawnRate;
            this.time = startTime;
            this.sceneSize = sceneSize;
            points = 0;

            minLifetime = 1;
            maxLifetime = 5;
            minSpriteSize = 10;
            maxSpriteSize = 20;
        }

        //Для спавна объекта-кружка
        public void spawnObject()
        {
            //Случайная позиция на канвасе
            double x = rng.NextDouble() * (sceneSize.Width - maxSpriteSize);
            double y = rng.NextDouble() * (sceneSize.Height - maxSpriteSize);
            Point position = new Point(x, y);

            //Случайный размер и время
            double size = minSpriteSize + (rng.NextDouble() * (maxSpriteSize - minSpriteSize));
            double lifetime = minLifetime + (rng.NextDouble() * (maxLifetime - minLifetime));

            CObject newObj = new CObject(position, size, lifetime);
            objects.Add(newObj);
        }

        //Уничтожение объекта
        public void destroyObject(CObject obj)
        {
            objects.Remove(obj);
        }

        //Обновление состояния
        public void update(double delta)
        {
            time += delta;

            if (time >= spawnRate)
            {
                spawnObject();
                time = 0;
            }

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].updateLifetime(delta))
                {
                    destroyObject(objects[i]);
                }
            }
        }

        //Обработка клика мышкой
        public void mouseClick(Point mousePosition)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].isMouseOnObject(mousePosition))
                {
                    points += objects[i].getPointsValue();
                    destroyObject(objects[i]);
                    break;
                }
            }
        }

        //Геттеры для приватных полей
        public List<CObject> getObjects() => objects;
        public double getPoints() => points;
        public double getSpawnRate() => spawnRate;
        public void setSpawnRate(double rate) => spawnRate = rate;

        public void UpdateSceneSize(Size newSize)
        {
            sceneSize = newSize;
        }
    }
}

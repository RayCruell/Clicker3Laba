using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Clicker3Laba
{
    public partial class MainWindow : Window
    {
        private CController gameController;
        private CPlayer gamePlayer;
        private DispatcherTimer gameTimer;
        private double gameTimeLeft = 60;
        private bool gameStarted = false;
        private double totalPointsLost = 0; // Счетчик потерянных очков
        private DateTime lastTickTime;
        private double spawnRate = 2.0;
        private double minLifetime = 1.0;
        private double maxLifetime = 5.0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Канвас имеет реальные размеры
            if (gameController != null)
            {

            }
        }

        private void InitializeGame()
        {
            // Убираем параметры spawnRate, startTime, sceneSize
            gameController = new CController(); // просто пустой конструктор
            gamePlayer = new CPlayer();

            // Настройка таймера игры
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1); // 1 секунда
            gameTimer.Tick += GameTimer_Tick;

            UpdateUI();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!gameStarted)
            {
                StartGame();
            }
            else
            {
                EndGame();
            }
        }

        private void StartGame()
        {
            gameStarted = true;
            gameTimeLeft = 60;
            totalPointsLost = 0;
            startButton.Content = "STOP";

            lastTickTime = DateTime.Now;
            gameTimer.Start();

            AddLog("Game started! Click the circles!");
        }

        private void EndGame()
        {
            gameStarted = false;
            startButton.Content = "START";
            gameTimer.Stop();

            AddLog($"Game over! Final score: {gameController.getPoints():F1}");
            AddLog($"Total points lost: {totalPointsLost:F1}");

            gameCanvas.Children.Clear();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (gameStarted)
            {
                gamePlayer.update(1.0);

                // ДОБАВЛЯЕМ СПАВН ОБЪЕКТОВ!
                SpawnRandomObject();

                // Обновление времени жизни объектов
                var objectsToUpdate = new List<CCollectable>(gameController.getObjects());
                foreach (var obj in objectsToUpdate)
                {
                    if (obj.updateLifetime(1.0))
                    {
                        gameController.removeObject(obj);
                    }
                }

                gameTimeLeft -= 1.0;
                if (gameTimeLeft <= 0)
                {
                    EndGame();
                    return;
                }

                RenderObjects();
                UpdateUI();
            }
        }

        private Random rng = new Random();
        private double spawnTimer = 0;

        private void SpawnRandomObject()
        {
            spawnTimer += 1.0;

            if (spawnTimer >= 2.0) // Спавним каждые 2 секунды
            {
                spawnTimer = 0;

                // Случайная позиция на канвасе
                double x = rng.NextDouble() * (gameCanvas.ActualWidth - 30);
                double y = rng.NextDouble() * (gameCanvas.ActualHeight - 30);
                Point position = new Point(x, y);

                // Случайный размер и время жизни
                double size = 10 + (rng.NextDouble() * 10);
                double lifetime = minLifetime + (rng.NextDouble() * (maxLifetime - minLifetime));

                // Создаем случайный тип объекта
                CCollectable newObj;
                int type = rng.Next(0, 4); // 0-3

                switch (type)
                {
                    case 0:
                        newObj = new CPointGiver(position, size, lifetime, 10);
                        break;
                    case 1:
                        newObj = new CClickSpeedUp(position, size, lifetime, 0.8);
                        break;
                    case 2:
                        newObj = new CLifetimeChanger(position, size, lifetime, 1.5);
                        break;
                    case 3:
                        newObj = new CSpawnRateChanger(position, size, lifetime, 0.7);
                        break;
                    default:
                        newObj = new CPointGiver(position, size, lifetime, 10);
                        break;
                }

                gameController.addObject(newObj);
            }
        }

        private void RenderObjects()
        {
            gameCanvas.Children.Clear();

            foreach (var obj in gameController.getObjects())
            {
                var sprite = obj.getSprite();
                gameCanvas.Children.Add(sprite);
            }
        }

        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!gameStarted) return;

            var mousePosition = e.GetPosition(gameCanvas);

            foreach (var obj in gameController.getObjects())
            {
                if (obj.onClick(gamePlayer, gameController, mousePosition))
                {
                    // Обрабатываем разные типы объектов
                    if (obj is CPointGiver)
                    {
                        AddLog($"Collected points! +10 points");
                    }
                    else if (obj is CClickSpeedUp speedUp)
                    {
                        AddLog($"Click speed increased!");
                        // speedUp уже сам вызывает player.increaseSpeed()
                    }
                    else if (obj is CLifetimeChanger lifetimeChanger)
                    {
                        // Увеличиваем время жизни объектов
                        minLifetime *= lifetimeChanger.GetLifetimeModifier();
                        maxLifetime *= lifetimeChanger.GetLifetimeModifier();
                        AddLog($"Lifetime increased to: {minLifetime:F1}-{maxLifetime:F1}s");
                    }
                    else if (obj is CSpawnRateChanger spawnRateChanger)
                    {
                        // Уменьшаем время между спавном
                        spawnRate = Math.Max(0.5, spawnRate * spawnRateChanger.GetSpawnRateModifier());
                        AddLog($"Spawn rate decreased to: {spawnRate:F1}s");
                    }

                    gameController.removeObject(obj);
                    break;
                }
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            scoreText.Text = gameController.getPoints().ToString("F1");
            timeText.Text = Math.Ceiling(gameTimeLeft).ToString("0");
        }

        private void AddLog(string message)
        {
            statusText.Text = $"{DateTime.Now:HH:mm:ss} - {message}\n" + statusText.Text;
        }

        private void LogLostPoints()
        {
            
        }
    }
}
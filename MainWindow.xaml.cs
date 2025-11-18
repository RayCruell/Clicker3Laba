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
        private Controller gameController;
        private DispatcherTimer gameTimer;
        private double gameTimeLeft = 60;
        private bool gameStarted = false;
        private double totalPointsLost = 0; // Счетчик потерянных очков

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
            //Временные размеры
            gameController = new Controller(
                spawnRate: 1.5, 
                startTime: 0,
                sceneSize: new Size(600, 400) 
            );

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1); //Обратный отсчёт по секунде
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
            gameTimer.Start();

            // Обновляем реальные размеры канваса
            gameController.UpdateSceneSize(new Size(gameCanvas.ActualWidth, gameCanvas.ActualHeight));

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
                gameController.update(1.0);

                gameTimeLeft -= 1.0; 

                if (gameTimeLeft <= 0)
                {
                    gameTimeLeft = 0;
                    EndGame();
                    return;
                }

                RenderObjects();
                UpdateUI();
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
            double pointsBefore = gameController.getPoints();
            gameController.mouseClick(mousePosition);
            double pointsAfter = gameController.getPoints();

            if (pointsAfter > pointsBefore)
            {
                AddLog($"+{(pointsAfter - pointsBefore):F1} points!");
            }
        }

        private void UpdateUI()
        {
            scoreText.Text = gameController.getPoints().ToString("F1");
            timeText.Text = gameTimeLeft > 0 ? Math.Ceiling(gameTimeLeft).ToString("0") : "0";
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
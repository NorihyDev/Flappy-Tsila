namespace Flappy_Tsila;

public partial class GamePage : ContentPage
{
    double birdY = 0;
    double velocity = 0;

    double gravity = 0.55;
    double jumpForce = -8.5;

    double pipeX = 420;
    double pipeGapY = 260;
    double pipeGap = 180;
    double pipeSpeed = 3.2;

    int score = 0;
    bool gameRunning = true;
    bool scored = false;

    Random random = new Random();

    public GamePage()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            ResetGame();
            StartGameLoop();
        };
    }

    private void ResetGame()
    {
        birdY = 0;
        velocity = 0;

        pipeX = GameArea.Width + 80;
        pipeGapY = 230;
        score = 0;
        scored = false;
        gameRunning = true;

        ScoreLabel.Text = "0";
        GameOverPanel.IsVisible = false;

        Bird.TranslationY = birdY;
        Bird.Rotation = 0;

        PositionPipes();
    }

    private void StartGameLoop()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!gameRunning)
                return false;

            UpdateBird();
            UpdatePipes();
            CheckCollision();

            return true;
        });
    }

    private void UpdateBird()
    {
        velocity += gravity;
        birdY += velocity;

        Bird.TranslationY = birdY;
        Bird.Rotation = Math.Clamp(velocity * 4, -25, 65);
    }

    private void UpdatePipes()
    {
        pipeX -= pipeSpeed;

        if (pipeX < -100)
        {
            pipeX = GameArea.Width + 80;
            pipeGapY = random.Next(170, (int)Math.Max(220, GameArea.Height - 220));
            scored = false;
        }

        if (!scored && pipeX + 80 < GameArea.Width / 2)
        {
            score++;
            ScoreLabel.Text = score.ToString();
            scored = true;
        }

        PositionPipes();
    }

    private void PositionPipes()
    {
        TopPipe.TranslationX = pipeX;
        BottomPipe.TranslationX = pipeX;

        TopPipe.TranslationY = pipeGapY - pipeGap / 2 - 420;
        BottomPipe.TranslationY = pipeGapY + pipeGap / 2;
    }

    private void CheckCollision()
    {
        double birdCenterX = GameArea.Width / 2;
        double birdCenterY = GameArea.Height / 2 + birdY;

        double birdLeft = birdCenterX - 24;
        double birdRight = birdCenterX + 24;
        double birdTop = birdCenterY - 18;
        double birdBottom = birdCenterY + 18;

        double groundTop = GameArea.Height - 100;

        if (birdTop <= 0 || birdBottom >= groundTop)
        {
            GameOver();
            return;
        }

        double pipeLeft = pipeX;
        double pipeRight = pipeX + 80;

        double topPipeBottom = pipeGapY - pipeGap / 2;
        double bottomPipeTop = pipeGapY + pipeGap / 2;

        bool touchesPipeX = birdRight > pipeLeft && birdLeft < pipeRight;
        bool touchesPipeY = birdTop < topPipeBottom || birdBottom > bottomPipeTop;

        if (touchesPipeX && touchesPipeY)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameRunning = false;
        GameOverPanel.IsVisible = true;
    }

    private void OnGameTapped(object? sender, TappedEventArgs e)
    {
        if (!gameRunning)
            return;

        velocity = jumpForce;
    }

    private void OnRestartClicked(object? sender, EventArgs e)
    {
        ResetGame();
        StartGameLoop();
    }
}
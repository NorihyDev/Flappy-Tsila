namespace Flappy_Tsila;

public partial class MainPage : ContentPage
{
    double birdTime = 0;

    public MainPage()
    {
        InitializeComponent();
        StartBirdTimer();
    }

    private void StartBirdTimer()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            birdTime += 0.05;

            BirdImage.TranslationY = Math.Sin(birdTime) * 12;

            return true;
        });
    }

    private async void OnPlayClicked(object? sender, EventArgs e)
    {
        await PlayButton.ScaleToAsync(0.9, 100);
        await PlayButton.ScaleToAsync(1.1, 120);
        await PlayButton.ScaleToAsync(1, 100);

        await Shell.Current.Navigation.PushAsync(new GamePage());
    }
}
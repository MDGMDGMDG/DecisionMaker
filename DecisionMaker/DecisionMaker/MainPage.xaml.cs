using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DecisionMaker
{
    public partial class MainPage : ContentPage
    {
        bool _backgroundScaleToggle;
        bool _ballMovementToggle;

        public List<string> Answers { get; set; }

        public MainPage()
        {
            InitializeComponent();

            ToggleAccelerometer();

            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;

            Answers = new List<string>() { "Yes", "No", "Maybe", "Can't tell", "The stars are not sure", "Ofcourse", "Yes, but be careful" };
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(SensorSpeed.Fastest);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private async void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            await App.Current.MainPage.DisplayAlert("Shake detected", "You shook the device!", "OK");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MoveBall();
            ScaleBackground();
        }

        /// <summary>
        /// Scale background
        /// </summary>
        private async void MoveBall()
        {
            while (!_ballMovementToggle)
            {
                await ballGrid.TranslateTo(0, 50, 1000, Easing.SinInOut);
                await ballGrid.TranslateTo(0, -50, 1000, Easing.SinInOut);
            }
        }

        /// <summary>
        /// move ball up down
        /// </summary>
        private async void ScaleBackground()
        {
            while (!_backgroundScaleToggle)
            {
                await redBackground.ScaleTo(2, 10000, Easing.SinInOut);
                await redBackground.ScaleTo(1, 10000, Easing.SinInOut);
            }
        }

        /// <summary>
        /// Present an answer and haptic feedback
        /// </summary>
        private async void redBall_Clicked(object sender, EventArgs e)
        {
            await answerLabel.FadeTo(0, 1000);
            answerLabel.Text = Answers[new Random().Next(Answers.Count)];
            await answerLabel.FadeTo(0.65, 1500);
        }
    }
}

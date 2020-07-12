using System;
using RangeSelection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RangeSelectorViewSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Children = {
                        //new BoxView { HeightRequest = 100, BackgroundColor = Color.Red },
                        new RangeSlider
                        {
                            TrackSize = 10,
                            LowerThumbSize = 20,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            UpperThumbView = new Label
                            {
                                Text = "X",
                                VerticalTextAlignment = TextAlignment.Center,
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                        }
                    }
                }
            };
        }
    }
}

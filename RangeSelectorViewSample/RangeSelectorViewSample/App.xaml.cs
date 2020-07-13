using RangeSelection;
using Xamarin.Forms;

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
                            },
                            LowerValueLabelStyle = new Style(typeof(Label))
                            {
                                Setters =
                                {
                                    new Setter { Property = Label.FontAttributesProperty, Value = FontAttributes.Bold}
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}

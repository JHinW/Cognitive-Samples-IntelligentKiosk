using ServiceHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IntelligentKioskSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [KioskExperience(Title = "Images Identity", ImagePath = "ms-appx:/Assets/person-group.png", ExperienceType = ExperienceType.Kiosk)]
    public sealed partial class PersonGroupIdentityPage : Page
    {
        public PersonGroupIdentityPage()
        {
            this.InitializeComponent();
            this.cameraHostGrid.ItemsSource = null;
            MainGrid.DataContext = this;
            this.FnameBlockWidth = new GridLength(1500, GridUnitType.Pixel);
        }

        public GridLength FnameBlockWidth
        {
            get { return (GridLength)GetValue(FnameBlockWidthProperty); }
            set { SetValue(FnameBlockWidthProperty, value); }
        }

        public static readonly DependencyProperty FnameBlockWidthProperty = DependencyProperty.Register(
               "FnameBlockWidth",
               typeof(GridLength),
               typeof(PersonGroupIdentityPage),
               new PropertyMetadata(new GridLength(100, GridUnitType.Pixel)));

        private void OnImageSearchCompleted(object sender, IEnumerable<ImageAnalyzer> args)
        {
            Exception lastError = null;
            var images = new List<ImageAnalyzer>();
            var width = 1300 / (args.Count() > 3 ? 3 : args.Count());
            this.FnameBlockWidth = new GridLength(args.Count() == 1? 800: width, GridUnitType.Pixel);
            foreach (var item in args)
            {
                try
                {
                    if (item.GetImageStreamCallback != null)
                    {
                        images.Add(new ImageAnalyzer(item.GetImageStreamCallback));
                    }
                    else
                    {
                        images.Add(new ImageAnalyzer(item.ImageUrl));
                    }
                }
                catch (Exception e)
                {
                    this.cameraHostGrid.ItemsSource = null;
                    lastError = e;
                }
            }
            this.cameraHostGrid.ItemsSource = images;
            this.cameraGuideHost.Opacity = 0;
        }

        private void OnImageSearchCanceled(object sender, EventArgs e)
        {
            this.trainingImageCollectorFlyout.Hide();
        }

        private void OnImageSearchLocalFilesProvided(object sender, EventArgs e)
        {
            this.trainingImageCollectorFlyout.ShowAt(this.AddFacesAppBarButton);
        }

        private async void OnDeletePersonClicked(object sender, RoutedEventArgs e)
        {
            //await Util.ConfirmActionAndExecute("Delete person?", async () => { await DeletePersonAsync(); });
        }

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.UpdateCameraHostSize();
        }

    }
}

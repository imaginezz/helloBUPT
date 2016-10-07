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
using HelloBUPT.Common;
using HelloBUPT.Theme;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HelloBUPT.Project.Setting {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Setting : Page {
        public MainPage mainPage = AppSetting.mainPage;
        public CurrentTheme currentTheme = AppSetting.mainPage.currentTheme;
        private ResourceDictionary themeResourceDictionary = new ResourceDictionary();
        public Setting() {
            this.InitializeComponent();
            themeResourceDictionary.Source = new Uri("ms-appx:///Theme/LightThemeDictionary.xaml", UriKind.Absolute);
        }

        private void ThemePickerToggleButton_Toggled(object sender, RoutedEventArgs e) {
            currentTheme.Theme = currentTheme.Theme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar")) {
                var statusBar = AppSetting.statusBar;
                if (statusBar != null) {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = currentTheme.Theme == ElementTheme.Light ? Color.FromArgb(0xFF, 0xF6, 0xF6, 0xF6) : Color.FromArgb(0xFF, 0x2D, 0x2D, 0x2D);
                    statusBar.ForegroundColor = currentTheme.Theme == ElementTheme.Light ? Colors.Black : Colors.White;
                }
            }

        }
    }
}

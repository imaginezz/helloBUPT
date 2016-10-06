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
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HelloBUPT.Project.Setting {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Setting : Page {
        public MainPage mainPage = AppSetting.mainPage;
        public CurrentTheme currentTheme = AppSetting.mainPage.currentTheme;
        public Setting() {
            this.InitializeComponent();
        }

        private void ThemePickerToggleButton_Toggled(object sender, RoutedEventArgs e) {
            currentTheme.Theme = currentTheme.Theme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
        }
    }
}

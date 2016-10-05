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
using HelloBUPT.SideBarMenu;
using HelloBUPT.Theme;
using HelloBUPT.Common;
using System.Diagnostics;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloBUPT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public CurrentTheme currentTheme { get; } = new CurrentTheme();
        public SplitViewModel splitViewModel { get; } = new SplitViewModel();
        public string NavMenuTitle = "Hello BUPT";
        public List<NavMenuItem> NavMenuList { get; } = new List<NavMenuItem> {
                new NavMenuItem() { Icon = Symbol.World, Text = "Web", DestPage = typeof(Project.CampusNetwork.LoginPage)},
                new NavMenuItem() { Icon = Symbol.Setting, Text = "Setting", DestPage = typeof(Project.Setting.Setting)}
        };
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem) {
            var item = (NavMenuItem)((SplitViewModel)sender).ItemFromContainer(listViewItem);
            if (item != null) {
                mainFrame.Navigate(item.DestPage);
            }
        }
        public MainPage()
        {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(Project.CampusNetwork.LoginPage));
            AppSetting.mainPage = this;
        }
    }
}
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
using Windows.UI.Core;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloBUPT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        // 整个主界面的主题
        public CurrentTheme currentTheme { get; } = new CurrentTheme();
        

        // 右侧汉堡菜单
        public SplitViewModel splitViewModel { get; } = new SplitViewModel();
        public string NavMenuTitle = "Hello BUPT";
        // 汉堡菜单的菜单项
        public List<NavMenuItem> NavMenuList { get; } = new List<NavMenuItem> {
                new NavMenuItem() { Icon = Symbol.World, Text = "网关登录", DestPage = typeof(Project.CampusNetwork.LoginPage)},
                new NavMenuItem() { Icon = Symbol.Setting, Text = "设置", DestPage = typeof(Project.Setting.Setting)}
        };
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem) {
            var item = (NavMenuItem)((SplitViewModel)sender).ItemFromContainer(listViewItem);
            if (item != null) {
                mainFrame.Navigate(item.DestPage);
            }
        }


        public MainPage() {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(Project.CampusNetwork.LoginPage));
            mainFrame.Navigated += OnNavigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            AppSetting.mainPage = this;
        }


        // 导航
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")) {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")) {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
            base.OnNavigatedFrom(e);
        }

        private void OnNavigated(object sender, NavigationEventArgs e) {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        //发出后退请求时
        private void OnBackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e) {
            if (mainFrame == null)
                return;
            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (mainFrame.CanGoBack && e.Handled == false) {
                e.Handled = true;
                mainFrame.GoBack();
            }
        }

        //对于Mobile Device，按下硬件“后退”按钮时响应
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e) {
            if (splitViewModel.IsPaneOpen) {
                e.Handled = true;
                splitViewModel.IsPaneOpen = false;
            }
        }
    }
}
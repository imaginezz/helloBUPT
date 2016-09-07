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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace beiyou
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CampusNetwork campusNetwork;
        public MainPage()
        {
            this.InitializeComponent();
            campusNetwork = new CampusNetwork();
        }

        private async void Login_Click(object sender, RoutedEventArgs e) {
            DebugLib.DebugOutput("student id " + StudentId.Text);
            DebugLib.DebugOutput("student passwd " + StudentPasswd.Text);
            await campusNetwork.Login(StudentId.Text, StudentPasswd.Text);
        }

        private async void Logout_Click(object sender, RoutedEventArgs e) {
            await campusNetwork.Logout();
        }

        private void checkSave_Click(object sender, RoutedEventArgs e) {
            if(checkSave.IsChecked == false) {

            }
        }
    }
}
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

namespace beiyou.Project.CampusNetwork {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page {
        CampusNetworkClass campusNetwork;

        private string InformationText {
            set {
                Information.Text = value;
            }
        }
        private string StudentIdText {
            set {
                StudentId.Text = value;
            }
            get {
                return StudentId.Text;
            }
        }
        private string StudentPasswdText {
            set {
                StudentPasswd.Password = value;
            }
            get {
                return StudentPasswd.Password;
            }
        }

        public LoginPage() {
            this.InitializeComponent();
            campusNetwork = new CampusNetworkClass();
            string id, passwd;
            bool check;
            campusNetwork.readAccount(out id, out passwd, out check);
            StudentIdText = id;
            StudentPasswdText = passwd;
            checkSave.IsChecked = check;
        }
        private async void Login_Click(object sender, RoutedEventArgs e) {
            InformationText=await campusNetwork.Login(StudentIdText, StudentPasswdText);
            CheckAccount();
        }

        private async void Logout_Click(object sender, RoutedEventArgs e) {
            InformationText=await campusNetwork.Logout();
            CheckAccount();
        }

        private void CheckAccount() {
            if (checkSave.IsChecked == true) {
                campusNetwork.saveAccount(StudentIdText, StudentPasswdText);
            } else {
                campusNetwork.clearAccount();
                StudentIdText = string.Empty;
                StudentPasswdText = string.Empty;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.Storage;


namespace HelloBUPT.Theme {
    public class CurrentTheme : INotifyPropertyChanged{
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ElementTheme _theme = ElementTheme.Light;
        public ElementTheme Theme {
            get {
                return _theme;
            }
            set {
                _theme = value;
                this.OnPropertyChanged("Theme");
            }
        }

        public CurrentTheme() {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("Theme")) {
                _theme = (string)localSettings.Values["Theme"] == "Dark" ? ElementTheme.Dark : ElementTheme.Light;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

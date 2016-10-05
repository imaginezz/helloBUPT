using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;


namespace HelloBUPT.Theme {
    public class CurrentTheme : INotifyPropertyChanged{
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ElementTheme theme = ElementTheme.Light;
        public ElementTheme Theme {
            get {
                return theme;
            }
            set {
                theme = value;
                this.OnPropertyChanged("Theme");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

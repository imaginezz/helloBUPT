using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HelloBUPT.SideBarMenu {
    public class SplitViewModel : INotificationObject {
        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems {
            get { return _menuItems; }
            set {
                _menuItems = value;
                OnPropertyChanged("MenuItems");
            }
        }

        private bool _isInDetailsMode;
        public bool IsInDetailsMode {
            get { return _isInDetailsMode; }
            set {
                _isInDetailsMode = value;
                OnPropertyChanged("IsInDetailsMode");
            }
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen {
            get { return _isPaneOpen; }
            set {
                _isPaneOpen = value;
                OnPropertyChanged("IsPaneOpen");
            }
        }

        private SplitViewDisplayMode _displayMode;
        public SplitViewDisplayMode DisplayMode {
            get { return _displayMode; }
            set {
                _displayMode = value;
                OnPropertyChanged("DisplayMode");
            }
        }

        private string _hamburgerTitle;
        public string HamburgerTitle {
            get { return _hamburgerTitle; }
            set {
                _hamburgerTitle = value;
                OnPropertyChanged("HamburgerTitle");
            }
        }

        public DelegateCommand HamburgerButtonCommand { get; private set; }
        public void HamburgerButton(object parameter) {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay  || 
                DisplayMode == SplitViewDisplayMode.Overlay) {
                IsPaneOpen = IsPaneOpen ? false : true;
            }
        }

        public SplitViewModel() {
            HamburgerTitle = "HelloBUPT";
            MenuItems = new ObservableCollection<MenuItem>() {
                new MenuItem() { Icon = Symbol.World, Text = "Web" },
                new MenuItem() { Icon = Symbol.Setting, Text = "Setting"}
            };
            HamburgerButtonCommand = new DelegateCommand();
            HamburgerButtonCommand.ExecuteAction = new Action<object>(HamburgerButton);
        }
    }
}

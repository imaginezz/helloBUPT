using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;
using System.Diagnostics;

namespace HelloBUPT.SideBarMenu {
    public class SplitViewModel : ListView, INotifyPropertyChanged{
        private SplitView splitViewHost;

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

        public DelegateCommand HamburgerButtonCommand { get; private set; }
        public void HamburgerButton(object parameter) {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay  || 
                DisplayMode == SplitViewDisplayMode.Overlay) {
                IsPaneOpen = IsPaneOpen ? false : true;
            }
        }

        /// <summary>
        /// Mark the <paramref name="item"/> as selected and ensures everything else is not.
        /// If the <paramref name="item"/> is null then everything is unselected.
        /// </summary>
        /// <param name="item"></param>
        public void SetSelectedItem(ListViewItem item) {
            int index = -1;
            if (item != null) {
                index = this.IndexFromContainer(item);
            }

            for (int i = 0; i < this.Items.Count; i++) {
                var lvi = (ListViewItem)this.ContainerFromIndex(i);
                if (i != index) {
                    lvi.IsSelected = false;
                }
                else if (i == index) {
                    lvi.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Occurs when an item has been selected
        /// </summary>
        public event EventHandler<ListViewItem> ItemInvoked;

        private void ItemClickedHandler(object sender, ItemClickEventArgs e) {
            // Triggered when the item is selected using something other than a keyboard
            var item = this.ContainerFromItem(e.ClickedItem);
            this.InvokeItem(item);
        }

        private void InvokeItem(object focusedItem) {
            this.SetSelectedItem(focusedItem as ListViewItem);
            this.ItemInvoked?.Invoke(this, focusedItem as ListViewItem);

            if (this.splitViewHost == null || this.splitViewHost.IsPaneOpen) {
                if (this.splitViewHost != null &&
                    (this.splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                    this.splitViewHost.DisplayMode == SplitViewDisplayMode.Overlay)) {
                    this.splitViewHost.IsPaneOpen = false;
                    OnPropertyChanged("IsPaneOpen");
                }
                if (focusedItem is ListViewItem) {
                    ((ListViewItem)focusedItem).Focus(FocusState.Programmatic);
                }
            }
        }

        public SplitViewModel() {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.IsItemClickEnabled = true;
            this.ItemClick += ItemClickedHandler;
            this.Loaded += (s, a) => {
                var parent = VisualTreeHelper.GetParent(this);
                while (parent != null && !(parent is Page) && !(parent is SplitView)) {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                if (parent != null && parent is SplitView) {
                    this.splitViewHost = parent as SplitView;
                }
            };
            HamburgerButtonCommand = new DelegateCommand();
            HamburgerButtonCommand.ExecuteAction = new Action<object>(HamburgerButton);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string PropertyName = null) {
            if (PropertyChanged != null) {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}

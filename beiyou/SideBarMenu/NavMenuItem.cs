using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace HelloBUPT.SideBarMenu {
    public class NavMenuItem {
        public Symbol Icon { get; set; }
        public string Text { get; set; }
        public Type DestPage { get; set; }
    }
}

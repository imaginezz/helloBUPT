using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBUPT.Common {
    public static class SolidBrushToColorConvert {
        public static Windows.UI.Color Convert(Windows.UI.Xaml.Media.SolidColorBrush solidColorBrush) {
            return solidColorBrush.Color;
        }

        public static Windows.UI.Xaml.Media.SolidColorBrush ConvertBack(Windows.UI.Color color) {
            return new Windows.UI.Xaml.Media.SolidColorBrush(color);
        }
    }
}

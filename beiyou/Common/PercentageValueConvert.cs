using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Diagnostics;

namespace HelloBUPT.Common {
    public class PercentageValueConvert : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            Debug.WriteLine(System.Convert.ToDouble(parameter) * System.Convert.ToDouble(value));
            return System.Convert.ToDouble(parameter) * System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return System.Convert.ToDouble(value) / System.Convert.ToDouble(parameter);
        }

    }
}

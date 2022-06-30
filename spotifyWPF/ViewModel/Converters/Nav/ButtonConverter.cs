using spotifyWPF.Model.Nav;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace spotifyWPF.ViewModel.Converters.Nav
{
    public class ButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return value;
            NavButton button = value as NavButton;
            if (button.Active)
            {
                return "../Resources/Icons/home_clicked.png";
            }
            return "../Resources/Icons/home_unclicked.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace test1234.classes
{
    public class nullTextValueToDefaultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (parameter.ToString())
            {
                case "ageRest":
                    if (value == null)
                    {
                        return null;
                    }
                    return "(" + System.Convert.ToString(value) + "+) ";

                case "releaseYear":
                    if (value == null)
                    {
                        return null;
                    }
                    return System.Convert.ToString(System.Convert.ToDateTime(value).Year) + " г.";

                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

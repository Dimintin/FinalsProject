using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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

                case "dateShort":
                    if (value == null)
                    {
                        return null;
                    }
                    return System.Convert.ToDateTime(value).ToLongTimeString();

                case "dateLong":
                    if (value == null)
                    {
                        return null;
                    }
                    return System.Convert.ToDateTime(value).ToLongDateString();

                case "photoLink":
                    if (value == null)
                    {
                        return null;
                    }
                    return "../img/icons8-male-user-32.png";

                case "username":
                    if (value == null)
                    {
                        return null;
                    }
                    return "Войти";

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

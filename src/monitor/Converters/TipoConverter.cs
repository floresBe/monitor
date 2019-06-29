using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace monitor.Converters
{
    class TipoConverter : IValueConverter
    {
        enum TipoUsuario : int { Inactivo,Ingeniero,ClaseV };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            if (value == null)
                return null;

            int tipoUsuario = int.Parse(value.ToString());
            switch (tipoUsuario)
            {
                case (int)TipoUsuario.Ingeniero:
                    result = "Ingeniero";
                    break;
                case (int)TipoUsuario.ClaseV:
                    result = "Clase V";
                    break;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

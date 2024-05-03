using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTools.Common
{
    internal class TypeConverter
    {
        internal static T? ConvertToType<T>(string? stringValue, bool throwOnError = false) where T : struct
        {
            // Versuche, den Typkonverter für den gegebenen Typ zu finden
            System.ComponentModel.TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                if (stringValue != null)
                {
                    try
                    {
                        // Konvertiere den String in den generischen Typ 
                        var convertedValue = converter.ConvertFromString(stringValue);

                        return (T?)convertedValue;
                    }
                    catch (Exception)
                    {
                        if (throwOnError)
                        {
                            throw;
                        }
                    }
                }

                return null;
            }
            else
            {
                throw new InvalidOperationException($"Cannot convert string to {typeof(T).Name}");
            }
        }
    }
}

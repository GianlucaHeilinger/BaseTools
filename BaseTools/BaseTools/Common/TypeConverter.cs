using System;
using System.ComponentModel;

namespace BaseTools.Common
{
    internal static class TypeConverter
    {
        internal static T? ConvertToType<T>(string? stringValue, bool throwOnError = false) where T : struct
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(typeof(string)))
            {
                if (!string.IsNullOrEmpty(stringValue))
                {
                    try
                    {
                        return (T?)converter.ConvertFromString(stringValue);
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
            throw new InvalidOperationException($"Cannot convert string to {typeof(T).Name}");
        }
    }
}

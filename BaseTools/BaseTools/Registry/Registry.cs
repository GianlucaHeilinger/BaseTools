using BaseTools.Common;
using BaseTools.Trace;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BaseTools.Registry
{
    public static class Registry
    {
        public static RegistryKey? RegistryKey { get; private set; }
        private const string _settingsString = "Settings";
        private const string _softwareString = "Software";

        public static void InitRegistry(string mainKey)
        {
            try
            {
                TraceWriter.WriteLine($"Init Registry with Key: '{mainKey}'", LineType.Start);

                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(_softwareString, true);
                RegistryKey = key?.CreateSubKey(mainKey, true);

                TraceWriter.WriteLine($"Init Registry with Key: '{mainKey}' successful", LineType.End);
            }
            catch (Exception)
            {
                TraceWriter.WriteLine($"Init Registry with Key: '{mainKey}' failed", LineType.End);

                throw;
            }
        }

        private static bool CheckRegistryKey()
        {
            if (RegistryKey == null) { TraceWriter.WriteLine("Registry not Initialized", LineType.End); }

            return RegistryKey != null;
        }

        public static bool SaveValueToRegistry<T>(string name, T? value, RegistryValueKind valueKind = RegistryValueKind.String, string subKeyName = _settingsString)
        {
            try
            {
                TraceWriter.WriteLine($"Saving Value to Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.Start);

                if (!CheckRegistryKey()) { return false; }
                var optionsKey = RegistryKey!.CreateSubKey(subKeyName, true);

                optionsKey?.SetValue(name, value ?? default(T) ?? (object)string.Empty, valueKind);

                TraceWriter.WriteLine($"Saved Value to Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.End);
            }
            catch
            {
                TraceWriter.WriteLine($"Saving Value to Registry failed. Name '{name}' | Subkey '{subKeyName}'", LineType.End);
                return false;
            }

            return true;
        }

        public static string? GetValueFromRegistry(string name, object? defaultValue = null, string subKeyName = _settingsString)
        {
            try
            {
                TraceWriter.WriteLine($"Retrieving Value from Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.Start);

                var value = GetValueFromRegistryInternal(name, defaultValue, subKeyName);

                TraceWriter.WriteLine($"Retrieved Value from Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.End);

                return value;
            }
            catch
            {
                TraceWriter.WriteLine($"Retrieved Value from Registry failed. Name '{name}' | Subkey '{subKeyName}' | Returning default Value", LineType.End);
                return defaultValue?.ToString();
            }
        }

        private static string? GetValueFromRegistryInternal(string name, object? defaultValue = null, string subKeyName = _settingsString)
        {
            try
            {
                if (!CheckRegistryKey()) { return defaultValue?.ToString(); }

                var key = RegistryKey!.CreateSubKey(subKeyName);

                var value = key?.GetValue(name, defaultValue)?.ToString();

                return value;
            }
            catch
            {
                throw;
            }
        }

        public static T? GetStructValueFromRegistry<T>(string name, T? defaultValue = null, string subKeyName = _settingsString) where T : struct
        {
            try
            {
                TraceWriter.WriteLine($"Retrieving Value from Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.Start);

                var value = GetValueFromRegistryInternal(name, defaultValue, subKeyName);

                TraceWriter.WriteLine($"Retrieved Value from Registry. Name '{name}' | Subkey '{subKeyName}'");

                try
                {
                    TraceWriter.WriteLine($"Converting Value from Registry to type '{typeof(T).Name}'. Name '{name}' | Subkey '{subKeyName}'");

                    var convertedValue = TypeConverter.ConvertToType<T>(value);

                    TraceWriter.WriteLine($"Converted Value from Registry to type '{typeof(T).Name}'. Name '{name}' | Subkey '{subKeyName}'", LineType.End);

                    return convertedValue;
                }
                catch (Exception)
                {
                    TraceWriter.WriteLine($"Converting Value from Registry to type '{typeof(T).Name}' failed. Name '{name}' | Subkey '{subKeyName}' | Returning default Value", LineType.End);
                    return defaultValue;
                }
            }
            catch
            {
                TraceWriter.WriteLine($"Retrieving Value from Registry failed. Name '{name}' | Subkey '{subKeyName}' | Returning default Value", LineType.End);
                return defaultValue;
            }
        }

        public static bool DeleteValue(string name, string subKeyName = _settingsString)
        {
            try
            {
                TraceWriter.WriteLine($"Deleting Value from Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.Start);

                if (!CheckRegistryKey()) { return false; }

                var key = RegistryKey!.CreateSubKey(subKeyName);

                key?.DeleteValue(name, false);

                TraceWriter.WriteLine($"Deleted Value from Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.End);
            }
            catch
            {
                TraceWriter.WriteLine($"Deleting Value from Registry failed. Name '{name}' | Subkey '{subKeyName}'", LineType.End);

                return false;
            }

            return true;
        }

        public static bool DeleteAllValues()
        {
            try
            {
                TraceWriter.WriteLine($"Deleting all Values from Registry.", LineType.Start);

                if (!CheckRegistryKey()) { return false; }

                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(_softwareString, true);
                key?.DeleteSubKeyTree(RegistryKey!.Name.Split("\\").Last(), true);

                TraceWriter.WriteLine($"Deleted all Values from Registry.", LineType.Start);
            }
            catch
            {
                TraceWriter.WriteLine($"Deleting all Values from Registry failed.", LineType.End);
                return false;
            }

            return true;
        }
    }
}

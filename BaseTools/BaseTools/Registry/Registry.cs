using BaseTools.Common;
using BaseTools.Trace;
using Microsoft.Win32;
using System;

namespace BaseTools.Registry
{
    /// <summary>
    /// Provides methods to interact with the Windows Registry.
    /// </summary>
    public static class Registry
    {
        /// <summary>
        /// Gets the current registry key.
        /// </summary>
        public static RegistryKey? ApplicationRegistryKey { get; private set; }
        private const string _settingsString = "Settings";
        private const string _softwareString = "Software";

        /// <summary>
        /// Initializes the registry with the specified key.
        /// </summary>
        /// <param name="mainKey">The main key to initialize the registry with.</param>
        public static void InitRegistry(string mainKey)
        {
            try
            {
                TraceWriter.WriteLine($"Init Registry with Key: '{mainKey}'", LineType.Start);

                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(_softwareString, true);
                ApplicationRegistryKey = key?.CreateSubKey(mainKey, true);

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
            if (ApplicationRegistryKey == null)
            {
                TraceWriter.WriteLine("Registry not Initialized", LineType.End);
            }

            return ApplicationRegistryKey != null;
        }

        /// <summary>
        /// Saves a value to the registry.
        /// </summary>
        /// <typeparam name="T">The type of the value to save.</typeparam>
        /// <param name="name">The name of the value.</param>
        /// <param name="value">The value to save.</param>
        /// <param name="valueKind">The kind of the value.</param>
        /// <param name="subKeyName">The subkey name.</param>
        /// <returns>True if the value was saved successfully; otherwise, false.</returns>
        public static bool SaveValueToRegistry<T>(string name, T? value, RegistryValueKind valueKind = RegistryValueKind.String, string subKeyName = _settingsString)
        {
            try
            {
                TraceWriter.WriteLine($"Saving Value to Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.Start);

                if (!CheckRegistryKey()) return false;
                using var optionsKey = ApplicationRegistryKey!.CreateSubKey(subKeyName, true);

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

        /// <summary>
        /// Retrieves a value from the registry.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <param name="defaultValue">The default value to return if the value is not found.</param>
        /// <param name="subKeyName">The subkey name.</param>
        /// <returns>The retrieved value, or the default value if the value is not found.</returns>
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
            if (!CheckRegistryKey()) return defaultValue?.ToString();

            using var key = ApplicationRegistryKey!.CreateSubKey(subKeyName);
            return key?.GetValue(name, defaultValue)?.ToString();
        }

        /// <summary>
        /// Retrieves a struct value from the registry.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="name">The name of the value.</param>
        /// <param name="defaultValue">The default value to return if the value is not found.</param>
        /// <param name="subKeyName">The subkey name.</param>
        /// <returns>The retrieved value, or the default value if the value is not found.</returns>
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

        /// <summary>
        /// Deletes a value from the registry.
        /// </summary>
        /// <param name="name">The name of the value.</param>
        /// <param name="subKeyName">The subkey name.</param>
        /// <returns>True if the value was deleted successfully; otherwise, false.</returns>
        public static bool DeleteValue(string name, string subKeyName = _settingsString)
        {
            try
            {
                TraceWriter.WriteLine($"Deleting Value from Registry. Name '{name}' | Subkey '{subKeyName}'", LineType.Start);

                if (!CheckRegistryKey()) return false;

                using var key = ApplicationRegistryKey!.CreateSubKey(subKeyName);
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

        /// <summary>
        /// Deletes all values from the registry.
        /// </summary>
        /// <returns>True if all values were deleted successfully; otherwise, false.</returns>
        public static bool DeleteAllValues()
        {
            try
            {
                TraceWriter.WriteLine($"Deleting all Values from Registry.", LineType.Start);

                if (!CheckRegistryKey()) return false;

                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(_softwareString, true);
                key?.DeleteSubKeyTree(ApplicationRegistryKey!.Name.Split("\\").Last(), true);

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
using BaseTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BaseToolsTests
{
    [TestClass]
    public class RegistryTests
    {
        [TestInitialize]
        public void Init()
        {
            Registry.InitRegistry("BaseToolsTests");
        }

        [TestMethod]
        public void AddValueToRegistry()
        {
            var testValue = "TestValue";
            var testKey = "TestKey";
            var testCustomSubKey = "TestSubKey";

            //Standard
            bool saved = Registry.SaveValueToRegistry(testKey, testValue);

            if (!saved)
            {
                Assert.Fail("Saving Value to Registry failed");
            }

            //SubKey
            saved = Registry.SaveValueToRegistry(testKey, testValue, subKeyName: testCustomSubKey);

            if (!saved)
            {
                Assert.Fail("Saving Value to Registry failed");
            }
        }

        [TestMethod]
        public void GetValueFromRegistry()
        {
            var testValue = "TestValue";
            var testKey = "TestKey";
            var testCustomSubKey = "TestSubKey";

            //Standard
            Registry.SaveValueToRegistry(testKey, testValue);

            var value = Registry.GetValueFromRegistry(testKey);

            Assert.AreEqual(testValue, value);

            //SubKey
            Registry.SaveValueToRegistry(testKey, testValue, subKeyName: testCustomSubKey);

            value = Registry.GetValueFromRegistry(testKey, subKeyName: testCustomSubKey);

            Assert.AreEqual(testValue, value);
        }

        [TestMethod]
        public void DeleteValueFromRegistry()
        {
            var testValue = "TestValue";
            var testKey = "TestKey";
            var testCustomSubKey = "TestSubKey";

            //Standard
            Registry.SaveValueToRegistry(testKey, testValue);

            var value = Registry.GetValueFromRegistry(testKey);

            Assert.AreEqual(testValue, value);

            Registry.DeleteValue(testKey);

            value = Registry.GetValueFromRegistry(testKey);

            Assert.AreNotEqual(testValue, value);
            Assert.AreEqual(default, value);

            //SubKey
            Registry.SaveValueToRegistry(testKey, testValue, subKeyName: testCustomSubKey);

            value = Registry.GetValueFromRegistry(testKey, subKeyName: testCustomSubKey);

            Assert.AreEqual(testValue, value);

            Registry.DeleteValue(testKey, subKeyName: testCustomSubKey);

            value = Registry.GetValueFromRegistry(testKey, subKeyName: testCustomSubKey);

            Assert.AreNotEqual(testValue, value);
            Assert.AreEqual(default, value);
        }

        [TestMethod]
        public void DeleteAllValuesFromRegistry()
        {
            var regValues = new Dictionary<string, string>();

            for (int i = 0; i < 10; i++)
            {
                var valueToCreate = "Value " + i;
                var keyToCreate = "Key " + i;

                regValues.Add(keyToCreate, valueToCreate);
            }

            foreach (var regItemToCreate in regValues)
            {
                Registry.SaveValueToRegistry(regItemToCreate.Key, regItemToCreate.Value);
            }

            Registry.DeleteAllValues();

            foreach (var regItemToCreate in regValues)
            {
                var value = Registry.GetValueFromRegistry(regItemToCreate.Key);
                
                Assert.AreNotEqual(regItemToCreate.Value, value);
            }
        }
    }
}
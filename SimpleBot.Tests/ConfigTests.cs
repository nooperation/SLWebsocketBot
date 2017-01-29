using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBot.Tests
{
  [TestFixture]
  class ConfigTests
  {
    public struct TestStruct
    {
      public string NameProperty { get; set; }
      public string _name_field;
    }

    public class TestClass
    {
      public string NameProperty { get; set; }
      public List<string> StringList { get; set; }
      public string _name_field;
    }

    private readonly string kProgramName = "SimpleBot.Tests";

    // Primitives
    private readonly bool kTestBool = true;
    private readonly byte kTestByte = 1;
    private readonly sbyte kTestSbyte = 2;
    private readonly short kTestShort = 3;
    private readonly ushort kTestUshort = 4;
    private readonly int kTestInt = 5;
    private readonly uint kTestUint = 6;
    private readonly long kTestLong = 7;
    private readonly ulong kTestUlong = 8;
    private readonly char kTestChar = '9';
    private readonly double kTestDouble = 10.1010;
    private readonly float kTestFloat = 11.1111f;
    private readonly string kTestString = "Hello World";

    // Advanced
    private readonly string[] kTestStringArray = new string[] { "Foo", "Bar", "Baz" };
    private readonly List<string> kTestStringList = new List<string>() { "Foo", "Bar", "Baz" };
    private readonly List<List<string>> kTestListOfStringLists = new List<List<string>>() {
      new List<string>() { "Foo", "Bar" },
      new List<string>() { "Derp" }
    };
    private readonly Dictionary<string, string> kTestStringDictionary = new Dictionary<string, string>() {
      {"Key", "Value" },
      {"Hello", "World" },
      {"Foo", "Bar" }
    };
    private readonly TestClass kTestClass = new TestClass()
    {
      NameProperty = "Name property goes here",
      _name_field = "Name field goes here",
      StringList = new List<string>() {
          "One",
          "Two",
          "Three"
        }
    };
    private readonly TestStruct kTestStruct = new TestStruct()
    {
      NameProperty = "Struct name property",
      _name_field = "Struct name field"
    };

    private void SetValues(Config config)
    {
      config.SetSetting("test_bool", kTestBool);
      config.SetSetting("test_byte", kTestByte);
      config.SetSetting("test_sbyte", kTestSbyte);
      config.SetSetting("test_short", kTestShort);
      config.SetSetting("test_ushort", kTestUshort);
      config.SetSetting("test_int", kTestInt);
      config.SetSetting("test_uint", kTestUint);
      config.SetSetting("test_long", kTestLong);
      config.SetSetting("test_ulong", kTestUlong);
      config.SetSetting("test_char", kTestChar);
      config.SetSetting("test_double", kTestDouble);
      config.SetSetting("test_float", kTestFloat);
      config.SetSetting("test_string", kTestString);
    }

    private void SetValuesAdvanced(Config config)
    {
      config.SetSetting("test_string_array", kTestStringArray);
      config.SetSetting("test_string_list", kTestStringList);
      config.SetSetting("test_list_of_string_lists", kTestListOfStringLists);
      config.SetSetting("test_string_dictionary", kTestStringDictionary);
      config.SetSetting("test_struct", kTestStruct);
      config.SetSetting("test_class", kTestClass);
    }

    private void CheckValuesAdvanced(Config config)
    {
      var test_string_array = config.GetSetting<string[]>("test_string_array");
      Assert.AreEqual(test_string_array.GetType(), typeof(string[]));
      Assert.AreEqual(test_string_array, kTestStringArray);

      var test_string_list = config.GetSetting<List<string>>("test_string_list");
      Assert.AreEqual(test_string_list.GetType(), typeof(List<string>));
      Assert.AreEqual(test_string_list, kTestStringList);

      var test_list_of_string_lists = config.GetSetting<List<List<string>>>("test_list_of_string_lists");
      Assert.AreEqual(test_list_of_string_lists.GetType(), typeof(List<List<string>>));
      Assert.AreEqual(test_list_of_string_lists, kTestListOfStringLists);

      var test_string_dictionary = config.GetSetting<Dictionary<string, string>>("test_string_dictionary");
      Assert.AreEqual(test_string_dictionary.GetType(), typeof(Dictionary<string, string>));
      Assert.AreEqual(test_string_dictionary, kTestStringDictionary);

      var test_struct = config.GetSetting<TestStruct>("test_struct");
      Assert.AreEqual(test_struct.GetType(), typeof(TestStruct));
      Assert.AreEqual(test_struct, kTestStruct);

      var test_class = config.GetSetting<TestClass>("test_class");
      Assert.AreEqual(test_class.GetType(), typeof(TestClass));
      Assert.AreEqual(test_class.NameProperty, kTestClass.NameProperty);
      Assert.AreEqual(test_class.StringList, kTestClass.StringList);
      Assert.AreEqual(test_class._name_field, kTestClass._name_field);
    }

    private void CheckValues(Config config)
    {
      var test_bool = config.GetSetting<bool>("test_bool");
      Assert.AreEqual(test_bool.GetType(), typeof(bool));
      Assert.AreEqual(test_bool, kTestBool);

      var test_byte = config.GetSetting<byte>("test_byte");
      Assert.AreEqual(test_byte.GetType(), typeof(byte));
      Assert.AreEqual(test_byte, kTestByte);

      var test_sbyte = config.GetSetting<sbyte>("test_sbyte");
      Assert.AreEqual(test_sbyte.GetType(), typeof(sbyte));
      Assert.AreEqual(test_sbyte, kTestSbyte);

      var test_short = config.GetSetting<short>("test_short");
      Assert.AreEqual(test_short.GetType(), typeof(short));
      Assert.AreEqual(test_short, kTestShort);

      var test_ushort = config.GetSetting<ushort>("test_ushort");
      Assert.AreEqual(test_ushort.GetType(), typeof(ushort));
      Assert.AreEqual(test_ushort, kTestUshort);

      var test_int = config.GetSetting<int>("test_int");
      Assert.AreEqual(test_int.GetType(), typeof(int));
      Assert.AreEqual(test_int, kTestInt);

      var test_uint = config.GetSetting<uint>("test_uint");
      Assert.AreEqual(test_uint.GetType(), typeof(uint));
      Assert.AreEqual(test_uint, kTestUint);

      var test_long = config.GetSetting<long>("test_long");
      Assert.AreEqual(test_long.GetType(), typeof(long));
      Assert.AreEqual(test_long, kTestLong);

      var test_ulong = config.GetSetting<ulong>("test_ulong");
      Assert.AreEqual(test_ulong.GetType(), typeof(ulong));
      Assert.AreEqual(test_ulong, kTestUlong);

      var test_char = config.GetSetting<char>("test_char");
      Assert.AreEqual(test_char.GetType(), typeof(char));
      Assert.AreEqual(test_char, kTestChar);

      var test_double = config.GetSetting<double>("test_double");
      Assert.AreEqual(test_double.GetType(), typeof(double));
      Assert.AreEqual(test_double, kTestDouble);

      var test_float = config.GetSetting<float>("test_float");
      Assert.AreEqual(test_float.GetType(), typeof(float));
      Assert.AreEqual(test_float, kTestFloat);

      var test_string = config.GetSetting<string>("test_string");
      Assert.AreEqual(test_string.GetType(), typeof(string));
      Assert.AreEqual(test_string, kTestString);
    }

    [Test]
    public void TestInMemory()
    {
      Config config = new Config(kProgramName);
      SetValues(config);
      CheckValues(config);
    }

    [Test]
    public void TestWriteAndRead()
    {
      Config config = new Config(kProgramName);
      SetValues(config);
      config.Save();

      config = new Config(kProgramName);
      config.Load();
      CheckValues(config);
    }

    [Test]
    public void TestInMemoryAdvanced()
    {
      Config config = new Config(kProgramName);
      SetValuesAdvanced(config);
      CheckValuesAdvanced(config);
    }

    [Test]
    public void TestWriteAndReadAdvanced()
    {
      Config config = new Config(kProgramName);
      SetValuesAdvanced(config);
      config.Save();

      config = new Config(kProgramName);
      config.Load();
      CheckValuesAdvanced(config);
    }

    [Test]
    public void TestUnknownSettings()
    {
      Config config = new Config(kProgramName);
      Assert.Catch(() =>
      {
        var test_string_array = config.GetSetting<string>("test_string");
      });
    }

    [Test]
    public void TestUnknownSettingsWithDefault()
    {
      Config config = new Config(kProgramName);
      var test_string_array = config.GetSetting<string>("test_string", "Default");
      Assert.AreEqual(test_string_array, "Default");
    }
    
    [Test]
    public void TestBadConversion()
    {
      Config config = new Config(kProgramName);
      config.SetSetting("test_string_list", kTestStringList);

      Assert.Catch(() =>
      {
        var test_string = config.GetSetting<string>("test_string_list");
      });
    }
  }
}

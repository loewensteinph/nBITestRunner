using System.Configuration;

namespace TestrunnerHelper.Config
{
    public class TestSuitesCollection : ConfigurationElementCollection
    {
        public new TestSuiteConfigElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;

                return (TestSuiteConfigElement) BaseGet(name);
            }
        }

        public TestSuiteConfigElement this[int index] => (TestSuiteConfigElement) BaseGet(index);

        public override ConfigurationElementCollectionType CollectionType =>
            ConfigurationElementCollectionType.BasicMap;

        protected override string ElementName => "testSuite";

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (var idx = 0; idx < Count; idx++)
                if (this[idx].Name.ToLower() == name)
                    return idx;
            return -1;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TestSuiteConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TestSuiteConfigElement) element).Name;
        }

        public void Add(TestSuitesConfigElement details)
        {
            BaseAdd(details);
        }
    }
}
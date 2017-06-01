using System.Configuration;

namespace TestrunnerHelper.Config
{
    public class TestProjectCollection : ConfigurationElementCollection
    {
        public TestProjectCollection()
        {
            var details = (TestProjectConfigElement) CreateNewElement();
            if (details.RelativePath != "")
                Add(details);
        }

        public override ConfigurationElementCollectionType CollectionType =>
            ConfigurationElementCollectionType.BasicMap;

        public TestProjectConfigElement this[int index]
        {
            get => (TestProjectConfigElement) BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new TestProjectConfigElement this[string name] => (TestProjectConfigElement) BaseGet(name);

        protected override string ElementName => "testProject";

        protected override ConfigurationElement CreateNewElement()
        {
            return new TestProjectConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TestProjectConfigElement) element).RelativePath;
        }

        public int IndexOf(TestProjectConfigElement details)
        {
            return BaseIndexOf(details);
        }

        public void Add(TestProjectConfigElement details)
        {
            BaseAdd(details);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(TestProjectConfigElement details)
        {
            if (BaseIndexOf(details) >= 0)
                BaseRemove(details.RelativePath);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
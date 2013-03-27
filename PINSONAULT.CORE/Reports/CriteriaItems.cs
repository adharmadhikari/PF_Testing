using System;

namespace Pinsonault.Data.Reports
{

    public class CriteriaItem
    {
        public string Key;
        public string Title;
        public string Text;
        private readonly Func<string, string> _formatter;

        public CriteriaItem(string key, string title) : this(key, title, null)
        {
        }
        public CriteriaItem(string key, string title, Func<string, string> formatter)
        {
            Key = key;
            Title = title;
            _formatter = formatter;
        }

        public void Evaluate(string value)
        {
            Text = _formatter(value);
        }

        //public String Format(String value)
        //{
        //    return _formatter(value);
        //}
    }
}
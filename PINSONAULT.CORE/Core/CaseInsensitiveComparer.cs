using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinsonault.Core
{
    public class CaseInsensitiveComparer : System.Collections.Generic.EqualityComparer<string>
    {
        public override bool Equals(string x, string y)
        {
            return string.Compare(x, y, true) == 0;
        }

        public override int GetHashCode(string obj)
        {
            if(obj != null)
                return obj.ToLower().GetHashCode();

            return 0;
        }

    }
}

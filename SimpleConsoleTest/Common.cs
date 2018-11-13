using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsoleTest
{
    public class Compare<T>
    {
        public static bool Equal(List<T> t1, List<T> t2)
        {
            if (t1 == null && t2 == null)
            {
                return true;
            }
            else if (t1 == null && t2 != null)
            {
                return false;
            }
            else if (t1 != null && t2 == null)
            {
                return false;
            }
            else if (t1.GetType() != t2.GetType())
            {
                return false;
            }
            return false;
        }
    }
}

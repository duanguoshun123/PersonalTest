using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Class
{
    /// <summary>
    /// 快速计算质数
    /// </summary>
    public class AlgorithmGossip
    {
        public void AlgorithmGossipCal()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5, 12, 19, 65, 66, 67, 58, 57, 60, 106, 101, 103, 109, 1036, 1056, 189, 203, 209, 205, 206, 210, 398, 397 };
            var listZhiShu = new List<int>();
            Console.WriteLine("list所有数");
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write($"{list[i]}、");
                var j = 2;
                var key = 0;
                for (j = 2; j <= Math.Sqrt(list[i]); j++)
                {
                    if (list[i] % j == 0)
                    {
                        key = 1;
                        break;
                    }
                }
                if (j > Math.Sqrt(list[i]) && key == 0)
                {
                    listZhiShu.Add(list[i]);
                }
            }
            Console.WriteLine("list中质数");
            for (int i = 0; i < listZhiShu.Count; i++)
            {
                Console.Write($"{listZhiShu[i]}、");
            }
        }
    }
}

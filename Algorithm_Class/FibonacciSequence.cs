using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Class
{
    /// <summary>
    /// 费氏数列
    /// </summary>
    public class FibonacciSequence
    {
        /// <summary>
        /// 费氏数列计算
        /// Fibonacci为1200年代的欧洲数学家，在他的着作中曾经提到：「若有一只免子每个月生一只小免
        /// 子，一个月后小免子也开始生产。起初只有一只免子，一个月后就有两只免子，二个月后有三
        /// 只免子，三个月后有五只免子（小免子投入生产）……。
        /// 如以下： 1、1 、2、3、5、8、13、21、34、55、89……
        /// 依说明，我们可以将费氏数列定义为以下：
        /// fn = fn-1 + fn-2
        /// fn = n
        /// if n > 1
        /// if n = 0, 1
        /// </summary>
        /// <param name="n"></param>
        public int FibonacciSequenceCal(int n)
        {
            if (n == 0 || n == 1)
            {
                return 1;
            }
            else
            {
                return FibonacciSequenceCal(n - 1) + FibonacciSequenceCal(n - 2);
            }
        }
    }
}

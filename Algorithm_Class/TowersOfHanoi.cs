using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm_Class
{
    /// <summary>
    /// 算法之河内之塔
    /// </summary>
    public class TowersOfHanoi
    {
        /// <summary>
        /// 如果柱子标为ABC，要由A搬至C，在只有一个盘子时，就将它直接搬至C，当有两个盘 
        /// 子，就将B当作辅助柱。如果盘数超过2个，将第三个以下的盘子遮起来，就很简单了，每次处
        /// 理两个盘子，也就是：A->B、A ->C、B->C这三个步骤，而被遮住的部份，其实就是进入程式
        /// 的递回处理。事实上，若有n个盘子，则移动完毕所需之次数为2^n - 1，所以当盘数为64时，则64 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        public void Hanoi(int n, char A, char B, char C)
        {
            if (n == 1)
            {
                Console.WriteLine($"Move sheet {n} from {A} to {C}\n");
            }
            else
            {
                Hanoi(n - 1, A, B, C);
                Console.WriteLine($"Move sheet {n} from {A} to {C}\n");
                Hanoi(n - 1, B, A, C);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm_Class;

namespace AlgorithmConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // 河内之塔测试
            int n;
            Console.Write("请输入盘数: ");
            n = int.Parse(Console.ReadLine());
            TowersOfHanoi hanoi=new TowersOfHanoi();
            hanoi.Hanoi(n,'A','B','C');
        }
    }
}

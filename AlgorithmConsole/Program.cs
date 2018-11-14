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

            //#region 河内之塔测试
            //int n;
            //Console.Write("请输入盘数: ");
            //n = int.Parse(Console.ReadLine());
            //TowersOfHanoi hanoi = new TowersOfHanoi();
            //hanoi.Hanoi(n, 'A', 'B', 'C');
            //#endregion

            //#region 费列算法测试
            //FibonacciSequence fibonacciSequence = new FibonacciSequence();
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.Write($"{fibonacciSequence.FibonacciSequenceCal(i)}、");
            //} 
            //#endregion
            #region 快速找出质数
            AlgorithmGossip algorithmGossip = new AlgorithmGossip();
            algorithmGossip.AlgorithmGossipCal();
            // 超大数据计算（判断是否是质数为例）
            algorithmGossip.IsPrime();

            #endregion
        }
    }
}

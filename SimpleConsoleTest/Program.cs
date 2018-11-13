using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConsoleTest
{
    class Program
    {
        public const int MAXN = 10000;
        static int m_Move = 0;
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            recurHanoi(5);
            sw.Stop();
            Console.WriteLine("递归算法Stopwatch 时间精度：{0}ms", sw.ElapsedMilliseconds);
            sw.Start();
            notRecurHanoi(5);
            sw.Stop();
            Console.WriteLine("非递归算法Stopwatch 时间精度：{0}ms", sw.ElapsedMilliseconds);
            RecordBoilerInfo.Test();


        }
        static int RecursionStu(int index)
        {
            if (index == 0 || index == 1)
            {
                return 1;
            }
            else
            {
                return RecursionStu(index - 1) + RecursionStu(index - 2);
            }
        }
        static void recurHanoi(char from, char use, char to, int n)
        {
            if (0 == n)
                return;
            recurHanoi(from, to, use, n - 1);
            Console.WriteLine($"{n}从{from}移到{to}");
            ++m_Move;
            recurHanoi(use, from, to, n - 1);
        }
        static void recurHanoi(int n)
        {
            Console.WriteLine("*********递归算法**********");
            m_Move = 0;
            recurHanoi('A', 'B', 'C', n);
            Console.WriteLine($"总共移动{m_Move}次");
            Console.WriteLine($"***************************");
        }
        static void recurHanoi02(char from, char use, char to, int n)
        {
            if (11 == n)
                return;
            recurHanoi02(use, from, to, n++);
            Console.WriteLine($"{n}从{from}移到{to}");
            ++m_Move;
        }


        struct Node
        {
            public int number;
            public int id;
            public char from;
            public char use;
            public char to;
        };
        static void notRecurHanoi(int n)
        {
            Console.WriteLine("*********非递归算法**********");
            Node[] myStack = new Node[MAXN];
            Node now;
            int top = 0;
            m_Move = 0;
            now.from = 'A'; now.use = 'B'; now.to = 'C'; now.number = n; now.id = n;
            myStack[++top] = now;
            char from, use, to;
            int number, id;
            while (top > 0)
            {
                if (1 == myStack[top].number)
                {
                    print(myStack[top]);
                    --top;
                }
                else
                {
                    from = myStack[top].from; use = myStack[top].use; to = myStack[top].to; number = myStack[top].number; id = myStack[top].id;
                    --top;

                    now.from = use; now.use = from; now.to = to; now.number = number - 1; now.id = id - 1;
                    myStack[++top] = now;

                    now.from = from; now.use = use; now.to = to; now.number = 1; now.id = id;
                    myStack[++top] = now;

                    now.from = from; now.use = to; now.to = use; now.number = number - 1; now.id = id - 1;
                    myStack[++top] = now;
                }
               
            }
            Console.WriteLine($"总共移动{m_Move}次");
            Console.WriteLine($"***************************");
        }
        static void print(Node now)
        {
            ++m_Move;
            Console.WriteLine($"{now.id}从{now.from}移到{now.to}");
        }
    }
}

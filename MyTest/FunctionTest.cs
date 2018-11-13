using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyTest
{
    public delegate int AddHandler(int a, int b, int c);
    [TestClass]
    public class FunctionTest
    {
        [TestMethod]
        public void Distinct()
        {
            List<Student> list = new List<Student>()
            {
                new Student()
                {
                    id = 1,
                    name = "guoshun",
                    sex = "男"
                },
                new Student()
                {
                    id = 2,
                    name = "guoshun",
                    sex = "女"
                }  ,
                new Student()
                {
                    id = 3,
                    name = "guoshun3",
                    sex = "男"
                },
                new Student()
                {
                    id = 1,
                    name = "guoshun",
                    sex = "男"
                }
            };
            list = list.GroupBy(p => new { p.name, p.id }).Select(r => r.First()).ToList();

        }
        [TestMethod]
        public void DecimalTest()
        {
            decimal testData = new decimal(0.123);
            var resultData = Decimal.Round(testData, 17);
            return;
        }
        [TestMethod]
        public void TaskTest()
        {
            List<int> listInt = new List<int>();
            List<string> listString = new List<string>();
            Task task1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("task1,task1运行前的结果listInt 个数：{0}", listInt.Count);
                for (int i = 0; i < 10000000; i++)
                {
                    listInt.Add(i);
                }
                Console.WriteLine("task1,task1运行的结果listInt 个数：{0}", listInt.Count);
            });
            Console.WriteLine("task1结束,task1运行的结果listInt 个数：{0}", listInt.Count);
            Task task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("task2,task2运行前的结果listString 个数：{0}", listString.Count);
                for (int i = 0; i < 10000; i++)
                {
                    listString.Add("string_" + i);
                }
                Console.WriteLine("task2,task2运行后的结果listString 个数：{0}", listString.Count);
            });
            Console.WriteLine("task2结束,task2运行的结果listString 个数：{0}", listString.Count);
            Task.WaitAll(task1, task2);
            Console.WriteLine("结束,listInt 个数：{0},listString 个数：{1}", listInt.Count, listString.Count);
        }
        [TestMethod]
        public void AsynTest()
        {
            Console.WriteLine("===== 异步回调 AsyncInvokeTest =====");
            AddHandler handler = new AddHandler(AddClass.Add);

            //异步操作接口(注意BeginInvoke方法的不同！)
            IAsyncResult result = handler.BeginInvoke(1, 2, 3, new AsyncCallback(CallbackFunc), "AsycState:OK");

            Console.WriteLine("------继续做别的事情。。。--------");
            return;
        }
        [TestMethod]
        public void AsynTest02()
        {
            Thread td1 = new Thread(new ThreadStart(SynchronousCall.Main_S));
            Thread td2 = new Thread(new ThreadStart(AsynchronousCall.Main_S));
            td1.Start();
            td2.Start(2000);
            //SynchronousCall.Main_S();
            //AsynchronousCall.Main_S();
        }
        static Thread Mainthread;  //静态变量，用来获取主线程
        [TestMethod]
        public void ThreadTest()
        {
            Mainthread = Thread.CurrentThread;//获取主线程
            Console.WriteLine("在主进程中启动一个线程!");
            Thread firstChild = new Thread(new ParameterizedThreadStart(ThreadProc));//threadStart 是一个委托,代表一个类型的方法
            firstChild.Name = "线程1";
            firstChild.IsBackground = true;
            firstChild.Start(firstChild.Name);//启动线程
            Thread secondChild = new Thread(new ParameterizedThreadStart(ThreadProc));
            secondChild.Name = "线程2";
            secondChild.IsBackground = true;
            secondChild.Start(secondChild.Name);
            Console.WriteLine("主线程结束");
            Console.WriteLine(Mainthread.ThreadState);
            Mainthread.Abort();
        }
        [TestMethod]
        public void AsynTest03()
        {
            //#region 异步执行
            //Func<int, int, int> d1 = TakeAWhile;
            //IAsyncResult ar = d1.BeginInvoke(1, 3000, null, null);
            //while (!ar.IsCompleted)
            //{
            //    Console.Write(".");
            //    Thread.Sleep(50);
            //}
            //int result = d1.EndInvoke(ar);
            //Console.Write("result:{0}", result);
            //#endregion

            #region 异步回调执行
            Func<int, int, int> d2 = TakeAWhile;
            IAsyncResult ar2 = d2.BeginInvoke(1, 3000, TakesAWhileCompleted, d2);
            for (int i = 0; i < 100; i++)
            {
                Console.Write(".");
                Thread.Sleep(50);
            }
            #endregion
        }
        public static void CallbackFunc(IAsyncResult result)
        {
            //result 是“加法类.Add()方法”的返回值             
            //AsyncResult 是IAsyncResult接口的一个实现类，引用空间：System.Runtime.Remoting.Messaging             
            //AsyncDelegate 属性可以强制转换为用户定义的委托的实际类。
            AddHandler handler = (AddHandler)((AsyncResult)result).AsyncDelegate;
            Console.WriteLine(handler.EndInvoke(result));
            Console.WriteLine(result.AsyncState);
        }
        public static int TakeAWhile(int data, int ms)
        {
            Console.Write("TakesAWhile started");
            Thread.Sleep(ms);
            Console.WriteLine("TakeAWhile Completed");
            return ++data;
        }
        public static void TakesAWhileCompleted(IAsyncResult ar)
        {
            if (ar == null)
                throw new ArgumentNullException("ar");
            Func<int, int, int> dl = (Func<int, int, int>)ar.AsyncState;
            int result = dl.EndInvoke(ar);
            Console.WriteLine("Result:{0}", result);
        }
        private static void ThreadProc(object str)
        {

            for (int i = 0; i < 10; i++)
            {

                Console.WriteLine(Mainthread.ThreadState);
                Console.Write(str + "调用ThreadProc: " + i.ToString() + "\r\n");
                if (i == 9)
                    Console.WriteLine(str + "结束");
                Thread.Sleep(2000);//线程被阻塞的毫秒数。0表示应挂起此线程以使其他等待线程能够执行
            }
        }
    }
    public class AddClass
    {
        public static int Add(int a, int b, int c)
        {
            Console.WriteLine("\n开始计算：" + a + "+" + b + "+" + c);
            Thread.Sleep(8000); //模拟该方法运行三秒
            Console.WriteLine("计算完成！");
            return a + b + c;
        }

        public static int Jian(int a, int b, int c)
        {
            Console.WriteLine("\n开始计算：" + a + "-" + b + "-" + c);
            Thread.Sleep(5000); //模拟该方法运行三秒
            Console.WriteLine("计算完成！");
            return a - b - c;
        }
    }
    /// <summary>
    /// 同步调用
    /// </summary>
    public class SynchronousCall
    {
        public static void Main_S()
        {
            Console.WriteLine("===== 同步调用 SyncInvokeTest =====");
            AddHandler handler = new AddHandler(AddClass.Add);
            int result = handler.Invoke(1, 2, 3);

            Console.WriteLine("继续做别的事情。。。");

            Console.WriteLine(result);
        }

    }
    /// <summary>
    /// 异步调用
    /// </summary>
    public class AsynchronousCall
    {
        public static void Main_S()
        {
            Console.WriteLine("===== 异步调用 AsyncInvokeTest =====");
            AddHandler handler = new AddHandler(AddClass.Jian);

            //IAsyncResult: 异步操作接口(interface)
            //BeginInvoke: 委托(delegate)的一个异步方法的开始
            IAsyncResult result = handler.BeginInvoke(1, 2, 3, null, null);
            Console.WriteLine("------继续做别的事情。。。\n");

            //异步操作返回
            Console.WriteLine(handler.EndInvoke(result));
            return;
        }

    }

    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
    }

}

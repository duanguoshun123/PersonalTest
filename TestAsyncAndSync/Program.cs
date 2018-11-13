using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAsyncAndSync
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Async Test job:");

            Console.WriteLine("main start..");

            Console.WriteLine("MyMethod()异步方法同步执行：");

            //MyMethod().Wait();//在main中等待async方法执行完成

            Int64 i = MyMethod().GetAwaiter().GetResult();//用于在main中同步获取async方法的返回结果

            Console.WriteLine("result is :" + i);

            Console.WriteLine("main end..");

            Console.ReadKey(true);
        }

        static async Task<Int64> MyMethod()
        {
            var result = await Task.Run(async () =>
            {
                return await TaskMethod();
            });
            return result;
        }
        private static async Task<Int64> TaskMethod()
        {  //记录调用接口前的时间  
            TimeSpan startTime = new TimeSpan(DateTime.Now.Ticks);
            Int64 result = 0;
            for (Int64 i = 0; i < 1000000000; i++)
            {
                result += i;
                //await Task.Delay(1); //模拟耗时操作
            }
            //记录调用接口后的时间  
            TimeSpan endTime = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = endTime.Subtract(startTime).Duration();

            //计算时间间隔，求出调用接口所需要的时间  
            String spanTime = ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分" + ts.Seconds.ToString() + "秒" + ts.Milliseconds.ToString();
            //打印时间  
            var timeSpend = spanTime;
            string msg = string.Format("{0}条相加,花费{1}时间", 100000000, spanTime);
            Console.WriteLine(msg);
            return result;
        }
    }
}

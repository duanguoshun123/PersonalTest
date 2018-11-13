using RedisCacheHelper.DbContextTem;
using RedisCacheHelper.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RedisCacheHelper
{
    class Program
    {
        static void Main(string[] args)
        {

            //RedisClient client = new RedisClient("localhost", 6379);
            //client.FlushAll();
            //Customer customer1 = new Customer() { Id = new Guid(), Name = "测试", Password = "123" };
            //client.Add<Customer>("Customer",customer1);
            //var customers = client.GetAll<Customer>();
            //foreach (var item in customers)
            //{
            //    Console.WriteLine($"id:{item.Id}\t name:{item.Name}\t password:{item.Password}");
            //}
            //using (var context=new MyDbContext())
            //{

            //}
            var CONNECTION_STRING = "server=.;uid=sa;pwd=Miss20170129;database=CodeFirstDemoDb";
            using (var con = new SqlConnection(CONNECTION_STRING))
            {
                con.Open();
                var cmd = new SqlCommand("delete from Customers", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("insert into Customers values ('" + Guid.NewGuid() + "',N'test01',N'Ab123')", con);
                cmd.ExecuteNonQuery();
            }

            //for (var i = 0; i < 5; i++)
            //{
            //    Task.Factory.StartNew((state) =>
            //    {
            //        using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            //        {
            //            using (var con = new SqlConnection(CONNECTION_STRING))
            //            {
            //                con.Open();

            //                var index = (int)state + 1;
            //                var padding = new String(' ', (index - 1) * 3);

            //                Console.WriteLine(string.Format("{0}第{1}次：开始读取", padding, index));
            //                var cmd = new SqlCommand("select top 1 * from Customers(holdlock)", con);
            //                cmd.ExecuteReader().Close();

            //                Console.WriteLine(string.Format("{0}第{1}次：开始休眠", padding, index));
            //                System.Threading.Thread.Sleep(1000 * ((int)state + 1));

            //                Console.WriteLine(string.Format("{0}第{1}次：开始修改", padding, index));
            //                cmd = new SqlCommand("update Customers set Name = N'段光宇" + state + "' where Name=N'test01'", con);
            //                cmd.ExecuteNonQuery();
            //                Console.WriteLine(string.Format("{0}第{1}次：完成修改", padding, index));
            //            }

            //            ts.Complete();
            //        }
            //    }, i).ContinueWith((t) =>
            //    {
            //        Console.WriteLine(t.Exception.InnerException.Message);
            //    }, TaskContinuationOptions.OnlyOnFaulted);
            //}
            //for (var i = 0; i < 5; i++)
            //{
            //    Task.Factory.StartNew((state) =>
            //    {
            //        using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
            //        {
            //            using (var con = new SqlConnection(CONNECTION_STRING))
            //            {
            //                con.Open();

            //                var index = (int)state + 1;
            //                var padding = new String(' ', (index - 1) * 3);
            //                var databaseName = con.Database;
            //                Console.WriteLine(string.Format("{0}第{1}次：开始读取", padding, index));
            //                var cmd = new SqlCommand("select top 1 * from Customers", con);
            //                cmd.ExecuteReader().Close();

            //                Console.WriteLine(string.Format("{0}第{1}次：开始休眠", padding, index));
            //                System.Threading.Thread.Sleep(1000 * ((int)state + 1));

            //                Console.WriteLine(string.Format("{0}第{1}次：开始修改", padding, index));
            //                cmd = new SqlCommand($"ALTER DATABASE {databaseName} SET ALLOW_SNAPSHOT_ISOLATION ON \t\n  update Customers set Name = N'段光宇" + state + "' where Name=N'test01'", con);
            //                cmd.ExecuteNonQuery();
            //                Console.WriteLine(string.Format("{0}第{1}次：完成修改", padding, index));
            //            }

            //            ts.Complete();
            //        }
            //    }, i).ContinueWith((t) =>
            //    {
            //        Console.WriteLine(t.Exception.InnerException.Message);
            //    }, TaskContinuationOptions.OnlyOnFaulted);
            //}


            //for (var i = 0; i < 5; i++)
            //{
            //    Task.Factory.StartNew((state) =>
            //    {
            //        using (var ts = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            //        {
            //            using (var con = new SqlConnection(CONNECTION_STRING))
            //            {
            //                con.Open();

            //                var index = (int)state + 1;
            //                var padding = new String(' ', (index - 1) * 3);

            //                Console.WriteLine(string.Format("{0}第{1}次：开始读取", padding, index));
            //                var cmd = new SqlCommand("select top 1 * from Customers(updlock)", con);
            //                cmd.ExecuteReader().Close();

            //                Console.WriteLine(string.Format("{0}第{1}次：开始休眠", padding, index));
            //                System.Threading.Thread.Sleep(1000 * ((int)state + 1));

            //                Console.WriteLine(string.Format("{0}第{1}次：开始修改", padding, index));
            //                cmd = new SqlCommand("update Customers set Name = N'段光宇" + state + "' where Name=N'test01'", con);
            //                cmd.ExecuteNonQuery();
            //                Console.WriteLine(string.Format("{0}第{1}次：完成修改", padding, index));
            //            }

            //            ts.Complete();
            //        }
            //    }, i).ContinueWith((t) =>
            //    {
            //        Console.WriteLine(t.Exception.InnerException.Message);
            //    }, TaskContinuationOptions.OnlyOnFaulted);
            //}
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //for (int i = 0; i < 5; i++)
            //{
            //    Task.Factory.StartNew((stateCode) =>
            //    {
            //        using (var context = new MyDbContext())
            //        {
            //            DataUtil.UseDefaultTransactionByAction(System.Data.IsolationLevel.RepeatableRead, () =>
            //            {
            //                var index = (int)stateCode + 1;
            //                var padding = new String(' ', (index - 1) * 3);
            //                Console.WriteLine(string.Format("{0}第{1}次：开始读取", padding, index));
            //                var result = context.customer.OrderByDescending(p => p.Id).FirstOrDefault();
            //                Console.WriteLine($"{padding}第{index}次读取的结果Id:{result?.Id},Name:{result?.Name},Password:{result?.Password}");
            //                Console.WriteLine(string.Format("{0}第{1}次：开始休眠", padding, index));
            //                //System.Threading.Thread.Sleep(1000 * ((int)stateCode + 1));
            //                Console.WriteLine(string.Format("{0}第{1}次：开始修改", padding, index));
            //                if (result == null)
            //                {
            //                    throw new Exception($"不存在{result.Name}数据");
            //                }
            //                result.Name = "test修改_" + stateCode;
            //                Console.WriteLine(string.Format("{0}第{1}次：完成修改", padding, index));
            //                Console.WriteLine($"最后一次读取的结果Id:{result?.Id},Name:{result?.Name},Password:{result?.Password}");
            //                context.SaveChanges();
            //            });
            //        }
            //    }, i).ContinueWith((t) =>
            //    {
            //        Console.WriteLine(t.Exception.InnerException.Message+$"_{i}");
            //    }, TaskContinuationOptions.OnlyOnFaulted);

            //}
            sw.Stop();
            Console.WriteLine("Stopwatch 时间精度：{0}ms", sw.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}

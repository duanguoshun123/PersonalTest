using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinqHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var studentInCourses = new List<CourseStudent>(){
                    new CourseStudent{CourseName="Art",StID=1},
                    new CourseStudent{CourseName="Art",StID=3},
                    new CourseStudent{CourseName="History",StID=1},
                    new CourseStudent{CourseName="History",StID=2},
                    new CourseStudent{CourseName="Physics",StID=3}
                     };
            var students = new List<Student>()
            {
                   new Student{StID=1,LastName="张三"},
                   new Student{StID=2,LastName="李四"},
                   new Student{StID=3,LastName="王五"}
            };
            //获取选修2门课以上同学的名字
            var query = (from s in students
                         join c in studentInCourses on s.StID equals c.StID
                         //where (from x in studentInCourses where x.StID == c.StID select x.StID).Count() > 1
                         where (from x in studentInCourses group x by x.StID into g select new { count = g.Count(), g.FirstOrDefault().StID }).Where(x => x.count > 1).Select(p => p.StID).ToList().Contains(c.StID)
                         select s.LastName).Distinct();
            foreach (var v in query) Console.Write("{0}\t", v);
            Console.WriteLine("");
            //获取选修历史同学的名字
            var query1 = from s in students
                         join c in studentInCourses on s.StID equals c.StID
                         where c.CourseName == "History"
                         select s.LastName;
            foreach (var v in query1) Console.Write("{0}\t", v);


            //var someInts = from a in Enumerable.Range(1, 5) //必须的第一个from子句
            //               from b in Enumerable.Range(6, 5) //查询主体的第一个子句
            //               where a < 3 && b < 10
            //               select new { a, b, sum = a + b }; //匿名类型对象
            //foreach (var v in someInts) Console.WriteLine(v);

            var someInts = from a in Enumerable.Range(1, 5)
                           from b in Enumerable.Range(6, 5)
                           let sum = a + b //在新的变量中保存结果
                           where sum == 12
                           select new { a, b, sum };
            foreach (var v in someInts) Console.WriteLine(v);

            var persons = new[] { //匿名类型的对象数组
            new {Name="张三",Sex="男",Age=32,Address="广东深圳"},
            new {Name="李四",Sex="男",Age=26,Address="广东广州"},
            new {Name="王五",Sex="女",Age=22,Address="广东深圳"},
            new {Name="赵六",Sex="男",Age=33,Address="广东东莞"}
             };
            var query02 = from p in persons
                          group p by p.Address;
            foreach (var v in query02) //枚举分组
            {
                Console.WriteLine("{0}", v.Key); //分组键
                foreach (var t in v) //枚举分组中的项
                {
                    Console.WriteLine("Name:{0},Sex:{1},Age:{2},Address:{3}", t.Name, t.Sex, t.Age, t.Address);
                }
            }

            IList<int> list = Enumerable.Range(1, 100).ToList();
            Func<int, bool> myDel = delegate (int x) { return x % 2 == 1; };//委托匿名方法
            var countOdd1 = list.Count(myDel);//调用委托
            var countOdd2 = list.Count(x => x % 2 == 1);//Lambda表达式
            Console.WriteLine("委托参数得到奇数的个数:{0}", countOdd1);
            Console.WriteLine("Lambda得到奇数的个数:{0}", countOdd2);

            query02.SideForEach((x, i) =>
            {
                foreach (var item in x)
                {
                    Console.WriteLine($"{i}:{item.Address},{item.Age},{item.Name},{item.Sex}");
                }
            });

            foreach (var v in query02) //枚举分组
            {
                Console.WriteLine("{0}", v.Key); //分组键
                foreach (var t in v) //枚举分组中的项
                {
                    Console.WriteLine("Name:{0},Sex:{1},Age:{2},Address:{3}", t.Name, t.Sex, t.Age, t.Address);
                }
            }
            var likeListTests = new List<string>() { "A123Sll", "AB123", "AC_123", "C123", "C1234A" };
            var result = likeListTests.Where(p => p.StartsWith("A"));

            ParameterExpression paraLeft = Expression.Parameter(typeof(int), "a");
            ParameterExpression paraRight = Expression.Parameter(typeof(int), "b");

            BinaryExpression binaryLeft = Expression.Multiply(paraLeft, paraRight);
            ConstantExpression conRight = Expression.Constant(2, typeof(int));

            BinaryExpression binaryBody = Expression.Add(binaryLeft, conRight);

            //LambdaExpression lambda =
            //    Expression.Lambda<Func<int, int, int>>(binaryBody, paraLeft, paraRight);
            Expression<Func<int, int, int>> lambda =
                Expression.Lambda<Func<int, int, int>>(binaryBody, paraLeft, paraRight);

            Console.WriteLine(lambda.ToString());

            Func<int, int, int> myLambda = lambda.Compile();

            int result2 = myLambda(2, 3);
            Console.WriteLine("result:" + result2.ToString());


            BinaryExpression body = Expression.Add(
            Expression.Constant(2),
            Expression.Constant(3));

            Expression<Func<int>> expression =
                Expression.Lambda<Func<int>>(body, null);

            Func<int> lambda1 = expression.Compile();

            Console.WriteLine(lambda1());

            // 将lamada 修改
            Expression<Func<int, int, int>> lambda3 = (a, b) => a + (b + 2);

            var operationsVisitor = new OperationsVisitor();
            Expression modifyExpression = operationsVisitor.Modify(lambda3);

            Console.WriteLine(modifyExpression.ToString());

            Console.Read();

        }

    }
    public class Student
    {
        public int StID;
        public string LastName;
    }
    public class CourseStudent
    {
        public string CourseName;
        public int StID;
    }
    public static class Common
    {
        public static void SideForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            int index = 0;
            foreach (var item in enumerable)
            {
                action(item, index);
                index++;
            }
        }
    }
    public class OperationsVisitor : ExpressionVisitor
    {
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b.NodeType == ExpressionType.Add)
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);
                return Expression.Subtract(left, right);
            }

            return base.VisitBinary(b);
        }
    }
}

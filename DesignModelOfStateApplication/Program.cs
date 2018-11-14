using DesignModelClass.StateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignModelClass.BuildModel;

namespace DesignModelOfStateApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("来了一位顾客，需要买两种类型的电脑");
            Computer computer1 = new Computer("Lenova", "A-578920", "001", "001");
            Computer computer2 = new Computer("Apple", "L-38525", "002", "002");
            Console.WriteLine("销售经理接单，要求两名员工去安装电脑");
            Director director = new Director();
            director.Construct(new ConcreteBuilder("小王", "ID-001", computer1));
            computer1.Show();
            director.Construct(new ConcreteBuilder("小李", "ID-002", computer2));
            computer2.Show();
            Console.WriteLine("交货，销售经理完成了一单");
            Console.WriteLine("又来了一个顾客，要求跟上一个顾客一样");
            Console.WriteLine("销售经理接单，本想让前面两个员工去做，但是第一个员工请假了，于是换了一个员工");
            director.Construct(new ConcreteBuilder("小白", "ID-001", computer1));
            computer1.Show();
            director.Construct(new ConcreteBuilder("小李", "ID-002", computer2));
            computer2.Show();
            Console.WriteLine("交货，销售经理完成了一单");
        }
    }
}

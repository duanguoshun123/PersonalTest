using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignModelClass.BuildModel
{
    public class ConcreteBuilder : Builder
    {
        /// <summary>
        /// 建造者名称（这里是装电脑人）
        /// </summary>
        public string ConcreteBuilderName { get; set; }
        /// <summary>
        /// 建造者编号（这里是电脑店员工编号）
        /// </summary>
        public string ConcreteBuilderId { get; set; }

        private Computer Computer;

        public ConcreteBuilder(string concreteBuilderName, string concreteBuilderId, Computer computer)
        {
            this.ConcreteBuilderId = concreteBuilderId;
            this.ConcreteBuilderName = concreteBuilderName;
            this.Computer = computer;
            StartWork();
        }

        public void StartWork()
        {
            Console.WriteLine($"员工{this.ConcreteBuilderId}-{this.ConcreteBuilderName}开始组装电脑{this.Computer?.ComputerName}");
        }
        public override void BuildPartCPU()
        {
            if (Computer != null)
            {
                 Computer.Add($"CPU-{this.Computer?.ComputerCpuType}");
            }
        }

        public override void BuildPartMainBoard()
        {
            if (Computer != null)
            {
                Computer.Add($"MainBoard-{this.Computer?.ComputerMainBoardType}");
            }
        }

        public override Computer GetComputer()
        {
            return this.Computer;
        }
    }
}

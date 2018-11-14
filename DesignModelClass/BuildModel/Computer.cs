using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignModelClass.BuildModel
{
    /// <summary>
    /// 电脑类
    /// </summary>
    public class Computer
    {
        /// <summary>
        /// 电脑型号
        /// </summary>
        public string ComputerType { get; set; }
        /// <summary>
        /// 电脑品牌名
        /// </summary>
        public string ComputerName { get; set; }

        /// <summary>
        /// 电脑CPU型号
        /// </summary>
        public string ComputerCpuType { get; set; }
        /// <summary>
        /// 电脑主板型号
        /// </summary>
        public string ComputerMainBoardType { get; set; }

        public Computer(string computerName, string computerType, string computerCpuType, string computerMainBoardType)
        {
            this.ComputerName = computerName;
            this.ComputerType = ComputerType;
            this.ComputerCpuType = computerCpuType;
            this.ComputerMainBoardType = computerMainBoardType;

        }

        // 电脑组建集合
        private IList<string> parts = new List<string>();
        /// <summary>
        /// 安装组件
        /// </summary>
        /// <param name="part"></param>
        public void Add(string part)
        {
            parts.Add(part);
        }

        public void Show()
        {
            Console.WriteLine($"{this.ComputerName}-{this.ComputerType}电脑正在组装.......");
            foreach (var part in parts)
            {
                Console.WriteLine($"组建{part}已装好");
            }
            Console.WriteLine($"{this.ComputerName}-{this.ComputerType}电脑组装完成");
        }
    }
}

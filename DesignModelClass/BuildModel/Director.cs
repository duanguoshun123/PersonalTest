using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignModelClass.BuildModel
{
    /// <summary>
    /// 建造者模式  指挥者
    /// </summary>
    public class Director
    {
        /// <summary>
        /// 发起指挥
        /// </summary>
        public void Construct(Builder builder)
        {
            builder.BuildPartCPU();
            builder.BuildPartMainBoard();
        }
    }
}

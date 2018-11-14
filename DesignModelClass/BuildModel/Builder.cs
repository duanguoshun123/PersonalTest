using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignModelClass.BuildModel
{
    /// <summary>
    /// 抽象建造者 ，这个场景下为 "组装人" ，这里也可以定义为接口
    /// </summary>
    public abstract class Builder
    {
        /// <summary>
        /// 装CPU
        /// </summary>
        public abstract void BuildPartCPU();
        /// <summary>
        /// 装主板
        /// </summary>
        public abstract void BuildPartMainBoard();

        // 当然还有装硬盘，电源等组件，这里省略

        /// <summary>
        /// 获得组装好的电脑
        /// </summary>
        /// <returns></returns>
        public abstract Computer GetComputer();
    }
}

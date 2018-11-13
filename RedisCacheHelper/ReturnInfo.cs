using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheHelper
{


    /// <summary>
    /// TransmissionReturnInfo
    /// 不要用来代替异常处理，也不要用来包装传递数据，不要用来做validation的返回类型。基本上专用于网络传输内容的类型。不要用于业务逻辑层、数据访问层等。
    /// 将来应移至 GMK.OperationSystem.ViewModel 项目
    /// 在 controller 需要将异常信息和正常返回信息都包装为 ReturnInfo ，作为网络传输的内容，
    /// 用法参考 JsonNetResult 、 BaseController.OnException 、 TemplateController.ValidateBeforeGenerate 、等
    /// </summary>
    public class ReturnInfo
    {
        /// <summary>
        ///
        /// </summary>
        //[JsonProperty(Order = 1, PropertyName = "ReturnStatus")]
        public ReturnStatus ReturnStatus { get; set; }

        /// <summary>
        /// true: 成功, false: 失败
        /// Status字段是最开始设计的，后来发现bool不能满足，新加了ReturnStatus的枚举
        /// 以后不推荐使用该字段，统一使用ResultStatus
        /// </summary>
        //[JsonProperty(Order = 2, PropertyName = "Status")]
        public bool Status { get; set; }

        //[JsonProperty(Order = 3, PropertyName = "Message")]
        public string Message { get; set; }

        //[JsonProperty(Order = 4, PropertyName = "Data")]
        public object Data { get; set; }

        //[Obsolete]
        //[JsonIgnore]
        //public virtual object ExtraData { get; set; }

        /// <summary>
        /// TransmissionReturnInfo
        /// 不要用来代替异常处理，也不要用来包装传递数据，不要用来做validation的返回类型。基本上专用于网络传输内容的类型。不要用于业务逻辑层、数据访问层等。
        /// 在 controller 需要将异常信息和正常返回信息都包装为 ReturnInfo ，作为网络传输的内容，
        /// 用法参考 JsonNetResult 、 BaseController.OnException 、 TemplateController.ValidateBeforeGenerate 、等
        /// </summary>
        public ReturnInfo()
        {
            Status = true;
            this.ReturnStatus = ReturnStatus.Success;
            Message = "";
        }

        /// <summary>
        /// TransmissionReturnInfo
        /// 不要用来代替异常处理，也不要用来包装传递数据，不要用来做validation的返回类型。基本上专用于网络传输内容的类型。不要用于业务逻辑层、数据访问层等。
        /// 在 controller 需要将异常信息和正常返回信息都包装为 ReturnInfo ，作为网络传输的内容，
        /// 用法参考 JsonNetResult 、 BaseController.OnException 、 TemplateController.ValidateBeforeGenerate 、等
        /// </summary>
        public ReturnInfo(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Status = true;
                this.ReturnStatus = ReturnStatus.Success;
                Message = string.Empty;
            }
            else
            {
                Status = false;
                ReturnStatus = ReturnStatus.Fail;
                Message = msg;
            }
        }

        /// <summary>
        /// TransmissionReturnInfo
        /// 不要用来代替异常处理，也不要用来包装传递数据，不要用来做validation的返回类型。基本上专用于网络传输内容的类型。不要用于业务逻辑层、数据访问层等。
        /// 在 controller 需要将异常信息和正常返回信息都包装为 ReturnInfo ，作为网络传输的内容，
        /// 用法参考 JsonNetResult 、 BaseController.OnException 、 TemplateController.ValidateBeforeGenerate 、等
        /// </summary>
        public ReturnInfo(string msg, ReturnStatus returnStatus)
        {
            if (string.IsNullOrEmpty(msg))
            {
                Status = true;
                this.ReturnStatus = ReturnStatus.Success;
                Message = string.Empty;
            }
            else
            {
                Status = returnStatus != ReturnStatus.Fail;
                ReturnStatus = returnStatus;
                Message = msg;
            }
        }
    }
    /// <summary>
    /// 返回状态
    /// </summary>
    public enum ReturnStatus
    {
        Success = 1,

        Warning = 2,

        Fail = 4,
    }
}

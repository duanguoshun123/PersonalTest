using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheHelper
{
    public class Pagination
    {
        public const int DefaultPageSize = 15;

        private int _CurrentPage = 1;

        public int CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value < 1 ? 1 : value;
            }
        }

        private int _PageSize = DefaultPageSize;

        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value < 1 ? DefaultPageSize : value;
            }
        }

        /// <summary>
        /// result
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// result
        /// </summary>
        public int TotalCount { get { return ItemCount; } set { ItemCount = value; } }

        /// <summary>
        /// result
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0 || ItemCount == 0)
                {
                    return 1;
                }

                return (ItemCount / PageSize) + (ItemCount % PageSize == 0 ? 0 : 1);
            }
        }

        public bool IsQuery { private get; set; }

        public bool ShouldSerializeIsQuery()
        {
            return false;
        }

        public bool ShouldSerializeItemCount()
        {
            return !IsQuery;
        }

        public bool ShouldSerializeTotalCount()
        {
            return !IsQuery;
        }

        public bool ShouldSerializePageCount()
        {
            return !IsQuery;
        }

        public int GetSkip()
        {
            int result = 0;

            if (CurrentPage > 0 && PageSize > 0)
            {
                result = (CurrentPage - 1) * PageSize;
            }

            return result;
        }
    }
}

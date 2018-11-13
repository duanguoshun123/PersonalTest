using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheHelper
{
    public static class DataUtil
    {
        #region ReadUncommitted

        /// <summary>
        /// 在query内部不能使用
        /// </summary>
        public static TResult QueryResultReadUncommitted<TSource, TResult>(IQueryable<TSource> source, bool isReadUncommitted, Func<IQueryable<TSource>, TResult> queryResult)
        {
            return UseReadUncommittedTransaction(isReadUncommitted, () => queryResult(source));
            //TResult result = default(TResult);
            //UseReadUncommittedTransaction(isReadUncommitted, () =>
            //{
            //    result = queryResult(source);
            //});
            //return result;

            //if (!isReadUncommitted)
            //{
            //    return queryResult(source);
            //}

            //using (var scope = new System.Transactions.TransactionScope(
            //    System.Transactions.TransactionScopeOption.Required,
            //    new System.Transactions.TransactionOptions()
            //    {
            //        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            //    }))
            //{
            //    TResult result = queryResult(source);
            //    scope.Complete();
            //    return result;
            //}
        }

        /// <summary>
        /// 在query内部不能使用
        /// </summary>
        public static bool AnyReadUncommitted<TSource>(this IQueryable<TSource> source, bool isReadUncommitted)
        {
            return DataUtil.QueryResultReadUncommitted(source, isReadUncommitted, Queryable.Any);
        }

        /// <summary>
        /// 在query内部不能使用
        /// </summary>
        public static decimal? SumReadUncommitted(this IQueryable<decimal?> source, bool isReadUncommitted)
        {
            return DataUtil.QueryResultReadUncommitted(source, isReadUncommitted, Queryable.Sum);
        }

        /// <summary>
        /// 在query内部不能使用
        /// </summary>
        public static List<TSource> ToListReadUncommitted<TSource>(this IQueryable<TSource> source, bool isReadUncommitted)
        {
            return DataUtil.QueryResultReadUncommitted(source, isReadUncommitted, Enumerable.ToList);
        }

        /// <summary>
        /// 在query内部不能使用
        /// </summary>
        public static int CountReadUncommitted<TSource>(this IQueryable<TSource> source, bool isReadUncommitted)
        {
            return DataUtil.QueryResultReadUncommitted(source, isReadUncommitted, Queryable.Count);
        }

        /// <summary>
        /// 在query内部不能使用
        /// </summary>
        public static TSource FirstOrDefaultReadUncommitted<TSource>(this IQueryable<TSource> source, bool isReadUncommitted)
        {
            return DataUtil.QueryResultReadUncommitted(source, isReadUncommitted, Queryable.FirstOrDefault);
        }

        public static IQueryable<TEntity> PaginateReadUncommitted<TEntity>(this IOrderedQueryable<TEntity> orderedQueryable, bool isReadUncommitted, Pagination pagination)
        {
            if (pagination != null)
            {
                pagination.TotalCount = orderedQueryable.CountReadUncommitted(isReadUncommitted);
                return orderedQueryable.Skip(pagination.GetSkip()).Take(pagination.PageSize);
            }
            else
            {
                return orderedQueryable;
            }
        }

        public static IQueryable<TEntity> PaginateReadUncommitted<TEntity>(this IQueryable<TEntity> source, bool isReadUncommitted, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> sort, Pagination pagination, bool sortEver)
        {
            if (sort != null && (pagination != null || sortEver))
            {
                return sort(source).PaginateReadUncommitted(isReadUncommitted, pagination);
            }
            else
            {
                return source;
            }
        }

        //public static int ExecuteProcedureReadUncommitted(bool isReadUncommitted, Func<int> sp)
        //{
        //    if (!isReadUncommitted)
        //    {
        //        return sp();
        //    }

        //    using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required,
        //        new System.Transactions.TransactionOptions
        //        {
        //            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted,
        //        }))
        //    {
        //        var result = sp();
        //        scope.Complete();
        //        return result;
        //    }
        //}

        //public static List<TElement> ExecuteProcedureReadUncommitted<TElement>(bool isReadUncommitted, Func<IEnumerable<TElement>> sp)
        //{
        //    if (!isReadUncommitted)
        //    {
        //        return sp().ToList();
        //    }

        //    using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required,
        //        new System.Transactions.TransactionOptions
        //        {
        //            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted,
        //        }))
        //    {
        //        var result = sp().ToList();
        //        scope.Complete();
        //        return result;
        //    }
        //}

        #endregion ReadUncommitted

        #region Use Transaction

        /// <summary>
        /// wrap Func&lt;T&gt; callback, (T 不能是 ReturnInfo)
        /// </summary>
        internal static T UseTransactionByFunc<T>(Func<System.Transactions.TransactionScope> newScope, Func<T> callback)
        {
            using (var scope = newScope())
            {
                var result = callback();

                //var returnInfo = result as ReturnInfo;
                //if (returnInfo != null && !returnInfo.Status)
                //{
                //    return result;
                //}

                //if (notCommit)
                //{
                //    return result;
                //}

                scope.Complete();
                return result;
            }
        }

        ///// <summary>
        ///// wrap Func&lt;T&gt; callback, (T 不能是 ReturnInfo)
        ///// </summary>
        //internal static T UseTransactionByFunc<T>(Func<System.Transactions.TransactionScope> newScope, Func<T> callback, bool omitException = false, T failResult = default(T))
        //{
        //    using (var scope = newScope())
        //    {
        //        if (omitException)
        //        {
        //            var result = ExceptionLogUtil.OmitException(failResult, callback, true);
        //            if (result.Item2 == null)
        //            {
        //                scope.Complete();
        //            }
        //            return result.Item1;
        //        }
        //        else
        //        {
        //            var result = callback();

        //            //var returnInfo = result as ReturnInfo;
        //            //if (returnInfo != null && !returnInfo.Status)
        //            //{
        //            //    return result;
        //            //}

        //            //if (notCommit)
        //            //{
        //            //    return result;
        //            //}

        //            scope.Complete();
        //            return result;
        //        }
        //    }
        //}

        /// <summary>
        /// wrap Func&lt;ReturnInfo&gt; callback, ( 应该使用异常，而不是 ReturnInfo ， Func&lt;ReturnInfo&gt; 要改造成 Action)
        /// </summary>
        internal static ReturnInfo UseTransactionByReturnInfoFunc(Func<System.Transactions.TransactionScope> newScope, Func<ReturnInfo> callback)
        {
            //return ExceptionLogUtil.GetReturnInfoByReturnInfoFunc(() =>
            //{
            return UseTransactionByFunc<ReturnInfo>(newScope, () =>
            {
                var result = callback();
                result.HandleReturnInfo();
                return result;
            });
            //}, false);

            ////try
            ////{
            ////    return UseTransactionByFunc<ReturnInfo>(newScope, () =>
            ////    {
            ////        var result = callback();
            ////        result.HandleReturnInfo();
            ////        return result;
            ////    });
            ////}
            ////catch (BaseCustomException ex)
            ////{
            ////    LogHelper.LogError(ex);
            ////    return ex.ReturnInfo ?? new ReturnInfo(ex.Message);
            ////}
            ////catch (Exception ex)
            ////{
            ////    LogHelper.LogError(ex);
            ////    throw;
            ////}
        }

        /// <summary>
        /// wrap Action callback
        /// </summary>
        internal static void UseTransactionByAction(Func<System.Transactions.TransactionScope> newScope, Action callback)
        {
            using (var scope = newScope())
            {
                callback();
                //if (!notCommit)
                //{
                scope.Complete();
                //}
            }
        }

        ///// <summary>
        ///// wrap Action callback
        ///// </summary>
        //internal static void UseTransactionByAction(Func<System.Transactions.TransactionScope> newScope, Action callback, bool omitException)
        //{
        //    using (var scope = newScope())
        //    {
        //        if (omitException)
        //        {
        //            if (ExceptionLogUtil.OmitException(callback, true) == null)
        //            {
        //                scope.Complete();
        //            }
        //        }
        //        else
        //        {
        //            callback();
        //            //if (!notCommit)
        //            //{
        //            scope.Complete();
        //            //}
        //        }
        //    }
        //}

        /// <summary>
        /// 不应用来做增删改操作，只用于查询
        /// </summary>
        public static T UseReadUncommittedTransaction<T>(bool isUseReadUncommitted, Func<T> callback)
        {
            if (!isUseReadUncommitted)
            {
                return callback();
            }
            return UseTransactionByFunc(DefaultNewScopeFunc(System.Transactions.IsolationLevel.ReadUncommitted, true), callback);
        }

        ///// <summary>
        ///// 不应用来做增删改操作，只用于查询
        ///// </summary>
        //public static void UseReadUncommittedTransaction(bool isUseReadUncommitted, Action callback)
        //{
        //    if (!isUseReadUncommitted)
        //    {
        //        callback();
        //        return;
        //    }
        //    using (var scope = new System.Transactions.TransactionScope(
        //        System.Transactions.TransactionScopeOption.Required,
        //        new System.Transactions.TransactionOptions()
        //        {
        //            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted,
        //        }))
        //    {
        //        callback();
        //        scope.Complete();
        //    }
        //}

        internal static Func<System.Transactions.TransactionScope> DefaultNewScopeFunc(System.Transactions.IsolationLevel isolationLevel, System.Transactions.TransactionScopeOption transactionScopeOption)
        {
            return () => new System.Transactions.TransactionScope(transactionScopeOption, new System.Transactions.TransactionOptions
            {
                IsolationLevel = isolationLevel,
            });
        }

        internal static Func<System.Transactions.TransactionScope> DefaultNewScopeFunc(System.Transactions.IsolationLevel isolationLevel, bool isAlwaysNewScope)
        {
            var scopeOption = isAlwaysNewScope ? System.Transactions.TransactionScopeOption.RequiresNew : System.Transactions.TransactionScopeOption.Required;
            var transactionOptions = new System.Transactions.TransactionOptions
            {
                IsolationLevel = isolationLevel,
            };
            return () => new System.Transactions.TransactionScope(scopeOption, transactionOptions);
        }

        internal static System.Transactions.TransactionScope SuppressTransactionScopeFunc()
        {
            return new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress);
        }

        /// <summary>
        /// 抑制环境事务
        /// </summary>
        public static T UseSuppressTransactionByFunc<T>(Func<T> callback)
        {
            return UseTransactionByFunc(SuppressTransactionScopeFunc, callback);
        }

        /// <summary>
        /// 抑制环境事务
        /// </summary>
        public static void UseSuppressTransactionByAction(Action callback)
        {
            UseTransactionByAction(SuppressTransactionScopeFunc, callback);
        }

        /// <summary>
        /// wrap Func&lt;T&gt; callback, (T 不能是 ReturnInfo)
        /// </summary>
        public static T UseDefaultTransactionByFunc<T>(this System.Data.IsolationLevel defaultIsolationLevel, Func<T> callback)
        {
            return UseTransactionByFunc(DefaultNewScopeFunc(defaultIsolationLevel.GetTransactionsIsolationLevel(), false), callback);
        }

        /// <summary>
        /// wrap Func&lt;ReturnInfo&gt; callback, ( 应该使用异常，而不是 ReturnInfo ， Func&lt;ReturnInfo&gt; 要改造成 Action)
        /// </summary>
        public static ReturnInfo UseDefaultTransactionByReturnInfoFunc(this System.Data.IsolationLevel defaultIsolationLevel, Func<ReturnInfo> callback)
        {
            return UseTransactionByReturnInfoFunc(DefaultNewScopeFunc(defaultIsolationLevel.GetTransactionsIsolationLevel(), false), callback);
        }

        /// <summary>
        /// wrap Action callback
        /// </summary>
        public static void UseDefaultTransactionByAction(this System.Data.IsolationLevel defaultIsolationLevel, Action callback)
        {
            UseTransactionByAction(DefaultNewScopeFunc(defaultIsolationLevel.GetTransactionsIsolationLevel(), false), callback);
        }

        /// <summary>
        /// wrap Func&lt;T&gt; callback, (T 不能是 ReturnInfo)
        /// </summary>
        public static T UseDefaultTransactionByFuncWithScopeOption<T>(this System.Data.IsolationLevel defaultIsolationLevel, bool isAlwaysNewScope, Func<T> callback)
        {
            return UseTransactionByFunc(DefaultNewScopeFunc(defaultIsolationLevel.GetTransactionsIsolationLevel(), isAlwaysNewScope), callback);
        }

        ///// <summary>
        ///// wrap Func&lt;ReturnInfo&gt; callback, ( 应该使用异常，而不是 ReturnInfo ， Func&lt;ReturnInfo&gt; 要改造成 Action)
        ///// </summary>
        //public static ReturnInfo UseDefaultTransactionByReturnInfoFuncWithScopeOption(this System.Data.IsolationLevel defaultIsolationLevel, bool isAlwaysNewScope, Func<ReturnInfo> callback)
        //{
        //    return UseTransactionByReturnInfoFunc(DefaultNewScopeFunc(defaultIsolationLevel.GetTransactionsIsolationLevel(), isAlwaysNewScope), callback);
        //}

        /// <summary>
        /// wrap Action callback
        /// </summary>
        public static void UseDefaultTransactionByActionWithScopeOption(this System.Data.IsolationLevel defaultIsolationLevel, bool isAlwaysNewScope, Action callback)
        {
            UseTransactionByAction(DefaultNewScopeFunc(defaultIsolationLevel.GetTransactionsIsolationLevel(), isAlwaysNewScope), callback);
        }

        //[Obsolete]
        //public static T LegacyUseDefaultTransactionByFunc<T>(Func<T> callback)
        //{
        //    return LegacyDefaultIsolationLevel.UseDefaultTransactionByFunc(callback);
        //    //return UseTransactionByFunc(() => new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions
        //    //{
        //    //    IsolationLevel = _LegacyDefaultIsolationLevel,
        //    //}), callback);
        //}

        //[Obsolete]
        //public static ReturnInfo LegacyUseDefaultTransactionByReturnInfoFunc(Func<ReturnInfo> callback)
        //{
        //    return LegacyDefaultIsolationLevel.UseDefaultTransactionByReturnInfoFunc(callback);
        //    //return ExceptionUtil.GetReturnInfoByReturnInfoFunc(() =>
        //    //{
        //    //    return LegacyUseDefaultTransactionByFunc(() =>
        //    //    {
        //    //        var result = callback();
        //    //        result.HandleReturnInfo();
        //    //        return result;
        //    //    });
        //    //});

        //    //try
        //    //{
        //    //    return LegacyUseDefaultTransactionByFunc(() =>
        //    //    {
        //    //        var result = callback();
        //    //        result.HandleReturnInfo();
        //    //        return result;
        //    //    });
        //    //}
        //    //catch (BaseCustomException ex)
        //    //{
        //    //    LogHelper.LogError(ex);
        //    //    return ex.ReturnInfo ?? new ReturnInfo(ex.Message);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    LogHelper.LogError(ex);
        //    //    throw;
        //    //}
        //}

        //[Obsolete]
        //public static void LegacyUseDefaultTransactionByAction(Action callback)
        //{
        //    LegacyDefaultIsolationLevel.UseDefaultTransactionByAction(callback);
        //    //using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions
        //    //{
        //    //    IsolationLevel = _LegacyDefaultIsolationLevel,
        //    //}))
        //    //{
        //    //    callback();
        //    //    scope.Complete();
        //    //}
        //}

        #endregion Use Transaction

        public static System.Transactions.IsolationLevel GetTransactionsIsolationLevel(this System.Data.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case System.Data.IsolationLevel.Unspecified:
                    return System.Transactions.IsolationLevel.Unspecified;

                case System.Data.IsolationLevel.Chaos:
                    return System.Transactions.IsolationLevel.Chaos;

                case System.Data.IsolationLevel.ReadUncommitted:
                    return System.Transactions.IsolationLevel.ReadUncommitted;

                case System.Data.IsolationLevel.ReadCommitted:
                    return System.Transactions.IsolationLevel.ReadCommitted;

                case System.Data.IsolationLevel.RepeatableRead:
                    return System.Transactions.IsolationLevel.RepeatableRead;

                case System.Data.IsolationLevel.Serializable:
                    return System.Transactions.IsolationLevel.Serializable;

                case System.Data.IsolationLevel.Snapshot:
                    return System.Transactions.IsolationLevel.Snapshot;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static System.Data.IsolationLevel? GetCurrentTransactionDataIsolationLevel()
        {
            return System.Transactions.Transaction.Current?.IsolationLevel.GetDataIsolationLevel();
        }

        public static System.Data.IsolationLevel GetDataIsolationLevel(this System.Transactions.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case System.Transactions.IsolationLevel.Serializable:
                    return System.Data.IsolationLevel.Serializable;

                case System.Transactions.IsolationLevel.RepeatableRead:
                    return System.Data.IsolationLevel.RepeatableRead;

                case System.Transactions.IsolationLevel.ReadCommitted:
                    return System.Data.IsolationLevel.ReadCommitted;

                case System.Transactions.IsolationLevel.ReadUncommitted:
                    return System.Data.IsolationLevel.ReadUncommitted;

                case System.Transactions.IsolationLevel.Snapshot:
                    return System.Data.IsolationLevel.Snapshot;

                case System.Transactions.IsolationLevel.Chaos:
                    return System.Data.IsolationLevel.Chaos;

                case System.Transactions.IsolationLevel.Unspecified:
                    return System.Data.IsolationLevel.Unspecified;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        ////[Obsolete]
        ////private static System.Transactions.IsolationLevel _LegacyDefaultIsolationLevel { get; } = System.Transactions.IsolationLevel.ReadCommitted;

        //[Obsolete]
        //internal static System.Data.IsolationLevel LegacyDefaultIsolationLevel { get; } = System.Data.IsolationLevel.ReadCommitted;

        /// <summary>
        /// 若 !Status，则抛出异常
        /// </summary>
        public static void HandleReturnInfo(this ReturnInfo result)
        {
            if (!result.Status)
            {
                throw new BaseCustomException(result);
            }
        }
    }
}

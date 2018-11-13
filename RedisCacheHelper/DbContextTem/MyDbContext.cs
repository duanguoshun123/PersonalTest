using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RedisCacheHelper.Model;
using System.Data;

namespace RedisCacheHelper.DbContextTem
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
            : base("name=ConnCodeFirst")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            this.Database.Connection.StateChange += this.OnStateChange;
        }
        public DbSet<Order> order { get; set; }
        public DbSet<Customer> customer { get; set; }
        private void OnStateChange(object sender, StateChangeEventArgs args)
        {
            if (args.CurrentState == ConnectionState.Open && args.OriginalState != ConnectionState.Open)
            {
                using (var command = this.Database.Connection.CreateCommand())
                {
                    var isolationLevel = this.GetCurrentTransactionScopeDataIsolationLevel();
                    string commandText = this.CommandTextSetTransactionIsolationLevel(isolationLevel);

                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }
        public System.Data.IsolationLevel GetCurrentTransactionScopeDataIsolationLevel()
        {
            return DataUtil.GetCurrentTransactionDataIsolationLevel() ?? _DefaultIsolationLevel;
        }
        public string CommandTextSetTransactionIsolationLevel(System.Data.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case System.Data.IsolationLevel.Unspecified:
                    throw new ArgumentOutOfRangeException();

                case System.Data.IsolationLevel.Chaos:
                    throw new ArgumentOutOfRangeException();

                case System.Data.IsolationLevel.ReadUncommitted:
                    return "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";

                case System.Data.IsolationLevel.ReadCommitted:
                    return "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";

                case System.Data.IsolationLevel.RepeatableRead:
                    return "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ";

                case System.Data.IsolationLevel.Serializable:
                    return "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE";

                case System.Data.IsolationLevel.Snapshot:
                    return "SET TRANSACTION ISOLATION LEVEL SNAPSHOT";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private static bool _AllowSnapshotIsolation { get; } = false;
        public static System.Data.IsolationLevel _DefaultIsolationLevel { get; } = _AllowSnapshotIsolation ? IsolationLevel.Snapshot : IsolationLevel.ReadCommitted;
    }
}

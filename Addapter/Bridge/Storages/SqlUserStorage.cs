using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;

namespace Bridge_Postgres.Storages
{
    public class SqlUserStorage: IUserStorage
    {
        public readonly string _connectionString;
        public SqlUserStorage(string connectionString = null)
        {
            _connectionString = connectionString;
        }
        public User GetUser(Guid userId)
        {
            throw new NotImplementedException();
        }
        public void SaveUser(User user) { }
        public void DeleteUser(Guid userId) { }
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}

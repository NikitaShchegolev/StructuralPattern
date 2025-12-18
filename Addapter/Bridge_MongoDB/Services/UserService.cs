using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge_MongoDB.Interface;
using Bridge_MongoDB.Model;

namespace Bridge_MongoDB.Service
{
    public static class UserService
    {
        public static List<User> ValidateAllUsersMongo(IUserStorage mongoStorage) 
        {
            List<User> allExistingUserInMongo = mongoStorage.FindUsers(x=>true);
            return allExistingUserInMongo;
        }


        public static List<User> ValidateAllUsersPostgres(IUserStorage postgresStorage)
        {
            var allExistingUsers = postgresStorage.FindUsers(u => true);

            List<User> allExistingUserInPostgres = postgresStorage.FindUsers(x => true);
            
            return allExistingUserInPostgres;
        }
    }
}

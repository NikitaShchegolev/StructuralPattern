using Bridge.Interface;
using Bridge.Model;

namespace Bridge.Services
{
    public static class UserValidator
    {
        public static void ValidateAllUsers(IUserStorage mongoStorage, IUserStorage postgresStorage)
        {
            var allExistingUsers = mongoStorage.FindUsers(u => true);
            
            Console.WriteLine($"Найдено пользователей в MongoDB: {allExistingUsers.Count}\n");
            
            foreach (var user in allExistingUsers)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine($"Проверка пользователя:");
                Console.WriteLine($"  Id: {user.Id}");
                Console.WriteLine($"  Name: {user.Name}");
                Console.WriteLine($"  LastName: {user.LastName}");
                Console.WriteLine($"  Статус: {user.Updata}");
                Console.WriteLine($"  Дата создания: {user.CreatedAt:dd.MM.yyyy HH:mm:ss}");
                
                var userInPostgres = postgresStorage.GetUser(user.Id);
                
                if (userInPostgres != null)
                {
                    bool idMatch = user.Id == userInPostgres.Id;
                    bool nameMatch = user.Name == userInPostgres.Name;
                    bool lastNameMatch = user.LastName == userInPostgres.LastName;
                    
                    Console.WriteLine($"\n  PostgreSQL синхронизация:");
                    Console.WriteLine($"  |- Id совпадает: {(idMatch ? "ДА" : "НЕТ")}");
                    Console.WriteLine($"  |- Name совпадает: {(nameMatch ? "ДА" : "НЕТ")}");
                    Console.WriteLine($"  |- LastName совпадает: {(lastNameMatch ? "ДА" : "НЕТ")}");
                    
                    if (idMatch && nameMatch && lastNameMatch)
                    {
                        Console.WriteLine($"  Полная синхронизация с PostgreSQL");
                    }
                    else
                    {
                        Console.WriteLine($"  Расхождение данных с PostgreSQL!");
                    }
                }
                else
                {
                    Console.WriteLine($"  Пользователь НЕ найден в PostgreSQL");
                }
                
                Console.WriteLine();
            }
        }
        
        public static void ValidateUser(User userMongo, User userPostgres, string userName)
        {
            bool idMatch = userMongo.Id == userPostgres.Id;
            bool nameMatch = userMongo.Name == userPostgres.Name;
            bool lastNameMatch = userMongo.LastName == userPostgres.LastName;
            
            //Console.WriteLine($"{userName}: {userMongo.Name} {userMongo.LastName}");
            //Console.WriteLine($"  Id: {(idMatch ? "[OK]" : "[ERR]")} | Name: {(nameMatch ? "[OK]" : "[ERR]")} | LastName: {(lastNameMatch ? "[OK]" : "[ERR]")}");
        }
    }
}

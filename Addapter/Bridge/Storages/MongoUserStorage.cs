using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Bridge.Interface;
using Bridge.Model;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Bridge.Storages
{
    public class MongoUserStorage : IUserStorage
    {
        private readonly IMongoDatabase _dataBase;

        public MongoUserStorage(string connectionString, string dataBase)
        {
            //Добавляем клиента MongoDb
            var client = new MongoClient(connectionString);
            //Подключаемся к базе
            _dataBase = client.GetDatabase(dataBase);
        }
        /// <summary>
        /// Найти пользователя
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Имя пользователя</returns>
        public List<User> FindUsers(Func<User, bool> predicate)
        {
            //Получаем всех пользователей  в  подпапке "Users" базы "Users" - надо исправить...
            IMongoCollection<User> collections  = _dataBase.GetCollection<User>("Users");
            Expression<Func<User,bool>> filter = user => true;
            List<User> allUsers = collections.Find(filter).ToList();
            return allUsers.Where(predicate).ToList();
        }
        /// <summary>
        /// Получить Guid пользователя по Guid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Guid пользователя</returns>
        public User GetUser(Guid userId)
        {
            IMongoCollection<User> collection = _dataBase.GetCollection<User>("Users");
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            return collection.Find(filter).FirstOrDefault();
        }
        /// <summary>
        /// Сохранить пользователя в базе
        /// </summary>
        /// <param name="user"></param>
        public void SaveUser(User user)
        {
            //получение коллекции
            IMongoCollection<User> collection = _dataBase.GetCollection<User>("Users");
            //фильтр пользователя по его Id
            //Builders<User>.Filter.Eq - фильтр в Mongo db
            FilterDefinition<User> filterById = Builders<User>.Filter.Eq(x => x.Id,user.Id);
            var existUserById = collection.Find(filterById).FirstOrDefault();

            //Первая проверка: Существует ли пользователь с таким ID то тогда обновляются данные пользователя
            switch (existUserById != null)
            {
                case true:
                    collection.ReplaceOne(filterById, user);//обновление данных пользователя
                                                            //по данным передаваемым в метод если такой id
                                                            //если id не найден то просто выход из условия
                    return;
            }
            //Сравнение пользовательских данных с передаваемыми в метод данными из User и если данные есть то filterByData появляется дубликат
            FilterDefinition<User> filterByData = Builders<User>.Filter.And
                (
                Builders<User>.Filter.Eq(x=>x.Name, user.Name),
                Builders<User>.Filter.Eq(x=>x.LastName, user.LastName),
                Builders<User>.Filter.Eq(x=>x.Updata, user.Updata)
                );

            var duplicateUser = collection.Find(filterByData).ToList();
            int count = 0;
            //Выод дубликатов
            duplicateUser.ForEach(x => { Console.WriteLine($"Дубликат - {count++}., {x}"); });
            switch (duplicateUser.FirstOrDefault() != null)
            {
                case true:
                    return;// Найден дубликат - выходим без создания
            }
            collection.InsertOne(user);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="userId">GUID Пользователя</param>
        public void DeleteUser(Guid userId)
        {
            //Подключиться к коллекции 
            IMongoCollection<User> collection = _dataBase.GetCollection<User>("User");
            //Найти через сравнение нужный id
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(x => x.Id, userId);
            //Удалить эту запись по отфильтрованному значению
            collection.DeleteOne(filter);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bridge.Model;

namespace Bridge.Interface
{
    /// <summary>
    /// Интерфейс для работы с хранилищем пользователей
    /// Реализует полный набор CRUD операций
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        /// Получить пользователя по GUID (READ)
        /// </summary>
        /// <param name="userId">GUID пользователя</param>
        /// <returns>Пользователь</returns>
        User GetUser(Guid userId);
        
        /// <summary>
        /// Сохранить или обновить пользователя в базе данных (CREATE/UPDATE)
        /// </summary>
        /// <param name="user">Пользователь для сохранения</param>
        void SaveUser(User user);
        
        /// <summary>
        /// Найти пользователей по условию (READ)
        /// </summary>
        /// <param name="predicate">Условие поиска</param>
        /// <returns>Список найденных пользователей</returns>
        List<User> FindUsers(Func<User, bool> predicate);
        
        /// <summary>
        /// Удалить пользователя из базы данных (DELETE)
        /// </summary>
        /// <param name="userId">GUID пользователя для удаления</param>
        void DeleteUser(Guid userId);
    }
}

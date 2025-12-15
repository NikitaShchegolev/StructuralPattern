using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Model
{
    public class User
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Обновление
        /// </summary>
        public string Updata { get; set; }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        public string DeleteUsers { get; set; }
    }
}

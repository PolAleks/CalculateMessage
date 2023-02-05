using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBot.Models;

namespace TBot.Services
{
    public interface IStorage
    {
        /// <summary>
        /// Получение сессии пользователя по идентификатору
        /// </summary>
        /// <param name="chatId">Уникальный идентификатор пользователя</param>
        /// <returns></returns>
        Session GetSession(long chatId);
    }
}

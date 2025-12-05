using System;
using System.Collections.Generic;
using System.Text;

using Addapter.ExemplesClass;

namespace Addapter.ExemplesInterface
{
    /// <summary>
    /// Интерфейс адаптера сервиса выпуска карт
    /// </summary>
    public interface ICardIssueServiceAdapter
    {
        /// <summary>
        /// Выпускает карту с заданными параметрами
        /// </summary>
        /// <param name="cardInfo">Информация о карте</param>
        /// <returns>Строка с информацией о выпущенной карте</returns>
        string Issue(CardInfo cardInfo);
    }
}

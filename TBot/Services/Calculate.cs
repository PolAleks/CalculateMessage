using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBot.Services
{
    /// <summary>
    /// Сервис реализующий интерфейс ICalculate
    /// </summary>
    public class Calculate : ICalculate
    {
        /// <summary>
        /// Вычисляем количество символов в сообщении
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <returns></returns>
        private string GetCountSymbol(string? message)
        {
            return $"Количество символов в строке: {message?.Length ?? 0}";
        }
        /// <summary>
        /// Вычисляем сумму чисел в сообщении
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <returns></returns>
        private string GetSumNumbers(string? message)
        {
            if (message == null) return $"Пустая строка";

            int amount = 0;

            try
            {
                string[] numbers = message.Split(' ');
                foreach (string number in numbers)
                {
                    amount += Convert.ToInt32(number);
                }
            }
            catch
            {
                return $"Некорректный ввод данных, попробуйте еще раз!";
            }
            return $"Сумма чисел = {amount}";
        }
        /// <summary>
        /// Метод для обработки входящего сообщения
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="typeOperation">делегат принимающий способ обработки сообщения</param>
        /// <returns></returns>
        private string TypeOperation(string message, Func<string, string> typeOperation) => typeOperation(message);

        public string GetCount(string message, string method)
        {
            return method switch
            {
                "GetCountSymbol" => TypeOperation(message, GetCountSymbol),
                "GetAmountNumber" => TypeOperation(message, GetSumNumbers),
                _=> "Выбран некорректный обработчик входящего сообщения"
            };
        }
    }
}

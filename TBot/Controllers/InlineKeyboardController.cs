using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using TBot.Models;

namespace TBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(IStorage memoryStorage, ITelegramBotClient telegramClient)
        {
            _memoryStorage = memoryStorage;
            _telegramClient = telegramClient;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

            if (callbackQuery?.Data == null)
                return;

            _memoryStorage.GetSession(callbackQuery.From.Id).MenuSelect = callbackQuery.Data;

            string selectMenu = callbackQuery.Data switch
            {
                "GetCountSymbol" => "Введите текст для которого необходимо посчитать количество символов",
                "GetAmountNumber" => "Для вычисления суммы, введите целые числа через пробел",
                _ => string.Empty
            };

            await _telegramClient.SendTextMessageAsync(chatId: callbackQuery.From.Id,
                                                       text: selectMenu,
                                                       cancellationToken: ct);
        }
    }
}

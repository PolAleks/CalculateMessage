using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TBot.Controllers
{
    public class TextMessageController
    {
        private ITelegramBotClient _telegramClient;
        private IStorage _memoryStorage;
        private ICalculate _calculate;

        public TextMessageController(ITelegramBotClient telegramClient,
                                     IStorage memoryStorage,
                                     ICalculate calculate)
        {
            _telegramClient = telegramClient;
            _memoryStorage = memoryStorage;
            _calculate = calculate; 
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Количество символов" , $"GetCountSymbol"),
                        InlineKeyboardButton.WithCallbackData($" Сумму целых чисел" , $"GetAmountNumber")
                    });

                    // Передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(chatId: message.Chat.Id, 
                                                               text: $"<b>  Наш бот может вычислить сумму чисел или длину строки.</b> {Environment.NewLine}" +
                                                                     $"Что будем вычислять?{Environment.NewLine}", 
                                                               cancellationToken: ct, 
                                                               parseMode: ParseMode.Html,  // Режим форматирования текста с поддержкой HTML
                                                               replyMarkup: new InlineKeyboardMarkup(buttons)); // Кнопки

                    break;
                default:
                    // Вычисляем результат в зависимости от выбранного пункта меню
                    var result = _calculate.GetCount(message.Text, _memoryStorage.GetSession(message.Chat.Id).MenuSelect);

                    // Отправляем пользователю сообщение с результатом работы нашего сервиса
                    await _telegramClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                               text: result, 
                                                               cancellationToken: ct);
                    break;
            }

        }
    }
}

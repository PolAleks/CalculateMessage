using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TBot.Controllers
{
    public class DefaultMessageController
    {
        private ITelegramBotClient _telegramClient;
        public DefaultMessageController(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");
            await _telegramClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                       text: $"Получено сообщение не поддерживаемого формата",
                                                       cancellationToken: ct);
        }
    }
}

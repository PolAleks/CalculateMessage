using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using TBot.Controllers;

namespace TBot
{
    class Bot : BackgroundService
    {
        private ITelegramBotClient _telegramClient;

        // Контроллеры различных видов сообщений
        private TextMessageController _textMessageController;
        private InlineKeyboardController _inlineKeyboardController;
        private DefaultMessageController _defaultMessageController;
        public Bot(ITelegramBotClient telegramClient, 
                   TextMessageController textMessageController, 
                   InlineKeyboardController inlineKeyboardController, 
                   DefaultMessageController defaultMessageController)
        {
            _telegramClient = telegramClient;
            _textMessageController = textMessageController;
            _inlineKeyboardController = inlineKeyboardController;
            _defaultMessageController = defaultMessageController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(updateHandler: HandleUpdateAsync,
                                           pollingErrorHandler: HandleErrorAsync,
                                           receiverOptions: new ReceiverOptions() { AllowedUpdates = { } }, // Разрешаем получение обновлений\сообщений всех типов
                                           cancellationToken: stoppingToken);
            Console.WriteLine("Бот запущен");
        }

        async Task HandleUpdateAsync (ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Обрабатываем нажатие кнопки
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }
                
            // Обрабатываем входящее сообщение
            if (update.Message is not { } message)
                return;
            else
            {
                switch (message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(message, cancellationToken); 
                        break;
                    default:
                        await _defaultMessageController.Handle(message, cancellationToken);
                        return;
                }
            }
        }

        Task HandleErrorAsync (ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _=> exception.ToString()
            };

            // Вывод на консоль информации об ошибки
            Console.WriteLine(errorMessage);

            // Задержка перед повторным подключением
            Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
            Thread.Sleep(10000);

            return Task.CompletedTask;
        }
    }
}

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TBot.Controllers;
using TBot.Services;
using TBot.Configuration;

namespace TBot
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services))  // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");

        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем в контейнер зависимостей данные конфигурации приложения
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(appSettings);

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken)); // Создается один объект на весь процесс работы приложения
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();

            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<TextMessageController>(); // Создаем новый объект при каждои обращении к нему
            services.AddTransient<InlineKeyboardController>();
            services.AddTransient<DefaultMessageController>();

            // Регистриуем хранилище сессий
            services.AddSingleton<IStorage, MemoryStorage>(); // Одно хранилище  сесий для всех пользователей
            // Регистрируем сервис обработки входящих сообщений
            services.AddSingleton<ICalculate, Calculate>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "5585931894:AAE4AKL7WXyt3XGF9QxaWmvX7--sV5LKehs"
            };
        }
    }
}
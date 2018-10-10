using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NotificationTaskScheduler.DbModels;
using NotificationTaskScheduler.Mail;
using NotificationTaskScheduler.MailGenerators;
using NotificationTaskScheduler.Models;
using NotificationTaskScheduler.NotificationTasks;
using NotificationTaskScheduler.RecipientGetterStrategies;
using NotificationTaskScheduler.Services;

namespace NotificationTaskScheduler
{
    class Program
    {
        public static IServiceProvider ContainerServiceProvider { get; private set; }
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ContainerServiceProvider = serviceCollection.BuildServiceProvider();
            ContainerServiceProvider.GetService<TaskRunner>().Run(NotificationType.SendDbLobReminder);

            //Console.Read();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            serviceCollection.AddSingleton<ILogger>(provider => new FileLogger(config["AppSettings:LoggerPath"]));
            serviceCollection.AddSingleton<IMailServer, SmtpMailServer>();

            serviceCollection.AddTransient<TaskRunner>();
            serviceCollection.AddSingleton(provider => new Sender());
            serviceCollection.AddSingleton(provider => config);
            serviceCollection.AddScoped<IMailGenerator, SendDbLobReminderMailGenerator>();
            serviceCollection.AddTransient<IRecipientGetterStrategy, SendDbLobReminderRecipientGetterStrategy>();
            serviceCollection.AddTransient<INotificationTask, SendDbLobReminderNotificationTask>();
            serviceCollection.AddSingleton<ILdapService, LdapService>();

            serviceCollection.AddDbContext<NotificationDbContext>(options =>
                options.UseSqlServer(config["ConnectionStrings:NotificationDatabase"]));

        }
    }
}

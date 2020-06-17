using KeySellerBot.Model.Callbacks;
using KeySellerBot.Model.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model
{
    public static class Bot
    {
        private static TelegramBotClient _client;

        private static List<CommandBase> _commands;
        public static IReadOnlyList<CommandBase> Commands => _commands.AsReadOnly();
        
        private static List<CallbackQureryBase> _callbackHandlers;
        public static IReadOnlyList<CallbackQureryBase> CallbackHandlers => _callbackHandlers.AsReadOnly();

        public static async Task<TelegramBotClient> GetClient()
        {
            if (_client != null)
                return _client;

            _client = new TelegramBotClient(BotSettings.ApiKey);

            _commands = new List<CommandBase>();
            _commands.Add(new StartCommand());
            _commands.Add(new MenuCommand());
            _commands.Add(new DebugCommand());
            _commands.Add(new CancelInvoceCommand());
            _commands.Add(new GetFileIdCommand());

            _callbackHandlers = new List<CallbackQureryBase>();
            _callbackHandlers.Add(new CatalogCallback());
            _callbackHandlers.Add(new ReferCallback());
            _callbackHandlers.Add(new PaymentCallback());
            _callbackHandlers.Add(new FreeCourseCallback());
            _callbackHandlers.Add(new DownloadCallback());

            var hook = string.Format(BotSettings.HookUrl, "api/update/sellerbothook");
            await _client.SetWebhookAsync(hook);

            return _client;
        }

        public static async Task AddTestCourse()
        {
            var newCourse1 = new Course()
            {
                ID = 1,
                Author = "Sam",
                Descriprion = "Long long text. May be HTML for markup",
                OriginalPrice = 15000,
                Price = 2500,
                FileId = "BQACAgIAAxkBAAICeF6e0trm7CTEq_zaWOW0m-U4Ii62AAIiBgACelr5SLg38i6Al1vwGAQ",
                Name = "[Qfewsrsr] Test Course 1",
                Buyers = new List<long>() { 399858500 },
                InvoiceString = "213908sedi120"
            };

            var newCourse2 = new Course()
            {
                ID = 2,
                Author = "dfgdfg",
                Descriprion = "gfddEfdsfds",
                OriginalPrice = 25000,
                Price = 6000,
                FileId = "dsadsadw432qd234s23434543",
                Name = "[Rdvb] Test Course 2",
                InvoiceString = "2sw43kldf0"
            };

            await Catalog.AddCourse(newCourse1);
            await Catalog.AddCourse(newCourse2);
        }

        public static long GetAdminId()
        {
            return 399858500;
        }

        public static bool TryInvoke(string command, Message message, TelegramBotClient client)
        {
            foreach (var item in Commands)
            {
                if (item.Contains(command))
                {
                    item.Invoke(message, client);
                    return true;
                }
            }
            return false;
        }
    }
}

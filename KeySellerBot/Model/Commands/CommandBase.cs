using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model.Commands
{
    public abstract class CommandBase
    {
        public abstract string Name { get; }

        public abstract Task Invoke(Message message, TelegramBotClient client);

        /// <summary>
        /// Ivoke command from callback
        /// </summary>
        public abstract Task CallbackInvoke(CallbackQuery callback, TelegramBotClient client);


        public bool Contains(string command)
        {
            return command.Contains(this.Name); //&& command.Contains(BotsSettings.BOT_NAME);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model.Callbacks
{
    public abstract class CallbackQureryBase
    {
        public abstract string Name { get; }

        public abstract Task Invoke(CallbackQuery callbackQuery, TelegramBotClient client);

        public bool Contains(string callback)
        {
            return callback.StartsWith(this.Name);
        }
    }
}

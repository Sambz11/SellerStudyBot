using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model.Commands
{
    public class DebugCommand : CommandBase
    {
        public override string Name => "debug";


        public override async Task Invoke(Message message, TelegramBotClient client)
        {
            string debugOutput = "";
            foreach (var item in message.EntityValues)
            {
                debugOutput = debugOutput + "\n" + item;
            }
            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Entity Values = {message.EntityValues.ToArray()}");

            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Entity Values = {debugOutput}");

        }

        public override async Task CallbackInvoke(CallbackQuery callback, TelegramBotClient client)
        {
            var message = callback.Message;
            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "debug info: ");
        }
    }
}

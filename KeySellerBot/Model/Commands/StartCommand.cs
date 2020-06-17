using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model.Commands
{
    public class StartCommand : CommandBase
    {
        public override string Name => "start";

        public override async Task Invoke(Message message, TelegramBotClient client)
        {
            if (!UserOperation.Exist((long)message.From.Id))
            {
                var options = message.Text.Split("=");

                if (options.Length >= 2)
                {
                    if (long.TryParse(options[1], out long inviter))
                        await UserOperation.AddUser(message.From.Id, inviter);
                }
                else
                    await UserOperation.AddUser(message.From.Id);
            }

            Bot.TryInvoke("menu", message, client);

        }

        public override async Task CallbackInvoke(CallbackQuery callback, TelegramBotClient client)
        {
            await client.EditMessageTextAsync(
                chatId: callback.Message.Chat.Id,
                messageId: callback.Message.MessageId,
                text: "help");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KeySellerBot.Model.Callbacks
{
    public class ReferCallback : CallbackQureryBase
    {
        public override string Name => "referalProgram";

        public override async Task Invoke(CallbackQuery callbackQuery, TelegramBotClient client)
        {
            var message = callbackQuery.Message;
            var data = callbackQuery.Data.Split("@");


            if (data[1] == "referalProgram")
            {
                var backButton = new InlineKeyboardButton()
                {
                    Text = "<<< Назад",
                    CallbackData = "referalProgram@back"
                };

                var payOutButton = new InlineKeyboardButton()
                {
                    Text = "\uD83D\uDCB5 Вывод",
                    CallbackData = "referalProgram@payOut"
                };

                var keyboard = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>()
                {
                    new List<InlineKeyboardButton>() { payOutButton },
                    new List<InlineKeyboardButton>() { backButton }
                });

                decimal currentBalans = 0;
                int referalCount = 0;

                string sendingText = Resourses.TextResourses.ReferMessage;

                string refLink = BotSettings.BotLink + "?start=" + callbackQuery.From.Id;

                await client.EditMessageTextAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId,
                    text: String.Format(sendingText, refLink, referalCount, currentBalans),
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyMarkup: keyboard);
            }

            else if (data[1] == "back")
            {
                foreach (var item in Bot.Commands)
                {
                    if (item.Contains("menu"))
                    {
                        await item.CallbackInvoke(callbackQuery, client);
                        break;
                    };
                }
            }

            else if (data[1] == "payOut")
            {
                await client.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: "");
            }

            else
            {
                await client.EditMessageTextAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId,
                    text: "How do you done it?");
            }

            await client.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id);
        }
    }
}

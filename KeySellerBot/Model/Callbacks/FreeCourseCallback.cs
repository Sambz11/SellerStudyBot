using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KeySellerBot.Model.Callbacks
{
    public class FreeCourseCallback : CallbackQureryBase
    {
        public override string Name => "freeCourse";

        public override async Task Invoke(CallbackQuery callbackQuery, TelegramBotClient client)
        {
            var message = callbackQuery.Message;
            var data = callbackQuery.Data.Split("@");

            if (data[1] == "freeCourse")
            {
                var backButton = new InlineKeyboardButton()
                {
                    Text = "<<< Назад",
                    CallbackData = "referalProgram@back"
                };
                var keyboard = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>()
                {
                    new List<InlineKeyboardButton>() { backButton }
                });

                string sendingText = "<b>[Бесплатный курс</b> - Beta<b>]</b>\n\nЧтобы получить какой-либо курс бесплатно, то можете оставлять комментарии с вашей партнерской ссылкой в группах вконтакте в группах соответствующего курса или распространять её в социальных сетях.\nСредства, полученные за рефералов вы можете как вывести себе на счет, так и потратить на какой-либо курс.";

                await client.EditMessageTextAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId,
                    text: sendingText,
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

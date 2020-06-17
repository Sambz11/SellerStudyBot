using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KeySellerBot.Model.Callbacks
{
    public class CatalogCallback : CallbackQureryBase
    {
        public override string Name => "catalog";

        public override async Task Invoke(CallbackQuery callback, TelegramBotClient client)
        {
            var message = callback.Message;
            var data = callback.Data.Split("@");


            if (data[1] == "coursesList")
            {
                int page = 1;
                int pageDown = 0;
                int pageUp = 2;

                if (data.Length > 2)
                {
                    if (!int.TryParse(data[2], out page))
                    {
                        page = 1;
                        pageDown = 0;
                        pageUp = 2;
                    }

                    if (page <= 0)
                    {
                        page = Catalog.Count;
                        pageDown = page - 1;
                        pageUp = 1;
                    }
                    else if (page > Catalog.Count)
                    {
                        page = 1;
                        pageDown = Catalog.Count - 1;
                        pageUp = 2;
                    }
                    else
                    {
                        pageDown = page - 1;
                        pageUp = page + 1;
                    }

                }

                var course = await Catalog.GetCourse(page);

                bool courseWasBought = false;

                if (course.Buyers != null)
                    courseWasBought = course.Buyers.Contains((long)callback.From.Id);

                var messageText = $"<b>{course.Name}</b>\n\n{course.Descriprion}\n\nОбычная цена: {course.OriginalPrice} РУБ\nНаша цена: {course.Price} РУБ";

                var backButton = new InlineKeyboardButton()
                {
                    Text = "<<< Назад",
                    CallbackData = "catalog@back"
                };

                var nextButton = new InlineKeyboardButton()
                {
                    Text = "\u27A1",
                    CallbackData = "catalog@coursesList@" + pageUp.ToString()
                };

                var prevButton = new InlineKeyboardButton()
                {
                    Text = "\u2B05",
                    CallbackData = "catalog@coursesList@" + pageDown.ToString()
                };

                var payButton = new InlineKeyboardButton()
                {
                    Text = "\uD83D\uDED2 Купить курс",
                    CallbackData = "payment@create@" + course.ID
                };

                var downloadButton = new InlineKeyboardButton()
                {
                    Text = "\uD83D\uDCBE Скачать",
                    CallbackData = "download@" + course.ID
                };

                var pagerButton = new InlineKeyboardButton()
                {
                    Text = course.ID.ToString() + "/30",
                    CallbackData = "catalog@" + "empty"
                };

                var keyboard = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>()
                {
                    new List<InlineKeyboardButton>() { prevButton, pagerButton, nextButton },
                    new List<InlineKeyboardButton>() { courseWasBought ? downloadButton : payButton },
                    new List<InlineKeyboardButton>() { backButton }
                });

                await client.AnswerCallbackQueryAsync(
                    callbackQueryId: callback.Id);

                await client.EditMessageTextAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId,
                    text: messageText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyMarkup: keyboard);
            }

            else if (data[1] == "back")
            {
                foreach (var item in Bot.Commands)
                {
                    if (item.Contains("menu"))
                    {
                        await item.CallbackInvoke(callback, client);
                        break;
                    };
                }
            }

            else if (data[1] == "empty")
            {
            }

            else
            {
                await client.EditMessageTextAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId,
                    text: "How do you done it?");
            }

            await client.AnswerCallbackQueryAsync(
                callbackQueryId: callback.Id);
        }

    
    }
}

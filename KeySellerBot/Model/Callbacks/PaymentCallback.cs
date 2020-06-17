using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KeySellerBot.Model.Callbacks
{
    public class PaymentCallback : CallbackQureryBase
    {
        public override string Name => "payment";

        public override async Task Invoke(CallbackQuery callbackQuery, TelegramBotClient client)
        {
            var message = callbackQuery.Message;
            var data = callbackQuery.Data.Split("@");

            string invoceLink = "";

            var backButton = new InlineKeyboardButton()
            {
                Text = "<<< Назад",
                CallbackData = "catalog@coursesList@" + data[2]
            };

            if (data[1] == "check")
            {
                if (int.TryParse(data[2], out int courseNum))
                {
                    Course course = await Catalog.GetCourse(courseNum);
                    var invoiceId = callbackQuery.From.Id.ToString() + "-" + course.InvoiceString;
                    
                    if (await Payment.CheckInvoiceAsync(invoiceId))
                    {
                        await Catalog.AddBayer(course, callbackQuery.From.Id);
                        await client.AnswerCallbackQueryAsync(callbackQuery.Id, "Успешно!");

                        var downloadButton = new InlineKeyboardButton()
                        {
                            Text = "\uD83D\uDCBE Скачать",
                            CallbackData = "download@" + course.ID
                        };

                        var keyboard2 = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>()
                        {
                            new List<InlineKeyboardButton>() { downloadButton },
                            new List<InlineKeyboardButton>() { backButton }
                        });

                        await client.EditMessageTextAsync(
                            chatId: message.Chat.Id,
                            messageId: message.MessageId,
                            text: "Платеж прошел успешно! Теперь можете скачать курс.",
                            replyMarkup: keyboard2);

                    }
                    else
                    {
                        await client.AnswerCallbackQueryAsync(callbackQuery.Id, "Платеж еще не завершен.");
                    }
                }
            }
            else if (data[1] == "create")
            {
                if (int.TryParse(data[2], out int courseId))
                {
                    Course course = await Catalog.GetCourse(courseId);

                    var response = await Payment.CreateInvoiceAsync(
                        amount: course.Price,
                        invoiceID: callbackQuery.From.Id.ToString() + "-" + course.InvoiceString,
                        comment: course.Name);

                    invoceLink = response.PayUrl;
                }

                var openPaymentPageButton = new InlineKeyboardButton()
                {
                    Text = "Перейти к оплате",
                    Url = invoceLink
                };

                var checkPaymentButton = new InlineKeyboardButton()
                {
                    Text = "Проверить платеж",
                    CallbackData = "payment@check@" + data[2]
                };

                var keyboard = new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>()
                {
                    new List<InlineKeyboardButton>() { openPaymentPageButton },
                    new List<InlineKeyboardButton>() { checkPaymentButton },
                    new List<InlineKeyboardButton>() { backButton }
                });

                string sendingText = "<b>[Еще один шаг]</b>\n\nНажмите кнопку [Перейти к оплате] и вы перейдет на страницу оплаты. После завершения опрерации нажмите [Проверить платеж], и если оплата прошла успешно, вы сможете скачать все материалы данного курса.";

                await client.EditMessageTextAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId,
                    text: sendingText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                    replyMarkup: keyboard);
            }
        }   
    }
}

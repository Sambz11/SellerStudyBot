using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace KeySellerBot.Model.Commands
{
    public class MenuCommand : CommandBase
    {
        public override string Name => "menu";

        #region Buttons
        //Кнопка "Список курсов"
        private static InlineKeyboardButton coursesButton = new InlineKeyboardButton()
        {
            Text = "\uD83D\uDCC1 Каталог курсов",
            CallbackData = "catalog@coursesList"
        };

        //Кнопка "Курс бесплатно"
        private static InlineKeyboardButton freeCourseButton = new InlineKeyboardButton()
        {
            Text = "\uD83D\uDCB8 Курс бесплатно",
            CallbackData = "freeCourse@freeCourse"
        };

        //Кнопка "Партнерская программа"
        private static InlineKeyboardButton referalsButton = new InlineKeyboardButton()
        {
            Text = "\uD83D\uDCE3 Партнерская программа",
            CallbackData = "referalProgram@referalProgram"
        };

        InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(
                new List<List<InlineKeyboardButton>>()
                {
                    new List<InlineKeyboardButton>() { coursesButton },
                    new List<InlineKeyboardButton>() { freeCourseButton },
                    new List<InlineKeyboardButton>() { referalsButton }
                });
        #endregion

        private static string messageText = Resourses.TextResourses.HelloMessage;

        public override async Task Invoke(Message message, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: String.Format(messageText, message.From.FirstName),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                replyMarkup: keyboard);
        }

        public override async Task CallbackInvoke(CallbackQuery callback, TelegramBotClient client)
        {
            await client.EditMessageTextAsync(
                chatId: callback.Message.Chat.Id,
                messageId: callback.Message.MessageId,
                text: String.Format(messageText, callback.From.FirstName),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                replyMarkup: keyboard);
        }
    }
}

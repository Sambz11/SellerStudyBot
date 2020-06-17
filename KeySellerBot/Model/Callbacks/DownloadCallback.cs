using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model.Callbacks
{
    public class DownloadCallback : CallbackQureryBase
    {
        public override string Name => "download";

        public override async Task Invoke(CallbackQuery callbackQuery, TelegramBotClient client)
        {
            var data = callbackQuery.Data.Split("@")[1];

            Int32.TryParse(data, out int courseId);

            var dowloadedCourse = await Catalog.GetCourse(courseId);

            var d = new Telegram.Bot.Types.InputFiles.InputOnlineFile(dowloadedCourse.FileId);
            await client.SendDocumentAsync(
                chatId: callbackQuery.Message.Chat.Id,
                caption: dowloadedCourse.Name,
                document: d);

            await client.AnswerCallbackQueryAsync(callbackQuery.Id);
            
            await client.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Благодарим за покупку! Успехов в освоении материала.");
        }
    }
}

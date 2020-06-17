using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model.Commands
{
    public class GetFileIdCommand : CommandBase
    {
        public override string Name => "getfileid";

        public override async Task CallbackInvoke(CallbackQuery callback, TelegramBotClient client)
        {
            await client.AnswerCallbackQueryAsync(callback.Id);
        }

        public override async Task Invoke(Message message, TelegramBotClient client)
        {
            string fileId = message.Document.FileId;
            string fileName = message.Document.FileName;
            string fileSize = message.Document.FileSize.ToString();

            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"File ID: {fileId}\nFile Name: {fileName}\n FileSize: {fileSize}");
        }
    }
}

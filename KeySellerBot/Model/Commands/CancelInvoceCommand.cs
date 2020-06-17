using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Model.Commands
{
    /// <summary>
    /// Delete some invoice
    /// Admin command for testig 
    /// </summary>
    public class CancelInvoceCommand : CommandBase
    {
        public override string Name => "cancel";

        public override async Task CallbackInvoke(CallbackQuery callback, TelegramBotClient client)
        {
            await client.AnswerCallbackQueryAsync(callback.Id);
        }

        public override async Task Invoke(Message message, TelegramBotClient client)
        {
            if (message.From.Id != Bot.GetAdminId())
            {
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Неизвестная команда: /cancel");
                return;
            }

            var param = message.Text.Split(" ");

            if (param.Length >= 2)
            {
                var invoceId = param[1];
                var cancelationResponse = await Payment.CancelInvoiceAsync(invoceId);

                string sendingString = "Сумма: {0}\nСтатус: {1}\nДата Выставления: {2}\nКомментарий: {3}";
                string messageText = String.Format(
                    sendingString,
                    cancelationResponse.Amount.Value,
                    cancelationResponse.Status.Value,
                    cancelationResponse.CreationDateTime,
                    cancelationResponse.Comment);

                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: messageText);
            }
            else
            {
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Отменяем через пробел");
            }

        }
    }
}

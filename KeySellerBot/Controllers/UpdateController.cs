using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KeySellerBot.Model;
using KeySellerBot.Model.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace KeySellerBot.Controllers
{
    [ApiController]
    [Route("api/update/[action]")]
    public class UpdateController : ControllerBase
    {
        public async Task<StatusCodeResult> SellerBotHook()
        {
            //debag chat id - 399858500
            var someString = await new StreamReader(Request.Body).ReadToEndAsync();
            Update update = JsonConvert.DeserializeObject<Update>(someString);

            var client = await Bot.GetClient();
            var commands = Bot.Commands;

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                try
                {
                    if (update.Message.Entities?[0].Type == Telegram.Bot.Types.Enums.MessageEntityType.BotCommand)
                    {
                        bool commandFound = false;
                        foreach (var command in commands)
                        {
                            if (command.Contains(update.Message.Text))
                            {
                                await command.Invoke(update.Message, client);
                                commandFound = true;
                                break;
                            }
                        }

                        if (!commandFound)
                        {
                            await client.SendTextMessageAsync(
                                    chatId: update.Message.Chat.Id,
                                    text: $"Неизвестная комада: {update.Message.Text}");
                        }

                        return StatusCode(200);
                    }
                    else
                    {
                        if (update.Message.Chat.Id == Bot.GetAdminId())
                        {
                            var textData = someString + "\n\nMessage Type: " + update.Message.Type.ToString();

                            await client.SendTextMessageAsync(
                                update.Message.Chat.Id,
                                text: textData);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (update.Message.Chat.Id == Bot.GetAdminId())
                    {
                        var errorMessage = e.ToString() + "\n\n" + someString + "\n\nUpdate_Type:" + update.Type.ToString();
                        await client.SendTextMessageAsync(new ChatId(399858500), errorMessage);
                    }
                    return StatusCode(200);
                }
            }

            else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var message = update.CallbackQuery.Message;

                foreach (var callback in Bot.CallbackHandlers)
                {
                    if (callback.Contains(update.CallbackQuery.Data))
                    {
                        await callback.Invoke(update.CallbackQuery, client);
                        break;
                    }
                }
            }

            else
            {
                var textData = someString + "\n\nUpdate Type: " + update.Type.ToString();

                await client.SendTextMessageAsync(
                    update.Message.Chat.Id,
                    text: textData);
            }

            return StatusCode(200);
        }

    }
}
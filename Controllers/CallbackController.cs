using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using Telegram.Bot;
using VkListener.Models;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using Newtonsoft.Json;
using VkNet.Model.Attachments;

namespace VkListener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IVkApi vk;
        private readonly TelegramSettings tSettings;
        public CallbackController(IConfiguration _configuration, IVkApi _vk, TelegramSettings _tSettings)
        {
            configuration = _configuration;
            vk = _vk;
            tSettings = _tSettings;
        }

        public IActionResult Callback([FromBody] Updates updates)
        {
            switch (updates.Type)
            {
                case "confirmation":
                    return Ok(configuration["Vk:respStr"]);
             
                case "wall_post_new":
                    {
                        var message = Message.FromJson(new VkResponse(updates.Object));
                        //var fromUser =  vk.Users.Get(new long[] { (long)message.FromId });                      

                        //vk.Messages.Send(new MessagesSendParams
                        //{
                        //    RandomId = new DateTime().Millisecond,
                        //    PeerId = 83079736,
                        //    Message = message.Text
                        //});

                        TelegramBotClient telega = new TelegramBotClient(tSettings.Key);
                        telega.SendTextMessageAsync(tSettings.ChatId, "ihbp: \n" + message.PeerId+ " разместил(a) новую запись: \n" + message.Text + " \n");

                        //telega.SendTextMessageAsync(tSettings.ChatId,"ihbp: \n"+ fromUser[0].FirstName+" "+ fromUser[0].LastName+" разместил(a) новую запись: \n"+ message.Text+" \n");
                        //return Ok("ihbp: \n" + fromUser[0].FirstName + " " + fromUser[0].LastName + " разместил(a) новую запись: \n" + message.Text + " \n");

                        break;                      
                    }
            }
            return Ok("ok");
        }
    }
}

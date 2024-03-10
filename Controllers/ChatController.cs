using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebApiForGptBlazor.Models;
using AutoMapper;
using WebApiForGptBlazor.Mapper;

namespace WebApiForGptBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatGptCloneTstDbContext chatGptCloneTestDbContext;
        private readonly OpenAiApi openAiApi;
        private readonly IMapper mapper;
        
        public ChatController(ChatGptCloneTstDbContext chatGptCloneTestDbContext, OpenAiApi openAiApi,IMapper mapper)
        {
            this.openAiApi = openAiApi;
            this.chatGptCloneTestDbContext = chatGptCloneTestDbContext;
            this.mapper = mapper;
        }

        private async Task<Message[]>GetRequestAnswerPairAsync(int chatId,string message)
        {
            
            var answer = await openAiApi.GetAnswerAsync(message);

            var messages = new Message[2]{

                new Message()
                {
                    Isgptauthor = false,
                    Body = message,
                    Fkchatid = chatId
                },
                new Message()
                {
                    Isgptauthor = true,
                    Body = answer,
                    Fkchatid = chatId

                }
            };
            return messages;
        }
        [HttpGet("/getmessagesfromchatbychatid")]
        public async Task< IActionResult> GetMessagesFromChatByChatIdAssync(int chatId)
        {
            try
            {
                var messages = chatGptCloneTestDbContext.Messages.Where(x => x.Fkchatid == chatId);
                return Ok(messages.Select(m=>mapper.Map<MessageWithoutChat>(m)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/GetChatsByUserId")]
        public IActionResult GetChatsByUserIdAsync(int UserId)
        {
            try
            {
                var chats = chatGptCloneTestDbContext.Chats.Where(x => x.Fkuserid == UserId);

                return Ok(chats.Select(c=>mapper.Map<ChatWithoutMessages>(c)));
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpGet("/createnewchat")]
        public async Task<IActionResult> CreateNewChatAsync(int userId,string message)
        {
            try
            {
                var chat = new Chat()
                {
                    Fkuserid = userId,
                    Theme = await openAiApi.GetAnswerAsync($"буквально 2 слова о чем вопрос {message}")
                };
                await chatGptCloneTestDbContext.Chats.AddAsync(chat);
                await chatGptCloneTestDbContext.SaveChangesAsync();

                var messages =  await GetRequestAnswerPairAsync(chat.Id,message);
                chat.Messages = messages;

                await chatGptCloneTestDbContext.Messages.AddRangeAsync(messages);
                await chatGptCloneTestDbContext.SaveChangesAsync();

                return Ok(messages[1].Body);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("/addmessagestochat")]
        public async Task<IActionResult> AddMessagesToChatAsync(int chatId, string message)
        {
            
            try
            {
                var messages = await GetRequestAnswerPairAsync(chatId, message);
                await chatGptCloneTestDbContext.Messages.AddRangeAsync(messages);
                await chatGptCloneTestDbContext.SaveChangesAsync();
                return Ok(messages[1].Body);
            }
            catch
            {
                return BadRequest("");
            }
        } 
        
    }
}

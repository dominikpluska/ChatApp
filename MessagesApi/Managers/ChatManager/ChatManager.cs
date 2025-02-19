using AutoMapper;
using MessagesApi.DatabaseOperations.Commands.ChatCommands;
using MessagesApi.DatabaseOperations.Repository.ChatRepository;
using MessagesApi.Dto;
using MessagesApi.MessagesHub;
using MessagesApi.Models;
using MessagesApi.Services;
using MessagesApi.UserAccessor;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using System.Reflection;

namespace MessagesApi.Managers.ChatManager
{
    public class ChatManager : IChatManager
    {
        private readonly IChatCommands _chatCommands;
        private readonly IChatRepository _chatRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IHubContext<MessagesHub.MessagesHub> _hubContext;

        public ChatManager(IChatCommands chatCommands, IChatRepository chatRepository, IUserAccessor userAccessor,
            IMapper mapper, IAuthenticationService authenticationService, IHubContext<MessagesHub.MessagesHub> hubContext)
        {
            _authenticationService = authenticationService;
            _chatCommands = chatCommands;
            _chatRepository = chatRepository;
            _userAccessor = userAccessor;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        //Currently working on
        //TODO Insert it to the chats list
        public async Task<IResult> OpenChat(string chatterId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(chatterId);

                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null || !userProperties.IsActive)
                {
                    return Results.Problem("User does not exist or is inactive!");
                }

                if(userProperties.UserAccountId == chatterId)
                {
                    return Results.Problem("You can't have chat with yourself silly!");
                }

                var chatterProperties = await _authenticationService.GetAccountProperties(chatterId);

                if (chatterProperties == null || !chatterProperties.IsActive)
                {
                    return Results.Problem("User does not exist or is inactive!");
                }

                var checkIfChatExists = await _chatRepository.FindChat(userProperties.UserAccountId, chatterProperties.UserAccountId);

                if(checkIfChatExists != null)
                {
                    return Results.Ok(checkIfChatExists);
                }

                Chat chat = new()
                {
                    ChatParticipants = new List<string>()
                    {
                        userProperties.UserAccountId,
                        chatterProperties.UserAccountId
                    }
                };

                var chatId = await _chatCommands.CreateChat(chat);

                //Insert it to the chats list

                return Results.Ok(chatId);

            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        //public async Task<IResult> SendChatRequest(ChatRequestDto chatRequestDto)
        //{
        //    try
        //    {
        //        ArgumentNullException.ThrowIfNull(chatRequestDto);
        //        var userId = _userAccessor.UserId;
        //        var userProperties = await _authenticationService.GetAccountProperties(userId);

        //        if (userProperties == null || !userProperties.IsActive)
        //        {
        //            return Results.Problem("User does not exist or is inactive!");
        //        }

        //        foreach (var user in chatRequestDto.RequestRecipient)
        //        {
        //            var requestRecipient = await _authenticationService.GetAccountProperties(user);

        //            if (requestRecipient == null || requestRecipient.IsActive == false)
        //            {
        //                chatRequestDto.RequestRecipient.Remove(user);
        //            }
        //        }

        //        List<ChatParticipant> chatParticipants = new List<ChatParticipant>()
        //        {
        //            new ChatParticipant()
        //            {
        //                UserId = userProperties.UserAccountId,
        //                IsAccepted = true,
        //            }
        //        };


        //        foreach (var requestRecipient in chatRequestDto.RequestRecipient)
        //        {
        //            ChatParticipant chatParticipant = new()
        //            {
        //                UserId = requestRecipient,
        //                IsAccepted = false,
        //            };

        //            chatParticipants.Add(chatParticipant);
        //        }

        //        Chat chat = new Chat()
        //        {
        //            ChatParticipants = chatParticipants
        //        };

        //        await _chatCommands.CreateChat(chat);

        //        //Push Notification if successfull

        //        return Results.Ok("Chat has been created!");

        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return Results.Problem("Argument Null Exception!", ex.Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Results.Problem(ex.Message);
        //    }

        //}

        //public async Task<IResult> AcceptChatRequest(AcceptChatRequestDto acceptChatRequestDto)
        //{
        //    try
        //    {
        //        ArgumentNullException.ThrowIfNull(acceptChatRequestDto);
        //        var userId = _userAccessor.UserId;
        //        var userProperties = await _authenticationService.GetAccountProperties(userId);


        //        if (userProperties == null || !userProperties.IsActive)
        //        {
        //            return Results.Problem("User does not exist or is inactive!");
        //        }

        //        var chatId = await _chatRepository.CheckChat(ObjectId.Parse(acceptChatRequestDto.ChatId));

        //        //To be tested!
        //        if (chatId == null)
        //        {
        //            return Results.Problem("Chat does not exits!");
        //        }

        //        var chatParticipants = await _chatRepository.GetChatParticipants(chatId);

        //        var getChatParticipant = chatParticipants.Where(x => x.UserId == userId).FirstOrDefault();
        //        if (getChatParticipant == null)
        //        {
        //            return Results.Problem("The user isn't a part of the chat session!");
        //        }

        //        await _chatCommands.AcceptChatRequest(chatId, getChatParticipant.UserId);

        //        return Results.Ok("Accepted!");

        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        return Results.Problem("Argument Null Exception!", ex.Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Results.Problem(ex.Message);
        //    }

        //}

        public async Task<IResult> PostMessage(MessageDto messageDto)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(messageDto);

                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null || !userProperties.IsActive)
                {
                    return Results.Problem("User is disabled or incative!");
                }
                var chatId = ObjectId.Parse(messageDto.ChatId);
                var chatParticipants = await _chatRepository.GetChatParticipants(chatId);

                var getChatParticipant = chatParticipants.Where(x => x == userId).FirstOrDefault();
                if (getChatParticipant == null)
                {
                    return Results.Problem("The user isn't a part of the chat session!");
                }

                Message message = new()
                {
                    UserId = userId,
                    TextMessage = messageDto.TextMessage,
                };

                await _chatCommands.InsertNewMessage(chatId, message);

                MessageRetrivedDto messageRetrivedDto = new() ;

                var mappedMessage = _mapper.Map(message, messageRetrivedDto);

                mappedMessage.UserName = userProperties!.UserName;

                await _hubContext.Clients.Group(messageDto.ChatId).SendAsync("ReceiveMessage", mappedMessage);

                return Results.Ok(mappedMessage);
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> GetMessages(string chatId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(chatId);
                var chatIdParsed = ObjectId.Parse(chatId);

                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null || !userProperties.IsActive)
                {
                    return Results.Problem("User is disabled or incative!");
                    //await Clients.Caller.SendAsync(nameof(GetMessages), "User is disabled or incative!");
                }

                var participants = await _chatRepository.GetChatParticipants(chatIdParsed);
                var checkUser = participants.Where(x => x == userId).FirstOrDefault();

                if (checkUser == null)
                {
                    return Results.Problem("User is not a chat participant!");
                    //await Clients.Caller.SendAsync(nameof(GetMessages), "User is not a chat participant!");
                }

                var result = await _chatRepository.GetChatMessages(chatIdParsed);

                //await Clients.Caller.SendAsync(nameof(GetMessages), result);

                List<MessageRetrivedDto> messageRetrivedDtos = new();

                var mappedResults = _mapper.Map(result, messageRetrivedDtos);


                IdRequestsDto idRequestsDtos = new()
                {
                    Ids = participants
                };

                var userList = await _authenticationService.GetUserListByIds(idRequestsDtos);

                var mergedList = mappedResults.GroupJoin(userList, result => result.UserId, user => user.UserAccountId, (result, users) =>
                {
                    var matchingUser = users.FirstOrDefault();
                    result.UserName = matchingUser?.UserName!;
                    return result;
                }).ToList();

                return Results.Ok(new { users = userList, messages = mergedList });
                //await Clients.Caller.SendAsync(nameof(GetMessages), new { users = userList, messages = mergedList });
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);
                //await Clients.Caller.SendAsync(nameof(GetMessages), "Argument Null Exception!", ex.Message);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
                //await Clients.Caller.SendAsync(nameof(GetMessages), ex.Message);
            }
        }

        //public async Task<IResult> GetMessages(string chatId)
        //{
        //   try
        //   {
        //        ArgumentNullException.ThrowIfNull(chatId);
        //        var chatIdParsed = ObjectId.Parse(chatId);

        //        var userId = _userAccessor.UserId;
        //        var userProperties = await _authenticationService.GetAccountProperties(userId);

        //        if (userProperties == null || !userProperties.IsActive)
        //        {
        //            return Results.Problem("User does not exist or is inactive!");
        //        }

        //        var participants = await _chatRepository.GetChatParticipants(chatIdParsed);
        //        var checkUser = participants.Where(x => x == userId).FirstOrDefault();

        //        if (checkUser == null)
        //        {
        //            return Results.Problem("User is not a chat participant!");
        //        }

        //        var result = await _chatRepository.GetChatMessages(chatIdParsed);

        //        List<MessageRetrivedDto> messageRetrivedDtos = new();

        //        var mappedResults =  _mapper.Map(result, messageRetrivedDtos);


        //        IdRequestsDto idRequestsDtos = new()
        //        {
        //            Ids = participants
        //        };

        //        var userList = await _authenticationService.GetUserListByIds(idRequestsDtos);

        //        var mergedList = mappedResults.GroupJoin(userList, result => result.UserId, user => user.UserAccountId, (result, users) =>
        //        {
        //            var matchingUser = users.FirstOrDefault();
        //            result.UserName = matchingUser?.UserName!;
        //            return result;
        //        }).ToList();

                

        //        return Results.Ok(new {users = userList, messages = mergedList});
        //    }
        //   catch (ArgumentNullException ex)
        //   {
        //       return Results.Problem("Argument Null Exception!", ex.Message);

        //   }
        //   catch (Exception ex)
        //   {
        //       return Results.Problem(ex.Message);
        //   }
        //}

        public async Task<IResult> GetMessage(string chatId, string messageId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(chatId);
                ArgumentNullException.ThrowIfNull(messageId);

                var chatIdParsed = ObjectId.Parse(chatId);
                var messageIdParsed = ObjectId.Parse(messageId);

                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null || !userProperties.IsActive)
                {
                    return Results.Problem("User does not exist or is inactive!");
                }

                var participants = await _chatRepository.GetChatParticipants(chatIdParsed);
                var checkUser = participants.Where(x => x == userId).FirstOrDefault();

                if (checkUser == null)
                {
                    return Results.Problem("User is not a chat participant!");
                }

                var message = await _chatRepository.GetChatMessage(chatIdParsed, messageIdParsed);

                return Results.Ok(message);
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> UpdateChatMessage(MessageUpdateDto messageUpdateDto)
        {
            try
            {
                var userId = _userAccessor.UserId;
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null || !userProperties.IsActive)
                {
                    return Results.Problem("User does not exist or is inactive!");
                }

                var participants = await _chatRepository.GetChatParticipants(ObjectId.Parse(messageUpdateDto.ChatId));
                var checkUser = participants.Where(x => x == userId).FirstOrDefault();

                if (checkUser == null)
                {
                    return Results.Problem("User is not a chat participant!");
                }

                await _chatCommands.UpdateMessage(messageUpdateDto);
                return Results.Ok("Updated!");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        //Check if message exists before
        //Check if user is allowed to delete their own message
        public async Task<IResult> DeleteChatMessage(string chatId, string messageId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(chatId);
                ArgumentNullException.ThrowIfNull(messageId);

                //var userId = _userAccessor.UserId;
                var userId = "7696f54b-1f17-45a8-8a21-69fdf4b0d9e1";
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null || !userProperties.IsActive)
                {
                    return Results.Problem("User does not exist or is inactive!");
                }

                var chatIdParsed = ObjectId.Parse(chatId);
                var messageIdParsed = ObjectId.Parse(messageId);

                var participants = await _chatRepository.GetChatParticipants(chatIdParsed);
                var checkUser = participants.Where(x => x == userId).FirstOrDefault();

                if (checkUser == null)
                {
                    return Results.Problem("User is not a chat participant!");
                }

                var message = await _chatRepository.GetChatMessage(chatIdParsed ,messageIdParsed);

                if(message == null)
                {
                    return Results.Problem("Chat message does not exist!");
                }

                await _chatCommands.DeleteMessage(chatIdParsed, messageIdParsed);

                return Results.Ok("Message deleted");
            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        public async Task<IResult> LeaveChat(string chatId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(chatId);

                //var userId = _userAccessor.UserId;
                var userId = "d3f04fb9-2d33-4450-b29d-f05e6abc0374";
                var userProperties = await _authenticationService.GetAccountProperties(userId);

                if (userProperties == null || !userProperties.IsActive)
                {
                    return Results.Problem("User does not exist or is inactive!");
                }

                var chatIdParsed = ObjectId.Parse(chatId);

                var participants = await _chatRepository.GetChatParticipants(chatIdParsed);
                var checkUser = participants.Where(x => x == userId).FirstOrDefault();

                if (checkUser == null)
                {
                    return Results.Problem("User is not a chat participant!");
                }

                //Check if there is only one participant

                if(participants.Count() == 1)
                {
                    await _chatCommands.DropChat(chatIdParsed);
                }
                else
                {
                    await _chatCommands.RemoveChatParticipant(chatIdParsed, userId);
                }
            
                return Results.Ok("Chat left!");

            }
            catch (ArgumentNullException ex)
            {
                return Results.Problem("Argument Null Exception!", ex.Message);

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }


        //Add functionality to add new users to already existing chats
        

    }
}

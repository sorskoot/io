﻿using System;
using System.Collections.Generic;
using System.Linq;

using TwitchLib.Client.Models;

namespace IO.Core.Models
{
    public class ChatHubMessage
    {
        private const string emoteImageTag = "<img class=\"emote\" src=\"{0}\"/>";
        private const string displayNameTag = "<span style=\"color:{0}\" class=\"name\">{1}</span>";

        public static ChatHubMessage FromChatMessage(ChatMessage chatMessage, StreamUserModel streamUserModel)
        {
            var chatHubMessage = new ChatHubMessage() 
            {
                ChatMessage = chatMessage,
                Badges = chatMessage.Badges,
                Bits = chatMessage.Bits,
                Id = chatMessage.Id,
                IsBroadcaster = chatMessage.IsBroadcaster,
                IsModerator = chatMessage.IsModerator,
                IsSubscriber = chatMessage.IsSubscriber,
                Message = chatMessage.Message,
                SubscribedMonthCount = chatMessage.SubscribedMonthCount,
                ColorHex = chatMessage.ColorHex,
                Username = chatMessage.Username,
                DisplayName = chatMessage.DisplayName,
                UserType = (int)chatMessage.UserType, 
                StreamUserModel = streamUserModel
            };
            chatHubMessage.HubMessage = chatHubMessage.GenerateHubMessage();
            return chatHubMessage;
        }

        public static ChatHubMessage FromBot(string message)
        {
            var chatHubMessage = new ChatHubMessage()
            {
                Bits = 0,
                Badges = new List<KeyValuePair<string, string>>(),
                Id = Guid.NewGuid().ToString(),
                IsBroadcaster = false,
                IsModerator = true,
                IsSubscriber = true,
                Message = message,
                SubscribedMonthCount = 12,
                ColorHex = "#01b9ff",
                Username = Constants.TwitchChatBotUsername,
                DisplayName = Constants.TwitchChatBotUsername,
                UserType = 1,
                StreamUserModel = new StreamUserModel("abc123", Constants.TwitchChatBotUsername, "")
            };
            chatHubMessage.HubMessage = chatHubMessage.GenerateHubMessage();
            return chatHubMessage;
        }

        private ChatMessage ChatMessage { get; set; }

        public StreamUserModel StreamUserModel { get; set; }

        public List<KeyValuePair<string, string>> Badges { get; set; }

        public int Bits { get; set; }

        public string Id { get; set; }

        public bool IsBroadcaster { get; set; }

        public bool IsModerator { get; set; }

        public bool IsSubscriber { get; set; }

        public string Message { get; set; }

        public int SubscribedMonthCount { get; set; }

        public string ColorHex { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public int UserType { get; set; }

        public string HubMessage { get; set; }

        private string GenerateHubMessage()
        {
            string result = "";
            if (!string.IsNullOrEmpty(Message))
            {
                //result = string.Format(displayNameTag, ColorHex, DisplayName + ": ");
                result += Message;

                // Replace emotes with img tags
                if (ChatMessage != null &&
                    ChatMessage.EmoteSet != null &&
                    ChatMessage.EmoteSet.Emotes.Count > 0)
                {
                    // Need to consider the meh emote with urls.
                    EmoteSet.Emote mehEmote = ChatMessage.EmoteSet.Emotes.FirstOrDefault(a => a.Name.Equals(":/"));
                    if (mehEmote != null)
                    {
                        result = result.Replace(mehEmote.Name, string.Format(emoteImageTag, mehEmote.ImageUrl));
                    }

                    List<EmoteSet.Emote> nonMehEmotes = ChatMessage.EmoteSet.Emotes.Where(w => !w.Name.Equals(":/")).ToList();

                    foreach (EmoteSet.Emote emote in nonMehEmotes)
                    {
                        result = result.Replace(emote.Name, string.Format(emoteImageTag, emote.ImageUrl));
                    }
                }

            }
            return result;
        }
    }
}

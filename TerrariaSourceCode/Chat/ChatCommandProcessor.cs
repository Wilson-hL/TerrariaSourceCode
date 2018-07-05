// Decompiled with JetBrains decompiler
// Type: Terraria.Chat.ChatCommandProcessor
// Assembly: Terraria, Version=1.3.5.1, Culture=neutral, PublicKeyToken=null
// MVID: E90A5A2F-CD10-4A2C-9D2A-6B036D4E8877
// Assembly location: E:\Decompile\Terraria\Terraria.exe

using System.Collections.Generic;
using System.Linq;
using ReLogic.Utilities;
using Terraria.Chat.Commands;
using Terraria.Localization;

namespace Terraria.Chat
{
    public class ChatCommandProcessor : IChatProcessor
    {
        private readonly Dictionary<ChatCommandId, IChatCommand> _commands =
            new Dictionary<ChatCommandId, IChatCommand>();

        private IChatCommand _defaultCommand;

        private readonly Dictionary<LocalizedText, ChatCommandId> _localizedCommands =
            new Dictionary<LocalizedText, ChatCommandId>();

        public bool ProcessOutgoingMessage(ChatMessage message)
        {
            var keyValuePair =
                _localizedCommands.FirstOrDefault(
                    pair =>
                        HasLocalizedCommand(message, pair.Key));
            var commandId = keyValuePair.Value;
            if (keyValuePair.Key == null)
                return false;
            message.SetCommand(commandId);
            message.Text = RemoveCommandPrefix(message.Text, keyValuePair.Key);
            return true;
        }

        public bool ProcessReceivedMessage(ChatMessage message, int clientId)
        {
            IChatCommand chatCommand;
            if (_commands.TryGetValue(message.CommandId, out chatCommand))
            {
                chatCommand.ProcessMessage(message.Text, (byte) clientId);
                return true;
            }

            if (_defaultCommand == null)
                return false;
            _defaultCommand.ProcessMessage(message.Text, (byte) clientId);
            return true;
        }

        public ChatCommandProcessor AddCommand<T>() where T : IChatCommand, new()
        {
            var commandKey = "ChatCommand." +
                             AttributeUtilities
                                 .GetCacheableAttribute<T, ChatCommandAttribute>().Name;
            var index = ChatCommandId.FromType<T>();
            _commands[index] = new T();
            if (Language.Exists(commandKey))
            {
                _localizedCommands.Add(Language.GetText(commandKey), index);
            }
            else
            {
                commandKey += "_";
                foreach (var key in Language.FindAll((key, text) =>
                    key.StartsWith(commandKey)))
                    _localizedCommands.Add(key, index);
            }

            return this;
        }

        public ChatCommandProcessor AddDefaultCommand<T>() where T : IChatCommand, new()
        {
            AddCommand<T>();
            _defaultCommand = _commands[ChatCommandId.FromType<T>()];
            return this;
        }

        private static bool HasLocalizedCommand(ChatMessage message, LocalizedText command)
        {
            var lower = message.Text.ToLower();
            var str = command.Value;
            if (!lower.StartsWith(str))
                return false;
            if (lower.Length == str.Length)
                return true;
            return lower[str.Length] == ' ';
        }

        private static string RemoveCommandPrefix(string messageText, LocalizedText command)
        {
            var str = command.Value;
            if (!messageText.StartsWith(str) || messageText.Length == str.Length || messageText[str.Length] != ' ')
                return "";
            return messageText.Substring(str.Length + 1);
        }
    }
}
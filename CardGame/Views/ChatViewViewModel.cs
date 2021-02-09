using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CardGame.Enums;
using CardGame.Managers;
using CardGame.Models;
using Utility.MVVM;

namespace CardGame.Views
{
    public class ChatViewViewModel : ObservableObject
    {
        /// <summary>
        /// Backing field
        /// </summary>
        private ObservableCollection<Message> _messages;

        /// <summary>
        /// Backing field
        /// </summary>
        private string _message;

        /// <summary>
        /// Creates a new instance of <see cref="ChatViewViewModel"/>
        /// </summary>
        public ChatViewViewModel()
        {
            Messages = new ObservableCollection<Message>();
            Mediator.RegisterEnums(Operations.AddMessage, SetMessages);
        }

        /// <summary>
        /// Gets or sets the messages
        /// </summary>
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => SetField(ref _messages, value);
        }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetField(ref _message, value);
        }

        /// <summary>
        /// Sends the message
        /// </summary>
        public async void SendMessage()
        {
            var msg = Message;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Messages.Add(new Message
                {
                    Control = new ChatMessageView
                    {
                        Message = msg,
                        TimeStamp = DateTime.Now.ToShortTimeString(),
                        UserName = ConnectionManager.Name
                    }
                });
            });

            ConnectionManager.SendCommand($"message {msg}");
        }

        private async void SetMessages(object param)
        {
            if(!(param is Message message))
                return;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                message.Control = new ChatMessageView
                {
                    Message = message.Value,
                    TimeStamp = message.Time,
                    UserName = message.User
                };
                Messages.Add(message);
            });
        }
    }
}

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
        private ObservableCollection<ChatMessageView> _messages;

        /// <summary>
        /// Creates a new instance of <see cref="ChatViewViewModel"/>
        /// </summary>
        public ChatViewViewModel()
        {
            Messages = new ObservableCollection<ChatMessageView>();
            Mediator.RegisterEnums(Operations.AddMessage, SetMessages);
        }

        /// <summary>
        /// Gets or sets the messages
        /// </summary>
        public ObservableCollection<ChatMessageView> Messages
        {
            get => _messages;
            set => SetField(ref _messages, value);
        }

        private async void SetMessages(object param)
        {
            if(!(param is Message message))
                return;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Messages.Add(new ChatMessageView
                {
                    Message = message.Value
                });
            });
        }
    }
}

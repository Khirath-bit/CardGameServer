using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using CardGame.Enums;
using CardGame.Managers;
using CardGame.Models;
using Utility.MVVM;

namespace CardGame.Views
{
    public class ClientsOnlineViewViewModel : ObservableObject
    {
        /// <summary>
        /// Backing field
        /// </summary>
        private ObservableCollection<Client> _connections;

        /// <summary>
        /// Backing field
        /// </summary>
        private ChatViewViewModel _chatControl;

        /// <summary>
        /// Gets or sets all connections
        /// </summary>
        public ObservableCollection<Client> Connections
        {
            get => _connections;
            set => SetField(ref _connections, value);
        }

        /// <summary>
        /// Gets or sets the chat control
        /// </summary>
        public ChatViewViewModel ChatControl
        {
            get => _chatControl;
            set => SetField(ref _chatControl, value);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClientsOnlineViewViewModel"/>
        /// </summary>
        public ClientsOnlineViewViewModel()
        {
            Mediator.RegisterEnums(ListClients.ListAllConnections, ListConnections);
            Connections = new ObservableCollection<Client>();
            ChatControl = new ChatViewViewModel();
        }

        /// <summary>
        /// Lists all connections
        /// </summary>
        public async void ListConnections(object data)
        {
            if (data == null)
                return;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Connections.Clear();
            });

            var names = data.ToString().Split(',');

            foreach (var name in names)
            {
                var validName = name.Split('{')[0];

                var id = Guid.Parse(name.Split('{')[1]);

                if(id == ConnectionManager.ConnectionId) //don't list myself
                    continue;

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Connections.Add(new Client { Name = validName, Id = id});
                });
            }
        }
    }
}

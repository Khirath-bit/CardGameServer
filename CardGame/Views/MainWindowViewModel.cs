using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using CardGame.Managers;
using MahApps.Metro.Controls.Dialogs;
using Utility.MVVM;

namespace CardGame.Views
{
    public class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Backing field
        /// </summary>
        private ClientsOnlineViewViewModel _clientsOnlineControl;

        /// <summary>
        /// Backing field
        /// </summary>
        private bool _connected;

        /// <summary>
        /// Contains the dialog coordinator
        /// </summary>
        private readonly IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// <see cref="MainWindowViewModel"/>
        /// </summary>
        public MainWindowViewModel(IDialogCoordinator coordinator)
        {
            _dialogCoordinator = coordinator;
            ClientsOnlineControl = new ClientsOnlineViewViewModel();
            Connected = false;
        }

        /// <summary>
        /// Connect to server command
        /// </summary>
        public ICommand ConnectToServerCommand => new DelegateCommand(ConnectToServer);

        /// <summary>
        /// Gets or sets the clients online control
        /// </summary>
        public ClientsOnlineViewViewModel ClientsOnlineControl
        {
            get => _clientsOnlineControl;
            set => SetField(ref _clientsOnlineControl, value);
        }

        /// <summary>
        /// Gets or sets the connection color
        /// </summary>
        public bool Connected
        {
            get => _connected;
            set => SetField(ref _connected, value);
        }

        /// <summary>
        /// Try to connect
        /// </summary>
        private async void ConnectToServer()
        {
            if(Connected)
                return;

            Connected = ConnectionManager.Connect();

            if(!Connected)
                return;

            var name = await _dialogCoordinator.ShowInputAsync(this, "Name", "Input your name.");

            ConnectionManager.SendCommand($"setname {name}");

            ClientsOnlineControl.Load();
        }
    }
}

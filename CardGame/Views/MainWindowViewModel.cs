using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CardGame.Enums;
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
        /// Backing field
        /// </summary>
        private bool _showJoinButton;

        /// <summary>
        /// Backing field
        /// </summary>
        private bool _showSpectateButton ;

        /// <summary>
        /// Backing field
        /// </summary>
        private bool _showStartGameButton;

        /// <summary>
        /// Contains the dialog coordinator
        /// </summary>
        private readonly IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// Backing field
        /// </summary>
        private ObservableObject _gameControl;

        /// <summary>
        /// Backing field
        /// </summary>
        private GameType _gameType;

        /// <summary>
        /// Contains the game manager
        /// </summary>
        private readonly GameManager _gameManager;

        /// <summary>
        /// <see cref="MainWindowViewModel"/>
        /// </summary>
        public MainWindowViewModel(IDialogCoordinator coordinator)
        {
            _dialogCoordinator = coordinator;
            Mediator.RegisterEnums(Operations.SetGameType, SetGameType);
            Connected = false;
            _gameManager = new GameManager();
        }

        /// <summary>
        /// Connect to server command
        /// </summary>
        public ICommand ConnectToServerCommand => new DelegateCommand(ConnectToServer);

        /// <summary>
        /// Connect to server command
        /// </summary>
        public ICommand JoinGameCommand => new DelegateCommand(JoinGame);

        /// <summary>
        /// Connect to server command
        /// </summary>
        public ICommand SpectateGameCommand => new DelegateCommand(SpectateGame);

        /// <summary>
        /// Connect to server command
        /// </summary>
        public ICommand StartGameCommand => new DelegateCommand(StartGame);

        /// <summary>
        /// Gets or sets the clients online control
        /// </summary>
        public ClientsOnlineViewViewModel ClientsOnlineControl
        {
            get => _clientsOnlineControl;
            set => SetField(ref _clientsOnlineControl, value);
        }

        /// <summary>
        /// Gets or sets the game type
        /// </summary>
        public GameType GameType
        {
            get => _gameType;
            set
            {
                _gameManager.CommunicateGameType(value);
                SetField(ref _gameType, value);
            }
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
        /// Gets or sets joined game
        /// </summary>
        public bool ShowJoinButton
        {
            get => _showJoinButton;
            set => SetField(ref _showJoinButton, value);
        }

        /// <summary>
        /// Gets or sets joined game
        /// </summary>
        public bool ShowSpectateButton
        {
            get => _showSpectateButton;
            set => SetField(ref _showSpectateButton, value);
        }

        /// <summary>
        /// Gets or sets joined game
        /// </summary>
        public bool ShowStartGameButton
        {
            get => _showStartGameButton;
            set => SetField(ref _showStartGameButton, value);
        }

        /// <summary>
        /// Gets or sets the game control
        /// </summary>
        public ObservableObject GameControl
        {
            get => _gameControl;
            set => SetField(ref _gameControl, value);
        }

        /// <summary>
        /// Try to connect
        /// </summary>
        private async void ConnectToServer()
        {
            if (Connected)
                return;

            Connected = ConnectionManager.Connect();

            if (!Connected)
                return;

            ShowJoinButton = true;
            ShowSpectateButton = true;

            ClientsOnlineControl = new ClientsOnlineViewViewModel();

            var name = await _dialogCoordinator.ShowInputAsync(this, "Name", "Input your name.");

            ConnectionManager.SendCommand($"setname {name}");

            ConnectionManager.Name = name;

            Thread.Sleep(300);
            ConnectionManager.SendCommand("list connections");
        }

        /// <summary>
        /// Joins the game
        /// </summary>
        private async void JoinGame()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ShowSpectateButton = true;
                ShowJoinButton = false;
                ShowStartGameButton = true;
            });
            

            ConnectionManager.SendCommand("game join");
        }

        /// <summary>
        /// Spectates the game
        /// </summary>
        private async void SpectateGame()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ShowSpectateButton = false;
                ShowJoinButton = true;
                ShowStartGameButton = false;
            });


            ConnectionManager.SendCommand("game spectate");
        }

        private void StartGame()
        {
            ConnectionManager.SendCommand("game start");
        }

        /// <summary>
        /// Sets the game type
        /// </summary>
        private async void SetGameType(object param)
        {
            if (!(param is GameType type))
                return;

            await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        var ob = Activator.CreateInstance(_gameManager.GameTypeControls[type]);

                        GameControl = ob as ObservableObject;

                        GameType = type;
                    }
                    catch (Exception e)
                    {
                        GameControl = null;
                    }

                });
        }
    }
}

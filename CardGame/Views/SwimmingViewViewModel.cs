using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CardGame.Enums;
using CardGame.Managers;
using CardGame.Models;
using Newtonsoft.Json;
using Utility.MVVM;

namespace CardGame.Views
{
    public class SwimmingViewViewModel : ObservableObject
    {
        /// <summary>
        /// Backing field
        /// </summary>
        private ObservableCollection<Card> _middleCards;

        /// <summary>
        /// Backing field
        /// </summary>
        private ObservableCollection<Card> _playerCards;

        /// <summary>
        /// Backing field
        /// </summary>
        private string _informationText;

        /// <summary>
        /// Backing field
        /// </summary>
        private bool _isOnTurn;

        /// <summary>
        /// Creates a new instance of <see cref="SwimmingViewViewModel"/>
        /// </summary>
        public SwimmingViewViewModel()
        {
            Mediator.RegisterEnums(Operations.SetMiddleCardsSwimming, SetMiddleCards);
            Mediator.RegisterEnums(Operations.SetPlayerCardsSwimming, SetPlayerCards);
            Mediator.RegisterEnums(Operations.StartGameSwimming, Init);
            Mediator.RegisterEnums(Operations.InfotextSwimming, SetInfoText);
            Mediator.RegisterEnums(Operations.OnturnSwimming, OnTurn);
            Init();
        }

        /// <summary>
        /// Gets or sets the middle cards
        /// </summary>
        public ObservableCollection<Card> MiddleCards
        {
            get => _middleCards;
            set
            {
                SetField(ref _middleCards, value);
                OnPropertyChanged("MiddleCardsSet");
                InformationText = "";
            }
        }

        /// <summary>
        /// Gets or sets the middle cards
        /// </summary>
        public ObservableCollection<Card> PlayerCards
        {
            get => _playerCards;
            set
            {
                SetField(ref _playerCards, value);
                OnPropertyChanged("IsBeginner");
                OnPropertyChanged("MiddleCardsSet");
                OnPropertyChanged("PlayerCardsSet");
                if (IsBeginner)
                    InformationText = "Du darfst beginnen!";
            } 
        }

        /// <summary>
        /// Occurs when the user takes the beginner hand
        /// </summary>
        public ICommand TakeHandCommand => new RelayCommand(TakeHand);

        /// <summary>
        /// Occurs when the user takes all cards
        /// </summary>
        public ICommand TakeAllCommand => new DelegateCommand(TakeAll);

        /// <summary>
        /// Occurs when the user skips
        /// </summary>
        public ICommand SkipCommand => new DelegateCommand(SkipTurn);

        /// <summary>
        /// Occurs when the user passes
        /// </summary>
        public ICommand PassCommand => new DelegateCommand(PassTurn);

        /// <summary>
        /// Gets if any middle cards are available yet
        /// </summary>
        public bool MiddleCardsSet => MiddleCards.Any();

        /// <summary>
        /// Gets if any player cards are set
        /// </summary>
        public bool PlayerCardsSet => PlayerCards.Any();

        /// <summary>
        /// Gets or sets if the user is on turn
        /// </summary>
        public bool IsOnTurn
        {
            get => _isOnTurn;
            set => SetField(ref _isOnTurn, value);
        }

        /// <summary>
        /// Gets if the player is the beginner in this turn
        /// </summary>
        public bool IsBeginner => PlayerCards?.Count == 6;

        /// <summary>
        /// Gets or sets the information text
        /// </summary>
        public string InformationText
        {
            get => _informationText;
            set => SetField(ref _informationText, value);
        }

        /// <summary>
        /// Sets the middle cards
        /// </summary>
        public async void SetMiddleCards(object param)
        {
            try
            {
                var cards = JsonConvert.DeserializeObject<List<Card>>(param.ToString());

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    MiddleCards = new ObservableCollection<Card>(cards);
                    
                });
            }
            catch (Exception e)
            {
                return;
            }
        }

        /// <summary>
        /// Sets the player cards
        /// </summary>
        public async void SetPlayerCards(object param)
        {
            try
            {
                var cards = JsonConvert.DeserializeObject<List<Card>>(param.ToString());

                if (cards == null)
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        PlayerCards = new ObservableCollection<Card>();
                    });
                    
                    return;
                }

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    PlayerCards = new ObservableCollection<Card>(cards);
                });
            }
            catch (Exception e)
            {
                return;
            }
        }

        /// <summary>
        /// Init the game
        /// </summary>
        private async void Init(object param = null)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                MiddleCards = new ObservableCollection<Card>();
                PlayerCards = new ObservableCollection<Card>();
                OnPropertyChanged("MiddleCardsSet");
                OnPropertyChanged("IsBeginner");
                InformationText = "Schwimmen!";
            });
        }

        /// <summary>
        /// Communicates to the server which hand was taken by the user
        /// </summary>
        /// <param name="param"></param>
        private void TakeHand(object param)
        {
            ConnectionManager.SendCommand($"action swimming beginnerhand {param}");
        }

        /// <summary>
        /// Enables turn option controls
        /// </summary>
        private async void OnTurn(object param)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => { IsOnTurn = true; });
        }

        /// <summary>
        /// Sets the infotext
        /// </summary>
        private async void SetInfoText(object text)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => { InformationText = text.ToString(); });
        }

        /// <summary>
        /// Takes all middle cards
        /// </summary>
        private async void TakeAll()
        {
            var turn = new SwimmingTurn();
            turn.HandCards = MiddleCards.ToList();
            await Application.Current.Dispatcher.InvokeAsync(() => { PlayerCards = MiddleCards; });
            
            ConnectionManager.SendCommand($"action swimming turn {JsonConvert.SerializeObject(turn)}");
        }

        /// <summary>
        /// Skips this turn
        /// </summary>
        private void SkipTurn()
        {
            var turn = new SwimmingTurn();
            turn.Skip = true;

            ConnectionManager.SendCommand($"action swimming turn {JsonConvert.SerializeObject(turn)}");
        }

        private void PassTurn()
        {
            var turn = new SwimmingTurn();
            turn.Pass = true;
            ConnectionManager.SendCommand($"action swimming turn {JsonConvert.SerializeObject(turn)}");
        }
    }
}

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
        /// Creates a new instance of <see cref="SwimmingViewViewModel"/>
        /// </summary>
        public SwimmingViewViewModel()
        {
            Mediator.RegisterEnums(Operations.SetMiddleCardsSwimming, SetMiddleCards);
            Mediator.RegisterEnums(Operations.SetPlayerCardsSwimming, SetPlayerCards);
            Mediator.RegisterEnums(Operations.StartGame, Init);
            Init();
        }

        /// <summary>
        /// Gets or sets the middle cards
        /// </summary>
        public ObservableCollection<Card> MiddleCards
        {
            get => _middleCards;
            set => SetField(ref _middleCards, value);
        }

        /// <summary>
        /// Gets or sets the middle cards
        /// </summary>
        public ObservableCollection<Card> PlayerCards
        {
            get => _playerCards;
            set => SetField(ref _playerCards, value);
        }

        /// <summary>
        /// Occurs when the user takes the beginner hand
        /// </summary>
        public ICommand TakeHandCommand => new RelayCommand(TakeHand);

        /// <summary>
        /// Gets if any middle cards are available yet
        /// </summary>
        public bool MiddleCardsSet => MiddleCards.Any(); 

        /// <summary>
        /// Gets if the player is the beginner in this turn
        /// </summary>
        public bool IsBeginner => PlayerCards?.Count == 6;

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
                    OnPropertyChanged("MiddleCardsSet");
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
                        OnPropertyChanged("IsBeginner");
                        OnPropertyChanged("MiddleCardsSet");
                    });
                    
                    return;
                }

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    PlayerCards = new ObservableCollection<Card>(cards);
                    OnPropertyChanged("IsBeginner");
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
                OnPropertyChanged("MiddleCardsSet");
                OnPropertyChanged("IsBeginner");
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
    }
}

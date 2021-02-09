using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CardGame.Enums;
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
            MiddleCards = new ObservableCollection<Card>();
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
    }
}

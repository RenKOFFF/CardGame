using System;
using System.Collections.Generic;
using CardGame.Gameplay.Cards.Data;
using UnityEngine;

namespace CardGame.Gameplay.Cards
{
    public class CardGroup : MonoBehaviour
    {
        public event Action<CardGroup> OnGroupEmpty;
        
        [SerializeField] private Card[] _emptyCards;

        private readonly Stack<Card> _cards = new();
        
        public int CardCount => _emptyCards.Length;
        public Card TopCard => _cards.Count == 0 ? null : _cards.Peek();

        public void Initialize(List<CardInfo> cardsInfos)
        {
            for (var i = 0; i < _emptyCards.Length; i++)
            {
                var emptyCard = _emptyCards[i];
                var cardInfo = cardsInfos[i];

                Register(emptyCard, cardInfo);
                emptyCard.PlayShowAnimation(i);
                
                if (i == 0)
                {
                    emptyCard.SubscribeOnClick(OnLastPicked);
                }
            }

            _cards.Peek().ShowFront();
        }

        private void Register(Card card, CardInfo cardInfo)
        {
            var previousCard = _cards.Count == 0 ? null : _cards.Peek();

            if (previousCard != null)
            {
                card.SetParent(previousCard);
                previousCard.SetChild(card);
            }

            card.Initialize(cardInfo);
            card.SubscribeOnClick(TryPickCard);

            _cards.Push(card);
        }

        private void TryPickCard(Card card)
        {
            if (_cards.Peek() == card && CardInfo.IsNeighbourCards(CardsBankController.Instance.CurrentCard.Denomination, card.Denomination))
            {
                card.UnsubscribeOnClick(TryPickCard);

                _cards.Pop();
                var parent = card.Parent;

                if (parent != null)
                    parent.ShowFront();

                CardsBankController.Instance.SetCard(card);
            }
        }

        private void OnLastPicked(Card card)
        {
            if (_cards.Count != 0 && _cards.Peek() != card)
                return;
            
            card.UnsubscribeOnClick(TryPickCard);
            
            OnGroupEmpty?.Invoke(this);
        }
    }
}
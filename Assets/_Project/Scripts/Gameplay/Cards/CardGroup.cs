using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Gameplay.Cards.Data;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Gameplay.Cards
{
    public class CardGroup : MonoBehaviour
    {
        [SerializeField] private Card[] _emptyCards;
        
        private readonly Stack<Card> _cards = new();

        public void Initialize(Dictionary<Denomination, List<CardInfo>> cards)
        {
            for (var i = 0; i < _emptyCards.Length; i++)
            {
                var emptyCard = _emptyCards[i];
                
                Register(emptyCard, cards);
                emptyCard.PlayShowAnimation(i);
            }

            _cards.Peek().ShowFront();
        }

        private void Register(Card card, Dictionary<Denomination, List<CardInfo>> cards)
        {
            var rndCardInfo = cards
                .SelectMany(x => x.Value)
                .OrderBy(x => Guid.NewGuid()).First();
            
            var previousCard = _cards.Count == 0 ? null : _cards.Peek();

            if (previousCard != null)
            {
                card.SetParent(previousCard);
                previousCard.SetChild(card);
            }
            
            card.Initialize(rndCardInfo);
            card.SubscribeOnClick(TryPickCard);
            
            _cards.Push(card);
        }

        private void TryPickCard(Card card)
        {
            if (_cards.Peek() == card && IsNeighbourCards(card))
            {
                card.UnsubscribeOnClick(TryPickCard);
                
                _cards.Pop();
                var parent = card.Parent;

                if (parent != null) 
                    parent.ShowFront();
                
                CardsBankController.Instance.SetCard(card);
            }
        }

        private static bool IsNeighbourCards(Card card)
        {
            var currentDenomination = CardsBankController.Instance.CurrentCard.Denomination;
            
            Denomination topNeighbour;
            Denomination bottomNeighbour;
            
            if (currentDenomination == Denomination.King)
                topNeighbour = Denomination.A;
            else topNeighbour = currentDenomination + 1;
            
            if (currentDenomination == Denomination.A)
                bottomNeighbour = Denomination.King;
            else bottomNeighbour = currentDenomination - 1;
            
            return card.Denomination == topNeighbour || card.Denomination == bottomNeighbour;
            
        }
    }
}
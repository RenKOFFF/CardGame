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
        
        private readonly List<Card> _cards = new();

        public void Initialize(Dictionary<Denomination, List<CardInfo>> cards)
        {
            foreach (var emptyCard in _emptyCards)
            {
                Register(emptyCard, cards);
            }
            
            _cards.Last().ShowFront();
        }

        private void Register(Card card, Dictionary<Denomination, List<CardInfo>> cards)
        {
            var rndCardInfo = cards
                .SelectMany(x => x.Value)
                .OrderBy(x => Guid.NewGuid()).First();
            
            var lastCard = _cards.LastOrDefault();
            card.SetParent(lastCard);

            if (lastCard != null)
                lastCard.SetChild(card);
            
            card.Initialize(rndCardInfo);
            card.SubscribeOnClick(TryPickCard);
            
            _cards.Add(card);
        }

        private void TryPickCard(Card card)
        {
            if (_cards.LastOrDefault() == card)
            {
                card.UnsubscribeOnClick(TryPickCard);
                
                card.transform
                    .DOScale(Vector3.zero, 0.75f)
                    .SetEase(Ease.InBack)
                    .OnKill(() =>
                    {
                        _cards.Remove(card);
                        var parent = card.Parent;

                        if (parent != null) 
                            parent.ShowFront();

                        Destroy(card.gameObject);
                    });
                
            }
        }
    }
}
using System;
using System.Collections.Generic;
using CardGame.Gameplay.Cards.Data;
using CardGame.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Gameplay.Cards
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private Sprite _backSprite;
        [SerializeField] private CardSuit[] _cardsSuitsData;
        [SerializeField] private CardGroup[] _cardGroups;

        private readonly Dictionary<Denomination, List<CardInfo>> _cards = new();

        public void Initialize()
        {
            foreach (var cardSuit in _cardsSuitsData)
            {
                var cardsSprites = cardSuit.CardsSprites;
                var backSprite = _backSprite;
                
                for (var i = 0; i < cardsSprites.Length; i++)
                {
                    var cardSprite = cardsSprites[i];
                    InitializeCardsInfo((Denomination)(i + 1), cardSprite, backSprite);
                }
            }

            InitializeCardGroups();
        }

        public IEnumerable<CardInfo> GetRandomCards(int count)
        {
            var result = new List<CardInfo>();

            for (var i = 0; i < count; i++)
            {
                var randomDenomination = Utility.GetRandomOf<Denomination>(min:1);
                var randomSuit = Random.Range(0, _cards[randomDenomination].Count);
                
                result.Add(_cards[randomDenomination][randomSuit]);
            }
            
            return result;
        }

        private void InitializeCardsInfo(Denomination denomination, Sprite cardSprite, Sprite backSprite)
        {
            var cardInfo = new CardInfo(denomination, cardSprite, backSprite);

            if (!_cards.ContainsKey(denomination))
            {
                _cards.Add(denomination, new List<CardInfo>());
            }
            
            _cards[denomination].Add(cardInfo);
        }

        private void InitializeCardGroups()
        {
            foreach (var cardGroup in _cardGroups)
            {
                cardGroup.Initialize(_cards);
            }
        }
    }
}
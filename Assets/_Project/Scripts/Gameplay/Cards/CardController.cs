using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Gameplay.Cards.Data;
using CardGame.Gameplay.Cards.Deck;
using UnityEngine;

namespace CardGame.Gameplay.Cards
{
    public class CardController : MonoBehaviour
    {
        public Action<bool> OnGameOver;
        
        [SerializeField] private Sprite _backSprite;
        [SerializeField] private CardSuit[] _cardsSuitsData;
        
        [SerializeField] private CardGroup[] _cardGroups;
        [SerializeField] private CardsBankController _bankController;

        private readonly Dictionary<Denomination, List<CardInfo>> _cards = new();
        
        private CardsDeck _cardsDeck;

        public void Initialize()
        {
            foreach (var cardSuit in _cardsSuitsData)
            {
                var cardsSprites = cardSuit.CardsSprites;
                var backSprite = _backSprite;
                var suit = cardSuit.Suit;
                
                for (var i = 0; i < cardsSprites.Length; i++)
                {
                    var cardSprite = cardsSprites[i];
                    InitializeCardsInfo(suit, (Denomination)(i + 1), cardSprite, backSprite);
                }
            }
            
            _cardsDeck = new CardsDeck(_cards);
            var sequences = _cardsDeck.CurrentSequences.ToArray();

            InitializeCardGroups(sequences);
            InitializeBanks(sequences);
        }

        private void InitializeCardsInfo(Suits suit, Denomination denomination, Sprite cardSprite, Sprite backSprite)
        {
            var cardInfo = new CardInfo(suit, denomination, cardSprite, backSprite);

            if (!_cards.ContainsKey(denomination))
            {
                _cards.Add(denomination, new List<CardInfo>());
            }
            
            _cards[denomination].Add(cardInfo);
        }

        private void InitializeCardGroups(IEnumerable<CardSequence> sequences)
        {
            var sequencesIntoGroups = DistributeSequencesIntoGroups(sequences);

            foreach (var group in sequencesIntoGroups)
            {
                group.Key.Initialize(group.Value);
            }
        }

        private void InitializeBanks(IEnumerable<CardSequence> sequences)
        {
            var startingCards = sequences
                .Select(s => s.FirstCard)
                .ToArray();
            
            _bankController.Initialize(startingCards);
        }

        private Dictionary<CardGroup, List<CardInfo>> DistributeSequencesIntoGroups(IEnumerable<CardSequence> sequences)
        {
            var cardSequences = sequences.ToArray();
            var cardsByCardGroup = new Dictionary<CardGroup, List<CardInfo>>();
            
            var groupQueue = new Queue<CardGroup>(_cardGroups);
            
            foreach (var sequence in cardSequences)
            {
                foreach (var info in sequence.Sequence)
                {
                    var nexGroup = groupQueue.Dequeue();

                    if (nexGroup != null && IsFullData(nexGroup) == false)
                    {
                        groupQueue.Enqueue(nexGroup);
                    }
                    else
                    {
                        return cardsByCardGroup;
                    }
                    
                    if (!cardsByCardGroup.ContainsKey(nexGroup))
                    {
                        cardsByCardGroup.Add(nexGroup, new List<CardInfo>());
                    }
            
                    cardsByCardGroup[nexGroup].Add(info);
                }
            }

            return cardsByCardGroup;
            
            bool IsFullData(CardGroup group)
            {
                if (!cardsByCardGroup.ContainsKey(group))
                {
                    return false;
                }
                
                return cardsByCardGroup[group].Count == group.CardCount;
            }
        }
    }
}
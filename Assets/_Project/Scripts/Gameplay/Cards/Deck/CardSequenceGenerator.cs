using System.Collections.Generic;
using System.Linq;
using CardGame.Gameplay.Cards.Data;
using UnityEngine;

namespace CardGame.Gameplay.Cards.Deck
{
    public class CardSequenceGenerator
    {
        public const int MIN_CARD_SEQUENCE_LENGTH = 2;
        public const int MAX_CARD_SEQUENCE_LENGTH = 7;
        
        public const float TOP_CARD_PROBABILITY = 0.65f;
        public const float BOTTOM_CARD_PROBABILITY = 0.35f;
        public const float CHANGE_DIRECTION_PROBABILITY = 0.15f;

        public CardSequenceGenerator(IEnumerable<CardInfo> allCards)
        {
            Sequences = new List<CardSequence>();
            SeparateCards(allCards);
        }

        public List<CardSequence> Sequences { get; }

        private void SeparateCards(IEnumerable<CardInfo> allCards)
        {
            var cardInfos = allCards.ToList();

            while (cardInfos.Count - MAX_CARD_SEQUENCE_LENGTH > 0)
            {
                var sequenceLength = Random.Range(MIN_CARD_SEQUENCE_LENGTH, MAX_CARD_SEQUENCE_LENGTH + 1);
                var cardSequence = new CardSequence(cardInfos.GetRange(0, sequenceLength));
                
                Sequences.Add(cardSequence);
                cardInfos.RemoveRange(0, sequenceLength);
            }
            
            var lastCardSequence = new CardSequence(cardInfos);
                
            Sequences.Add(lastCardSequence);
            cardInfos.Clear();
        }

        public IEnumerable<CardSequence> GenerateSequences()
        {
            return Sequences;
        }
    }
}
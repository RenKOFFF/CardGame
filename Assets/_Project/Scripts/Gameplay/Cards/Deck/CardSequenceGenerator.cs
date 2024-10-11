using System.Collections.Generic;
using System.Linq;
using CardGame.Extensions;
using CardGame.Gameplay.Cards.Data;
using CardGame.Utils;
using UnityEngine;

namespace CardGame.Gameplay.Cards.Deck
{
    public class CardSequenceGenerator
    {
        public const int MIN_CARD_SEQUENCE_LENGTH = 2;
        public const int MAX_CARD_SEQUENCE_LENGTH = 7;

        public const float FORWARD_DIRECTION_PROBABILITY = 0.65f;
        public const float CHANGE_DIRECTION_PROBABILITY = 0.15f;

        public CardSequenceGenerator(Dictionary<Denomination, List<CardInfo>> cards, int cardsRequired)
        {
            Sequences = new List<CardSequence>();
            SeparateCards(cards, cardsRequired);
        }

        public List<CardSequence> Sequences { get; }

        private void SeparateCards(Dictionary<Denomination, List<CardInfo>> cards, int cardsRequired)
        {
            var totalSequencesLength = 0;
            
            while (totalSequencesLength < cardsRequired)
            {
                var isForwardDirection = Random.value <= FORWARD_DIRECTION_PROBABILITY;
                var canChangeDirection = true;
                
                var sequenceLength = Random.Range(MIN_CARD_SEQUENCE_LENGTH, 
                    Mathf.Min(MAX_CARD_SEQUENCE_LENGTH, cardsRequired - totalSequencesLength) + 1);
                
                var currentDenomination = Utility.GetRandomOf<Denomination>(min: 1);

                var currentSequence = new List<CardInfo>();
                
                for (var i = 0; i < sequenceLength + 1; i++)
                {
                    var currentCard = cards[currentDenomination].GetRandom();
                    currentSequence.Add(currentCard);

                    var neighbourCards = CardInfo.GetNeighbourCards(currentDenomination);
                    currentDenomination = isForwardDirection ? neighbourCards.Top : neighbourCards.Bottom;

                    if (canChangeDirection && Random.value <= CHANGE_DIRECTION_PROBABILITY)
                    {
                        isForwardDirection = !isForwardDirection;
                        canChangeDirection = false;
                    }
                }
                
                totalSequencesLength += sequenceLength;
                
                var cardSequence = new CardSequence(currentSequence);
                Sequences.Add(cardSequence);
            }
        }


        public IEnumerable<CardSequence> GenerateSequences()
        {
            return Sequences;
        }
    }
}
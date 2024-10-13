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
        private const int _MIN_CARD_SEQUENCE_LENGTH = 2;
        private const int _MAX_CARD_SEQUENCE_LENGTH = 7;

        private const float _FORWARD_DIRECTION_PROBABILITY = 0.65f;
        private const float _CHANGE_DIRECTION_PROBABILITY = 0.15f;
        
        private readonly Dictionary<Denomination, List<CardInfo>> _cards;
        private readonly int _cardsRequired;

        public CardSequenceGenerator(Dictionary<Denomination, List<CardInfo>> cards, int cardsRequired)
        {
            _cards = cards;
            _cardsRequired = cardsRequired;
            
            Sequences = new List<CardSequence>();
        }

        private List<CardSequence> Sequences { get; }

        public IEnumerable<CardSequence> GenerateSequences()
        {
            SeparateCards(_cards, _cardsRequired);
            // SeparateCards2(_cards, _cardsRequired);
            return Sequences;
        }

        private void SeparateCards(Dictionary<Denomination, List<CardInfo>> cards, int cardsRequired)
        {
            Sequences.Clear();
            
            var totalSequencesLength = 0;
            var selectedCards = new List<CardInfo>();
            
            while (totalSequencesLength < cardsRequired)
            {
                var isForwardDirection = Random.value <= _FORWARD_DIRECTION_PROBABILITY;
                var canChangeDirection = true;
                
                var sequenceLength = Random.Range(_MIN_CARD_SEQUENCE_LENGTH, Mathf.Min(_MAX_CARD_SEQUENCE_LENGTH, cardsRequired - totalSequencesLength) + 1);

                Denomination currentDenomination;
                
                if (Sequences.Count == 0)
                {
                    currentDenomination = Utility.GetRandomOf<Denomination>(min: 1);
                }
                else
                {
                    var distinctCards = cards
                        .Select(c => c.Key)
                        .Where(d => selectedCards
                            .Select(s => s.Denomination)
                            .Contains(d) == false)
                        .ToList();

                    if (distinctCards.Count == 0)
                    {
                        selectedCards.Clear();
                        currentDenomination = Utility.GetRandomOf<Denomination>(min: 1);
                    }
                    else
                    {
                        currentDenomination = distinctCards.GetRandom();
                    }
                }

                var currentSequence = new List<CardInfo>();
                
                for (var i = 0; i < sequenceLength + 1; i++)
                {
                    var currentCard = cards[currentDenomination].GetRandom();
                    currentSequence.Add(currentCard);

                    var neighbourCards = CardInfo.GetNeighbourCards(currentDenomination);
                    currentDenomination = isForwardDirection ? neighbourCards.Top : neighbourCards.Bottom;

                    if (canChangeDirection && Random.value <= _CHANGE_DIRECTION_PROBABILITY)
                    {
                        isForwardDirection = !isForwardDirection;
                        canChangeDirection = false;
                    }
                }
                
                totalSequencesLength += sequenceLength;
                
                var cardSequence = new CardSequence(currentSequence);
                selectedCards.AddRange(cardSequence.FullSequence);
                
                Sequences.Add(cardSequence);
            }
        }

        private void SeparateCardsSimple(Dictionary<Denomination, List<CardInfo>> cards, int cardsRequired)
        {
            Sequences.Clear();
            
            var totalSequencesLength = 0;
            var currentDenomination = Denomination.Ace;
            
            while (totalSequencesLength < cardsRequired)
            {
                var sequenceLength = 8;
                
                var currentSequence = new List<CardInfo>();
                
                for (var i = 0; i < sequenceLength + 1; i++)
                {
                    var currentCard = cards[currentDenomination].GetRandom();
                    currentSequence.Add(currentCard);

                    var neighbourCards = CardInfo.GetNeighbourCards(currentDenomination);
                    currentDenomination = neighbourCards.Top;
                }
                
                totalSequencesLength += sequenceLength;
                
                var cardSequence = new CardSequence(currentSequence);
                Sequences.Add(cardSequence);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using CardGame.Gameplay.Cards.Data;
using CardGame.Gameplay.Cards.Deck;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Gameplay.Cards
{
    public class CardController : MonoBehaviour
    {
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
                
                for (var i = 0; i < cardsSprites.Length; i++)
                {
                    var cardSprite = cardsSprites[i];
                    InitializeCardsInfo((Denomination)(i + 1), cardSprite, backSprite);
                }
            }
            
            _cardsDeck = new CardsDeck(_cards);
            var sequences = _cardsDeck.CurrentSequences.ToArray();

            InitializeBanks(sequences);
            InitializeCardGroups(sequences);
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

        private void InitializeBanks(IEnumerable<CardSequence> sequences)
        {
            var startingCards = sequences
                .Select(s => s.FirstCard)
                .ToArray();
            
            _bankController.Initialize(startingCards);
        }

        private void InitializeCardGroups(IEnumerable<CardSequence> sequences)
        {
            var sequencesIntoGroups = DistributeSequencesIntoGroups(sequences);

            foreach (var group in sequencesIntoGroups)
            {
                group.Key.Initialize(group.Value);
            }
        }

        private Dictionary<CardGroup, List<CardInfo>> DistributeSequencesIntoGroups(IEnumerable<CardSequence> sequences)
        {
            CardGroup lastCardGroup = null;
            
            var cardSequences = sequences.ToArray();
            var cardsByCardGroup = new Dictionary<CardGroup, List<CardInfo>>();
            
            foreach (var sequence in cardSequences)
            {
                foreach (var info in sequence.Sequence)
                {
                    var randomGroup = _cardGroups
                        .Where(g => g != lastCardGroup && IsFullData(g) == false)
                        .OrderBy(_ => Random.value)
                        .FirstOrDefault();
                    
                    if (randomGroup == null && IsFullData(lastCardGroup))
                    {
                        return cardsByCardGroup;
                    }

                    if (randomGroup != null)
                    {
                        lastCardGroup = randomGroup;
                    }

                    if (lastCardGroup == null)
                    {
                        throw new System.Exception("No card group found");
                    }
                    
                    if (!cardsByCardGroup.ContainsKey(lastCardGroup))
                    {
                        cardsByCardGroup.Add(lastCardGroup, new List<CardInfo>());
                    }
            
                    cardsByCardGroup[lastCardGroup].Add(info);
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
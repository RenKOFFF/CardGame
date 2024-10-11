using System.Collections.Generic;
using System.Linq;
using CardGame.Gameplay.Cards.Data;

namespace CardGame.Gameplay.Cards.Deck
{
    public class CardsDeck
    {
        private readonly Dictionary<Denomination, List<CardInfo>> _cards;
        private readonly IEnumerable<CardInfo> _allCards;
        
        private readonly CardSequenceGenerator _cardSequenceGenerator;

        public CardsDeck(Dictionary<Denomination, List<CardInfo>> cards, int cardsRequired)
        {
            // _allCards = cards
            //     .SelectMany(x => x.Value)
            //     .OrderBy(c => c.Suit)
            //     .ThenBy(c => c.Denomination)
            //     .ToArray();
            
            _cardSequenceGenerator = new CardSequenceGenerator(cards, cardsRequired);
            
            _cards = cards;
            
            CurrentSequences = GenerateSequences();
        }

        public IEnumerable<CardSequence> CurrentSequences { get; private set; }

        public IEnumerable<CardSequence> GenerateSequences()
        {
            CurrentSequences = _cardSequenceGenerator.GenerateSequences();
            return CurrentSequences;
        }
    }
}
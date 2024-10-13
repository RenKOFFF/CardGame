using System.Collections.Generic;
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
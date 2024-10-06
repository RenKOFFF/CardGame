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

        public CardsDeck(Dictionary<Denomination, List<CardInfo>> cards)
        {
            _allCards = cards.SelectMany(x => x.Value);
            
            _cardSequenceGenerator = new CardSequenceGenerator(_allCards);
            
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
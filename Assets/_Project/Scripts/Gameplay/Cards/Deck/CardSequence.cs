using System.Collections.Generic;
using System.Linq;
using CardGame.Gameplay.Cards.Data;

namespace CardGame.Gameplay.Cards.Deck
{
    public class CardSequence
    {
        public CardSequence(IEnumerable<CardInfo> cards)
        {
            var cardInfos = cards.ToArray();

            FirstCard = cardInfos.First();
            Sequence = cardInfos.Skip(1).ToArray();
        }

        public CardInfo FirstCard { get; }
        public CardInfo[] Sequence { get; }
        public CardInfo[] FullSequence => new[] { FirstCard }.Concat(Sequence).ToArray();
    }
}
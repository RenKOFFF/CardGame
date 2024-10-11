using UnityEngine;

namespace CardGame.Gameplay.Cards.Data
{
    public class CardInfo
    {
        public Suits Suit { get; }
        public Denomination Denomination { get; }
        public Sprite FrontSprite { get; }
        public Sprite BackSprite { get; }

        public CardInfo(Suits suit, Denomination denomination, Sprite frontSprite, Sprite backSprite)
        {
            Suit = suit;
            Denomination = denomination;
            FrontSprite = frontSprite;
            BackSprite = backSprite;
        }

        public static bool IsNeighbourCards(Denomination first, Denomination second)
        {
            Denomination topNeighbour;
            Denomination bottomNeighbour;

            if (first == Denomination.King)
                topNeighbour = Denomination.A;
            else topNeighbour = first + 1;

            if (first == Denomination.A)
                bottomNeighbour = Denomination.King;
            else bottomNeighbour = first - 1;

            return second == topNeighbour || second == bottomNeighbour;
        }

        public static (Denomination Top, Denomination Bottom) GetNeighbourCards(Denomination denomination)
        {
            return denomination switch
            {
                Denomination.King => (Denomination.A, Denomination.Queen),
                Denomination.A => (Denomination.Two, Denomination.King),
                _ => (denomination + 1, denomination - 1)
            };
        }
    }
}
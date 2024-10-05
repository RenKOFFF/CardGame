using UnityEngine;

namespace CardGame.Gameplay.Cards.Data
{
    public class CardInfo
    {
        public Denomination Denomination { get; }
        public Sprite FrontSprite { get; }
        public Sprite BackSprite { get; }

        public CardInfo(Denomination denomination, Sprite frontSprite, Sprite backSprite)
        {
            Denomination = denomination;
            FrontSprite = frontSprite;
            BackSprite = backSprite;
        }
    }
}
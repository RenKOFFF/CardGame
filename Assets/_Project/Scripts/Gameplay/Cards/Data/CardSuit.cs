using System;
using UnityEngine;

namespace CardGame.Gameplay.Cards.Data
{
    [Serializable]
    public class CardSuit
    {
        public Suits Suit;
        public Sprite[] CardsSprites;
    }
}
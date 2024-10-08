using CardGame.Gameplay.Cards;
using UnityEngine;

namespace CardGame
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CardController _cardController;

        private void Awake()
        {
            _cardController.Initialize();
        }
    }
}
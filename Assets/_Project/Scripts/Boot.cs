using CardGame.Gameplay.Cards;
using UnityEngine;

namespace CardGame
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private CardController _cardController;

        private void Awake()
        {
            _cardController.Initialize();
        }
    }
}
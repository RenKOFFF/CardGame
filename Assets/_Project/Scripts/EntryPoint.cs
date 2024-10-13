using CardGame.Gameplay;
using CardGame.Gameplay.Cards;
using UnityEngine;

namespace CardGame
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CardController _cardController;
        [SerializeField] private GameController _gameController;

        private void Awake()
        {
            _cardController.Initialize();
            _gameController.Initialize();
        }
    }
}
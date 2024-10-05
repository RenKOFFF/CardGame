using CardGame.Gameplay.Cards;
using UnityEngine;

namespace CardGame
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private CardController _cardController;
        [SerializeField] private CardsBankController _bankController;

        private void Awake()
        {
            _cardController.Initialize();
            _bankController.Initialize();
        }
    }
}
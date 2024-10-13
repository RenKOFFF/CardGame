using System;
using System.Linq;
using CardGame.Gameplay.Cards;
using CardGame.Gameplay.Cards.Data;
using UnityEngine;

namespace CardGame.Gameplay
{
    public class GameController : MonoBehaviour
    {
        public event Action<GameResult> OnGameOver;
        
        [SerializeField] private CardGroup[] _cardGroups;
        [SerializeField] private CardsBankController _cardsBankController;
        
        private int _emptyGroups;

        private void Start()
        {
            foreach (var cardGroup in _cardGroups)
            {
                cardGroup.OnGroupEmpty += OnGroupEmpty;
            }
            
            _cardsBankController.OnBankEmpty += OnBankEmpty;
        }

        private void OnDestroy()
        {
            foreach (var cardGroup in _cardGroups)
            {
                cardGroup.OnGroupEmpty -= OnGroupEmpty;
            }
            
            _cardsBankController.OnBankEmpty -= OnBankEmpty;
            _cardsBankController.OnSetCard -= CheckGameLose;
        }

        private void OnBankEmpty()
        {
            _cardsBankController.OnSetCard += CheckGameLose;
            CheckGameLose(_cardsBankController.CurrentCard);
        }

        private void CheckGameLose(Card card)
        {
            var groups = _cardGroups.Where(g => g.TopCard != null).ToArray();

            if (groups.Length == 0)
            {
                return;
            }
            
            if (groups.Any(cardGroup => CardInfo.IsNeighbourCards(card.Denomination, cardGroup.TopCard.Denomination)))
            {
                return;
            }

            _cardsBankController.OnSetCard -= CheckGameLose;
            OnGameOver?.Invoke(GameResult.Lose);
        }

        private void OnGroupEmpty(CardGroup cardGroup)
        {
            _emptyGroups++;
            
            if (_emptyGroups == _cardGroups.Length)
            {
                OnGameOver?.Invoke(GameResult.Win);
            }
        }
    }
    
    public enum GameResult
    {
        Win,
        Lose
    }
}
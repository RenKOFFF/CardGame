using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Gameplay.Cards
{
    public class CardsBankController : MonoBehaviour
    {
        private const float _POOL_SPACING_MIN = 5f;
        private const float _POOL_SPACING_MAX = 20f;

        public static CardsBankController Instance { get; private set; }
        public Card CurrentCard { get; set; }

        [SerializeField] private CardController _cardController;

        [SerializeField] private Transform _currentCardHolder;

        [SerializeField] private Transform _poolCardHolder;
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private int _poolSize = 5;

        private readonly List<Card> _passedCards = new();
        private readonly Stack<Card> _cardsPool = new();


        public void Initialize()
        {
            Instance = this;

            var randomData = _cardController.GetRandomCards(_poolSize).ToArray();

            for (var i = 0; i < _poolSize; i++)
            {
                var card = Instantiate(_cardPrefab, _poolCardHolder);
                _cardsPool.Push(card);

                card.transform.position = CalculateCardPosition(_poolSize, i);
                card.PlayShowAnimation(i);

                card.Initialize(randomData[i]);
                card.SubscribeOnClick(ChangeCurrentCard);
            }

            SetCard(_cardsPool.Pop());
        }

        public void SetCard(Card card)
        {
            CurrentCard = card;
            _passedCards.Add(card);

            CurrentCard.ShowFront();

            CurrentCard.transform.SetParent(_currentCardHolder, true);
            CurrentCard.transform.DOLocalJump(Vector3.zero, 20f, 1, 0.5f);
        }

        private void ChangeCurrentCard(Card card)
        {
            if (card == CurrentCard || _cardsPool.Count == 0 || _cardsPool.Peek() != card)
                return;

            SetCard(_cardsPool.Pop());
            RecalculateCardsPosition();
        }

        private void RecalculateCardsPosition()
        {
            var index = _cardsPool.Count;
            foreach (var card in _cardsPool)
            {
                index--;

                var position = CalculateCardPosition(_cardsPool.Count, index);

                var sequence = DOTween.Sequence();

                sequence
                    .Append(card.transform
                        .DOMove(position, 0.2f)
                        .SetEase(Ease.OutCirc))
                    .Join(card.transform
                        .DORotate(Vector3.forward * -15, 0.1f)
                        .SetEase(Ease.OutCirc))
                    .Insert(.1f, card.transform
                        .DORotate(Vector3.zero, 0.1f))
                    .SetDelay(index * 0.02f);
            }
        }

        private Vector3 CalculateCardPosition(int currentPoolSize, int cardIndex)
        {
            var spacing = Mathf.Lerp(_POOL_SPACING_MIN, _POOL_SPACING_MAX, (float)currentPoolSize / _poolSize);

            var holderPosition = _poolCardHolder.transform.position;
            var leftPoint = holderPosition + Vector3.left * (spacing * _poolSize);

            return Vector3.Lerp(leftPoint, holderPosition, ((float)cardIndex + 1) / currentPoolSize);
        }
    }
}
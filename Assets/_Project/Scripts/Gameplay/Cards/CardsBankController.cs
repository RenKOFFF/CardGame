using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Gameplay.Cards.Data;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Gameplay.Cards
{
    public class CardsBankController : MonoBehaviour
    {
        public event Action OnBankEmpty;
        public event Action<Card> OnSetCard;
        
        private const float _POOL_SPACING_MIN = 5f;
        private const float _POOL_SPACING_MAX = 20f;

        [SerializeField] private CardGroup[] _cardGroups;

        [SerializeField] private Transform _currentCardHolder;

        [SerializeField] private Transform _poolCardHolder;
        [SerializeField] private Card _cardPrefab;

        private readonly List<Card> _passedCards = new();
        private readonly Stack<Card> _cardsPool = new();

        private int _startPoolSize;
        private List<CardInfo> _bankCardsInfo;

        public static CardsBankController Instance { get; private set; }
        public Card CurrentCard { get; private set; }

        public void Initialize(CardInfo[] cardsInfo)
        {
            Instance = this;
            
            _bankCardsInfo = cardsInfo.ToList();
            _startPoolSize = cardsInfo.Length;

            for (var i = 0; i < _startPoolSize; i++)
            {
                var card = Instantiate(_cardPrefab, _poolCardHolder);
                _cardsPool.Push(card);

                card.transform.position = CalculateCardPosition(_startPoolSize, i);
                card.Initialize(cardsInfo[i]);
                card.PlayShowAnimation(i);

                card.SubscribeOnClick(ChangeCurrentCard);

                if (i == 0)
                {
                    card.SubscribeOnClick(OnLastBankCardPicked);
                }
            }
            
            ChangeCurrentCard(_cardsPool.Peek());
        }

        private void OnDestroy()
        {
            DOTween.Kill(this);
        }

        public void SetCard(Card card)
        {
            CurrentCard = card;
            _passedCards.Add(card);

            CurrentCard.ShowFront();

            CurrentCard.transform.SetParent(_currentCardHolder, true);
            CurrentCard.transform
                .DOLocalJump(Vector3.zero, 20f, 1, 0.5f)
                .SetTarget(this);

            OnSetCard?.Invoke(card);
        }

        private void ChangeCurrentCard(Card card)
        {
            if (card == CurrentCard || _cardsPool.Count == 0 || _cardsPool.Peek() != card)
                return;
            
            // var startSequenceCard = FindStartSequenceCard();
            // card.Initialize(startSequenceCard);

            SetCard(_cardsPool.Pop());
            RecalculateCardsPosition();
        }

        private void OnLastBankCardPicked(Card card)
        {
            if (_cardsPool.Count != 0 && _cardsPool.Peek() != card)
                return;
            
            card.UnsubscribeOnClick(OnLastBankCardPicked);
            
            OnBankEmpty?.Invoke();
        }

        private CardInfo FindStartSequenceCard()
        {
            var groupPeek = _cardGroups
                .Where(g => g.TopCard != null)
                .Select(g => g.TopCard);
            
            var currentInfos = _bankCardsInfo;

            var cardInfo = new List<CardInfo>();
            foreach (var peek in groupPeek)
            {
                var neighbourCards = currentInfos
                    .Where(c => CardInfo.IsNeighbourCards(peek.Denomination, c.Denomination))
                    .ToList();
                
                cardInfo.AddRange(neighbourCards);
            }

            var info = cardInfo.FirstOrDefault();
            if (info != null)
            {
                _bankCardsInfo.Remove(info);
                return info;
            }
            
            return _bankCardsInfo.First();
        }

        private void RecalculateCardsPosition()
        {
            var index = _cardsPool.Count;
            foreach (var card in _cardsPool)
            {
                index--;

                var position = CalculateCardPosition(_cardsPool.Count, index);

                var sequence = DOTween.Sequence();
                sequence.SetTarget(this);

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
            var spacing = Mathf.Lerp(_POOL_SPACING_MIN, _POOL_SPACING_MAX, (float)currentPoolSize / _startPoolSize);

            var holderPosition = _poolCardHolder.transform.position;
            var leftPoint = holderPosition + Vector3.left * (spacing * _startPoolSize);

            return Vector3.Lerp(leftPoint, holderPosition, ((float)cardIndex + 1) / currentPoolSize);
        }
    }
}
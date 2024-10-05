using System;
using CardGame.Gameplay.Cards.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame.Gameplay.Cards
{
    public class Card : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _suitImage;

        private CardInfo _info;

        private event Action<Card> OnClick;

        public Card Parent { get; private set; }
        public Card Child { get; private set; }
        public Denomination Denomination => _info.Denomination;

        public void Initialize(CardInfo cardInfo)
        {
            _info = cardInfo;

            _suitImage.sprite = _info.BackSprite;
            name = $"Card - {_info.Denomination}";
        }

        public void ShowFront()
        {
            var sequence = DOTween.Sequence();

            const float fullRotationDuration = 0.5f;
            const float rotateOneSideDuration = fullRotationDuration / 2;

            sequence
                .Append(_suitImage.transform
                    .DORotate(Vector3.up * 90f, rotateOneSideDuration)
                    .OnComplete(() =>
                    {
                        _suitImage.sprite = _info.FrontSprite;

                        _suitImage.transform.rotation = Quaternion.Euler(Vector3.up * 270f);
                    }))
                .Append(_suitImage.transform
                    .DORotate(Vector3.zero, rotateOneSideDuration));
        }
        
        public void PlayShowAnimation(int index)
        {
            const float delay = 0.15f;

            transform.localScale = Vector3.zero;

            transform
                .DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay * index);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(this);
        }

        public void SubscribeOnClick(Action<Card> onClick)
        {
            OnClick += onClick;
        }

        public void UnsubscribeOnClick(Action<Card> onClick)
        {
            OnClick -= onClick;
        }

        public void SetParent(Card card)
        {
            Parent = card;
        }

        public void SetChild(Card card)
        {
            Child = card;
        }
    }
}
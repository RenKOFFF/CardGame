using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class GameEndPanel : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        
        public void SubscribeOnClick(UnityAction callback)
        {
            _restartButton.onClick.AddListener(callback);
        }
        
        public void UnsubscribeOnClick(UnityAction callback)
        {
            _restartButton.onClick.RemoveListener(callback);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
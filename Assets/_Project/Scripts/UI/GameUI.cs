using CardGame.Gameplay.Cards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private CardController _cardController;
        
        [SerializeField] private Button _reloadButton;
        [SerializeField] private GameObject _winView;
        [SerializeField] private GameObject _loseView;
        
        private void Start()
        {
            _reloadButton.onClick.AddListener(ReloadLevel);
            _cardController.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            _cardController.OnGameOver -= OnGameOver;
        }

        private static void ReloadLevel()
        {
            var currentScene = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        private void OnGameOver(bool isWin)
        {
            switch (isWin)
            {
                case true:
                    _winView.SetActive(true);
                    break;
                
                case false:
                    _loseView.SetActive(true);
                    break;
            }
        }
    }
}
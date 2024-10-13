using System;
using CardGame.Gameplay;
using CardGame.Gameplay.Cards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        
        [SerializeField] private Button _reloadButton;
        [SerializeField] private GameEndPanel _winView;
        [SerializeField] private GameEndPanel _loseView;
        
        private void Start()
        {
            _reloadButton.onClick.AddListener(ReloadLevel);
            _gameController.OnGameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            _gameController.OnGameOver -= OnGameOver;
        }

        private static void ReloadLevel()
        {
            var currentScene = SceneManager.GetActiveScene();
            
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        private void OnGameOver(GameResult gameResult)
        {
            switch (gameResult)
            {
                case GameResult.Win:
                    _winView.Show();
                    _winView.SubscribeOnClick(ReloadLevel);
                    break;
                
                case GameResult.Lose:
                    _loseView.Show();
                    _loseView.SubscribeOnClick(ReloadLevel);
                    break;
            }
        }
    }
}
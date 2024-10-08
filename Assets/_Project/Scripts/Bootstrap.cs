using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        
        private void Start()
        {
            _startButton.onClick.AddListener(LoadGame);
        }

        private static void LoadGame()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
}
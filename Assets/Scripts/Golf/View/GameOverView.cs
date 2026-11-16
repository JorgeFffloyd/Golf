using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Golf.Golf.View
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject m_gameOver;
        [SerializeField] private Button m_menuButton;
        [SerializeField] private TMP_Text m_highScoreText;

        public void Show()
        {
            m_gameOver.SetActive(true);
        }

        public void Hide()
        {
            m_gameOver.SetActive(false);
        }

        public void SetHighScore(int score)
        {
            if (m_highScoreText != null)
            {
                m_highScoreText.text = "Best score: " + score;
            }
        }

        public void SetMenuAction(Action action)
        {
            m_menuButton.onClick.RemoveAllListeners();
            m_menuButton.onClick.AddListener(() => action?.Invoke());
        }
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.Golf.View
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject m_mainMenu;
        [SerializeField] private Button m_playButton;
        [SerializeField] private Button m_shopButton;
        [SerializeField] private TMP_Text m_highScoreText;

        public void Show()
        {
            m_mainMenu.SetActive(true);
        }

        public void Hide()
        {
            m_mainMenu.SetActive(false);
        }

        public void SetPlayAction(Action action)
        {
            m_playButton.onClick.RemoveAllListeners();
            m_playButton.onClick.AddListener(
                () => action?.Invoke()
            );
        }

        public void SetShopAction(Action action)
        {
            m_shopButton.onClick.RemoveAllListeners();
            m_shopButton.onClick.AddListener(
                () => action?.Invoke()
            );
        }

        public void SetHighScore(int score)
        {
            if (m_highScoreText != null)
            {
                m_highScoreText.text = $"BEST SCORE: {score}";
            }
        }
    }
}

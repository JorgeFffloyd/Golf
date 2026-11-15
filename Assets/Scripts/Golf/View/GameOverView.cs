using UnityEngine;
using UnityEngine.UI;
using System;

namespace Golf.Golf.View
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private GameObject m_gameOver;
        [SerializeField] private Button m_menuButton;

        public void Show()
        {
            m_gameOver.SetActive(true);
        }

        public void Hide()
        {
            m_gameOver.SetActive(false);
        }

        public void SetMenuAction(Action action)
        {
            m_menuButton.onClick.RemoveAllListeners();
            m_menuButton.onClick.AddListener(() => action?.Invoke());
        }
    }
}

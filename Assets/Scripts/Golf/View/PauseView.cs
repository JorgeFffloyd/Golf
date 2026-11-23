using System;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.Golf.View
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject m_pausePanel;
        [SerializeField] private Button m_pauseButton;
        [SerializeField] private Button m_resumeButton;
        [SerializeField] private Button m_menuButton;

        public void Show()
        {
            m_pausePanel.SetActive(true);
        }

        public void Hide()
        {
            m_pausePanel.SetActive(false);
        }

        public void SetPauseAction(Action action)
        {
            m_pauseButton.onClick.RemoveAllListeners();
            m_pauseButton.onClick.AddListener(() => action?.Invoke());
        }

        public void SetResumeAction(Action action)
        {
            m_resumeButton.onClick.RemoveAllListeners();
            m_resumeButton.onClick.AddListener(() => action?.Invoke());
        }

        public void SetMenuAction(Action action)
        {
            m_menuButton.onClick.RemoveAllListeners();
            m_menuButton.onClick.AddListener(() => action?.Invoke());
        }
    }
}


using System;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.Golf.View
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject m_mainMenu;
        [SerializeField] private Button m_playButton;

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
            m_playButton.onClick.AddListener(() => action?.Invoke());
        }
    }
}

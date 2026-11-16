using System;
using UnityEngine;
using UnityEngine.UI;

namespace Golf.Golf.View
{
    public class HitButton : MonoBehaviour
    {
        [SerializeField] private Button m_button;

        public event Action OnHit;

        private void Awake()
        {
            if (m_button == null)
                m_button = GetComponent<Button>();

            if (m_button != null)
                m_button.onClick.AddListener(HandleHit);
        }

        private void OnDestroy()
        {
            if (m_button != null)
                m_button.onClick.RemoveListener(HandleHit);
        }

        private void HandleHit()
        {
            OnHit?.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

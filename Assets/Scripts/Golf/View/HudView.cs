using TMPro;
using UnityEngine;

namespace Golf.Golf.View
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private GameObject m_hud;
        [SerializeField] private TMP_Text m_scoreText;
        [SerializeField] private TMP_Text m_balanceText;

        public void Show()
        {
            m_hud.SetActive(true);
        }

        public void Hide()
        {
            m_hud.SetActive(false);
        }

        public void SetScore(int score)
        {
            if (m_scoreText != null)
            {
                m_scoreText.text = "Score: " + score;
            }
        }

        public void SetBalance(int balance)
        {
            if (m_balanceText != null)
            {
                m_balanceText.text = "Gold: " + balance;
            }
        }
    }
}

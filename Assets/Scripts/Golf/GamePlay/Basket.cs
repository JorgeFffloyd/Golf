using System;
using UnityEngine;

namespace Golf.Golf.GamePlay
{
    public class Basket : MonoBehaviour
    {
        public event Action OnStoneHitBasket;
        public event Action OnGoldOreCollected;
        public event Action OnBombCollected;

        private bool m_gameOverTriggered;

        public void ResetBasket()
        {
            m_gameOverTriggered = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Bomb bomb = collision.collider.GetComponentInParent<Bomb>();

            if (bomb != null)
            {
                OnBombCollected?.Invoke();
                Destroy(bomb.gameObject);
                return;
            }

            Stone stone = collision.collider.GetComponentInParent<Stone>();

            if (stone != null)
            {
                if (m_gameOverTriggered) return;

                m_gameOverTriggered = true;
                OnStoneHitBasket?.Invoke();
                return;
            }

            GoldOre goldOre = collision.collider.GetComponentInParent<GoldOre>();

            if (goldOre != null && goldOre.TryCollect())
            {
                OnGoldOreCollected?.Invoke();
                Destroy(goldOre.gameObject);
            }
        }
    }
}
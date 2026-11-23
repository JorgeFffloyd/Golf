using UnityEngine;

namespace Golf.Golf.GamePlay
{
    public class Bomb : MonoBehaviour
    {
        private bool m_wasHit;

        public bool TryHit()
        {
            if (m_wasHit)
                return false;

            m_wasHit = true;
            return true;
        }
    }
}

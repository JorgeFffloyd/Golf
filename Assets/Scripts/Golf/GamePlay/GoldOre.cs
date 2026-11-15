namespace Golf.Golf.GamePlay
{
    public class GoldOre : UnityEngine.MonoBehaviour
    {
        private bool m_isCollected;

        public bool TryCollect()
        {
            if (m_isCollected)
                return false;

            m_isCollected = true;
            return true;
        }
    }
}

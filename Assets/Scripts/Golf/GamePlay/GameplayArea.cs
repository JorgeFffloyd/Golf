using UnityEngine;
using Golf.GamePlay;

namespace Golf.Golf.GamePlay
{
    public class GameplayArea : MonoBehaviour
    {
        [Header("Area Components")]
        [SerializeField] private Camera m_camera;
        [SerializeField] private StoneSpawner m_spawner;
        [SerializeField] private Basket m_basket;
        [SerializeField] private Transform m_stickPoint;

        [Header("Area Audio")]
        [SerializeField] private AudioSource m_music;

        public Camera AreaCamera => m_camera;
        public StoneSpawner Spawner => m_spawner;
        public Basket AreaBasket => m_basket;
        public Transform StickPoint => m_stickPoint;
        public AudioSource Music => m_music;

        public void SetAreaActive(bool active)
        {
            if (gameObject.activeSelf != active)
                gameObject.SetActive(active);
        }

        public bool IsConfigured()
        {
            return m_camera != null &&
                   m_spawner != null &&
                   m_basket != null &&
                   m_stickPoint != null &&
                   m_music != null;
        }
    }
}

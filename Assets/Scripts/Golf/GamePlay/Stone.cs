using UnityEngine;

namespace Golf.Golf.GamePlay
{
    public class Stone : MonoBehaviour
    {
        [SerializeField] private AudioSource m_hitAudioSource;

        public event System.Action<Stone> OnStoneCollision;

        private bool m_isHit;
        private Rigidbody m_rb;

        public bool IsHit => m_isHit;

        private void Awake()
        {
            m_rb = GetComponent<Rigidbody>();
        }

        public void Hit(Vector3 force)
        {
            if (m_isHit || m_rb == null)
                return;

            m_isHit = true;

            m_rb.isKinematic = false;
            m_rb.AddForce(force, ForceMode.Impulse);

            OnStoneCollision?.Invoke(this);

            if (m_hitAudioSource != null)
            {
                m_hitAudioSource.Play();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_hitAudioSource != null)
            {
                m_hitAudioSource.Play();
            }
        }

        public void SetPhysicsEnabled(bool enabled)
        {
            if (m_rb != null)
            {
                m_rb.isKinematic = !enabled;
            }
        }
    }
}

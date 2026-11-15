
using UnityEngine;

namespace Golf.GamePlay
{
    public class Stick : MonoBehaviour
    {
        [SerializeField] private AudioSource m_hitAudioSource;

        private Quaternion m_pointA;
        private Quaternion m_pointB;

        private bool m_isSwinging;
        private Rigidbody m_rb;

        private void Awake()
        {
            m_pointA = transform.localRotation;
            m_pointB = m_pointA * Quaternion.Euler(100f, 0f, 0f);

            m_rb = GetComponent<Rigidbody>();
        }

        public void StartSwing()
        {
            if (m_isSwinging)
                return;

            m_isSwinging = true;

            CancelInvoke(nameof(StopSwing));
            Invoke(nameof(StopSwing), 0.2f);
        }

        private void StopSwing()
        {
            m_isSwinging = false;
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (m_hitAudioSource != null)
            {
                m_hitAudioSource.Play();
            }
        }

        private void FixedUpdate()
        {
            if (m_rb == null)
                return;

            Quaternion targetRotation = m_isSwinging
                ? m_pointB
                : m_pointA;

            float speed = m_isSwinging ? 1500f : 500f;

            m_rb.MoveRotation(
                Quaternion.RotateTowards(
                    m_rb.rotation,
                    targetRotation,
                    speed * Time.fixedDeltaTime
                )
            );
        }
    }
}

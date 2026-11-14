using System;
using UnityEngine;

namespace Golf.Golf.GamePlay
{
    public class Stone : MonoBehaviour
    {
        public event Action<Stone> OnStoneCollision;

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
            m_rb.AddForce(force, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnStoneCollision?.Invoke(this);
        }

        public void SetPhysicsEnabled(bool enabled)
        {
            if (m_rb != null)
                m_rb.isKinematic = !enabled;
        }
    }
}
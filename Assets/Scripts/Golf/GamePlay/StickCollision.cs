using System;
using UnityEngine;
using Golf.Golf.GamePlay;

namespace Golf.GamePlay
{
    public class StickCollision : MonoBehaviour
    {
        [SerializeField] private AudioSource m_hitAudioSource;
        [SerializeField] private float m_hitForce = 8f;
        [SerializeField] private float m_upwardForce = 2f;

        private Stick m_stick;

        public event Action OnStoneHit;

        private void Awake()
        {
            m_stick = GetComponentInParent<Stick>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_stick == null || !m_stick.IsSwinging)
                return;

            Stone stone = collision.gameObject.GetComponentInParent<Stone>();

            if (stone == null || stone.IsHit)
                return;

            Vector3 direction =
                (stone.transform.position - transform.position).normalized;

            Vector3 force =
                direction * m_hitForce + Vector3.up * m_upwardForce;

            stone.Hit(force);

            if (!stone.IsHit)
                return;

            OnStoneHit?.Invoke();

            if (m_hitAudioSource != null)
                m_hitAudioSource.Play();
        }
    }
}

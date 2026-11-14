
using System.Collections;
using UnityEngine;

namespace Golf.GamePlay
{
    public class StoneSpawner : MonoBehaviour
    {
        [SerializeField] private float m_currentMaxTime = 3f;
        [SerializeField] private float m_timeDecrease = 0.2f;

        private float m_initialMaxTime;
        private GameObject m_stonePrefab;

        private Coroutine m_spawnCoroutine;
        private bool m_isRunning;

        private void Awake()
        {
            m_initialMaxTime = m_currentMaxTime;

            m_stonePrefab = Resources.Load<GameObject>("falling_stone");

        }

        public void ResetSpawner()
        {
            StopSpawning();
            m_currentMaxTime = m_initialMaxTime;
        }

        public void StartSpawning()
        {
            if (m_isRunning || m_stonePrefab == null)
                return;

            m_isRunning = true;
            m_spawnCoroutine = StartCoroutine(SpawnRoutine());
        }

        public void StopSpawning()
        {
            m_isRunning = false;

            if (m_spawnCoroutine != null)
            {
                StopCoroutine(m_spawnCoroutine);
                m_spawnCoroutine = null;
            }
        }

        private IEnumerator SpawnRoutine()
        {
            while (m_isRunning)
            {
                yield return new WaitForSeconds(
                    Random.Range(0.5f, m_currentMaxTime)
                );

                if (!m_isRunning)
                    yield break;

                GameObject stone = Instantiate(
                    m_stonePrefab,
                    transform.position + new Vector3(0f, 3f, 0f),
                    Quaternion.identity
                );

                stone.transform.localScale *= 0.3f;

                m_currentMaxTime = Mathf.Max(
                    0.5f,
                    m_currentMaxTime - m_timeDecrease
                );
            }

            m_spawnCoroutine = null;
        }

        private void OnDisable()
        {
            StopSpawning();
        }
    }
}

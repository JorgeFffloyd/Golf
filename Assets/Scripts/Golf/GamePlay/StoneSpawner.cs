
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Golf.GamePlay
{
    public class StoneSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private float m_currentMaxTime = 3f;
        [SerializeField] private float m_timeDecrease = 0.2f;

        [Header("Alternative Prefab")]
        [SerializeField] private GameObject m_alternativePrefab;

        [Range(0f, 1f)]
        [SerializeField] private float m_alternativeChance = 0.2f;

        [Header("Bomb")]
        [SerializeField] private GameObject m_bombPrefab;

        [Range(0f, 1f)]
        [SerializeField] private float m_bombChance = 0.1f;

        private float m_initialMaxTime;
        private GameObject m_stonePrefab;

        private Coroutine m_spawnCoroutine;
        private bool m_isRunning;

        private readonly List<GameObject> m_spawnedObjects =
            new List<GameObject>();

        private void Awake()
        {
            m_initialMaxTime = m_currentMaxTime;
            m_stonePrefab = Resources.Load<GameObject>("falling_stone");

            if (m_stonePrefab == null)
            {
                Debug.LogError(
                    "StoneSpawner: Не найден falling_stone в Resources!"
                );
            }
        }

        public void ResetSpawner()
        {
            StopSpawning();
            ClearSpawnedObjects();

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

        private void ClearSpawnedObjects()
        {
            foreach (GameObject spawnedObject in m_spawnedObjects)
            {
                if (spawnedObject != null)
                    Destroy(spawnedObject);
            }

            m_spawnedObjects.Clear();
        }

        private GameObject GetRandomPrefab()
        {
            float randomValue = Random.value;

            float bombChance = m_bombPrefab != null
                ? m_bombChance
                : 0f;

            float alternativeChance = m_alternativePrefab != null
                ? m_alternativeChance
                : 0f;

            if (randomValue < bombChance)
                return m_bombPrefab;

            if (randomValue < bombChance + alternativeChance)
                return m_alternativePrefab;

            return m_stonePrefab;
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

                GameObject prefabToSpawn = GetRandomPrefab();

                GameObject spawnedObject = Instantiate(
                    prefabToSpawn,
                    transform.position + new Vector3(0f, 3f, 0f),
                    Quaternion.identity
                );

                m_spawnedObjects.Add(spawnedObject);

                if (prefabToSpawn == m_stonePrefab)
                {
                    spawnedObject.transform.localScale *= 0.3f;
                }

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
            ClearSpawnedObjects();
        }
    }
}

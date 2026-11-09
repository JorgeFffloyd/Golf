using UnityEngine;
using System.Collections;

public class StoneSpawner : MonoBehaviour
{
    private float m_currentMaxTime = 3f;
    private float m_timeDecrease = 0.2f;
    private bool m_isRunning = false;
    private GameObject m_stonePrefab;

    private void Start()
    {
        m_stonePrefab = Resources.Load<GameObject>("falling_stone");

        m_isRunning = true;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (m_isRunning)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, m_currentMaxTime));

            if (!m_isRunning) yield break;

            GameObject newStone = Instantiate(m_stonePrefab, transform.position, Quaternion.identity);
            newStone.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            m_currentMaxTime -= m_timeDecrease;
            m_currentMaxTime = Mathf.Max(0.5f, m_currentMaxTime);
        }
    }

    public void StopSpawning()
    {
        m_isRunning = false;
        StopAllCoroutines();
    }
}
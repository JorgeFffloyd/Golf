using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : MonoBehaviour
{
    public GameObject fallingStoneTemplate;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("X");

            GameObject[] hills = GameObject.FindGameObjectsWithTag("Hill");
            Debug.Log("Найдено холмов: " + hills.Length);

            Transform cam = Camera.main.transform;
            GameObject nearestHill = null;
            float minDist = Mathf.Infinity;

            foreach (GameObject hill in hills)
            {
                float dist = Vector3.Distance(cam.position, hill.transform.position);
                Debug.Log("Холм: " + hill.name + ", расстояние: " + dist);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestHill = hill;
                }
            }

            Debug.Log("Ближайший холм: " + nearestHill.name);

            Vector3 spawnPos = nearestHill.transform.position + Vector3.up * 50f;
            spawnPos.x += Random.Range(-3f, 3f);
            Debug.Log("Позиция спавна: " + spawnPos);

            GameObject stone = Instantiate(fallingStoneTemplate, spawnPos, Quaternion.identity);
            Debug.Log("Камень создан: " + stone.name);}}}
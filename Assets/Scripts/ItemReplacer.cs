using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReplacer : MonoBehaviour
{
    public GameObject newItemPrefab;
    public string characterTag = "Character";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("=== Space нажат ===");

            if (newItemPrefab == null)
            { Debug.LogError("newItemPrefab = null!");
                return; }

            GameObject[] characters = GameObject.FindGameObjectsWithTag(characterTag);

            foreach (GameObject character in characters)
            {
                // Ищем рукки бальбо через рекурсию
                Transform hand = FindChild(character.transform, "B-hand_R");
                if (hand == null)
                {
                    Debug.LogWarning("У " + character.name + " нет B-hand_R!");
                    continue;
                }

                foreach (Transform child in hand)
                {
                    // отключение рендера
                    Renderer rend = child.GetComponent<Renderer>();
                    if (rend != null) rend.enabled = false;


                }
                //кручу грабли, но они всё равно кривые
                GameObject newItem = Instantiate(newItemPrefab, hand);
                newItem.transform.localPosition = Vector3.zero;
                newItem.transform.localRotation = Quaternion.Euler(0.041f, 0.291f, 85.044f);
                newItem.transform.localScale = Vector3.one;

                Debug.Log("Создан: " + newItem.name + " в руке " + character.name);
            } }
    }

    Transform FindChild(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindChild(child, name);
            if (result != null) return result;
        }
        return null; }}
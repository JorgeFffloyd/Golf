using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cloud : MonoBehaviour
{
    public GameObject rainCloud;           
    public List<Transform> characters;     
    public float moveDuration = 1.5f;      
    private int currentIndex = 0;
    private bool isMoving = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isMoving && characters.Count > 0)
        {
            currentIndex = (currentIndex + 1) % characters.Count;
            Transform target = characters[currentIndex];

            Vector3 targetPos = target.position + Vector3.up * 10f;

            StartCoroutine(MoveCloud(targetPos));}
    }

    IEnumerator MoveCloud(Vector3 targetPos)
    {
        isMoving = true;

        ParticleSystem rain = rainCloud.GetComponent<ParticleSystem>();
        if (rain != null) rain.Stop();
        float elapsed = 0f;
        Vector3 startPos = rainCloud.transform.position;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            rainCloud.transform.position = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;}

        rainCloud.transform.position = targetPos;

        if (rain != null) rain.Play();

        isMoving = false;}}
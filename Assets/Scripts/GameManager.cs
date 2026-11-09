using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool isGameOver { get; private set; } = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER!");

        StoneSpawner spawner = FindObjectOfType<StoneSpawner>();
        if (spawner != null) spawner.StopSpawning();
    }
}
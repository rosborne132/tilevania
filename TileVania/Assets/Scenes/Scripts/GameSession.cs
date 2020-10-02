using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;

    // This uses the singleton pattern. Meaning that only one 
    // session can exist at a time.
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1) { Destroy(gameObject); }
        else { DontDestroyOnLoad(gameObject); }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1) { TakeLife(); }
        else { ResetGameSession(); }
    }
}

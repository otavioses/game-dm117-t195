using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseBehavior : MonoBehaviour
{
    public static bool paused;

    [SerializeField]
    private GameObject menuPausePanel;

    /// <summary>
    /// Restart Scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ObstacleBehavior.velocidadeRolamento = 2.0f;
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    /// <param name="isPaused"></param>
    public void Pause(bool isPaused)
    {
        paused = isPaused;

        Time.timeScale = (paused) ? 0 : 1;

        menuPausePanel.SetActive(paused);
    }

    /// <summary>
    /// Load scence
    /// </summary>
    /// <param name="nameScene">Name of the scene to be loaded</param>
    public void loadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);

        menuPausePanel.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        //paused = false;
        Pause(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

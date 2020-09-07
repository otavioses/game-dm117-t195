using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Load scence
    /// </summary>
    /// <param name="nameScene">Name of the scene to be loaded</param>
    public void loadScene(string nameScene)
    {
        ObstacleBehavior.velocidadeRolamento = 0.10f;
 
        if (UnityAdController.showAds)
        {
            UnityAdController.ShowAd();
        }
        
        SceneManager.LoadScene(nameScene);
    }
}

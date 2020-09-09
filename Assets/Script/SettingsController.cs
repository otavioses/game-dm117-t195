using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    /// <summary>
    /// flag para controlar o mute/unmute
    /// </summary>
    public static bool muted = false;
    
    GameObject toogleMusic;

    // Start is called before the first frame update
    void Start()
    {
        toogleMusic = GameObject.Find("Music");
    }

    // Update is called once per frame
    void Update()
    {
        toogleMusic.GetComponent<Toggle>().isOn = !muted;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mute"></param>
    public void SetAudioMute(bool mute)
    {
        AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        for (int index = 0; index < sources.Length; ++index)
        {
            sources[index].mute = !mute;
        }
        muted = !mute;
    }

    /// <summary>
    /// carrega outra cena
    /// </summary>
    /// <param name="nameScene"></param>
    public void loadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}

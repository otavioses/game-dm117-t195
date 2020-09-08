using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static bool muted = false;
    // Start is called before the first frame update

    GameObject toogleMusic;
    void Start()
    {
        toogleMusic = GameObject.Find("Music");
    }

    // Update is called once per frame
    void Update()
    {
        toogleMusic.GetComponent<Toggle>().isOn = !muted;
    }
    public void Mute()
    {
        SetAudioMute(true);
    }

    public void Unmute()
    {
        SetAudioMute(false);
    }

    public void SetAudioMute(bool mute)
    {
        AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        for (int index = 0; index < sources.Length; ++index)
        {
            sources[index].mute = !mute;
        }
        muted = !mute;
    }

    public void loadSettingsScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}

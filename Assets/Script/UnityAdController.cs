using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Advertisements;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class UnityAdController : MonoBehaviour
{
    [Tooltip("Control to show adds")]
    public static bool showAds = true;

    public static DateTime? nextTimeReward;

    //Reference to obstacle
    public static ObstacleBehavior obstacle;

    public static void ShowAd()
    {
        ShowOptions options = new ShowOptions();

        options.resultCallback = Unpause;

        if (Advertisement.IsReady())
        {
            Advertisement.Show(options);
        }

        //Pauses the game while the ad is shown
        MenuPauseBehavior.paused = true;
        Time.timeScale = 0f;
    }

    public static void Unpause(ShowResult showResult)
    {
        //Unpause the game after the ad
        MenuPauseBehavior.paused = false;
        Time.timeScale = 1f;
    }

    public static void ShowRewardAd()
    {
        nextTimeReward = DateTime.Now.AddSeconds(15);

        if (Advertisement.IsReady())
        {
            MenuPauseBehavior.paused = true;
            Time.timeScale = 0f;


            var options = new ShowOptions
            {
                resultCallback = HanleAndShowResult
            };

            Advertisement.Show(options);
        }
    }

    public static void HanleAndShowResult(ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Finished:
                obstacle.Continue();
                break;
            case ShowResult.Skipped:
                Debug.LogError("Skipped");
                break;
            case ShowResult.Failed:
                Debug.LogError("Error");
                break;
        }

        MenuPauseBehavior.paused = false;
        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

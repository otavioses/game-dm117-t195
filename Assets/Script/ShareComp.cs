using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareComp : MonoBehaviour
{
	private string shareMessage;
	// Start is called before the first frame update
	void Start()
	{

	}

	private void ClickShareButton()
    {
		shareMessage = "Tenho a pontuação máxima";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, ss.EncodeToPNG());

		// To avoid memory leaks
		Destroy(ss);

		//new NativeShare().AddFile(filePath)
		//	.SetSubject("Rolando loucamente").SetText(shareMessage)
		//	.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
		//	.Share();

		// Share on WhatsApp only, if installed (Android only)
		//if( NativeShare.TargetExists( "com.whatsapp" ) )
		//	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
	}
}

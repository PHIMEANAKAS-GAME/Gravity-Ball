#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Welcome : EditorWindow
{
	private GUIStyle welcomeStyle = null;
	// Add menu item named "My Window" to the Window menu
	[MenuItem("Tools/GRAVITY BALL/FIRST START INFORMATIONS")]
	public static void Initialize () 
	{

		// Get existing open window or if none, make a new one:
		Welcome window = (Welcome)EditorWindow.GetWindow (typeof (Welcome), true, "PLEASE HAVE A LOOK TO THE DOCUMENTATION");


		GUIStyle style = new GUIStyle();

		window.position = new Rect(196, 196, sizeWidth, sizeHeight);
		window.minSize = new Vector2(sizeWidth, sizeHeight);
		window.maxSize = new Vector2(sizeWidth, sizeHeight);
		window.welcomeStyle = style;
		window.Show();

	}


	static float sizeWidth = 630;
	static float sizeHeight = 650;
	void OnGUI()
	{
		if(welcomeStyle == null)
			return;

		if (GUI.Button(new Rect(10, 10, 300, 150), "OPEN GRAVITY BALL DOCUMENTATION"))
		{
			Application.OpenURL("https://db.tt/zQba4syx");
		}

		if (GUI.Button(new Rect(10, 150 + 20, 300, 150), "OPEN ADS INTEGRATION DOCUMENTATION"))
		{
			Application.OpenURL("https://dl.dropboxusercontent.com/u/8289407/GravityBall/Documentation/_Ads_Integration_Documentation.pdf");
		}

		if (GUI.Button(new Rect(10, 300 + 30, 300, 150), "OPEN CLASSES DOCUMENTATION"))
		{
			Application.OpenURL("https://dl.dropboxusercontent.com/u/8289407/GravityBall/Documentation/html/annotated.html");
		}

		if (GUI.Button(new Rect(20 + 300, 10, 300, 150), "FORUM THREAD"))
		{
			Application.OpenURL("http://forum.unity3d.com/threads/gravity-ball-game-template-2d-dynamic-lights-and-shadows.373634/");
		}

		if (GUI.Button(new Rect(20 + 300, 150 + 20, 300, 150), "RATE THIS ASSET"))
		{
			Application.OpenURL("http://u3d.as/mns");
		}

		if (GUI.Button(new Rect(20 + 300, 300 + 30, 300, 150), "MORE ASSETS FROM US"))
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:8911");
		}

		if (GUI.Button(new Rect(10, 450 + 40, 610, 150), "DON'T PROMPT ME AGAIN"))
		{
			DoClose();
		}
	} 
	  
	void DoClose()
	{
		SetOpended();
		Close();
	}

	void SetOpended()
	{
		PlayerPrefs.SetInt("Documentation_Opened", 1);
		PlayerPrefs.Save();
	}
}

[InitializeOnLoad]
class StartupHelper
{
	static StartupHelper()
	{
		EditorApplication.update += Startup;
	}
	static void Startup()
	{


		if(Time.realtimeSinceStartup < 1)
			return;


		EditorApplication.update -= Startup;

		if (!PlayerPrefs.HasKey("Documentation_Opened"))
		{
			
			// Do your stuff here,
			//		Welcome window = (Welcome)EditorWindow.GetWindow (typeof (Welcome), true, "PLEASE HAVE A LOOK TO THE DOCUMENTATION");
			//		window.Show();
			//		window.Repaint();
			Welcome.Initialize();
		}
	}
} 

#endif
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Level Changer")]
public class SGT_LevelChanger : SGT_Singleton<SGT_LevelChanger>
{
	[SerializeField]
	private bool ignoreSceneZero;
	
	[SerializeField]
	private bool autoSwitch;
	
	/*[SerializeField]*/
	private bool removeCameraMessage;
	
	public bool IgnoreSceneZero
	{
		set
		{
			ignoreSceneZero = value;
		}
		
		get
		{
			return ignoreSceneZero;
		}
	}
	
	public bool AutoSwitch
	{
		set
		{
			autoSwitch = value;
		}
		
		get
		{
			return autoSwitch;
		}
	}
	
	new public void Awake()
	{
		base.Awake();
		
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(this);
	}
	
	public void Start()
	{
		if (removeCameraMessage == true)
		{
			RemoveMessages();
		}
		
		if (autoSwitch == true)
		{
			LoadLevel(Application.loadedLevel + 1);
		}
	}
	
	public void OnLevelWasLoaded()
	{
		if (removeCameraMessage == true)
		{
			RemoveMessages();
		}
	}
	
	public void OnGUI()
	{
		var rectSize = new Vector2((float)Screen.width * 0.1f, (float)Screen.height * 0.05f);
		
		var rect1 = new Rect(10, 50, rectSize.x, rectSize.y);
		var rect2 = new Rect(10, 10 + rect1.yMax, rectSize.x, rectSize.y);
		var rect3 = new Rect(10, 10 + rect2.yMax, rectSize.x, rectSize.y);
		
		if (GUI.Button(rect1, "Prev"))
		{
			LoadLevel(Application.loadedLevel - 1);
		}
		
		if (GUI.Button(rect2, "Next"))
		{
			LoadLevel(Application.loadedLevel + 1);
		}
		
		if (removeCameraMessage == false)
		{
			if (GUI.Button(rect3, "Hide Text"))
			{
				removeCameraMessage = true;
				
				RemoveMessages();
			}
		}
	}
	
	private void LoadLevel(int index)
	{
		var minIndex = ignoreSceneZero == true ? 1 : 0;
		
		if (index < minIndex)
		{
			index = Application.levelCount - 1;
		}
		
		if (index >= Application.levelCount)
		{
			index = minIndex;
		}
		
		Application.LoadLevel(index);
	}
	
	private void RemoveMessages()
	{
		if (Application.isPlaying == true)
		{
			var cameraMessages = FindObjectsOfType(typeof(SGT_CameraMessage)) as SGT_CameraMessage[];
			
			if (cameraMessages != null)
			{
				foreach (var cameraMessage in cameraMessages)
				{
					DestroyObject(cameraMessage);
				}
			}
		}
	}
}

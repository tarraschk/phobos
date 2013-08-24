using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Camera Message")]
public class SGT_CameraMessage : SGT_MonoBehaviour
{
	[SerializeField]
	private GUIStyle whiteStyle;
	
	[SerializeField]
	private GUIStyle blackStyle;
	
	[SerializeField]
	private Font font;
	
	[SerializeField]
	private string message = string.Empty;
	
	public Font Font
	{
		set
		{
			font = value;
		}
		
		get
		{
			return font;
		}
	}
	
	public string Message
	{
		set
		{
			message = value;
		}
		
		get
		{
			return message;
		}
	}
	
	public void OnGUI()
	{
		if (whiteStyle                  == null                  ) whiteStyle = new GUIStyle();
		if (whiteStyle.font             != font                  ) whiteStyle.font = font;
		if (whiteStyle.fontSize         != 20                    ) whiteStyle.fontSize = 20;
		if (whiteStyle.fontStyle        != FontStyle.Bold        ) whiteStyle.fontStyle = FontStyle.Bold;
		if (whiteStyle.wordWrap         != true                  ) whiteStyle.wordWrap = true;
		if (whiteStyle.alignment        != TextAnchor.LowerCenter) whiteStyle.alignment = TextAnchor.LowerCenter;
		if (whiteStyle.normal           == null                  ) whiteStyle.normal = new GUIStyleState();
		if (whiteStyle.normal.textColor != Color.white           ) whiteStyle.normal.textColor = Color.white;
		
		if (blackStyle                  == null                  ) blackStyle = new GUIStyle();
		if (blackStyle.font             != font                  ) blackStyle.font = font;
		if (blackStyle.fontSize         != 20                    ) blackStyle.fontSize = 20;
		if (blackStyle.fontStyle        != FontStyle.Bold        ) blackStyle.fontStyle = FontStyle.Bold;
		if (blackStyle.wordWrap         != true                  ) blackStyle.wordWrap = true;
		if (blackStyle.alignment        != TextAnchor.LowerCenter) blackStyle.alignment = TextAnchor.LowerCenter;
		if (blackStyle.normal           == null                  ) blackStyle.normal = new GUIStyleState();
		if (blackStyle.normal.textColor != Color.white           ) blackStyle.normal.textColor = Color.black;
		
		if (message == null) message = string.Empty;
		
		var sw   = (float)Screen.width;
		var sh   = (float)Screen.height;
		var rect = new Rect(sw * 0.025f, sh * (1.0f - 0.025f) - 100.0f, sw * 0.95f, 100.0f);
		
		rect.x += 1;
		GUI.Label(rect, message, blackStyle);
		
		rect.x -= 2;
		GUI.Label(rect, message, blackStyle);
		
		rect.x += 1;
		rect.y += 1;
		GUI.Label(rect, message, blackStyle);
		
		rect.y -= 2;
		GUI.Label(rect, message, blackStyle);
		
		rect.y += 1;
		GUI.Label(rect, message, whiteStyle);
	}
}

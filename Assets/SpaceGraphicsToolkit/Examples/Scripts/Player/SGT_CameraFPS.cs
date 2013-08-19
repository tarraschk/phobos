using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Camera FPS")]
public class SGT_CameraFPS : SGT_MonoBehaviour
{
	[SerializeField]
	private GUIStyle whiteStyle;
	
	[SerializeField]
	private GUIStyle blackStyle;
	
	[SerializeField]
	private Font font;
	
	/*[SerializeField]*/
	private float counter;
	
	/*[SerializeField]*/
	private int frames;
	
	/*[SerializeField]*/
	private float fps;
	
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
	
	public void OnGUI()
	{
		if (whiteStyle                  == null                 ) whiteStyle = new GUIStyle();
		if (whiteStyle.font             != font                 ) whiteStyle.font = font;
		if (whiteStyle.fontSize         != 20                   ) whiteStyle.fontSize = 20;
		if (whiteStyle.fontStyle        != FontStyle.Bold       ) whiteStyle.fontStyle = FontStyle.Bold;
		if (whiteStyle.alignment        != TextAnchor.UpperRight) whiteStyle.alignment = TextAnchor.UpperRight;
		if (whiteStyle.normal           == null                 ) whiteStyle.normal = new GUIStyleState();
		if (whiteStyle.normal.textColor != Color.white          ) whiteStyle.normal.textColor = Color.white;
		
		if (blackStyle                  == null                 ) blackStyle = new GUIStyle();
		if (blackStyle.font             != font                 ) blackStyle.font = font;
		if (blackStyle.fontSize         != 20                   ) blackStyle.fontSize = 20;
		if (blackStyle.fontStyle        != FontStyle.Bold       ) blackStyle.fontStyle = FontStyle.Bold;
		if (blackStyle.alignment        != TextAnchor.UpperRight) blackStyle.alignment = TextAnchor.UpperRight;
		if (blackStyle.normal           == null                 ) blackStyle.normal = new GUIStyleState();
		if (blackStyle.normal.textColor != Color.white          ) blackStyle.normal.textColor = Color.black;
		
		var sw   = (float)Screen.width;
		var sh   = (float)Screen.height;
		var rect = new Rect(sw * 0.025f, sh * 0.025f, sw * 0.95f, 30.0f);
		var text = "FPS: " + fps.ToString("0.0");
		
		rect.x += 1;
		GUI.Label(rect, text, blackStyle);
		
		rect.x -= 2;
		GUI.Label(rect, text, blackStyle);
		
		rect.x += 1;
		rect.y += 1;
		GUI.Label(rect, text, blackStyle);
		
		rect.y -= 2;
		GUI.Label(rect, text, blackStyle);
		
		rect.y += 1;
		GUI.Label(rect, text, whiteStyle);
	}
	
	public void Update()
	{
		counter += Time.deltaTime;
		frames  += 1;
		
		if (counter >= 1.0f)
		{
			fps = (float)frames / counter;
			
			counter = 0.0f;
			frames  = 0;
		}
	}
}

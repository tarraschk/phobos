using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Example/TimeScale Slider")]
public class SGT_TimeScaleSlider : MonoBehaviour
{
	public float minTimeScale = 0.001f;
	public float maxTimeScale = 10.0f;
	public float timeScale    = 1.0f;
	
	public void OnGUI()
	{
		var sw   = (float)Screen.width;
		var sh   = (float)Screen.height;
		var rect = new Rect(sw * 0.025f, sh * 0.025f, sw * 0.95f, 20.0f);
		
		timeScale = GUI.HorizontalSlider(rect, timeScale, minTimeScale, maxTimeScale);
		
		if (Mathf.Approximately(timeScale, Time.timeScale) == false)
		{
			Time.timeScale = timeScale;
		}
	}
}

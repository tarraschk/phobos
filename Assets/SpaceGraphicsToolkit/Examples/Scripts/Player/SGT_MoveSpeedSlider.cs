using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/MoveSpeed Slider")]
public class SGT_MoveSpeedSlider : MonoBehaviour
{
	public SGT_CameraMove cameraMove;
	public float          minSpeed = 1.0f;
	public float          maxSpeed = 100.0f;
	public float          speed    = 1.0f;
	
	public void OnGUI()
	{
		if (cameraMove != null)
		{
			if (Mathf.Approximately(speed, cameraMove.MoveSpeed) == false)
			{
				speed = Mathf.Clamp(cameraMove.MoveSpeed, minSpeed, maxSpeed);
			}
			
			var sw   = (float)Screen.width;
			var sh   = (float)Screen.height;
			var rect = new Rect(sw * 0.025f, sh * 0.025f, sw * 0.95f - 120, 20.0f);
			
			speed = GUI.HorizontalSlider(rect, speed, minSpeed, maxSpeed);
			
			if (Mathf.Approximately(speed, cameraMove.MoveSpeed) == false)
			{
				cameraMove.MoveSpeed = speed;
			}
		}
	}
}

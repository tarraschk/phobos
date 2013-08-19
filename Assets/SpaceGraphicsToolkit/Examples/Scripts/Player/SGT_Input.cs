using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Example/Input")]
public class SGT_Input : SGT_Singleton<SGT_Input>
{
	public bool hideWhenPlaying = true;
	
	/*[SerializeField]*/
	private static Vector3 oldMousePosition;
	
	/*[SerializeField]*/
	private static Vector3 newMousePosition;
	
	/*[SerializeField]*/
	private static Vector3 mouseDelta;
	
	/*[SerializeField]*/
	private static float screenSize;
	
	public static float ScreenSize
	{
		get
		{
			RequireInstance();
			
			return screenSize;
		}
	}
	
	public static Vector3 MousePositionXYZ
	{
		get
		{
			RequireInstance();
			
			return newMousePosition;
		}
	}
	
	public static Vector3 MousePositionXY
	{
		get
		{
			RequireInstance();
			
			return new Vector2(newMousePosition.x, newMousePosition.y);
		}
	}
	
	public static Vector3 MouseDeltaXYZ
	{
		get
		{
			RequireInstance();
			
			return mouseDelta;
		}
	}
	
	public static Vector2 MouseDeltaXY
	{
		get
		{
			RequireInstance();
			
			return new Vector2(mouseDelta.x, mouseDelta.y);
		}
	}
	
	public static float DragRoll
	{
		get
		{
			RequireInstance();
			
#if UNITY_IPHONE == true || UNITY_ANDROID == true
			return 0.0f; // TODO: Code this
#else
			var a = new Vector2(newMousePosition.x - (float)(Screen.width / 2), newMousePosition.y - (float)(Screen.height / 2));
			var b = a + new Vector2(mouseDelta.x, mouseDelta.y);
			
			return Mathf.DeltaAngle(Mathf.Atan2(a.x, a.y) * Mathf.Rad2Deg, Mathf.Atan2(b.x, b.y) * Mathf.Rad2Deg);
#endif
		}
	}
	
	public static float DragX
	{
		get
		{
			RequireInstance();
			
#if UNITY_IPHONE == true || UNITY_ANDROID == true
			var touches = Input.touches;
			
			if (touches != null && Input.touchCount == 1)
			{
				var touch = touches[0];
				
				if (touch.phase == TouchPhase.Moved)
				{
					return touch.deltaPosition.x * touch.deltaTime / screenSize;
				}
			}
			
			return 0.0f;
#else
			return UnityEngine.Input.GetAxis("Mouse X") / screenSize;
#endif
		}
	}
	
	public static float DragY
	{
		get
		{
			RequireInstance();
			
#if UNITY_IPHONE == true || UNITY_ANDROID == true
			var touches = Input.touches;
			
			if (touches != null && Input.touchCount == 1)
			{
				var touch = touches[0];
				
				if (touch.phase == TouchPhase.Moved)
				{
					return touch.deltaPosition.y * touch.deltaTime / screenSize;
				}
			}
			
			return 0.0f;
#else
			return UnityEngine.Input.GetAxis("Mouse Y") / screenSize;
#endif
		}
	}
	
	public static float Zoom
	{
		get
		{
#if UNITY_IPHONE == true || UNITY_ANDROID == true
			return 0.0f;
#else
			return Input.GetAxis("Mouse ScrollWheel");
#endif
		}
	}
	
	public static float MoveX
	{
		get
		{
#if UNITY_IPHONE == true || UNITY_ANDROID == true
			return 0.0f;
#else
			return Input.GetAxisRaw("Horizontal");
#endif
		}
	}
	
	public static float MoveY
	{
		get
		{
#if UNITY_IPHONE == true || UNITY_ANDROID == true
			return 0.0f;
#else
			return Input.GetAxisRaw("Vertical");
#endif
		}
	}
	
	public static bool GetKeyDown(KeyCode code, int equivalentTouchCount = 0)
	{
#if UNITY_IPHONE == true || UNITY_ANDROID == true
		if (equivalentTouchCount > 0)
		{
			var touches = Input.touches;
			
			if (touches != null && touches.Length == equivalentTouchCount)
			{
				foreach (var touch in touches)
				{
					if (touch.phase == TouchPhase.Began)
					{
						return true;
					}
				}
			}
		}
		
		return false;
#else
		return Input.GetKeyDown(code);
#endif
	}
	
	public static bool GetKey(KeyCode code, int equivalentTouchCount = 0)
	{
#if UNITY_IPHONE == true || UNITY_ANDROID == true
		if (equivalentTouchCount > 0)
		{
			var touches = Input.touches;
			
			return touches != null && touches.Length == equivalentTouchCount;
		}
		
		return false;
#else
		return Input.GetKey(code);
#endif
	}
	
	public new void Awake()
	{
		base.Awake();
		
		oldMousePosition = Input.mousePosition;
		newMousePosition = Input.mousePosition;
		
		Update();
	}
	
	public void Update()
	{
		oldMousePosition = newMousePosition;
		newMousePosition = Input.mousePosition;
		
		mouseDelta = newMousePosition - oldMousePosition;
		screenSize = Mathf.Min(Screen.width, Screen.height);
		
#if UNITY_EDITOR == true
		if (hideWhenPlaying == true && Application.isPlaying == true)
		{
			SGT_Helper.HideGameObject(this);
		}
		else
		{
			SGT_Helper.ShowGameObject(this);
		}
#endif
	}
}
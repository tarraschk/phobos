using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Light Source")]
public class SGT_LightSource : MonoBehaviour
{
	/*[SerializeField]*/
	private static SGT_LightSource instance;
	
	public static SGT_LightSource Find()
	{
		if (instance != null)
		{
			return instance;
		}
		
		instance = SGT_Helper.Find<SGT_LightSource>();
		
		return instance;
	}
}
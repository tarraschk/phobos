using UnityEngine;
using UnityEditor;

public static class SGT_AuxWindowHelper
{
	public static Object instance;
}

public abstract class SGT_AuxWindow<T, U> : EditorWindow
	where U : SGT_AuxWindow<T, U>
{
	private T    target;
	private bool isAux;
	private bool isField;
	private bool close;
	
	public T Target
	{
		get
		{
			return target;
		}
	}
	
	public bool IsField
	{
		get
		{
			return isField;
		}
	}
	
	public static U Create(T target, bool isField)
	{
		EditorWindow.GetWindow<U>().Close();
		
		var window = EditorWindow.CreateInstance<U>();
		
		window.target  = target;
		window.isField = isField;
		window.title   = "Utility";
		
		window.OnSetupAuxWindow();
		
		window.position = new Rect(100, 100, window.minSize.x, window.minSize.y);
		window.ShowUtility();
		
		SGT_AuxWindowHelper.instance = window;
		
		//window.ShowAuxWindow(); - Doesn't seem to work on mac properly with the rest of my UI?
		EditorGUIUtility.keyboardControl = 0;
		
		return window;
	}
	
	public abstract void OnSetupAuxWindow();
	public abstract void OnInspector();
	
	public void OnGUI()
	{
		if (isAux == true)
		{
			if (target != null)
			{
				OnInspector();
				
				if (GUI.changed == true)
				{
					Repaint();
				}
			}
			else
			{
				close = true;
			}
		}
	}
	
	public void Update()
	{
		if (isAux == true)
		{
			if (close == true)
			{
				Close();
			}
		}
		else
		{
			ShowAuxWindow();
			
			isAux = true;
		}
	}
	
	public void OnLostFocus()
	{
		//close = true;
	}
}
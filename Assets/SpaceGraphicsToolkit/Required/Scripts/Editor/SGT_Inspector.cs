using ObjectList = System.Collections.Generic.List<UnityEngine.Object>;

using UnityEngine;
using UnityEditor;
using System.Reflection;

public abstract class SGT_Inspector<T> : Editor
	where T : SGT_MonoBehaviour
{
	public T Target
	{
		get
		{
			return (T)target;
		}
	}
	
	public T[] Targets
	{
		get
		{
			var tArray = new T[targets.Length];
			
			for (var i = 0; i < tArray.Length; i++)
			{
				tArray[i] = (T)targets[i];
			}
			
			return tArray;
		}
	}
	
	protected void SetAll(string propertyName)
	{
		var property = typeof(T).GetProperty(propertyName);
		
		if (property == null)
		{
			Debug.Log("Failed to find: " + propertyName);
		}
		
		var baseValue = property.GetValue(target, null);
		
		if (SGT_EditorGUI.FieldModified == true)
		{
			SGT_EditorGUI.FieldModified = false;
			
			foreach (var t in targets)
			{
				property.SetValue(t, baseValue, null);
			}
		}
		else
		{
			foreach (var t in targets)
			{
				if (t != null)
				{
					var propertyValue = property.GetValue(t, null);
					var nullCount     = (baseValue == null ? 1 : 0) + (propertyValue == null ? 1 : 0);
					
					if (nullCount == 1 || (nullCount == 0 && baseValue.GetHashCode() != propertyValue.GetHashCode()))
					{
						var rect = SGT_EditorGUI.Reserve(28.0f);
						
						EditorGUI.HelpBox(rect, "The above value differs among the selected objects. Modifying this will modify all of them.", MessageType.Warning);
						
						break;
					}
				}
			}
		}
	}
	
	public override void OnInspectorGUI()
	{
		var curEvent = Event.current;
		var snapshot = (curEvent.type != EventType.Repaint && curEvent.type != EventType.Layout) || SGT_AuxWindowHelper.instance != null;
		
		if (snapshot == true)
		{
			var undoTargets = new ObjectList();
			
			Target.BuildUndoTargets(undoTargets);
			
			Undo.SetSnapshotTarget(undoTargets.ToArray(), "Change to " + Target.name);
			Undo.CreateSnapshot();
		}
		
		SGT_EditorGUI.ResetAll();
		
		OnInspector();
		
		if (SGT_EditorGUI.InspectorModified == true)
		{
			SGT_EditorGUI.InspectorModified = false;
			
			if (snapshot == true)
			{
				Undo.RegisterSnapshot();
				
				EditorUtility.SetDirty(target);
			}
			
			Repaint();
		}
	}
	
	public abstract void OnInspector();
}
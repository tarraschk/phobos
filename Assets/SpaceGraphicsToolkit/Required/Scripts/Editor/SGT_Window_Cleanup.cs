using UnityEngine;
using UnityEditor;

public class SGT_Window_Cleanup : SGT_Window<SGT_Window_Cleanup>
{
	[MenuItem("Component/Space Graphics Toolkit/Cleanup")]
	public static void Create()
	{
		CreateWithTitle("Cleanup");
	}
	
	public override void OnInspector()
	{
		var mbs = (SGT_MonoBehaviour[])Object.FindSceneObjectsOfType(typeof(SGT_MonoBehaviour));
		
		SGT_EditorGUI.HelpBox("This tool allows you to cleanup any hidden game objects that have been orphaned by SGT.", MessageType.Info);
		SGT_EditorGUI.HelpBox("If a game object is still visible after showing it, then it's still in use by SGT.", MessageType.Info);
		
		SGT_EditorGUI.Separator();
		
		if (SGT_EditorGUI.Button("Show All Hidden Game Objects") == true)
		{
			foreach (var mb in mbs)
			{
				if (mb != null)
				{
					foreach (Transform t in mb.transform)
					{
						if (Check(t) == true)
						{
							t.hideFlags = 0;
						}
					}
				}
			}
			
			Repaint();
		}
		
		SGT_EditorGUI.Separator();
		
		if (SGT_EditorGUI.Button("Delete All Hidden Game Objects") == true)
		{
			foreach (var mb in mbs)
			{
				if (mb != null)
				{
					foreach (Transform t in mb.transform)
					{
						if (Check(t) == true)
						{
							SGT_Helper.DestroyGameObject(t);
						}
					}
				}
			}
			
			Repaint();
		}
		
		SGT_EditorGUI.Separator();
		
		var totalHiddenCount = 0;
		
		for (var i = 0; i < mbs.Length; i++)
		{
			var mb = mbs[i];
			
			if (mb != null)
			{
				var hiddenCount = 0;
				
				foreach (Transform t in mb.transform)
				{
					if (Check(t) == true)
					{
						hiddenCount      += 1;
						totalHiddenCount += 1;
					}
				}
				
				if (hiddenCount > 0)
				{
					SGT_EditorGUI.BeginGroup("SGT Component (" + mb.name + ")");
					{
						for (var j = 0; j < mb.transform.childCount; j++)
						{
							var t = mb.transform.GetChild(j);
							
							if (Check(t) == true)
							{
								if (SGT_EditorGUI.TextWithButton("Hidden GameObject (" + t.name + ")", "Show", 50.0f) == true)
								{
									t.hideFlags = 0;
									
									GUI.changed = true;
									EditorUtility.SetDirty(mb);
									EditorApplication.RepaintHierarchyWindow();
									Repaint();
								}
							}
						}
					}
					SGT_EditorGUI.EndGroup();
					
					SGT_EditorGUI.Separator();
				}
			}
		}
		
		if (totalHiddenCount == 0)
		{
			SGT_EditorGUI.HelpBox("This scene contains no hidden children of SGT components.", MessageType.Info);
		}
	}
	
	private bool Check(Transform t)
	{
		if (t != null && t.hideFlags != 0)
		{
			return true;
		}
		
		return false;
	}
}
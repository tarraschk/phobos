using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SGT_SurfaceTessellatorCollider))]
public class SGT_Inspector_SurfaceTessellatorCollider : SGT_Inspector<SGT_SurfaceTessellatorCollider>
{
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		SGT_EditorGUI.BeginGroup("Collider");
		{
			Target.PhysicsMaterial = SGT_EditorGUI.ObjectField("Physics Material", null, Target.PhysicsMaterial);
			Target.HighestLOD      = SGT_EditorGUI.IntField("Highest LOD", "The highest patch level from the SurfaceTessellator that will be used. Note: Higher values will result in lower performance.", Target.HighestLOD);
			Target.VerticesPerMesh = SGT_EditorGUI.IntField("Vertices Per Mesh", "The maximum amount of verices that will be copied into each MeshCollider.", Target.VerticesPerMesh);
		}
		SGT_EditorGUI.EndGroup();
		
		SGT_EditorGUI.Separator();
	}
}
using ShaderList = System.Collections.Generic.List<UnityEngine.Shader>;

using UnityEngine;
using UnityEditor;

public class SGT_Window_ShadersInBuild : SGT_Window<SGT_Window_ShadersInBuild>
{
	public bool            buildAsteroidRing;
	public SGT_ShaderUsage buildAsteroidRingShadow;
	public SGT_ShaderUsage buildAsteroidRingSpin;
	
	public bool            buildCorona;
	public SGT_ShaderUsage buildCoronaCullNear;
	public SGT_ShaderUsage buildCoronaPerPixel;
	public SGT_ShaderUsage buildCoronaRing;
	
	public bool buildFog;
	
	public bool            buildGasGiant;
	public SGT_ShaderUsage buildGasGiantPlanetShadow;
	public SGT_ShaderUsage buildGasGiantRingShadow;
	
	public bool            buildPlanet;
	public SGT_ShaderUsage buildPlanetAtmosphere;
	public SGT_ShaderUsage buildPlanetClouds;
	public SGT_ShaderUsage buildPlanetPlanetShadow;
	public SGT_ShaderUsage buildPlanetRingShadow;
	public SGT_ShaderUsage buildPlanetScattering;
	public SGT_ShaderUsage buildPlanetNormal;
	public SGT_ShaderUsage buildPlanetSpecular;
	public SGT_ShaderUsage buildPlanetDetail;
	
	public bool            buildRing;
	public SGT_ShaderUsage buildRingStretched;
	public SGT_ShaderUsage buildRingShadow;
	public SGT_ShaderUsage buildRingLit;
	public SGT_ShaderUsage buildRingScattering;
	
	public bool buildSkysphere;
	
	public bool            buildStar;
	public SGT_ShaderUsage buildStarPerPixel;
	
	public bool buildStarfield;
	
	public bool buildDust;
	
	public bool buildNebula;
	
	[MenuItem("Component/Space Graphics Toolkit/Shaders In Build")]
	public static void Create()
	{
		CreateWithTitle("Shaders In Build");
	}
	
	public override void OnInspector()
	{
		SGT_EditorGUI.Separator();
		
		if (SGT_EditorGUI.Button("Include All Shaders") == true)
		{
			MoveShaders(false);
		}
		
		if (SGT_EditorGUI.Button("Sync Shaders") == true)
		{
			MoveShaders(true);
		}
		
		SGT_EditorGUI.Separator();
		
		buildAsteroidRing = SGT_EditorGUI.BeginToggleGroup("Asteroid Ring", null, buildAsteroidRing);
		{
			buildAsteroidRingShadow = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Shadow", null, buildAsteroidRingShadow);
			buildAsteroidRingSpin   = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Spin"  , null, buildAsteroidRingSpin  );
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildCorona = SGT_EditorGUI.BeginToggleGroup("Corona", null, buildCorona);
		{
			buildCoronaCullNear = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Cull Near", null, buildCoronaCullNear);
			buildCoronaPerPixel = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Per Pixel", null, buildCoronaPerPixel);
			buildCoronaRing     = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Ring"     , null, buildCoronaRing    );
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildFog = SGT_EditorGUI.BeginToggleGroup("Fog (Volumetric Probe)", null, buildFog);
		{
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildGasGiant = SGT_EditorGUI.BeginToggleGroup("Gas Giant", null, buildGasGiant);
		{
			buildGasGiantPlanetShadow = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Planet Shadow", null, buildGasGiantPlanetShadow);
			buildGasGiantRingShadow   = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Ring Shadow"  , null, buildGasGiantRingShadow  );
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildPlanet = SGT_EditorGUI.BeginToggleGroup("Planet", null, buildPlanet);
		{
			buildPlanetAtmosphere = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Atmosphere", null, buildPlanetAtmosphere);
			
			SGT_EditorGUI.BeginIndent(buildPlanetAtmosphere != SGT_ShaderUsage.NeverEnabled);
			{
				buildPlanetScattering = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Scattering", null, buildPlanetScattering);
			}
			SGT_EditorGUI.EndIndent();
			
			buildPlanetNormal       = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Normal"       , null, buildPlanetNormal      );
			buildPlanetSpecular     = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Specular"     , null, buildPlanetSpecular    );
			buildPlanetPlanetShadow = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Planet Shadow", null, buildPlanetPlanetShadow);
			buildPlanetRingShadow   = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Ring Shadow"  , null, buildPlanetRingShadow  );
			buildPlanetDetail       = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Detail"       , null, buildPlanetDetail      );
			buildPlanetClouds       = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Clouds"       , null, buildPlanetClouds      );
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildRing = SGT_EditorGUI.BeginToggleGroup("Ring", null, buildRing);
		{
			buildRingStretched  = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Stretched" , null, buildRingStretched );
			buildRingShadow     = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Shadow"    , null, buildRingShadow    );
			buildRingLit        = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Lit"       , null, buildRingLit       );
			buildRingScattering = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Scattering", null, buildRingScattering);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildSkysphere = SGT_EditorGUI.BeginToggleGroup("Skysphere", null, buildSkysphere);
		{
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildStar = SGT_EditorGUI.BeginToggleGroup("Star", null, buildStar);
		{
			buildStarPerPixel = (SGT_ShaderUsage)SGT_EditorGUI.EnumField("Per Pixel", null, buildStarPerPixel);
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildStarfield = SGT_EditorGUI.BeginToggleGroup("Starfield", null, buildStarfield);
		{
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildDust = SGT_EditorGUI.BeginToggleGroup("Dust", null, buildDust);
		{
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
		
		buildNebula = SGT_EditorGUI.BeginToggleGroup("Nebula", null, buildNebula);
		{
		}
		SGT_EditorGUI.EndToggleGroup();
		
		SGT_EditorGUI.Separator();
	}
	
	private void MoveShaders(bool sync)
	{
		var dummyMaterial = new Material(Shader.Find("Diffuse"));
		
		UnityEditorInternal.InternalEditorUtility.SetupShaderMenu(dummyMaterial);
		
		var shaders = (Shader[])Resources.FindObjectsOfTypeAll(typeof(Shader));
		
		dummyMaterial = SGT_Helper.DestroyObject(dummyMaterial);
		
		var includedShaders = sync ? IncludedShaders() : null;
		
		foreach (var shader in shaders)
		{
			if (shader.name.Contains("Hidden/SGT/") == true)
			{
				var include = true;
				
				if (sync == true)
				{
					var index = includedShaders.IndexOf(shader);
					
					if (index != -1)
					{
						includedShaders.RemoveAt(index);
					}
					else
					{
						include = false;
					}
				}
				
				MoveShader(shader, include);
			}
		}
	}
	
	private void MoveShader(Shader s, bool include)
	{
		var oldPath = AssetDatabase.GetAssetPath(s);
		var newPath = "";
		
		if (include == true)
		{
			newPath = oldPath.Replace("SpaceGraphicsToolkit/Required/Shaders/ExcludedResources", "SpaceGraphicsToolkit/Required/Shaders/Resources");
		}
		else
		{
			newPath = oldPath.Replace("SpaceGraphicsToolkit/Required/Shaders/Resources", "SpaceGraphicsToolkit/Required/Shaders/ExcludedResources");
		}
		
		if (oldPath != newPath)
		{
			AssetDatabase.MoveAsset(oldPath, newPath);
		}
	}
	
	public ShaderList IncludedShaders()
	{
		var shaderList = new ShaderList();
		
		if (buildAsteroidRing == true)
		{
			int minA; int maxA; UsageRange(buildAsteroidRingShadow, out minA, out maxA);
			int minB; int maxB; UsageRange(buildAsteroidRingSpin  , out minB, out maxB);
			
			for (var a = minA; a <= maxA; a++) {
			for (var b = minB; b <= maxB; b++) {
				var shaderName = "Variant";
				
				if (a == 1) shaderName += "Shadow";
				if (b == 1) shaderName += "Spin";
				
				AddBuildList(shaderList, "AsteroidRing/" + shaderName);
			} }
		}
		
		if (buildCorona == true)
		{
			int minA; int maxA; UsageRange(buildCoronaCullNear, out minA, out maxA);
			int minB; int maxB; UsageRange(buildCoronaPerPixel, out minB, out maxB);
			int minC; int maxC; UsageRange(buildCoronaRing    , out minC, out maxC);
			
			for (var a = minA; a <= maxA; a++) {
			for (var b = minB; b <= maxB; b++) {
			for (var c = minC; c <= maxC; c++) {
				var shaderName = "Variant";
				
				if (a == 1) shaderName += "CullNear";
				if (b == 1) shaderName += "PerPixel";
				if (c == 1) shaderName += "Ring";
				
				AddBuildList(shaderList, "Corona/" + shaderName);
			} } }
		}
		
		if (buildFog == true)
		{
			var shaderName = "Variant";
			
			AddBuildList(shaderList, "Fog/" + shaderName);
		}
		
		if (buildGasGiant == true)
		{
			int minB; int maxB; UsageRange(buildGasGiantPlanetShadow, out minB, out maxB);
			int minC; int maxC; UsageRange(buildGasGiantRingShadow  , out minC, out maxC);
			
			for (var a = 0   ; a <= 1   ; a++) {
			for (var b = minB; b <= maxB; b++) {
			for (var c = minC; c <= maxC; c++) {
				var shaderName = "Variant";
				
				if (a == 1) shaderName += "Outer";
				if (b == 1) shaderName += "PlanetShadow";
				if (c == 1) shaderName += "RingShadow";
				
				if (b == 1 && c == 1) continue;
				
				AddBuildList(shaderList, "GasGiant/" + shaderName);
			} } }
		}
		
		if (buildPlanet == true)
		{
			// Surface
			{
				int minA; int maxA; UsageRange(buildPlanetAtmosphere  , out minA, out maxA);
				int minC; int maxC; UsageRange(buildPlanetScattering  , out minC, out maxC);
				int minD; int maxD; UsageRange(buildPlanetNormal      , out minD, out maxD);
				int minE; int maxE; UsageRange(buildPlanetSpecular    , out minE, out maxE);
				int minF; int maxF; UsageRange(buildPlanetPlanetShadow, out minF, out maxF);
				int minG; int maxG; UsageRange(buildPlanetRingShadow  , out minG, out maxG);
				int minH; int maxH; UsageRange(buildPlanetDetail      , out minH, out maxH);
				
				for (var a = minA; a <= maxA; a++) {
				for (var b = 0   ; b <= 1   ; b++) {
				for (var c = minC; c <= maxC; c++) {
				for (var d = minD; d <= maxD; d++) {
				for (var e = minE; e <= maxE; e++) {
				for (var f = minF; f <= maxF; f++) {
				for (var g = minG; g <= maxG; g++) {
				for (var h = minH; h <= maxH; h++) {
					var shaderName = "Variant";
					
					if (a == 1) shaderName += "Atmosphere";
					if (b == 1) shaderName += "Outer";
					if (c == 1) shaderName += "Scattering";
					if (d == 1) shaderName += "Normal";
					if (e == 1) shaderName += "Specular";
					if (f == 1) shaderName += "PlanetShadow";
					if (g == 1) shaderName += "RingShadow";
					if (h == 1) shaderName += "Detail";
					
					if (c == 1) continue; // Currently disabled
					
					if (f == 1 && g == 1) continue;
					if (c == 1 && a == 0) continue;
					if (b == 1 && a == 0) continue;
					
					AddBuildList(shaderList, "PlanetSurface/" + shaderName);
				} } } } } } } }
			}
			
			// Atmosphere
			if (buildPlanetAtmosphere != SGT_ShaderUsage.NeverEnabled)
			{
				int minB; int maxB; UsageRange(buildPlanetScattering  , out minB, out maxB);
				int minC; int maxC; UsageRange(buildPlanetPlanetShadow, out minC, out maxC);
				int minD; int maxD; UsageRange(buildPlanetRingShadow  , out minD, out maxD);
				
				for (var a = 0   ; a <= 1   ; a++) {
				for (var b = minB; b <= maxB; b++) {
				for (var c = minC; c <= maxC; c++) {
				for (var d = minD; d <= maxD; d++) {
					var shaderName = "Variant";
					
					if (a == 1) shaderName += "Outer";
					if (b == 1) shaderName += "Scattering";
					if (c == 1) shaderName += "PlanetShadow";
					if (d == 1) shaderName += "RingShadow";
					
					if (c == 1 && d == 1) continue;
					
					AddBuildList(shaderList, "PlanetAtmosphere/" + shaderName);
				} } } }
			}
			
			// Clouds
			if (buildPlanetClouds != SGT_ShaderUsage.NeverEnabled)
			{
				int minA; int maxA; UsageRange(buildPlanetPlanetShadow, out minA, out maxA);
				int minB; int maxB; UsageRange(buildPlanetRingShadow  , out minB, out maxB);
				
				for (var a = minA; a <= maxA; a++) {
				for (var b = minB; b <= maxB; b++) {
					var shaderName = "Variant";
					
					if (a == 1) shaderName += "PlanetShadow";
					if (b == 1) shaderName += "RingShadow";
					
					if (a == 1 && b == 1) continue;
					
					AddBuildList(shaderList, "PlanetClouds/" + shaderName);
				} }
			}
		}
		
		if (buildRing == true)
		{
			int minA; int maxA; UsageRange(buildRingStretched , out minA, out maxA);
			int minB; int maxB; UsageRange(buildRingShadow    , out minB, out maxB);
			int minC; int maxC; UsageRange(buildRingLit       , out minC, out maxC);
			int minD; int maxD; UsageRange(buildRingScattering, out minD, out maxD);
			
			for (var a = minA; a <= maxA; a++) {
			for (var b = minB; b <= maxB; b++) {
			for (var c = minC; c <= maxC; c++) {
			for (var d = minD; d <= maxD; d++) {
				var shaderName = "Variant";
				
				if (a == 1) shaderName += "Stretched";
				if (b == 1) shaderName += "Shadow";
				if (c == 1) shaderName += "Lit";
				if (d == 1) shaderName += "Scattering";
				
				AddBuildList(shaderList, "Ring/" + shaderName);
			} } } }
		}
		
		if (buildSkysphere == true)
		{
			var shaderName = "Variant";
			
			AddBuildList(shaderList, "Skysphere/" + shaderName);
		}
		
		if (buildStar == true)
		{
			// Surface
			{
				int minB; int maxB; UsageRange(buildStarPerPixel, out minB, out maxB);
				
				for (var a = 0   ; a <= 1   ; a++) {
				for (var b = minB; b <= maxB; b++) {
					var shaderName = "Variant";
					
					if (a == 1) shaderName += "Outer";
					if (b == 1) shaderName += "PerPixel";
					
					AddBuildList(shaderList, "StarSurface/" + shaderName);
				} }
			}
			
			// Atmosphere
			{
				for (var a = 0   ; a <= 1   ; a++) {
					var shaderName = "Variant";
					
					if (a == 1) shaderName += "Outer";
					
					AddBuildList(shaderList, "StarAtmosphere/" + shaderName);
				}
			}
		}
		
		if (buildStarfield == true)
		{
			var shaderName = "Variant";
			
			AddBuildList(shaderList, "Starfield/" + shaderName);
		}
		
		if (buildDust == true)
		{
			AddBuildList(shaderList, "Dust/Additive");
			AddBuildList(shaderList, "Dust/Subtractive");
		}
		
		if (buildNebula == true)
		{
			AddBuildList(shaderList, "Nebula/Additive");
			AddBuildList(shaderList, "Nebula/Subtractive");
		}
		
		return shaderList;
	}
	
	private void UsageRange(SGT_ShaderUsage u, out int min, out int max)
	{
		min = 0;
		max = 0;
		
		switch (u)
		{
			case SGT_ShaderUsage.NeverEnabled:     min = 0; max = 0; break;
			case SGT_ShaderUsage.SometimesEnabled: min = 0; max = 1; break;
			case SGT_ShaderUsage.AlwaysEnabled:    min = 1; max = 1; break;
		}
	}
	
	private void AddBuildList(ShaderList shaderList, string shaderName)
	{
		var shader = Shader.Find("Hidden/SGT/" + shaderName);
		
		if (shader != null)
		{
			shaderList.Add(shader);
		}
		else
		{
			Debug.Log("Failed to find shader: " + shaderName);
		}
	}
}
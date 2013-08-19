using GameObjectList = System.Collections.Generic.List<UnityEngine.GameObject>;

using UnityEngine;

[AddComponentMenu("Space Graphics Toolkit/Example/Procedural Star System")]
public class SGT_ProceduralStarSystem : MonoBehaviour
{
	public Mesh      surfaceMesh;
	public Mesh      atmosphereMesh;
	public Texture[] planetTextures;
	public Texture[] gasGiantTextures;
	public Texture[] ringTextures;
	
	[SerializeField]
	[HideInInspector]
	private GameObjectList stuffInSystem = new GameObjectList();
	
	// This property will return a random texture stored in: planetTextures
	private Texture RandomPlanetTexture
	{
		get
		{
			if (planetTextures != null)
			{
				if (planetTextures.Length > 0)
				{
					return planetTextures[Random.Range(0, planetTextures.Length)];
				}
			}
			
			return null;
		}
	}
	
	// This property will return a random texture stored in: gasGiantTextures
	private Texture RandomGasGiantTexture
	{
		get
		{
			if (gasGiantTextures != null)
			{
				if (gasGiantTextures.Length > 0)
				{
					return gasGiantTextures[Random.Range(0, gasGiantTextures.Length)];
				}
			}
			
			return null;
		}
	}
	
	// This property will return a random texture stored in: ringTextures
	private Texture RandomRingTexture
	{
		get
		{
			if (ringTextures != null)
			{
				if (ringTextures.Length > 0)
				{
					return ringTextures[Random.Range(0, ringTextures.Length)];
				}
			}
			
			return null;
		}
	}
	
	// This property will return a random colour stored in the Color class.
	private Color RandomColour
	{
		get
		{
			switch (Random.Range(0, 7))
			{
				case 0: return Color.blue;
				case 1: return Color.cyan;
				case 2: return Color.gray;
				case 3: return Color.grey;
				case 4: return Color.magenta;
				case 5: return Color.red;
				case 6: return Color.white;
				case 7: return Color.yellow;
			}
			
			return Color.clear;
		}
	}
	
	// This function will draw the GUI buttons
	public void OnGUI()
	{
		var buttonWidth  = Mathf.Max(120.0f, (float)Screen.width  * 0.1f);
		var buttonHeight = Mathf.Max( 20.0f, (float)Screen.height * 0.05f);
		
		var rect1 = new Rect(10, 50, buttonWidth, buttonHeight);
		var rect2 = new Rect(10, 10 + rect1.yMax, buttonWidth, buttonHeight);
		var rect3 = new Rect(10, 10 + rect2.yMax, buttonWidth, buttonHeight);
		
		// Clicking this will delete any GameObjects stored in the stuffInSystem list
		if (GUI.Button(rect1, "Clear"))
		{
			foreach (var item in stuffInSystem)
			{
				Destroy(item.gameObject);
			}
			
			stuffInSystem.Clear();
		}
		
		// Clicking this will generate a planet
		if (GUI.Button(rect2, "Add Planet"))
		{
			// This will create a GameObject and add it to stuffInSystem
			var item = CreateItem("Procedural Planet");
			
			// This will add the SGT_Orbit component
			GenerateOrbit(item);
			
			// This will add the SGT_Planet component
			GeneratePlanet(item);
		}
		
		// Clicking this will generate a gas giant
		if (GUI.Button(rect3, "Add Gas Giant"))
		{
			// This will create a GameObject and add it to stuffInSystem
			var item = CreateItem("Procedural Gas Giant");
			
			// This will add the SGT_Orbit component
			GenerateOrbit(item);
			
			// This will add the SGT_GasGiant component
			GenerateGasGiant(item);
		}
	}
	
	private GameObject CreateItem(string newName)
	{
		// Spawn new GameObject
		var newItem = new GameObject(newName);
		
		// Parent it to this
		newItem.transform.parent = transform;
		
		// Randomise rotation
		newItem.transform.rotation = Random.rotation;
		
		// Add it to the system list
		stuffInSystem.Add(newItem);
		
		return newItem;
	}
	
	private void GenerateOrbit(GameObject item)
	{
		// Add orbit component
		var orbit = item.AddComponent<SGT_SimpleOrbit>();
		
		// Randomise orbit parameters
		orbit.Orbit         = true;
		orbit.OrbitAngle    = Random.Range(-Mathf.PI, Mathf.PI);
		orbit.OrbitDistance = Random.Range(30.0f, 200.0f);
		orbit.OrbitPeriod   = orbit.OrbitDistance * Random.Range(0.5f, 3.0f);
		
		// Randomise rotation parameters
		orbit.Rotation       = true;
		orbit.RotationPeriod = Random.Range(1.0f, 10.0f);
	}
	
	private void GeneratePlanet(GameObject item)
	{
		// Add planet component
		var planet = item.AddComponent<SGT_Planet>();
		
		// Randomise planet parameters
		planet.SurfaceMesh.ReplaceAll(surfaceMesh, 0);
		planet.SurfaceTextureDay.SetTexture(RandomPlanetTexture, 0);
		
		planet.SurfaceRadius = Random.Range(3.0f, 6.0f);
		
		// Modify lighting gradient
		var lighting = planet.PlanetLighting;
		lighting.AddColourNode(Color.black, Random.Range(0.0f , 0.45f));
		lighting.AddColourNode(Color.white, Random.Range(0.55f, 1.0f ));
		
		// Modify twilight colour
		var twilight = planet.AtmosphereTwilightColour;
		twilight.AddColourNode(RandomColour, 0.5f);
		twilight.AddAlphaNode(1.0f, Random.Range(0.0f , 0.45f));
		twilight.AddAlphaNode(0.0f, Random.Range(0.5f , 1.0f ));
		
		// 50% chance to add an atmosphere?
		if (RandomBool() == true)
		{
			planet.Atmosphere             = true;
			planet.AtmosphereMesh            = atmosphereMesh;
			planet.AtmosphereHeight          = Random.Range(1.0f, 3.0f);
			planet.AtmosphereFog             = Random.Range(0.0f, 0.25f);
			planet.AtmosphereFalloffInside   = Random.Range(0.01f, 0.25f);
		}
	}
	
	private void GenerateGasGiant(GameObject item)
	{
		// Add gas giant component
		var gasGiant = item.AddComponent<SGT_GasGiant>();
		
		// Randomise gas giant parameters
		gasGiant.GasGiantMesh             = atmosphereMesh;
		gasGiant.AtmosphereTextureDay     = RandomGasGiantTexture;
		gasGiant.AtmosphereRadius         = Random.Range(5.0f, 15.0f);
		gasGiant.AtmosphereDensity        = Random.Range(3.0f, 10.0f);
		gasGiant.AtmosphereDensityFalloff = Random.Range(1.0f, 10.0f);
		gasGiant.AtmosphereOblateness     = Random.Range(0.0f, 0.2f);
		
		// 50% chance to add a ring?
		if (RandomBool() == true)
		{
			// Add ring component
			var ring = item.AddComponent<SGT_Ring>();
			
			// Randomise ring parameters
			ring.RingTexture          = RandomRingTexture;
			ring.RingRadius           = gasGiant.AtmosphereRadius + Random.Range(8.0f, 16.0f);
			ring.RingWidth            = Random.Range(5.0f, 15.0f);
			ring.Shadow               = true;
			ring.ShadowRadius         = gasGiant.AtmosphereRadius;
			ring.ShadowWidth          = Random.Range(0.0f, 5.0f);
			ring.ShadowPenumbraColour = RandomColour;
		}
	}
	
	// This function will return true 50% of the time
	private bool RandomBool()
	{
		return Random.value > 0.5f;
	}
}
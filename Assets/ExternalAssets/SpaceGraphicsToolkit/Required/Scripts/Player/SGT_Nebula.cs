using ParticleList = System.Collections.Generic.List<SGT_Nebula.Particle>;

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Nebula")]
public class SGT_Nebula : SGT_MonoBehaviourUnique<SGT_Nebula>
{
	public enum Technique
	{
		Additive,
		Subtractive
	}
	
	public enum TextureHeightSource
	{
		Alpha,
		Red,
		Green,
		Blue,
		RGB,
		Min,
		Max
	}
	
	public struct Particle
	{
		public Vector3 Position;
		public float   Size;
		public float   Angle;
		public Color   Colour;
	}
	
	[SerializeField]
	private bool nebulaAutoRegen = true;
	
	[SerializeField]
	private bool nebulaModified = true;
	
	[SerializeField]
	private int nebulaSeed;
	
	[SerializeField]
	private int nebulaRenderQueue = 3000;
	
	[SerializeField]
	private Technique nebulaTechnique;
	
	[SerializeField]
	private Texture2D nebulaTexture;
	
	[SerializeField]
	private Material nebulaMaterial;
	
	[SerializeField]
	private float nebulaSize = 100.0f;
	
	[SerializeField]
	private int nebulaResolution = 5;
	
	[SerializeField]
	private bool nebulaMirror;
	
	[SerializeField]
	private TextureHeightSource heightSource = TextureHeightSource.Alpha;
	
	[SerializeField]
	private float heightScale = 1.0f;
	
	[SerializeField]
	private float heightOffset = 0.0f;
	
	[SerializeField]
	private bool heightInvert;
	
	[SerializeField]
	private float heightNoise = 0.0f;
	
	[SerializeField]
	private Texture particleTexture;
	
	[SerializeField]
	private float particleScale = 5.0f;
	
	[SerializeField]
	private Color particleColour = Color.white;
	
	[SerializeField]
	private float particleJitter = 0.0f;
	
	[SerializeField]
	private float particleFadeInDistance = 10.0f;
	
	[SerializeField]
	private GameObject nebula;
	
	[SerializeField]
	private int particleCount;
	
	[SerializeField]
	private Mesh generatedMesh;
	
	[SerializeField]
	private SGT_Mesh nebulaMesh;
	
	public bool NebulaModified
	{
		set
		{
			nebulaModified = value;
		}
		
		get
		{
			if (nebulaModified == false)
			{
				CheckForModifications();
			}
			
			return nebulaModified;
		}
	}
	
	public bool NebulaAutoRegen
	{
		set
		{
			nebulaAutoRegen = value;
		}
		
		get
		{
			return nebulaAutoRegen;
		}
	}
	
	public int NebulaRenderQueue
	{
		set
		{
			nebulaRenderQueue = value;
		}
		
		get
		{
			return nebulaRenderQueue;
		}
	}
	
	public int NebulaSeed
	{
		set
		{
			if (value != nebulaSeed)
			{
				nebulaSeed     = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return nebulaSeed;
		}
	}
	
	public Texture NebulaTexture
	{
		set
		{
			var v = value as Texture2D;
			
			if (v != nebulaTexture)
			{
				nebulaTexture  = v;
				nebulaModified = true;
			}
		}
		
		get
		{
			return nebulaTexture;
		}
	}
	
	public Technique NebulaTechnique
	{
		set
		{
			if (value != nebulaTechnique)
			{
				nebulaTechnique = value;
				
				SGT_Helper.DestroyObject(nebulaMaterial);
			}
		}
		
		get
		{
			return nebulaTechnique;
		}
	}
	
	public float NebulaSize
	{
		set
		{
			value = Mathf.Max(0.0f, value);
			
			if (value != nebulaSize)
			{
				nebulaSize     = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return nebulaSize;
		}
	}
	
	public int NebulaResolution
	{
		set
		{
			value = Mathf.Max(1, value);
			
			if (value != nebulaResolution)
			{
				nebulaResolution = value;
				nebulaModified   = true;
			}
		}
		
		get
		{
			return nebulaResolution;
		}
	}
	
	public bool NebulaMirror
	{
		set
		{
			if (value != nebulaMirror)
			{
				nebulaMirror   = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return nebulaMirror;
		}
	}
	
	public TextureHeightSource HeightSource
	{
		set
		{
			if (value != heightSource)
			{
				heightSource   = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return heightSource;
		}
	}
	
	public float HeightScale
	{
		set
		{
			if (value != heightScale)
			{
				heightScale    = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return heightScale;
		}
	}
	
	public float HeightOffset
	{
		set
		{
			if (value != heightOffset)
			{
				heightOffset   = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return heightOffset;
		}
	}
	
	public bool HeightInvert
	{
		set
		{
			if (value != heightInvert)
			{
				heightInvert   = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return heightInvert;
		}
	}
	
	public float HeightNoise
	{
		set
		{
			value = Mathf.Clamp01(value);
			
			if (value != heightNoise)
			{
				heightNoise  = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return heightNoise;
		}
	}
	
	public Texture ParticleTexture
	{
		set
		{
			particleTexture = value;
		}
		
		get
		{
			return particleTexture;
		}
	}
	
	public float ParticleScale
	{
		set
		{
			if (value != particleScale)
			{
				particleScale  = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return particleScale;
		}
	}
	
	public Color ParticleColour
	{
		set
		{
			particleColour = value;
		}
		
		get
		{
			return particleColour;
		}
	}
	
	public float ParticleJitter
	{
		set
		{
			value = Mathf.Clamp01(value);
			
			if (value != particleJitter)
			{
				particleJitter = value;
				nebulaModified = true;
			}
		}
		
		get
		{
			return particleJitter;
		}
	}
	
	public float ParticleFadeInDistance
	{
		set
		{
			particleFadeInDistance = Mathf.Max(0.0f, value);
		}
		
		get
		{
			return particleFadeInDistance;
		}
	}
	
	public void Awake()
	{
		if (ThisHasBeenDuplicated(new string[] { "nebulaMaterial", "generatedMesh" }) == true)
		{
			nebulaMaterial = null;
			generatedMesh  = null;
		}
	}
	
	public void LateUpdate()
	{
		Validate();
		                
		if (nebulaAutoRegen == true)
		{
			Regenerate();
		}
		
#if UNITY_EDITOR == true
		nebulaMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (nebulaMesh != null) nebulaMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (nebulaMesh != null) nebulaMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(nebula);
		SGT_Helper.DestroyObject(nebulaMaterial);
		SGT_Helper.DestroyObject(generatedMesh);
	}
	
	public void Regenerate()
	{
		if (nebulaModified == false)
		{
			CheckForModifications();
		}
		
		if (nebulaModified == true)
		{
			Validate();
			
			nebulaModified = false;
			
			var particles = new ParticleList();
			
			SGT_Helper.BeginRandomSeed(nebulaSeed);
			{
				for (var y = 0; y < nebulaResolution; y++)
				{
					for (var x = 0; x < nebulaResolution; x++)
					{
						if (nebulaTexture != null)
						{
							var u       = (float)x / (float)(nebulaResolution - 1);
							var v       = (float)y / (float)(nebulaResolution - 1);
							var noiseU  = Mathf.Clamp01(u + Random.Range(-heightNoise, heightNoise));
							var noiseV  = Mathf.Clamp01(v + Random.Range(-heightNoise, heightNoise));
							var jitterU = Mathf.Clamp01(u + Random.Range(-particleJitter, particleJitter));
							var jitterV = Mathf.Clamp01(v + Random.Range(-particleJitter, particleJitter));
							var colour  = nebulaTexture.GetPixelBilinear(noiseU, noiseV);
							var height  = 0.0f;
							var max     = Mathf.Max(Mathf.Max(colour.r, colour.g), colour.b);
							var rot     = Random.value * 360.0f;
							var offset  = heightOffset * nebulaSize;
							
							for (var i = nebulaMirror == true ? 0 : 1; i < 2; i++)
							{
								if (Random.value < max)
								{
									switch (heightSource)
									{
										case TextureHeightSource.Alpha: height = colour.a; break;
										case TextureHeightSource.Red:   height = colour.r; break;
										case TextureHeightSource.Green: height = colour.g; break;
										case TextureHeightSource.Blue:  height = colour.b; break;
										case TextureHeightSource.RGB:   height = (colour.r + colour.g + colour.b) / 3.0f; break;
										case TextureHeightSource.Min:   height = Mathf.Min(Mathf.Min(colour.r, colour.g), colour.b); break;
										case TextureHeightSource.Max:   height = max; break;
									}
									
									if (max > 0.0f)
									{
										var p    = new Particle();
										var posX = SGT_Helper.Remap(0.0f, 1.0f, jitterU, -nebulaSize, nebulaSize);
										var posZ = SGT_Helper.Remap(0.0f, 1.0f, jitterV, -nebulaSize, nebulaSize);
										var posY = (heightInvert == true ? 1.0f - height : height) * nebulaSize * heightScale + offset;
										
										p.Position = new Vector3(posX, i == 0 ? -posY : posY, posZ);
										p.Size     = (nebulaSize / (float)nebulaResolution) * particleScale * 2.0f;
										p.Colour   = colour;
										p.Angle    = rot;
										
										particles.Add(p);
									}
								}
							}
						}
					}
				}
			}
			SGT_Helper.EndRandomSeed();
			
			CreateMesh(particles);
		}
	}
	
	private void CreateMesh(ParticleList particles)
	{
		SGT_Helper.DestroyObject(generatedMesh);
		
		particleCount = particles.Count;
		generatedMesh = new Mesh();
		
		var positions = new Vector3[particleCount * 4];
		var indices   = new int[particleCount * 6];
		var uv0s      = new Vector2[particleCount * 4];
		var uv1s      = new Vector2[particleCount * 4];
		var normals   = new Vector3[particleCount * 4];
		var colours   = new Color[particleCount * 4];
		var bounds    = new Bounds();
		
		for (var i = 0; i < particleCount; i++)
		{
			var particle = particles[i];
			
			var i0 =  i * 6;
			var i1 = i0 + 1;
			var i2 = i1 + 1;
			var i3 = i2 + 1;
			var i4 = i3 + 1;
			var i5 = i4 + 1;
			
			var v0 =  i * 4;
			var v1 = v0 + 1;
			var v2 = v1 + 1;
			var v3 = v2 + 1;
			
			// Index data
			indices[i0] = v0;
			indices[i1] = v1;
			indices[i2] = v2;
			indices[i3] = v3;
			indices[i4] = v2;
			indices[i5] = v1;
			
			var right = SGT_Helper.Rotate(Vector2.right * SGT_Helper.InscribedBox, particle.Angle);
			var up    = SGT_Helper.Rotate(Vector2.up    * SGT_Helper.InscribedBox, particle.Angle);
			var uv1   = new Vector2(particle.Size * 0.5f, 0.0f);
			
			bounds.Encapsulate(particle.Position);
			
			// Write star values into vertex data
			positions[v0] = particle.Position;
			positions[v1] = particle.Position;
			positions[v2] = particle.Position;
			positions[v3] = particle.Position;
			
			normals[v0] = SGT_Helper.NewVector3(-right + up, 0.0f);
			normals[v1] = SGT_Helper.NewVector3( right + up, 0.0f);
			normals[v2] = SGT_Helper.NewVector3(-right - up, 0.0f);
			normals[v3] = SGT_Helper.NewVector3( right - up, 0.0f);
			
			colours[v0] = particle.Colour;
			colours[v1] = particle.Colour;
			colours[v2] = particle.Colour;
			colours[v3] = particle.Colour;
			
			uv0s[v0] = new Vector2(0.0f, 1.0f);
			uv0s[v1] = new Vector2(1.0f, 1.0f);
			uv0s[v2] = new Vector2(0.0f, 0.0f);
			uv0s[v3] = new Vector2(1.0f, 0.0f);
			
			uv1s[v0] = uv1;
			uv1s[v1] = uv1;
			uv1s[v2] = uv1;
			uv1s[v3] = uv1;
		}
		
		bounds.Expand(particleScale);
		
		generatedMesh.name      = "Nebula";
		generatedMesh.bounds    = bounds;
		generatedMesh.vertices  = positions;
		generatedMesh.normals   = normals;
		generatedMesh.colors    = colours;
		generatedMesh.uv        = uv0s;
		generatedMesh.uv1       = uv1s;
		generatedMesh.triangles = indices;
		
		nebulaMesh.SharedMesh = generatedMesh;
		nebulaMesh.Update();
	}
	
	private void Validate()
	{
		if (nebula         == null) nebula         = SGT_Helper.CreateGameObject("Nebula", this);
		if (nebulaMesh     == null) nebulaMesh     = new SGT_Mesh();
		if (nebulaMaterial == null) nebulaMaterial = SGT_Helper.CreateMaterial("Hidden/SGT/Nebula/" + nebulaTechnique);
		
		SGT_Helper.SetParent(nebula, gameObject);
		SGT_Helper.SetLayer(nebula, gameObject.layer);
		SGT_Helper.SetTag(nebula, gameObject.tag);
		
		var finalColour = SGT_Helper.Premultiply(particleColour);
		
		nebulaMaterial.SetColor("particleColour", finalColour);
		nebulaMaterial.SetTexture("particleTexture", particleTexture);
		nebulaMaterial.SetFloat("particleFadeInDistance", particleFadeInDistance);
		
		SGT_Helper.SetRenderQueue(nebulaMaterial, nebulaRenderQueue);
		
		nebulaMesh.GameObject          = nebula;
		nebulaMesh.HasMeshRenderer     = true;
		nebulaMesh.MeshRendererEnabled = SGT_Helper.IsBlack(finalColour) == false;
		nebulaMesh.SharedMesh          = generatedMesh;
		nebulaMesh.SharedMaterial      = nebulaMaterial;
		nebulaMesh.Update();
	}
	
	private void CheckForModifications()
	{
		if (nebula == null || generatedMesh == null)
		{
			nebulaModified = true;
		}
	}
}

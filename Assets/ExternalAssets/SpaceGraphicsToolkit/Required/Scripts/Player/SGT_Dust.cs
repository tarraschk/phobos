using ParticleList = System.Collections.Generic.List<SGT_Dust.Particle>;

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Dust")]
public class SGT_Dust : SGT_MonoBehaviourUnique<SGT_Dust>
{
	public enum Technique
	{
		Additive,
		Subtractive
	}
	
	public struct Particle
	{
		public Vector3 Position;
		public float   Size;
		public float   Angle;
		public Color   Colour;
	}
	
	[SerializeField]
	private bool dustAutoRegen = true;
	
	[SerializeField]
	private bool dustModified = true;
	
	[SerializeField]
	private int dustRenderQueue = 3000;
	
	[SerializeField]
	private int dustSeed;
	
	[SerializeField]
	private Technique dustTechnique;
	
	[SerializeField]
	private float dustRadius = 100.0f;
	
	[SerializeField]
	private int dustCount = 100;
	
	[SerializeField]
	private bool dustMirror;
	
	[SerializeField]
	private Camera dustCamera;
	
	[SerializeField]
	private Texture2D particleTexture;
	
	[SerializeField]
	private float particleScale = 0.5f;
	
	[SerializeField]
	private Color particleColour = Color.white;
	
	[SerializeField]
	private float particleFadeInDistance = 10.0f;
	
	[SerializeField]
	private float particleFadeOutDistance = 10.0f;
	
	[SerializeField]
	private GameObject dust;
	
	[SerializeField]
	private int particleCount;
	
	[SerializeField]
	private Mesh generatedMesh;
	
	[SerializeField]
	private SGT_Mesh dustMesh;
	
	[SerializeField]
	private Material dustMaterial;
	
	[SerializeField]
	private Quaternion dustCameraRotation;
	
	[SerializeField]
	private float particleRoll;
	
	public bool DustModified
	{
		set
		{
			dustModified = value;
		}
		
		get
		{
			if (dustModified == false)
			{
				CheckForModifications();
			}
			
			return dustModified;
		}
	}
	
	public bool DustAutoRegen
	{
		set
		{
			dustAutoRegen = value;
		}
		
		get
		{
			return dustAutoRegen;
		}
	}
	
	public int DustRenderQueue
	{
		set
		{
			dustRenderQueue = value;
		}
		
		get
		{
			return dustRenderQueue;
		}
	}
	
	public int DustSeed
	{
		set
		{
			if (value != dustSeed)
			{
				dustSeed     = value;
				dustModified = true;
			}
		}
		
		get
		{
			return dustSeed;
		}
	}
	
	public Technique DustTechnique
	{
		set
		{
			if (value != dustTechnique)
			{
				dustTechnique = value;
				
				SGT_Helper.DestroyObject(dustMaterial);
			}
		}
		
		get
		{
			return dustTechnique;
		}
	}
	
	public float DustRadius
	{
		set
		{
			value = Mathf.Max(0.0f, value);
			
			if (value != dustRadius)
			{
				dustRadius   = value;
				dustModified = true;
			}
		}
		
		get
		{
			return dustRadius;
		}
	}
	
	public int DustCount
	{
		set
		{
			value = Mathf.Clamp(value, 1, SGT_Helper.MeshVertexLimit / 4);
			
			if (value != dustCount)
			{
				dustCount    = value;
				dustModified = true;
			}
		}
		
		get
		{
			return dustCount;
		}
	}
	
	public Camera DustCamera
	{
		set
		{
			if (value != dustCamera)
			{
				dustCamera = value;
				
				if (dustCamera != null)
				{
					dustCameraRotation = dustCamera.transform.rotation;
				}
			}
		}
		
		get
		{
			return dustCamera;
		}
	}
	
	public Texture ParticleTexture
	{
		set
		{
			particleTexture = value as Texture2D;
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
				particleScale = value;
				dustModified  = true;
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
	
	public float ParticleFadeOutDistance
	{
		set
		{
			particleFadeOutDistance = Mathf.Max(0.0f, value);
		}
		
		get
		{
			return particleFadeOutDistance;
		}
	}
	
	public void Awake()
	{
		if (ThisHasBeenDuplicated(new string[] { "dustMaterial", "generatedMesh" }) == true)
		{
			dustMaterial  = null;
			generatedMesh = null;
		}
	}
	
	public void LateUpdate()
	{
		Validate();
		                
		if (dustAutoRegen == true)
		{
			Regenerate();
		}
		
		SGT_Helper.SetLocalScale(dust.transform, dustRadius * 2.0f);
		
#if UNITY_EDITOR == true
		dustMesh.HideInEditor();
#endif
	}
	
	public void OnEnable()
	{
		if (dustMesh != null) dustMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (dustMesh != null) dustMesh.OnDisable();
	}
	
	public new void OnDestroy()
	{
		base.OnDestroy();
		
		SGT_Helper.DestroyGameObject(dust);
		SGT_Helper.DestroyObject(dustMaterial);
		SGT_Helper.DestroyObject(generatedMesh);
	}
	
	public void Regenerate()
	{
		if (dustModified == false)
		{
			CheckForModifications();
		}
		
		if (dustModified == true)
		{
			Validate();
			
			dustModified = false;
			
			var particles = new ParticleList();
			
			SGT_Helper.BeginRandomSeed(dustSeed);
			{
				for (var y = 0; y < dustCount; y++)
				{
					var p    = new Particle();
					var posX = Random.Range(0.0f, 1.0f);
					var posY = Random.Range(0.0f, 1.0f);
					var posZ = Random.Range(0.0f, 1.0f);
					
					p.Position = new Vector3(posX, posY, posZ);
					p.Size     = particleScale;
					p.Colour   = Color.white * particleColour;
					p.Angle    = Random.value * 360.0f;
					
					particles.Add(p);
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
		
		generatedMesh.name      = "Dust";
		generatedMesh.vertices  = positions;
		generatedMesh.normals   = normals;
		generatedMesh.colors    = colours;
		generatedMesh.uv        = uv0s;
		generatedMesh.uv1       = uv1s;
		generatedMesh.triangles = indices;
		generatedMesh.bounds    = new Bounds(Vector3.zero, new Vector3(100000000.0f, 100000000.0f, 100000000.0f)); // NOTE: using infinity here causes an error
		
		dustMesh.SharedMesh = generatedMesh;
		dustMesh.Update();
	}
	
	private void Validate()
	{
		if (dust         == null) dust         = SGT_Helper.CreateGameObject("Dust", this);
		if (dustMesh     == null) dustMesh     = new SGT_Mesh();
		if (dustMaterial == null) dustMaterial = SGT_Helper.CreateMaterial("Hidden/SGT/Dust/" + dustTechnique);
		if (dustCamera   == null) dustCamera   = SGT_Helper.FindCamera();
		
		SGT_Helper.SetParent(dust, gameObject);
		SGT_Helper.SetLayer(dust, gameObject.layer);
		SGT_Helper.SetTag(dust, gameObject.tag);
		
		var finalColour           = SGT_Helper.Premultiply(particleColour);
		var oldDustCameraRotation = dustCameraRotation;
		
		if (dustCamera != null) dustCameraRotation = dustCamera.transform.rotation;
		
		var cameraRotationDelta = Quaternion.Inverse(oldDustCameraRotation) * dustCameraRotation;
		
		particleRoll -= cameraRotationDelta.eulerAngles.z;
		
		var roll = Quaternion.Euler(new Vector3(0.0f, 0.0f, particleRoll));
		
		dustMaterial.SetTexture("dustTexture", particleTexture);
		dustMaterial.SetFloat("dustRadius", dustRadius);
		dustMaterial.SetColor("particleColour", finalColour);
		dustMaterial.SetFloat("particleFadeInDistance", dustRadius / particleFadeInDistance);
		dustMaterial.SetFloat("particleFadeOutDistance", dustRadius / particleFadeOutDistance);
		dustMaterial.SetMatrix("particleRoll", SGT_MatrixHelper.Rotation(roll));
		
		SGT_Helper.SetRenderQueue(dustMaterial, dustRenderQueue);
		
		dustMesh.GameObject          = dust;
		dustMesh.HasMeshRenderer     = true;
		dustMesh.MeshRendererEnabled = SGT_Helper.IsBlack(finalColour) == false;
		dustMesh.SharedMesh          = generatedMesh;
		dustMesh.SharedMaterial      = dustMaterial;
		dustMesh.Update();
	}
	
	private void CheckForModifications()
	{
		if (dust == null || generatedMesh == null)
		{
			dustModified = true;
		}
	}
}

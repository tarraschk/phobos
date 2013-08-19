using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Space Graphics Toolkit/Thruster")]
public class SGT_Thruster : SGT_MonoBehaviour
{
	public enum ForceType
	{
		AddForce,
		AddForceAtPosition,
		AddRelativeForce
	}
	
	[SerializeField]
	private Camera thrusterObserver;
	
	[SerializeField]
	private bool thrusterFlame;
	
	[SerializeField]
	private GameObject thrusterFlameGameObject;
	
	[SerializeField]
	private bool thrusterFlare;
	
	[SerializeField]
	private GameObject thrusterFlareGameObject;
	
	[SerializeField]
	private float currentThrusterThrottle;
	
	[SerializeField]
	private float currentThrusterFlareScale;
	
	[SerializeField]
	private float targetThrusterThrottle;
	
	[SerializeField]
	private float thrusterTweenSpeed = 5.0f;
	
	[SerializeField]
	private bool thrusterPhysics;
	
	[SerializeField]
	private Rigidbody thrusterPhysicsRigidbody;
	
	[SerializeField]
	private float thrusterPhysicsForce;
	
	[SerializeField]
	private ForceType thrusterPhysicsForceType = ForceType.AddForceAtPosition;
	
	[SerializeField]
	private ForceMode thrusterPhysicsForceMode = ForceMode.Force;
	
	[SerializeField]
	private SGT_Mesh thrusterFlameMesh;
	
	[SerializeField]
	private Vector3 thrusterFlameOffset;
	
	[SerializeField]
	private Vector3 thrusterFlameScale;
	
	[SerializeField]
	private Vector3 thrusterFlameScaleChange = Vector3.one;
	
	[SerializeField]
	private float thrusterFlameScaleFlicker = 0.125f;
	
	[SerializeField]
	private SGT_Mesh thrusterFlareMesh;
	
	[SerializeField]
	private LayerMask thrusterFlareRaycastMask = -5;
	
	[SerializeField]
	private Vector3 thrusterFlareOffset;
	
	[SerializeField]
	private Vector3 thrusterFlareScale;
	
	[SerializeField]
	private Vector3 thrusterFlareScaleChange = Vector3.one;
	
	[SerializeField]
	private float thrusterFlareScaleFlicker = 0.125f;
	
	[SerializeField]
	private float thrusterFlareScaleTweenSpeed = 15.0f;
	
	public Camera ThrusterObserver
	{
		set
		{
			thrusterObserver = value;
		}
		
		get
		{
			return thrusterObserver;
		}
	}
	
	public float ThrusterThrottle
	{
		set
		{
			targetThrusterThrottle = Mathf.Clamp01(value);
		}
		
		get
		{
			return targetThrusterThrottle;
		}
	}
	
	public float ThrusterTweenSpeed
	{
		set
		{
			thrusterTweenSpeed = value;
		}
		
		get
		{
			return thrusterTweenSpeed;
		}
	}
	
	public bool ThrusterPhysics
	{
		set
		{
			thrusterPhysics = value;
		}
		
		get
		{
			return thrusterPhysics;
		}
	}
	
	public Rigidbody ThrusterPhysicsRigidbody
	{
		set
		{
			thrusterPhysicsRigidbody = value;
		}
		
		get
		{
			return thrusterPhysicsRigidbody;
		}
	}
	
	public float ThrusterPhysicsForce
	{
		set
		{
			thrusterPhysicsForce = value;
		}
		
		get
		{
			return thrusterPhysicsForce;
		}
	}
	
	public ForceType ThrusterPhysicsForceType
	{
		set
		{
			thrusterPhysicsForceType = value;
		}
		
		get
		{
			return thrusterPhysicsForceType;
		}
	}
	
	public ForceMode ThrusterPhysicsForceMode
	{
		set
		{
			thrusterPhysicsForceMode = value;
		}
		
		get
		{
			return thrusterPhysicsForceMode;
		}
	}
	
	public bool ThrusterFlame
	{
		set
		{
			thrusterFlame = value;
		}
		
		get
		{
			return thrusterFlame;
		}
	}
	
	public Mesh ThrusterFlameMesh
	{
		set
		{
			if (thrusterFlameMesh == null) thrusterFlameMesh = new SGT_Mesh();
			
			thrusterFlameMesh.SharedMesh = value;
		}
		
		get
		{
			return thrusterFlameMesh != null ? thrusterFlameMesh.SharedMesh : null;
		}
	}
	
	public Material ThrusterFlameMaterial
	{
		set
		{
			if (thrusterFlameMesh == null) thrusterFlameMesh = new SGT_Mesh();
			
			thrusterFlameMesh.SharedMaterial = value;
		}
		
		get
		{
			return thrusterFlameMesh != null ? thrusterFlameMesh.SharedMaterial : null;
		}
	}
	
	public Vector3 ThrusterFlameOffset
	{
		set
		{
			thrusterFlameOffset = value;
		}
		
		get
		{
			return thrusterFlameOffset;
		}
	}
	
	public Vector3 ThrusterFlameScale
	{
		set
		{
			thrusterFlameScale = value;
		}
		
		get
		{
			return thrusterFlameScale;
		}
	}
	
	public Vector3 ThrusterFlameScaleChange
	{
		set
		{
			thrusterFlameScaleChange = value;
		}
		
		get
		{
			return thrusterFlameScaleChange;
		}
	}
	
	public float ThrusterFlameScaleFlicker
	{
		set
		{
			thrusterFlameScaleFlicker = Mathf.Clamp01(value);
		}
		
		get
		{
			return thrusterFlameScaleFlicker;
		}
	}
	
	public bool ThrusterFlare
	{
		set
		{
			thrusterFlare = value;
		}
		
		get
		{
			return thrusterFlare;
		}
	}
	
	public Mesh ThrusterFlareMesh
	{
		set
		{
			if (thrusterFlareMesh == null) thrusterFlareMesh = new SGT_Mesh();
			
			thrusterFlareMesh.SharedMesh = value;
		}
		
		get
		{
			return thrusterFlareMesh != null ? thrusterFlareMesh.SharedMesh : null;
		}
	}
	
	public Material ThrusterFlareMaterial
	{
		set
		{
			if (thrusterFlareMesh == null) thrusterFlareMesh = new SGT_Mesh();
			
			thrusterFlareMesh.SharedMaterial = value;
		}
		
		get
		{
			return thrusterFlareMesh != null ? thrusterFlareMesh.SharedMaterial : null;
		}
	}
	
	public LayerMask ThrusterFlareRaycastMask
	{
		set
		{
			thrusterFlareRaycastMask = value;
		}
		
		get
		{
			return thrusterFlareRaycastMask;
		}
	}
	
	public Vector3 ThrusterFlareOffset
	{
		set
		{
			thrusterFlareOffset = value;
		}
		
		get
		{
			return thrusterFlareOffset;
		}
	}
	
	public Vector3 ThrusterFlareScale
	{
		set
		{
			thrusterFlareScale = value;
		}
		
		get
		{
			return thrusterFlareScale;
		}
	}
	
	public Vector3 ThrusterFlareScaleChange
	{
		set
		{
			thrusterFlareScaleChange = value;
		}
		
		get
		{
			return thrusterFlareScaleChange;
		}
	}
	
	public float ThrusterFlareScaleFlicker
	{
		set
		{
			thrusterFlareScaleFlicker = Mathf.Clamp01(value);
		}
		
		get
		{
			return thrusterFlareScaleFlicker;
		}
	}
	
	public float ThrusterFlareScaleTweenSpeed
	{
		set
		{
			thrusterFlareScaleTweenSpeed = value;
		}
		
		get
		{
			return thrusterFlareScaleTweenSpeed;
		}
	}
	
	public void OnEnable()
	{
		if (thrusterFlameMesh != null) thrusterFlameMesh.OnEnable();
		if (thrusterFlareMesh != null) thrusterFlareMesh.OnEnable();
	}
	
	public void OnDisable()
	{
		if (thrusterFlameMesh != null) thrusterFlameMesh.OnDisable();
		if (thrusterFlareMesh != null) thrusterFlareMesh.OnDisable();
	}
	
	public void OnDestroy()
	{
		SGT_Helper.DestroyGameObject(thrusterFlameGameObject);
		SGT_Helper.DestroyGameObject(thrusterFlareGameObject);
	}
	
	public void FixedUpdate()
	{
		if (thrusterPhysics == true)
		{
			if (thrusterPhysicsRigidbody == null) thrusterPhysicsRigidbody = SGT_Helper.GetComponentUpwards<Rigidbody>(gameObject);
			
			if (thrusterPhysicsRigidbody != null && targetThrusterThrottle != 0.0f)
			{
				var force = -transform.forward * thrusterPhysicsForce * targetThrusterThrottle * Time.fixedDeltaTime;
				
				switch (thrusterPhysicsForceType)
				{
					case ForceType.AddForce: thrusterPhysicsRigidbody.AddForce(force, thrusterPhysicsForceMode); break;
					case ForceType.AddForceAtPosition: thrusterPhysicsRigidbody.AddForceAtPosition(force, transform.position, thrusterPhysicsForceMode); break;
					case ForceType.AddRelativeForce: thrusterPhysicsRigidbody.AddRelativeForce(force, thrusterPhysicsForceMode); break;
				}
			}
		}
	}
	
	public void Start()
	{
		currentThrusterThrottle = targetThrusterThrottle;
	}
	
	public void LateUpdate()
	{
		if (thrusterObserver        == null) thrusterObserver        = SGT_Helper.FindCamera();
		if (thrusterFlameGameObject == null) thrusterFlameGameObject = SGT_Helper.CreateGameObject("Flame", gameObject);
		if (thrusterFlareGameObject == null) thrusterFlareGameObject = SGT_Helper.CreateGameObject("Flare", gameObject);
		
		SGT_Helper.SetParent(thrusterFlameGameObject, gameObject);
		SGT_Helper.SetLayer(thrusterFlameGameObject, gameObject.layer);
		SGT_Helper.SetTag(thrusterFlameGameObject, gameObject.tag);
		
		SGT_Helper.SetParent(thrusterFlareGameObject, gameObject);
		SGT_Helper.SetLayer(thrusterFlareGameObject, gameObject.layer);
		SGT_Helper.SetTag(thrusterFlareGameObject, gameObject.tag);
		
		if (thrusterPhysics == true && thrusterPhysicsRigidbody == null) thrusterPhysicsRigidbody = SGT_Helper.GetComponentUpwards<Rigidbody>(gameObject);
		
		var observerPosition = SGT_Helper.GetPosition(thrusterObserver);
		
		if (Application.isPlaying == true)
		{
			currentThrusterThrottle = Mathf.MoveTowards(currentThrusterThrottle, targetThrusterThrottle, thrusterTweenSpeed * Time.deltaTime);
		}
		else
		{
			currentThrusterThrottle = targetThrusterThrottle;
		}
		
		if (thrusterFlame == true)
		{
			if (thrusterFlameMesh == null) thrusterFlameMesh = new SGT_Mesh();
			
			// Offset flame
			SGT_Helper.SetLocalPosition(thrusterFlameGameObject.transform, thrusterFlameOffset);
			
			var finalFlameScale = thrusterFlameScale + thrusterFlameScaleChange * currentThrusterThrottle;
			
			// Hide/show flame
			if (finalFlameScale == Vector3.zero)
			{
				thrusterFlameMesh.MeshRendererEnabled = false;
			}
			else
			{
				if (Application.isPlaying == true)
				{
					finalFlameScale *= Random.Range(1.0f - thrusterFlameScaleFlicker, 1.0f);
				}
				
				thrusterFlameMesh.MeshRendererEnabled = true;
				
				SGT_Helper.SetLocalScale(thrusterFlameGameObject.transform, finalFlameScale);
				
				// Roll flame to observer
				var pointDir = transform.InverseTransformPoint(observerPosition);
				var roll     = Mathf.Atan2(pointDir.y, pointDir.x) * Mathf.Rad2Deg;
				
				SGT_Helper.SetRotation(thrusterFlameGameObject.transform, transform.rotation * Quaternion.Euler(0.0f, 0.0f, roll));
			}
			
			thrusterFlameMesh.GameObject      = thrusterFlameGameObject;
			thrusterFlameMesh.HasMeshRenderer = true;
			thrusterFlameMesh.Update();
		}
		else
		{
			if (thrusterFlameMesh != null) thrusterFlameMesh = thrusterFlameMesh.Clear();
		}
		
		if (thrusterFlare == true)
		{
			if (thrusterFlareMesh == null) thrusterFlareMesh = new SGT_Mesh();
			
			// Offset flare
			SGT_Helper.SetLocalPosition(thrusterFlareGameObject.transform, thrusterFlareOffset);
			
			// Flare visible?
			var a               = thrusterFlareGameObject.transform.position;
			var b               = observerPosition;
			var direction       = (b - a).normalized;
			var distance        = (b - a).magnitude;
			var targetFlareSize = 0.0f;
			
			// If the ray hits something, then hide the flare
			if (Physics.Raycast(a, direction, distance, thrusterFlareRaycastMask) == true)
			{
				targetFlareSize = 0.0f;
			}
			else
			{
				targetFlareSize = 1.0f;
			}
			
			// Point flare at observer
			if (thrusterObserver != null)
			{
				SGT_Helper.SetRotation(thrusterFlareGameObject.transform, thrusterObserver.transform.rotation);
			}
			
			// Fade flare in/out based on raycast
			if (Application.isPlaying == true)
			{
				currentThrusterFlareScale = Mathf.MoveTowards(currentThrusterFlareScale, targetFlareSize, thrusterFlareScaleTweenSpeed * Time.deltaTime);
			}
			else
			{
				currentThrusterFlareScale = targetFlareSize;
			}
			
			var finalFlareScale = currentThrusterFlareScale * (thrusterFlareScale + thrusterFlareScaleChange * currentThrusterThrottle);
			
			// Hide/show flare
			if (finalFlareScale == Vector3.zero)
			{
				thrusterFlareMesh.MeshRendererEnabled = false;
			}
			else
			{
				if (Application.isPlaying == true)
				{
					finalFlareScale *= Random.Range(1.0f - thrusterFlareScaleFlicker, 1.0f);
				}
				
				thrusterFlareMesh.MeshRendererEnabled = true;
				
				SGT_Helper.SetLocalScale(thrusterFlareGameObject.transform, finalFlareScale);
			}
			
			thrusterFlareMesh.GameObject      = thrusterFlareGameObject;
			thrusterFlareMesh.HasMeshRenderer = true;
			thrusterFlareMesh.Update();
		}
		else
		{
			if (thrusterFlareMesh != null) thrusterFlareMesh = thrusterFlareMesh.Clear();
		}
		
#if UNITY_EDITOR == true
		if (thrusterFlameMesh != null) thrusterFlameMesh.HideInEditor();
		if (thrusterFlareMesh != null) thrusterFlareMesh.HideInEditor();
		
		SGT_Helper.HideGameObject(thrusterFlameGameObject);
		SGT_Helper.HideGameObject(thrusterFlareGameObject);
#endif
	}
	
#if UNITY_EDITOR == true
	protected virtual void OnDrawGizmosSelected()
	{
		if (thrusterFlame == true)
		{
			var min = thrusterFlameScale * 2.0f;
			var max = thrusterFlameScale * 2.0f + thrusterFlameScaleChange * 2.0f;
			var f   = 1.0f - thrusterFlameScaleFlicker;
			
			SGT_Handles.Colour = new Color(1.0f, 1.0f, 0.0f, 0.25f); SGT_Handles.DrawSphere(transform.position, transform.rotation, min, min * f);
			SGT_Handles.Colour = new Color(1.0f, 1.0f, 0.0f, 0.5f); SGT_Handles.DrawSphere(transform.position, transform.rotation, max, max * f);
		}
		
		if (thrusterFlare == true)
		{
			var min = thrusterFlareScale * 2.0f;
			var max = thrusterFlareScale * 2.0f + thrusterFlareScaleChange * 2.0f;
			var f   = 1.0f - thrusterFlareScaleFlicker;
			
			SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.25f); SGT_Handles.DrawSphere(transform.position, transform.rotation, min, min * f);
			SGT_Handles.Colour = new Color(1.0f, 1.0f, 1.0f, 0.5f); SGT_Handles.DrawSphere(transform.position, transform.rotation, max, max * f);
		}
	}
#endif
	
	public void AddThrusterThrottle(float throttle)
	{
		targetThrusterThrottle = Mathf.Clamp01(targetThrusterThrottle + throttle);
	}
	
	public void SetThrusterThrottle(float throttle)
	{
		targetThrusterThrottle = Mathf.Clamp01(throttle);
	}
}
using UnityEngine;
using System.Collections;

public class Machinegun : Photon.MonoBehaviour
{
   

    public float range = 100.0f;
    public float fireRate = 0.2f;
    public float force = 10.0f;
    public float damage = 5.0f;
    public float bulletsPerClip = 40;
    public float clips = 10;
    public float reloadTime = 0.5f;
    private ParticleEmitter hitParticles;
    public Renderer muzzleFlash;

   public  bool localPlayer = false;
   public  string localPlayerName = "";

    private int bulletsLeft = 0;
    private float nextFireTime = 0.0f;
    private float m_LastFrameShot = -1;

    private string ammoText = "";

    public Transform mytrans;

    void Start()
    {
        mytrans = transform;
        localPlayerName = PlayerPrefs.GetString("playerName" + Application.platform);

        hitParticles = GetComponentInChildren<ParticleEmitter>() as ParticleEmitter;

        // We don't want to emit particles all the time, only when we hit something.
        if (hitParticles)
        {
            hitParticles.emit = false;
        }
        bulletsLeft = (int)bulletsPerClip;
        GetBulletsLeft();
    }

    void Update()
    {
        if (!localPlayer)
        {
            return;
        }

        // Did the user press fire?
        if (Input.GetButton("Fire1"))
        {
            Fire();
        }
    }

    void LateUpdate()
    {
        // We shot this frame, enable the muzzle flash
        if (m_LastFrameShot >= Time.frameCount - 3)
        {
            //muzzleFlash.transform.localRotation = Quaternion.AngleAxis(Random.value * 360, Vector3.forward);
            if (!localPlayer)
            {
                muzzleFlash.enabled = true;
            }

            if (audio)
            {
                if (!audio.isPlaying)
                    audio.Play();
                audio.loop = true;
            }
        }
        else
        {
            // We didn't, disable the muzzle flash
            muzzleFlash.enabled = false;

            // Play sound
            if (audio)
            {
                audio.loop = false;
            }
        }
    }
    void Fire()
    {
        if (bulletsLeft == 0)
        {
            return;
        }

        // If there is more than one bullet between the last and this frame
        // Reset the nextFireTime
        if (Time.time - fireRate > nextFireTime)
        {
            nextFireTime = Time.time - Time.deltaTime;
        }

        // Keep firing until we used up the fire time
        while (nextFireTime < Time.time && bulletsLeft != 0)
        {
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            RaycastHit hit;


            // Did we hit anything?
            if (Physics.Raycast(mytrans.position, direction, out hit, range))
            {
                FiredOneShot(true, hit.point, hit.normal);
                photonView.RPC("FiredOneShot", PhotonTargets.Others, false, hit.point, hit.normal);

                // Send a damage message to the hit object			
                string[] settingsArray = new string[2];
                settingsArray[0] = damage + "";
                settingsArray[1] = localPlayerName;

                hit.collider.SendMessage("ApplyDamage", settingsArray, SendMessageOptions.DontRequireReceiver);
            }

            bulletsLeft--;
            GetBulletsLeft();

            // Register that we shot this frame,
            // so that the LateUpdate function enabled the muzzleflash renderer for one frame
            m_LastFrameShot = Time.frameCount;
            //enabled = true;

            // Reload gun in reload Time		
            if (bulletsLeft == 0)
            {
                StartCoroutine(Reload());
            }

            nextFireTime += fireRate;

        }
    }
    [RPC]
    void FiredOneShot(bool shotOwner, Vector3 hitPoint, Vector3 hitNormal)
    {
        // Apply a force to the rigidbody we hit
        //if (hit.rigidbody){
        //	hit.rigidbody.AddForceAtPosition(force * direction, hit.point);
        //}

        // Place the particle system for spawing out of place where we hit the surface!
        // And spawn a couple of particles
        if (hitParticles)
        {
            hitParticles.transform.position = hitPoint;
            hitParticles.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitNormal);
            hitParticles.Emit();
        }

        m_LastFrameShot = Time.frameCount;

    }

    IEnumerator Reload()
    {
        // Wait for reload time first - then add more bullets!
        yield return new WaitForSeconds(reloadTime);

        // We have a clip left reload
        if (clips > 0)
        {
            clips--;
            bulletsLeft = (int)bulletsPerClip;
            if (localPlayer)
            {
                ammoText = bulletsLeft + "/" + (clips * bulletsPerClip);
            }
        }
    }

    void OnGUI()
    {
        if (localPlayer)
        {
            GUILayout.Space(20);
            GUILayout.Label("Ammo: " + ammoText);
        }
    }
    public int GetBulletsLeft()
    {
        ammoText = bulletsLeft + "/" + (clips * bulletsPerClip);
        return bulletsLeft;
    }
}
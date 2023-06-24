using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string weaponName = "Gun";
    public float damage = 10f;
    public float range = 100f;
    public float gunRecoil = 20f;
    public float screenRecoil = 10f;
    public float recoilResetTime = 15f;
    public float screenResetTime = 15f;
    public float fireRate = 15.0f;
    public float currentAmmo;
    public float magSize = 30f;
    public float explosionRadius = 0f;
    public float weaponSpread = 0.05f;
    public float weaponSpreadIncrease = 0.005f;
    public bool isAuto;
    public bool isExplosive;


    protected bool zeroAmmo;
    protected float drawAnimation = 0;
    protected bool currentlyReloading;
    protected float nextTimeToFire;
    public float animationDistance = 20f;
    public float weaponForce = 10f;
    protected GameManager gameManager;

    public ParticleSystem muzzleFlash;
    public Transform fpsCamera;
    public AudioSource playSound;
    public AudioClip fireSound;
    public AudioClip reload;
    public AudioClip reloadFinish;
    public AudioClip noAmmo;
    public GameObject hitImpact;
    public int playerLayer;
    protected int layerMask;
    public Transform gun;
    public Transform weaponHolder;
    public Transform playerCam;

    protected float gunSmoothingValue;
    protected float camSmoothingValue;
    protected float currentSpread = 0f;

    private void OnEnable()
    {
        currentlyReloading = true;
        muzzleFlash.Pause(true);
        playSound.clip = reloadFinish;
        playSound.Play();
        Invoke("LoadWeapon", 0.5f);
    }

    private void OnDisable()
    {
        weaponHolder.transform.localPosition = new Vector3(0f, -animationDistance, 0f);
        drawAnimation = -animationDistance;
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        drawAnimation = -animationDistance;
        weaponHolder.transform.localPosition = new Vector3(0f, -animationDistance, 0f);
        currentAmmo = magSize;
        screenRecoil = -screenRecoil;
        zeroAmmo = false;
        layerMask = 1 << playerLayer; // I still don't get why I have to bit shift to obtain the layer mask...
    }

    void Update()
    {
        if (currentAmmo == 0 && Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !currentlyReloading)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            zeroAmmo = true;
        } else
        {
            switch (isAuto)
            {
                case true:
                    if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !currentlyReloading && !zeroAmmo)
                    {
                        nextTimeToFire = Time.time + 1f / fireRate;
                        Shoot();
                    }
                    break;
                case false:
                    if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !currentlyReloading && !zeroAmmo)
                    {
                        nextTimeToFire = Time.time + 1f / fireRate;
                        Shoot();
                    }
                    break;
            }
        }

        if (!Input.GetButton("Fire1") && !Input.GetButtonDown("Fire1")) 
        {
            currentSpread = Mathf.Lerp(currentSpread, 0f, 1f * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.R) && !currentlyReloading && currentAmmo != magSize) 
        {
            playSound.clip = reload;
            playSound.Play();
            currentlyReloading = true;
            Invoke("Reload", 1f);
        }

        RecoilReset();
        DrawWeapon();
    }

    protected void DrawWeapon()
    {
        if (drawAnimation != 0)
        {
            drawAnimation = Mathf.Lerp(drawAnimation, 0, 15f * Time.deltaTime);
            weaponHolder.transform.localPosition = new Vector3(0f, drawAnimation, 0f);
        }
    }

    protected void LoadWeapon()
    {
        currentlyReloading = false;
    }

    protected void Reload()
    {
        currentAmmo = magSize;
        currentlyReloading = false;
        playSound.clip = reloadFinish;
        playSound.Play();
        gameManager.UpdateText();
        zeroAmmo = false;
    }

    public virtual void Shoot()
    {
        if (currentAmmo > 0)
        {
            muzzleFlash.Play();
            playSound.clip = fireSound;
            playSound.Play();
            RaycastHit hit;
            gunSmoothingValue = gunRecoil;
            Invoke("Recoil", 0.05f);

            float randomSpreadX = Random.Range(-currentSpread, currentSpread);
            float randomSpreadY = Random.Range(-currentSpread, currentSpread);

            currentSpread += weaponSpreadIncrease;
            currentSpread = Mathf.Clamp(currentSpread, 0, weaponSpread);

            if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + 
                new Vector3(randomSpreadX, randomSpreadY, 0f), 
                out hit, int.MaxValue, ~layerMask)) // use ~ to invert. If you want to hit ONLY said layer, then remove the ~
            {

                Enemy target = hit.transform.GetComponent<Enemy>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
                Instantiate(hitImpact, hit.point, Quaternion.LookRotation(hit.normal));

                if (isExplosive)
                {
                    Collider[] colliders = Physics.OverlapSphere(hit.point, explosionRadius);

                    foreach (Collider nearbyObject in colliders)
                    {
                        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                        if (rb != null) // BRO HOW MUCH IFS ARE YOU GONNA NEST????????? THAT'S ALREADY FOUR BRO
                        {
                            rb.AddExplosionForce(weaponForce, hit.point, explosionRadius);
                        }
                    }

                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * weaponForce);
                }
            }
            currentAmmo--;
            gameManager.UpdateText();
        } else
        {
            zeroAmmo = true;
            playSound.clip = noAmmo;
            playSound.Play();
        }
       
    }

    protected void Recoil()
    {
        camSmoothingValue += screenRecoil;
    }

    protected void RecoilReset()  
    {
        if (gunSmoothingValue > 0f)
        {
            gunSmoothingValue = Mathf.Lerp(gunSmoothingValue, 0f, recoilResetTime * Time.deltaTime);
            gun.transform.localRotation = Quaternion.Euler(0f, -90f, gunSmoothingValue);
        }
        
        if (camSmoothingValue < 0f)
        {
            camSmoothingValue = Mathf.Lerp(camSmoothingValue, 0f, screenResetTime * Time.deltaTime);
            playerCam.transform.localRotation = Quaternion.Euler(camSmoothingValue, 0f, 0f);
        }
        
    }

}

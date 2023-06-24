using UnityEngine;

public class Sniper : Gun
{
    public AudioClip chkchk;
    bool isScoped = false;
    public Camera scopeCamera;
    MovementController movementController;
    MouseController mouseController;
    public GameObject scope;

    private void OnDisable()
    {
        weaponHolder.transform.localPosition = new Vector3(0f, -animationDistance, 0f);
        drawAnimation = -animationDistance;
        Unscope();
        ForceRecoilOff();
    }

    private void Start()
    {
        currentAmmo = magSize;
        drawAnimation = -animationDistance;
        weaponHolder.transform.localPosition = new Vector3(0f, -animationDistance, 0f);
        screenRecoil = -screenRecoil;
        zeroAmmo = false;
        layerMask = 1 << playerLayer; // I still don't get why I have to bit shift to obtain the layer mask...
        movementController = MovementController.instance;
        mouseController = MouseController.instance;
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (gameManager.gameIsPaused)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !currentlyReloading && !zeroAmmo)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isScoped) 
            {
                Scope();
            } else
            {
                Unscope();
            }
        }

        if (Input.GetKey(KeyCode.R) && !currentlyReloading && currentAmmo != magSize)
        {
            effects.clip = reload;
            effects.Play();
            currentlyReloading = true;
            Invoke("Reload", 1f);
        }

        RecoilReset();
        DrawWeapon();
    }

    public override void Shoot()
    {
        if (currentAmmo > 0)
        {
            muzzleFlash.Play();
            playSound.clip = fireSound;
            playSound.Play();
            RaycastHit hit;
            gunSmoothingValue = gunRecoil;
            Invoke("Recoil", 0.05f);

            if (isScoped)
            {
                currentSpread = 0f;
            } else
            {
                currentSpread = 0.25f;
            }

            float randomSpreadX = Random.Range(-currentSpread, currentSpread);
            float randomSpreadY = Random.Range(-currentSpread, currentSpread);

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

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * weaponForce);
                }
            }
            currentAmmo--;
            gameManager.UpdateText();
            Invoke("Chkchk", 0.8f);
            if (isScoped)
            {
                Unscope();
            }
        }
        else
        {
            zeroAmmo = true;
            playSound.clip = noAmmo;
            playSound.Play();
        }
    }

    void Scope()
    {
        isScoped = true;
        movementController.isScoped = true; 
        scopeCamera.fieldOfView = 10f;
        scope.SetActive(true);
        gun.transform.localScale = new Vector3(0f, 0f, 0f);
        mouseController.mouseSensitivity *= 0.166666f;
    }

    void Unscope()
    {
        if (isScoped)
        {
            mouseController.mouseSensitivity /= 0.166666f;
        }
        isScoped = false;
        movementController.isScoped = false;
        scopeCamera.fieldOfView = 60f;
        scope.SetActive(false);
        gun.transform.localScale = new Vector3(1f, 1f, 1f);
        
    }

    void Chkchk()
    {
        effects.clip = chkchk;
        effects.Play();
        
    }

}

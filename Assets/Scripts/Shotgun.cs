using UnityEngine;

public class Shotgun : Gun
{
    public int numberOfPellets = 15;
    public AudioClip chkchk;

    public override void Shoot()
    {
        if (gameManager.gameIsPaused)
        {
            return;
        }
        if (currentAmmo > 0)
        {
            muzzleFlash.Play();
            playSound.clip = fireSound;
            playSound.Play();
            RaycastHit hit;
            gunSmoothingValue = gunRecoil;
            Invoke("Recoil", 0.05f);

            int i = 0;

            float[] randomSpreadX = new float[numberOfPellets];
            float[] randomSpreadY = new float[numberOfPellets];

            while (i != numberOfPellets)
            {
                randomSpreadX[i] = Random.Range(-weaponSpread, weaponSpread);
                randomSpreadY[i] = Random.Range(-weaponSpread, weaponSpread);
                i++;
            }

            i = 0;
            
            while (i != numberOfPellets) 
            {
                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward +
                new Vector3(randomSpreadX[i], randomSpreadY[i], 0f),
                out hit, int.MaxValue, ~layerMask))
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
                i++;

            }
            Invoke("Chkchk", 0.5f);
            currentAmmo--;
            
            gameManager.UpdateText();
        }
        else
        {
            zeroAmmo = true;
            playSound.clip = noAmmo;
            playSound.Play();
        }

    }

    void Chkchk()
    {
        effects.clip = chkchk;
        effects.Play();
    }

}

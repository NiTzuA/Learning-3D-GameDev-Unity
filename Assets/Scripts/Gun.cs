using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;

    public ParticleSystem muzzleFlash;
    public Camera fpsCamera;
    public AudioSource playSound;
    public AudioClip fireSound;

    void Update()
    {
        
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        muzzleFlash.Play();
        playSound.clip = fireSound;
        playSound.Play();
        RaycastHit hit; 

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    } 
}

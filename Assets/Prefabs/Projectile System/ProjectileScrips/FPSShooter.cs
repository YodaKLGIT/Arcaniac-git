using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShooter : MonoBehaviour
{
    public Camera cam;

    private Vector3 destination;
    private bool leftHand; // Tracks whose turn it is to shoot
    private float timeToFire; // Time to fire the next projectile
    public float fireRate = 4; // Rate of fire
    public float arcRange = 1; // Range of the arc

    public List<GameObject> projectiles; // List of different spells
    private int currentSpellIndex = 0; // Index to track the current spell

    public Transform LHFirePoint, RHFirePoint; // Left Hand and Right Hand Fire Points

    public AudioSource audioSource;
    public List<AudioClip> shootSounds; // List of sounds for different spells

    public float projectileSpeed = 30f; // Speed of the projectile

    public List<GameObject> LH_MuzzleFlashes, RH_MuzzleFlashes; // Muzzle Flashes for different spells

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Switch spells
        {
            SwitchSpell();
        }
    }

    void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
        }
        else
        {
            destination = ray.GetPoint(1000);
        }

        if (leftHand)
        {
            leftHand = false;
            InstantiateProjectile(LHFirePoint);
            PlayMuzzleFlash(LH_MuzzleFlashes[currentSpellIndex]);
        }
        else
        {
            leftHand = true;
            InstantiateProjectile(RHFirePoint);
            PlayMuzzleFlash(RH_MuzzleFlashes[currentSpellIndex]);
        }
    }

    void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate(projectiles[currentSpellIndex], firePoint.position, Quaternion.identity);
        ProjectileSound();

        Vector3 direction = (destination - firePoint.position).normalized;
        Rigidbody rb = projectileObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0), Random.Range(0.5f, 2));
        Destroy(projectileObj, 5f);
    }

    void ProjectileSound()
    {
        if (shootSounds.Count > currentSpellIndex && shootSounds[currentSpellIndex] != null)
        {
            audioSource.PlayOneShot(shootSounds[currentSpellIndex]);
        }
    }

    void PlayMuzzleFlash(GameObject muzzleFlash)
    {
        ParticleSystem[] particleSystems = muzzleFlash.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particleSystems)
        {
            ps.Stop();
            ps.Play();
        }
    }

    void SwitchSpell()
    {
        currentSpellIndex = (currentSpellIndex + 1) % projectiles.Count;
        Debug.Log("Switched to spell: " + currentSpellIndex);
    }
}
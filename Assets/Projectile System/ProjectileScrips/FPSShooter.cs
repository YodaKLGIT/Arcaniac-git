using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShooter : MonoBehaviour
{
    public Camera cam;
    public Transform LHFirePoint, RHFirePoint;
    public AudioSource audioSource;

    private Vector3 destination;
    private bool leftHand;
    private float timeToFire;
    public float fireRate = 4;
    public float arcRange = 1;
    public float projectileSpeed = 30f;

    private List<SpellData> selectedSpells = new List<SpellData>();
    private int currentSpellIndex = 0;

    public void SetSelectedSpells(List<SpellData> spells)
    {
        selectedSpells = new List<SpellData>(spells);
        currentSpellIndex = 0;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire && selectedSpells.Count > 0)
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }

        if (Input.GetKeyDown(KeyCode.Q) && selectedSpells.Count > 1)
        {
            currentSpellIndex = (currentSpellIndex + 1) % selectedSpells.Count;
            Debug.Log("Switched to spell: " + selectedSpells[currentSpellIndex].spellName);
        }
    }

    void ShootProjectile()
    {
        if (selectedSpells.Count == 0) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        destination = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(1000);

        if (leftHand)
        {
            leftHand = false;
            InstantiateProjectile(LHFirePoint);
        }
        else
        {
            leftHand = true;
            InstantiateProjectile(RHFirePoint);
        }
    }

    void InstantiateProjectile(Transform firePoint)
    {
        if (selectedSpells.Count == 0) return;

        GameObject projectileObj = Instantiate(selectedSpells[currentSpellIndex].projectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (destination - firePoint.position).normalized;

        Rigidbody rb = projectileObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0), Random.Range(0.5f, 2));
        Destroy(projectileObj, 5f);
    }
}

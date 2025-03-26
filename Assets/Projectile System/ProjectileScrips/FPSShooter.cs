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

    void Start()
    {
        selectedSpells = new List<SpellData>();
    }

    public void SetSelectedSpells(List<SpellData> spells)
    {
        selectedSpells = new List<SpellData>(spells);
        currentSpellIndex = 0;
        Debug.Log("FPSShooter received spells: " + string.Join(", ", selectedSpells.ConvertAll(s => s.spellName)));
    }

    void Update()
    {
        if (selectedSpells.Count == 0) return; // Prevent shooting when no spells are selected

        if (Input.GetButton("Fire1") && Time.time >= timeToFire) // Allow holding Fire1
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }
    }


    void ShootProjectile()
    {
        if (selectedSpells.Count == 0) return;

        // Determine the spell to use
        SpellData currentSpell = selectedSpells[currentSpellIndex];

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        destination = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(1000);

        // Fire from alternating hands
        Transform firePoint = leftHand ? LHFirePoint : RHFirePoint;
        leftHand = !leftHand;

        InstantiateProjectile(firePoint, currentSpell);

        // Cycle to the next spell
        currentSpellIndex = (currentSpellIndex + 1) % selectedSpells.Count;
    }

    void InstantiateProjectile(Transform firePoint, SpellData spell)
    {
        GameObject projectileObj = Instantiate(spell.projectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (destination - firePoint.position).normalized;

        Rigidbody rb = projectileObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        // Add arc effect for randomness
        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0), Random.Range(0.5f, 2));
        Destroy(projectileObj, 5f);
    }
}

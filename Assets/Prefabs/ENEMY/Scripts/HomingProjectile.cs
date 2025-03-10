using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 30f;          // Speed of the projectile
    public float maxRotationSpeed = 10f; // Strong homing at start
    public float minRotationSpeed = 2f;  // Weak homing later
    public float homingDuration = 1.5f; // How long the strong homing lasts
    public float lifetime = 5f;        // Destroy after X seconds

    private Transform target;
    private float homingTime; // Tracks how long homing has been active

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        homingTime = 0f;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        homingTime += Time.deltaTime;

        if (target != null)
        {
            // Calculate direction to target
            Vector3 direction = (target.position - transform.position).normalized;

            // Gradually reduce homing strength over time
            float t = Mathf.Clamp01(homingTime / homingDuration); // Goes from 0 to 1
            float rotationSpeed = Mathf.Lerp(maxRotationSpeed, minRotationSpeed, t);

            // Rotate towards the player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // Move forward
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

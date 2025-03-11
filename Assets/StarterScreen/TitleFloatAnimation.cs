using UnityEngine;

public class LogoAnimation : MonoBehaviour
{
    // Array to hold references to your letter GameObjects
    public GameObject[] letters;

    // Maximum distance each letter moves up or down
    public float moveAmount = 2f;

    // Speed of the oscillation (higher value = faster movement)
    public float speed = 1f;

    // Phase offset for the oscillation
    public float phaseOffset = Mathf.PI / 2;

    // Amplitude multiplier for the oscillation
    public float amplitudeMultiplier = 1f;

    private Vector3[] initialPositions;

    void Start()
    {
        // Store the initial positions of the letters
        initialPositions = new Vector3[letters.Length];
        for (int i = 0; i < letters.Length; i++)
        {
            initialPositions[i] = letters[i].transform.localPosition;
        }
    }

    void Update()
    {
        // Loop through each letter and apply smooth, continuous up/down movement
        for (int i = 0; i < letters.Length; i++)
        {
            // Determine the direction of movement for each letter (even moves up, odd moves down)
            float direction = (i % 2 == 0) ? 1f : -1f;

            // Calculate the smooth up/down movement using Mathf.Sin for continuous oscillation
            // Use a smaller time offset to ensure the letters don't stop for a long time
            float yMovement = Mathf.Sin((Time.time * speed) + (i % 2 == 0 ? 0 : phaseOffset)) * moveAmount * direction * amplitudeMultiplier;

            // Apply the movement to the letter, keeping it centered around its original position
            letters[i].transform.localPosition = new Vector3(
                letters[i].transform.localPosition.x,
                initialPositions[i].y + yMovement,  // Oscillate based on the initial y position
                letters[i].transform.localPosition.z
            );
        }
    }
}

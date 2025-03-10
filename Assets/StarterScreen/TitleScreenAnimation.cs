using UnityEngine;

public class TitleScreenAnimation : MonoBehaviour
{
    public float floatSpeed = 0.5f; // Speed of floating motion
    public float floatAmount = 0.2f; // Maximum floating height
    public float rotationAmount = 20f; // Max rotation angle (e.g., 20 degrees)
    public float rotationSpeed = 1f; // Speed of rotation

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Store the starting position
    }

    void Update()
    {
        // Floating motion (up and down)
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        // Swaying rotation (-20 to +20 degrees)
        float zRotation = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform arms; // Reference to the hands/arms object

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Make arms a child of the camera so they follow its position and rotation
        if (arms != null)
        {
            arms.SetParent(transform, true);
        }
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Rotate orientation (for player movement)
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}

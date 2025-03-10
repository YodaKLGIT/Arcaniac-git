using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class MoveCamera : MonoBehaviour
{

    public Transform cameraPosition;
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}

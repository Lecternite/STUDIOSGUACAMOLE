using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textFaceCamera : MonoBehaviour
{
    public cameraScript cam;

    void Awake()
    {
        cam = FindObjectOfType<cameraScript>();
        cam.cameraUpdated += rotate;
    }
    private void OnDestroy()
    {
        cam.cameraUpdated -= rotate;
    }

    void rotate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public static List<Vector3> respawn_points = new List<Vector3>();
    void Start()
    {
        Debug.Log(transform.position);
        respawn_points.Add(transform.position);
    }
}

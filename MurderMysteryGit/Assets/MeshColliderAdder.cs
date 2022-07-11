using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshColliderAdder : MonoBehaviour
{
    public void addColliders()
    {
        addCollider(gameObject);

        foreach (Transform childObject in transform)
        {
            addCollider(childObject.gameObject);
        }
    }

    private void addCollider(GameObject go)
    {

        MeshFilter meshfilter = go.GetComponent<MeshFilter>();

        if (meshfilter != null)
        {
            Mesh mesh = meshfilter.sharedMesh;
            if(mesh != null)
            {
                if (go.GetComponent<MeshCollider>() == null)
                {
                    MeshCollider meshCollider = go.AddComponent<MeshCollider>();

                    meshCollider.sharedMesh = mesh;
                }
                else
                {
                    MeshCollider meshCollider = go.GetComponent<MeshCollider>();

                    meshCollider.sharedMesh = mesh;
                }
            }
        }
    }
}
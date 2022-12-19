using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVisualScript : MonoBehaviour
{
    public Vector3 direction;

    public LineRenderer bullet_line;

    void Start()
    {
        Destroy(gameObject, 0.2f);
        bullet_line = GetComponent<LineRenderer>();
    }
    void Update()
    {
        transform.position += direction * 1f * Time.deltaTime;

        for (int i = 0; i < bullet_line.positionCount; i++)
        {
            bullet_line.SetPosition(i, bullet_line.GetPosition(i) + direction * 500f * Time.deltaTime);
        }

    }
}

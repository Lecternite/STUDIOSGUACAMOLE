using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Snapshot
{
    public float tick;
    public Vector3 position;

    public Snapshot(float _tick, Vector3 _position)
    {
        tick = _tick;
        position = _position;
    }
}


public class Interpolator
{
    public Vector3 to;
    public Vector3 from;

    public Vector3 current;
    public float t;
    public float lerpTime;

    public List<Snapshot> positions = new List<Snapshot>();

    public Interpolator()
    {
        current = Vector3.zero;
        t = 0f;
    }

    public static float InverseLerp(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }

    public void addTarget(float _tick, Vector3 _to)
    {
        positions.Add(new Snapshot(_tick, _to));
    }

    public Vector3 interpolate(float tick)
    {
        bool thing = false;
        for(int i = 0; i < positions.Count - 1; i++)
        {
            t = InverseLerp((float)positions[i].tick, (float)positions[i + 1].tick, tick);
            if (0 <= t && t <= 1f)
            {
                thing = true;
                from = positions[i].position;
                to = positions[i + 1].position;

                for(int j = 0; j < i; j++)
                {
                    positions.RemoveAt(j);
                    i -= 1;
                    j -= 1;
                }
                break;
            }
        }

        if (!thing)
        {
            Debug.Log("Interpolation has stopped | " + tick.ToString());
        }
        else
        {
            Debug.Log(positions.Count);
        }

        current = Vector3.Lerp(from, to, t);

        return current;
    }
}

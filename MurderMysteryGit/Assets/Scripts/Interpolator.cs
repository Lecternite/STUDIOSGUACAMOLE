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

    public List<Snapshot> positions = new List<Snapshot>();

    public bool interpolationStopped = false;

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
        interpolationStopped = true;
        for(int i = 0; i < positions.Count - 1; i++)
        {
            t = InverseLerp(positions[i].tick, positions[i + 1].tick, tick);
            if (0 <= t && t <= 1f)//means a suitable interpolated pose has been found
            {
                interpolationStopped = false;
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
        if (interpolationStopped)
        {
            Debug.LogError("This interpolator has stuttered");
        }
        current = Vector3.Lerp(from, to, t);

        return current;
    }
}

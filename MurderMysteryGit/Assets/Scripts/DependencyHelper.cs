using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DependencyHelper<T>
{
    public static uint idLatest = 0;
    public static event Action<T, uint> SpawnEvent;
    public static event Action<T, uint> DespawnEvent;
    public static List<T> host_instances = new List<T>();

    public T host;
    public uint myId;

    public DependencyHelper(T _host)
    {
        host = _host;
        idLatest += 1;
        myId = idLatest;

        host_instances.Add(host);

        SpawnEvent?.Invoke(host, myId);
    }

    ~DependencyHelper()
    {
        host_instances.Remove(host);

        DespawnEvent?.Invoke(host, myId);
    }
};

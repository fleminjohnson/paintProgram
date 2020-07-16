using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathService : SingletonBehaviour<PathService>
{
    [SerializeField]
    private PathScript pathPrefab;

    public PathScript GeneratePath(Vector3 pos)
    {
        return GameObject.Instantiate<PathScript>(pathPrefab, pos, Quaternion.identity);
    }
}

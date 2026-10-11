using UnityEngine;
using System.Collections.Generic;

public sealed class GameEnvironment
{     
    private static GameEnvironment instance;

    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints { get { return checkpoints; } }
    public List<GameObject> SafeLocations { get; } = new List<GameObject>();
    public Transform safeLocation;

    public static GameEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();

                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));

                instance.checkpoints.Sort((left, right) => string.CompareOrdinal(left.name, right.name));
                instance.SafeLocations.AddRange(GameObject.FindGameObjectsWithTag("Safe"));
                if (instance.SafeLocations.Count > 0)
                    instance.safeLocation = instance.SafeLocations[0].transform;
            }
            return instance;
        }
    }
}

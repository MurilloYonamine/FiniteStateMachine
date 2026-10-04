using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public sealed class GameEnvironment
{     
    private static GameEnvironment instance;

    private List<GameObject> checkpoints = new List<GameObject>();
    public List<GameObject> Checkpoints { get { return checkpoints; } }
    public Transform safeLocation;

    public static GameEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();

                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));

                instance.checkpoints = instance.Checkpoints.OrderBy(cp => cp.name).ToList();

                instance.safeLocation = GameObject.FindGameObjectWithTag("Safe").transform;
            }
            return instance;
        }
    }
}

// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Cinemachine;

public class PhaseSpawner : MonoBehaviour
{
    [SerializeField] private FSMAIController _npcPrefab;
    [SerializeField] private Camera _playerCamera;

    private readonly List<FSMAIController> _npcs = new List<FSMAIController>();
    public List<FSMAIController> Npcs => _npcs;

    public bool Spawn(int count)
    {
        Clear();
        List<GameObject> locations = GameEnvironment.Singleton.SafeLocations;
        if (count > locations.Count)
        {
            Debug.LogError("[PhaseSpawner] Not enough Safe points for this phase.");
            return false;
        }

        // Aleatoriza os índices dos pontos de spawn
        int[] indices = new int[locations.Count];
        for (int i = 0; i < indices.Length; i++) indices[i] = i;
        for (int i = indices.Length - 1; i > 0; i--)
        {
            int other = Random.Range(0, i + 1);
            int swap = indices[i];
            indices[i] = indices[other];
            indices[other] = swap;
        }

        // Instancia os NPCs nos pontos de spawn aleatórios
        for (int i = 0; i < count; i++)
        {
            Transform point = locations[indices[i]].transform;
            if (!NavMesh.SamplePosition(point.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                Debug.LogError("[PhaseSpawner] A Safe point is not on the NavMesh.");
                Clear();
                return false;
            }

            FSMAIController npc = Instantiate(_npcPrefab, hit.position, point.rotation);
            npc.Initialize(_playerCamera, point);
            CinemachineCamera[] cameras = npc.GetComponentsInChildren<CinemachineCamera>(true);
            for (int j = 0; j < cameras.Length; j++)
                cameras[j].enabled = false;
            _npcs.Add(npc);
        }
        return true;
    }

    public void Clear()
    {
        for (int i = 0; i < _npcs.Count; i++)
        {
            Destroy(_npcs[i].gameObject);
        }
        _npcs.Clear();
    }
}

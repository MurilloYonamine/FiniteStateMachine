// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;

[CreateAssetMenu(fileName = "Phase", menuName = "Hide and Seek/Phase")]
public class PhaseData : ScriptableObject
{
    [Min(1)] public int npcCount = 1;
    [Min(1f)] public float duration = 120f;
}

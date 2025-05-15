using UnityEngine;
[CreateAssetMenu(fileName = "RoundCards", menuName = "ScriptableObjects/RoundCards", order = 1)]
public class RoundCards : ScriptableObject
{
    public GameObject[] Prefabs;
    public int SpawnCount;
}

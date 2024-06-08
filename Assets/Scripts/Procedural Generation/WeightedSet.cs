using UnityEngine;

[CreateAssetMenu(fileName = "Weighted Set", menuName = "Create Weighted Set", order = 1)]
public class WeightedSet : ScriptableObject
{
    [System.Serializable]
    public class WeightedObject
    {
        public GameObject Object;
        public float Weight;
    }

    [SerializeField] WeightedObject[] Objects;

    public GameObject GetRandom()
    {
        float totalWeight = 0;
        foreach (var obj in Objects)
        {
            totalWeight += obj.Weight;
        }

        float random = Random.Range(0, totalWeight);
        foreach (var obj in Objects)
        {
            random -= obj.Weight;
            if (random <= 0)
            {
                return obj.Object;
            }
        }

        return null;
    }
}

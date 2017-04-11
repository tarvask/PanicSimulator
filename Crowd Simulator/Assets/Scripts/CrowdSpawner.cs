using UnityEngine;
using System.Collections;

public class CrowdSpawner : MonoBehaviour
{
    public Transform m_SpawnContainer;
    public Transform m_SpawnPoint;

    // overall count of crowders
    public int m_CrowdersToSpawn;

    // all types of crowders, that can be spawned
    public GameObject[] m_CrowdersTypes;

    // probabilities of crowder types by their weights
    public int[] m_CrowdersProbabilities;

    public void Spawn()
    {
        int overallWeight = 0;
        int typesLength = m_CrowdersTypes.Length;

        // compute overall weight
        for (int i = 0; i < typesLength; i++)
        {
            overallWeight += m_CrowdersProbabilities[i];
        }

        // spawn crowders
        for (int j = 0; j < m_CrowdersToSpawn; j++)
        {
            int choice = Random.Range(0, overallWeight);

            for (int k = 0; k < typesLength; k++)
            {
                if (choice < m_CrowdersProbabilities[k])
                {
                    SpawnType(m_CrowdersTypes[k]);
                    break;
                }
                else
                {
                    choice -= m_CrowdersProbabilities[k];
                }
            }
        }
    }

    void SpawnType(GameObject typePrefab)
    {
        GameObject crowder = Instantiate(typePrefab, m_SpawnContainer) as GameObject;
        crowder.transform.localScale = Vector3.one;
        crowder.transform.localPosition = m_SpawnPoint.localPosition;
    }
}

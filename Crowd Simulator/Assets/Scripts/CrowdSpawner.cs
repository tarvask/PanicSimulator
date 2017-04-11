using UnityEngine;
using System.Collections;

public class CrowdSpawner : MonoBehaviour
{
    public Transform m_SpawnContainer;
    
	// point to spawn around
	public Transform m_SpawnPoint;

	// describes spawn circle
	public float m_SpawnRadius;

	// angle step after each spawn
	public int m_SpawnAngleStep;

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
					Transform newPoint = new GameObject().transform;
					newPoint.parent = m_SpawnContainer;
					newPoint.localPosition = m_SpawnPoint.localPosition + m_SpawnPoint.forward * Random.Range (m_SpawnRadius / 2, m_SpawnRadius);
					newPoint.RotateAround (m_SpawnPoint.localPosition, Vector3.up, m_SpawnAngleStep*j);

					SpawnType(m_CrowdersTypes[k], newPoint.localPosition);
					Destroy (newPoint.gameObject);
                    break;
                }
                else
                {
                    choice -= m_CrowdersProbabilities[k];
                }
            }
        }
    }

	void SpawnType(GameObject typePrefab, Vector3 position)
    {
        GameObject crowder = Instantiate(typePrefab, m_SpawnContainer) as GameObject;
        crowder.transform.localScale = Vector3.one;
		crowder.transform.localPosition = position;
		Debug.Log (position.ToString ());
    }
}

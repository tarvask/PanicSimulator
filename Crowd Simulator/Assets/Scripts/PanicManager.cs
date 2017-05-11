using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanicManager : Singleton<PanicManager>
{
    public CrowdSpawner m_Spawner;

    public GameObject m_DiseasedPrefab;

    public Transform m_DiseasedContainer;

    public Material m_DiseaseMaterial;

	public GameObject m_EscapePoint;

    public List<PanicPoint> m_PanicPoints;

    public List<ChaserMovement> m_Chasers;

	// Use this for initialization
	void Start ()
    {
        m_Spawner.Spawn();

        foreach (ChaserMovement chaser in m_Chasers)
        {
            chaser.Init();
        }
	}

    public PanicPoint[] GetPanicPointsInLocation(Vector3 crowderPosition, float radius)
    {
        List<PanicPoint> resultPoints = new List<PanicPoint>();

        for (int i = 0; i < m_PanicPoints.Count; i++)
        {
            if (Vector3.Distance(m_PanicPoints[i].transform.localPosition, crowderPosition) < radius)
            {
                resultPoints.Add(m_PanicPoints[i]);
            }
        }

        // return all for a while
        return resultPoints.ToArray();
    }
}

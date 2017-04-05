using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanicManager : Singleton<PanicManager>
{
	public GameObject m_EscapePoint;

    public PanicPoint[] m_PanicPoints;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public PanicPoint[] GetPanicPointsInLocation(Vector3 crowderPosition, float radius)
    {
        List<PanicPoint> resultPoints = new List<PanicPoint>();

        for (int i = 0; i < m_PanicPoints.Length; i++)
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

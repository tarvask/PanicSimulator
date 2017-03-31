using UnityEngine;
using System.Collections;

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

    public PanicPoint[] GetPanicPointsInLocation(Vector3 crowderPosition)
    {
        // return all for a while
        return m_PanicPoints;
    }
}

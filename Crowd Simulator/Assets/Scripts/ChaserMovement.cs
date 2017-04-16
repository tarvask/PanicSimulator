using UnityEngine;
using System.Collections;

public class ChaserMovement : MonoBehaviour
{
    public float m_ViewRadius = 30f;
    public float m_VictimThinkPeriod = 1.0f;
    public float m_ChaserSpeed = 1.4f;

    public Rigidbody m_Body;

    GameObject m_Victim;

    float m_VictimThinkTimer;

	// Use this for initialization
	void Start ()
    {
        // move center of mass to the bottom to simulate vanka-vstanka
        m_Body.centerOfMass = new Vector3(0, -1, 0);

        UpdateVictimTarget ();
	}

    // Update is called once per frame
    void FixedUpdate ()
    {
        GameObject victim = GetCurrentVictim();

        if (victim != null)
        {
            // maybe need work with y-coord before
            Vector3 victimDirection = victim.transform.localPosition - transform.localPosition;
            victimDirection.y = 0;
            victimDirection.Normalize();

            Vector3 chaseMovement = victimDirection * Time.fixedDeltaTime * m_ChaserSpeed;
            transform.localPosition += chaseMovement;
        }

        // update timers
        m_VictimThinkTimer -= Time.fixedDeltaTime;
    }

    void UpdateVictimTarget()
    {
        Vector3 currentPosition = transform.localPosition;
        GameObject[] allVictims = PanicManager.Instance.m_Spawner.m_Crowders.ToArray();

        float minDistance = Mathf.Infinity;
        GameObject victim = null;

        // find the closest crowder that can be seen
        for (int i = 0; i < allVictims.Length; i++)
        {
            float curDistance = Vector3.Distance(allVictims[i].transform.localPosition, currentPosition);

            if (curDistance < m_ViewRadius && curDistance < minDistance)
            {
                minDistance = curDistance;
                victim = allVictims[i];
            }
        }

        m_Victim = victim;

        // droptimer
        m_VictimThinkTimer = Random.Range(m_VictimThinkPeriod - 0.15f, m_VictimThinkPeriod + 0.15f);
    }

    GameObject GetCurrentVictim()
    {
        if (m_VictimThinkTimer > 0)
        {
            return m_Victim;
        }
        else
        {
            UpdateVictimTarget ();
            return null;
        }
    }
}

using UnityEngine;
using System.Collections;

public class PanicMovement : MonoBehaviour
{
	public float m_Radius;
	public float m_Angle;
	public float m_BrownPeriod;
	public float m_EscapeThinkPeriod = 0.5f;
    public float m_EscapeSpeed = 1f;

    // two numbers, that describe the proportions of fear/escape movements
    public float m_FearCoeff = 2f;
    public float m_EscapeCoeff = 1.2f;

    public float m_ViewRadius = 5f;

    public Rigidbody m_Body;
    public CapsuleCollider m_Collider;

	Vector3 m_PrevBrownTarget;
	Vector3 m_NextBrownTarget;
	Vector3 m_AtomicBrownForwardMovement;
	Vector3 m_AtomicBrownBackMovement;

	Vector3 m_EscapeTarget;
    Vector3 m_EscapeDirection;

    Vector3 m_PanicDirection;

	float m_BrownTimer;
    float m_CurrentBrownPeriod;
	float m_EscapeThinkTimer;
    float m_PanicThinkTimer;

	// Use this for initialization
	void Start ()
	{
        // move center of mass to the bottom to simulate vanka-vstanka
        m_Body.centerOfMass = new Vector3(0, -1, 0);

		GetNewBrownianTarget ();
//        UpdateEscapeDirection ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        // get escape and panic directions, compute them with corresponding coeffs
        Vector3 panicDirection = GetCurrentPanicDirection();
		Vector3 escapeDirection = Vector3.zero; //GetCurrentEscapeDirection(); now crowders are totally ruled by fear
        Vector3 sanityDirection = (panicDirection * m_FearCoeff + escapeDirection * m_EscapeCoeff).normalized;

        // multiply direction by speed
        Vector3 sanityMovement = sanityDirection * Time.fixedDeltaTime * m_EscapeSpeed;

        // move body
//        transform.localPosition += GetBrownMovement();
//        transform.localPosition += sanityMovement;
        m_Body.MovePosition(transform.position + GetBrownMovement() + sanityMovement);

        // update timers
		m_BrownTimer -= Time.fixedDeltaTime;
        m_EscapeThinkTimer -= Time.fixedDeltaTime;
        m_PanicThinkTimer -= Time.fixedDeltaTime;
	}

	void GetNewBrownianTarget()
	{		
		GameObject nextTransformGO = new GameObject ();
		Transform nextTransform = nextTransformGO.transform;
		nextTransform.parent = transform;
		nextTransform.localPosition = Vector3.zero;
		nextTransform.localRotation = transform.localRotation;

		// turn to new position
		float angle = Random.Range(-m_Angle, m_Angle);
        nextTransform.RotateAround (nextTransform.position, Vector3.up, angle);

		// move to new position
        float delta = Random.Range(m_Radius / 2, m_Radius);
        nextTransform.localPosition += nextTransform.forward.normalized * delta;

		// update targets
        m_PrevBrownTarget = Vector3.zero;
		m_NextBrownTarget = new Vector3(nextTransform.localPosition.x, 0, nextTransform.localPosition.z);

        // take random brown period near given brown period
        m_CurrentBrownPeriod = Random.Range(m_BrownPeriod / 2, m_BrownPeriod * 3 / 2);

		// half period for one-side movement
        m_AtomicBrownForwardMovement = m_NextBrownTarget / (m_CurrentBrownPeriod / 2) * Time.fixedDeltaTime;
        m_AtomicBrownBackMovement = -m_AtomicBrownForwardMovement;

		// drop timer
        m_BrownTimer = m_CurrentBrownPeriod;

        // delete unneeded object
        Destroy(nextTransformGO);
	}

	Vector3 GetBrownMovement()
	{
        if (m_BrownTimer > m_CurrentBrownPeriod / 2)
		{
			return m_AtomicBrownForwardMovement * Mathf.Sin (Mathf.PI * m_BrownTimer / (m_CurrentBrownPeriod * 2));
		}
		else if (m_BrownTimer > 0)
		{
			return m_AtomicBrownBackMovement * Mathf.Sin (Mathf.PI * m_BrownTimer / m_CurrentBrownPeriod);
		}
		else
		{
			GetNewBrownianTarget ();
			return Vector3.zero;
		}
	}

	void UpdateEscapeDirection()
	{
		// get new target
		m_EscapeTarget = PanicManager.Instance.m_EscapePoint.transform.localPosition;
        m_EscapeTarget.y = transform.localPosition.y;

		// get movement
        m_EscapeDirection = m_EscapeTarget - transform.localPosition;

        // do not waste movement on vertical axis
        m_EscapeDirection.y = 0;
        m_EscapeDirection.Normalize();

        // droptimer
        m_EscapeThinkTimer = Random.Range(m_EscapeThinkPeriod - 0.15f, m_EscapeThinkPeriod + 0.15f);
	}

    void UpdatePanicDirection()
    {
        Vector3 currentPosition = transform.localPosition;
        PanicPoint[] panicPoints = PanicManager.Instance.GetPanicPointsInLocation(currentPosition, m_ViewRadius);
        m_PanicDirection = Vector3.zero;

        for (int i = 0; i < panicPoints.Length; i++)
        {
            m_PanicDirection += (currentPosition - panicPoints[i].transform.localPosition) * panicPoints[i].m_FearStrength;
        }

        m_PanicDirection.y = 0;
        m_PanicDirection.Normalize();

        // droptimer
        m_PanicThinkTimer = Random.Range(m_EscapeThinkPeriod - 0.15f, m_EscapeThinkPeriod + 0.15f);
    }

	Vector3 GetCurrentEscapeDirection()
	{
		if (m_EscapeThinkTimer > 0)
		{
            return m_EscapeDirection;
		}
		else
		{
            UpdateEscapeDirection ();
			return Vector3.zero;
		}
	}

    Vector3 GetCurrentPanicDirection()
    {
        if (m_PanicThinkTimer > 0)
        {
            return m_PanicDirection;
        }
        else
        {
            UpdatePanicDirection ();
            return Vector3.zero;
        }
    }
}

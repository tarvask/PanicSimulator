using UnityEngine;
using System.Collections;

public class PanicMovement : MonoBehaviour
{
	public float m_Radius;
	public float m_Angle;
	public float m_BrownPeriod;
	public float m_EscapeThinkPeriod;

	Vector3 m_PrevBrownTarget;
	Vector3 m_NextBrownTarget;
	Vector3 m_AtomicBrownForwardMovement;
	Vector3 m_AtomicBrownBackMovement;

	Vector3 m_EscapeTarget;
	Vector3 m_EscapeMovement;

	float m_BrownTimer;
	float m_EscapeThinkTimer;

	// Use this for initialization
	void Start ()
	{
		GetNewBrownianTarget ();
		GetNewEscapeTarget ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.localPosition += (GetBrownMovement() + GetEscapeMovement());
		m_BrownTimer -= Time.deltaTime;
		m_EscapeThinkTimer -= Time.deltaTime;
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
		nextTransform.RotateAround (Vector3.zero, Vector3.up, angle);

		// move to new position
		float delta = Random.Range(0, m_Radius);
		nextTransform.localPosition += transform.forward * delta;

		// update targets
		m_PrevBrownTarget = transform.localPosition;
		m_NextBrownTarget = new Vector3(nextTransform.localPosition.x, transform.localPosition.y, nextTransform.localPosition.z);

		// half period for one-side movement
		m_AtomicBrownForwardMovement = (m_NextBrownTarget - m_PrevBrownTarget) / (m_BrownPeriod / 2) * Time.deltaTime;
		m_AtomicBrownBackMovement = (m_PrevBrownTarget - m_NextBrownTarget) / (m_BrownPeriod / 2) * Time.deltaTime;

		// drop timer
		m_BrownTimer = m_BrownPeriod;
	}

	Vector3 GetBrownMovement()
	{
		if (m_BrownTimer > m_BrownPeriod / 2)
		{
			return m_AtomicBrownForwardMovement;
		}
		else if (m_BrownTimer > 0)
		{
			return m_AtomicBrownBackMovement;
		}
		else
		{
			GetNewBrownianTarget ();
			return Vector3.zero;
		}
	}

	void GetNewEscapeTarget()
	{
		// get new target
		m_EscapeTarget = PanicManager.Instance.m_EscapePoint.transform.localPosition;

		// get movement
		m_EscapeMovement = ( - transform.localPosition) / (m_BrownPeriod / 2) * Time.deltaTime;
	}

	Vector3 GetEscapeMovement()
	{
		if (m_EscapeThinkTimer > 0)
		{
			return m_EscapeMovement;
		}
		else
		{
			GetNewEscapeTarget ();
			return Vector3.zero;
		}
	}
}

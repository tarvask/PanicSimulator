using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	Transform m_Transform;
	Vector3 m_Position;

	Vector2 m_xBorders;
	Vector2 m_zBorders;

	public float m_Speed;

	// Use this for initialization
	void Start () 
	{
		m_Transform = transform;
		m_Position = m_Transform.position;

		SetBorders();
        Init();
	}	
	
	public void Init()
	{
        InputController inputController = gameObject.GetComponent<InputController>();
        inputController.OnHorizontalInput += MoveHorizontally;
        inputController.OnVerticalInput += MoveVertically;
	}

	void SetBorders()
	{
//		m_xBorders = new Vector2(FieldController.TERRAIN_X, 
//		                         FieldController.TERRAIN_X + FieldController.FIELD_SIZE);
//		m_zBorders = new Vector2(FieldController.TERRAIN_Z - FieldController.FIELD_SIZE/2, 
//		                         FieldController.TERRAIN_Z + FieldController.FIELD_SIZE/2);

        m_xBorders = new Vector2(0, 100);
        m_zBorders = new Vector2(-15, 85);
	}

	void MoveHorizontally(float value)
	{
		m_Position.x += Mathf.Sign(value) * m_Speed * Time.deltaTime;
		m_Position.x = Mathf.Clamp(m_Position.x, m_xBorders.x, m_xBorders.y);
		m_Transform.position = m_Position;
	}

	void MoveVertically(float value)
	{
		m_Position.z += Mathf.Sign(value) * m_Speed * Time.deltaTime;
		m_Position.z = Mathf.Clamp(m_Position.z, m_zBorders.x, m_zBorders.y);
		m_Transform.position = m_Position;
	}
}

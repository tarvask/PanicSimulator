using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class InputController : MonoBehaviour 
{
	float m_AxisInput;
	Vector2 m_MouseInput;

	public event Action<float> OnHorizontalInput;
	public event Action<float> OnVerticalInput;
	public event Action<float, float> OnTerrainClick;
	public event Action<GameObject> OnBuildingClick;

	// Update is called once per frame
	void Update () 
	{
		m_AxisInput = Input.GetAxis ("Horizontal");

		if (m_AxisInput != 0)
		{
			if (OnHorizontalInput != null)
			{
				OnHorizontalInput(Mathf.Sign(m_AxisInput));
			}
		}

		m_AxisInput = Input.GetAxis ("Vertical");
		
		if (m_AxisInput != 0)
		{
			if (OnVerticalInput != null)
			{
				OnVerticalInput(Mathf.Sign(m_AxisInput));
			}
		}


		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetButtonDown("Fire1"))
			{
				switch (GameController.Instance().m_GameMode) 
				{
					case GameController.GameMode.egm_Observe:			
						GetBuildingClick();
						break;

					case GameController.GameMode.egm_Info:

						break;

					case GameController.GameMode.egm_Build:
						GetTerrainClick();
						break;
				}
			}
		}
	}

	void GetBuildingClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit))
		{
			GameObject hitObject = hit.transform.gameObject;

			if (hitObject.name != "Terrain")
			{
				if (OnBuildingClick != null)
				{
					OnBuildingClick(hitObject);
				}
			}
		}
	}

	void GetTerrainClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits;
		hits = Physics.RaycastAll(ray);

		for (int i = 0; i < hits.Length; i++) 
		{
			RaycastHit hit = hits[i];

			if (hit.collider.gameObject.name == "Terrain")
			{
				if (OnTerrainClick != null)
				{
					OnTerrainClick(hit.point.x, hit.point.z);
				}

				break;
			}
		}
	}
}

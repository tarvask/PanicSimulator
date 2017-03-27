using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIInfoMenu : MonoBehaviour 
{
	public Button m_UICancelButton;
	public Button m_UIInfoButton;
	public Button m_UIDemolishButton;

	public event Action<GameBuilding> OnInfoClick;
	public event Action<GameBuilding> OnDemolishClick;

	GameBuilding m_Building;

	// Use this for initialization
	void Start () 
	{
		m_UICancelButton.onClick.AddListener(delegate{ GameController.Instance().m_UIController.OnCancelButtonClickHandler(); });
		m_UIInfoButton.onClick.AddListener(delegate{ OnInfoClick(m_Building); });
		m_UIDemolishButton.onClick.AddListener(delegate{ OnDemolishClick(m_Building); });
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetBuilding(GameBuilding building)
	{
		m_Building = building;
	}
}

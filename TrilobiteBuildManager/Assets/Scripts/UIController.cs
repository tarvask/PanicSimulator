using UnityEngine;
using System;
using System.Collections;

public class UIController : MonoBehaviour 
{
	public UIBuildMenu m_BuildMenu;
	public UIBuildModeMenu m_BuildModeMenu;
	public UIInfoMenu m_InfoMenu;
	public UIInfoWindow m_InfoWindow;

	public UIWorldText m_WorldText;

	public event Action<GameBuilding> OnDemolishClick;

	// Use this for initialization
	void Start () 
	{
		m_BuildMenu.m_UIBuildButton.onClick.AddListener(delegate{ OnBuildButtonClickHandler(); });
		m_BuildModeMenu.OnBuildModeButtonClick += OnBuildModeButtonClickHandler;
		m_InfoMenu.OnInfoClick += OnInfoClickHandler;
		m_InfoMenu.OnDemolishClick += OnDemolishClickHandler;
	}

	public void ActivateMenuInMode(GameController.GameMode gameMode, GameBuilding building)
	{
		CloseAllMenus();

		switch (gameMode) 
		{
			case GameController.GameMode.egm_Observe:
				m_BuildMenu.gameObject.SetActive(true);
				break;
			
			case GameController.GameMode.egm_Build:
			
				break;
			
			case GameController.GameMode.egm_Info:
				if (building != null)
				{
					OnBuildingClickHandler(building);
				}
				break;
		}
	}

	void CloseAllMenus()
	{
		m_BuildMenu.gameObject.SetActive(false);
		m_BuildModeMenu.gameObject.SetActive(false);
		m_InfoMenu.gameObject.SetActive(false);
		m_InfoWindow.gameObject.SetActive(false);
	}

	public void OnBuildButtonClickHandler()
	{
		CloseAllMenus();
		m_BuildModeMenu.gameObject.SetActive(true);
	}

	public void OnCancelButtonClickHandler()
	{
		GameController.Instance().ChangeGameMode(GameController.GameMode.egm_Observe, null);
	}

	public void OnBuildModeButtonClickHandler(FieldController.BuildingType buildMode)
	{
		GameController.Instance().ChangeBuildMode(buildMode);
	}

	public void OnBuildingClickHandler(GameBuilding building)
	{
		CloseAllMenus();
		m_InfoMenu.gameObject.SetActive(true);
		m_InfoMenu.SetBuilding(building);
	}

	public void ShowBuildingInfo(GameBuilding building)
	{
		m_WorldText.ShowBuildingInfo(building);
	}

	void OnInfoClickHandler(GameBuilding building)
	{
		CloseAllMenus();
		m_InfoWindow.gameObject.SetActive(true);
		m_InfoWindow.SetInfo(building);
	}

	void OnDemolishClickHandler(GameBuilding building)
	{
		OnDemolishClick(building);
	}
}

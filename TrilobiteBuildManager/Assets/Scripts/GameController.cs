using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	static GameController m_Instance;

	public InputController m_InputController;
	public CameraController m_CameraController;
	public FieldController m_FieldController;
	public UIController m_UIController;
		
	public enum GameMode
	{
		egm_Observe,
		egm_Build,
		egm_Info
	}

	public GameMode m_GameMode = GameMode.egm_Observe;

	public static GameController Instance()
	{
		return m_Instance;
	}
	// Use this for initialization
	void Start () 
	{
		m_Instance = this;

		m_FieldController.Init();
		m_CameraController.Init();

		ChangeGameMode(GameMode.egm_Observe);
	}

	public void ChangeGameMode(GameMode gameMode, GameBuilding building = null)
	{
		m_UIController.ActivateMenuInMode(gameMode, building);
		m_GameMode = gameMode;
	}

	public void ChangeBuildMode(FieldController.BuildingType buildMode)
	{
		m_FieldController.m_BuildMode = buildMode;
		ChangeGameMode(GameMode.egm_Build);
	}

	public void ShowBuildingInfo(GameBuilding building)
	{
		m_UIController.ShowBuildingInfo(building);
	}
}

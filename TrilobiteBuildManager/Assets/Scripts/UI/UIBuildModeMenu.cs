using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class UIBuildModeMenu : MonoBehaviour 
{
	public Button m_UICancelButton;
	public Button m_UIBuildMode1Button;
	public Button m_UIBuildMode2Button;
	public Button m_UIBuildMode3Button;

	public event Action<FieldController.BuildingType> OnBuildModeButtonClick;

	// Use this for initialization
	void Start () 
	{
		m_UICancelButton.onClick.AddListener(delegate{ GameController.Instance().m_UIController.OnCancelButtonClickHandler(); });
		m_UIBuildMode1Button.onClick.AddListener(delegate 
		                                         { OnBuildModeButtonClick(FieldController.BuildingType.ebm_1x1); });
		m_UIBuildMode2Button.onClick.AddListener(delegate 
		                                         { OnBuildModeButtonClick(FieldController.BuildingType.ebm_2x2); });
		m_UIBuildMode3Button.onClick.AddListener(delegate 
		                                         { OnBuildModeButtonClick(FieldController.BuildingType.ebm_3x3); });
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInfoWindow : MonoBehaviour 
{
	public Button m_UICancelButton;
	public Text m_Text;

	// Use this for initialization
	void Start () 
	{
		m_UICancelButton.onClick.AddListener(delegate{ GameController.Instance().m_UIController.OnCancelButtonClickHandler(); });
	}

	public void SetInfo(GameBuilding building)
	{
		m_Text.text = building.Description;
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// implemented UI-tooltip in world space, but it looks weird
public class UIWorldText : MonoBehaviour 
{
	public Text m_Text;

	public void ShowBuildingInfo(GameBuilding building)
	{
		gameObject.SetActive(true);
		transform.position = building.gameObject.transform.position;
		transform.position += new Vector3(0, 5, 0);
		m_Text.text = building.Description;
	}
}

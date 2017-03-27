using UnityEngine;
using System;
using System.Collections;

public class GameBuilding : MonoBehaviour
{
	[SerializeField]
	FieldController.BuildingType m_Type;

	public FieldController.BuildingType Type { get { return m_Type; } }
	public int BaseCellX { get; private set; }
	public int BaseCellZ { get; private set; }
	public static int Square (int buildingType)
	{ 
		int edge = (int)buildingType + 1;
		return edge*edge;
	}

	public string Description 
	{ 
		get 
		{ 
			int type = (int)Type + 1;
			return String.Format("Это дом {0}x{0}.\nОн занимает площадь в {1} клет{2}", type, type*type, DescriptionEnding(type*type));
		}
	}

	string DescriptionEnding(int type)
	{
		switch (type%10) 
		{
			case 1:
				return "ку";
			case 2:
			case 3:
			case 4:
				return "ки";
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 0:
			default:
				return "ок";		
		}
	}

	public void Place(int x, int z)
	{
		BaseCellX = x;
		BaseCellZ = z;

		int cubeScale = (int)Type + 1;
		Vector3 cubeCenterPos = new Vector3(cubeScale/2.0f, cubeScale/2.0f, cubeScale/2.0f);
		Vector3 pos = new Vector3(x, 0, z);
		pos += cubeCenterPos;
		transform.position = pos;
	}
}

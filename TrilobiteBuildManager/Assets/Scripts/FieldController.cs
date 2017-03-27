using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldController : MonoBehaviour 
{
	public const int FIELD_SIZE = 100;
	public const float  DEVELOPMENT_COEFF = 0.1f;
	public const int TERRAIN_X = 0;
	public const int TERRAIN_Z = 0;

	public enum BuildingType
	{
		ebm_1x1 = 0,
		ebm_2x2 = 1,
		ebm_3x3 = 2,
		ebm_max = 3
	}

	public GameBuilding[] m_BuildingPrefabs = new GameBuilding[(int)BuildingType.ebm_max];

	public BuildingType m_BuildMode = BuildingType.ebm_1x1;

	bool[,] m_Cells = new bool[FIELD_SIZE, FIELD_SIZE];

	public void Init()
	{
		GameController.Instance().m_InputController.OnBuildingClick += OnBuildingClickHandler;
		GameController.Instance().m_InputController.OnTerrainClick += OnTerrainClickHandler;
		GameController.Instance().m_UIController.OnDemolishClick += DemolishBuilding;

		InitialSpawn();
	}

	void OnBuildingClickHandler(GameObject building)
	{
		switch (GameController.Instance().m_GameMode) 
		{
			case GameController.GameMode.egm_Observe:
				// uncomment to enable UI-tooltip above the building
				//GameController.Instance().ShowBuildingInfo(building.GetComponent<GameBuilding>());
			GameController.Instance().ChangeGameMode(GameController.GameMode.egm_Info, 
			                                         building.GetComponent<GameBuilding>());
				break;

			case GameController.GameMode.egm_Build:

				break;

			case GameController.GameMode.egm_Info:
				
				break;
		}
	}

	void OnTerrainClickHandler(float x, float z)
	{
		TryToBuild(Mathf.FloorToInt(x), Mathf.FloorToInt(z), m_BuildMode);
	}

	GameBuilding TryToBuild(int x, int z, BuildingType mode)
	{
		GameBuilding newBuilding = null;

		if (CellsCanBeEngaged(x, z, mode))
		{
			newBuilding = SpawnBuilding(x, z, mode);
			EngageCells(newBuilding);
		}

		return newBuilding;
	}

	bool CellsCanBeEngaged(int x, int z, BuildingType mode)
	{
		for (int i = 0; i <= (int)mode; i++) 
		{
			for (int j = 0; j <= (int)mode; j++) 
			{
				if ((x+i >= FIELD_SIZE ) || (z+j >= FIELD_SIZE) || m_Cells[x+i, z+j])
				{
					return false;
				}
			}
		}

		return true;
	}

	void EngageCells(GameBuilding building)
	{
		for (int i = 0; i <= (int)(building.Type); i++) 
		{
			for (int j = 0; j <= (int)(building.Type); j++) 
			{
				m_Cells[building.BaseCellX + i, building.BaseCellZ + j] = true;
			}
		}
	}

	void FreeCells(GameBuilding building)
	{
		for (int i = 0; i <= (int)(building.Type); i++) 
		{
			for (int j = 0; j <= (int)(building.Type); j++) 
			{
				m_Cells[building.BaseCellX + i, building.BaseCellZ + j] = false;
			}
		}
	}

	GameBuilding SpawnBuilding(int x, int z, BuildingType mode)
	{
		GameBuilding cube = Instantiate(m_BuildingPrefabs[(int)mode]) as GameBuilding;
		cube.transform.parent = this.gameObject.transform;
		cube.Place(x, z);
		GameController.Instance().ChangeGameMode(GameController.GameMode.egm_Observe);

		return cube;
	}

	void DemolishBuilding(GameBuilding building)
	{
		FreeCells(building);
		Destroy (building.gameObject);
		GameController.Instance().ChangeGameMode(GameController.GameMode.egm_Observe);
	}

	void InitialSpawn()
	{
		int cellsNumberToSpawn = (int)(FIELD_SIZE*FIELD_SIZE*DEVELOPMENT_COEFF);
		int overallSquare = 0;
		int steps = 0;

		while (overallSquare < cellsNumberToSpawn) 
		{
			steps++;
			GameBuilding randomBuilding = SpawnRandom(cellsNumberToSpawn - overallSquare, steps);

			if (randomBuilding != null)
			{			
				overallSquare += GameBuilding.Square((int)(randomBuilding.Type));
			}
		}
	}

	GameBuilding SpawnRandom(int cellsLeftToSpawn, int stepsMade)
	{
		Random.seed = (int)(Time.time + stepsMade);
		int randomX = Random.Range(0, FIELD_SIZE-1);
		Random.seed = (int)(Time.time) + stepsMade + 1;
		int randomZ = Random.Range(0, FIELD_SIZE-1);
		int randomType = GetRandomType(cellsLeftToSpawn, stepsMade);

		return TryToBuild(randomX, randomZ, (BuildingType)randomType);
	}

	int GetRandomType(int cellsLeftToSpawn, int steps)
	{
		int randomType = 0;
		int maxType = (int)(BuildingType.ebm_max);
		Random.seed = (int)(Time.time) + steps + 2;

		if (cellsLeftToSpawn > GameBuilding.Square(maxType))
		{
			randomType = Random.Range(0, maxType);
		}
		else if (cellsLeftToSpawn > GameBuilding.Square(--maxType))
		{
			randomType = Random.Range(0, maxType);
		}

		return randomType;
	}
}

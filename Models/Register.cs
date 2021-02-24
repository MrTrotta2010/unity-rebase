using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Register
{
	private Dictionary<int, Vector3> articulations;

	public int GetCount()
	{
		return articulations.Count;
	}

	public Register()
	{
		articulations = new Dictionary<int, Vector3>();
	}

	public Register(int[] articulationList)
	{
		articulations = new Dictionary<int, Vector3>();
		SetArticulations(articulationList);
	}

	// Define quais articulações estarão no dicionário
	public void SetArticulations(int[] articulationList)
	{
		for (int i = 0; i < articulationList.Length; i++)
		{
			if (1 <= articulationList[i] && articulationList[i] <= 20)
			{
				try
				{
					articulations.Add(articulationList[i], new Vector3());
				}
				catch (ArgumentException)
				{
					throw new ArgumentException("Duplicate articulation in list");
				}
			}
			else
			{
				articulations.Clear();
				throw new IndexOutOfRangeException("Articulations can't be smaller than 1 or greater than 20");
			}
		}
	}

	public void SetArticulationCoordinates(int articulation, Vector3 coordinates)
	{
		if (articulations.ContainsKey(articulation))
		{
			articulations[articulation] = coordinates;
		}
		else
		{
			throw new KeyNotFoundException();
		}
	}
	
	public Vector3 GetArticulationCoordinates(int articulation)
	{
		try
		{
			return articulations[articulation];
		}
		catch (KeyNotFoundException)
		{
			throw new KeyNotFoundException();
		}
	}

	override public string ToString()
	{
		string[] list = new string[articulations.Count];
		int i = 0;
		foreach (KeyValuePair<int, Vector3> kvp in articulations)
		{
			list[i] = Math.Round(kvp.Value.x, 2).ToString().Replace(',', '.') + "," +
					Math.Round(kvp.Value.y, 2).ToString().Replace(',', '.') + "," +
					Math.Round(kvp.Value.z, 2).ToString().Replace(',', '.');
			i++;
		}
		return string.Join(";", list);
	}

	public string GetArticulationIndexPattern()
	{
		string[] list = new string[articulations.Count];
		int i = 0;
		foreach (KeyValuePair<int, Vector3> kvp in articulations)
		{
			list[i] = ("a" + kvp.Key);
			i++;
		}
		return string.Join(";", list);
	}

	public int[] GetArticulationList()
    {
		int[] articulationList = new int[articulations.Count];
		int i = 0;
		foreach (KeyValuePair<int, Vector3> kvp in articulations)
        {
			articulationList[i] = kvp.Key;
			i++;
        }
		return articulationList;
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ReBase
{
	[Serializable]
	public class Register
	{
		private Dictionary<int, Vector3> articulations;

		public int ArticulationCount { get => articulations.Count; }

		public int[] Articulations { get => articulations.Keys.ToArray(); }

		public Register()
		{
			articulations = new Dictionary<int, Vector3>();
		}

		public Register(int[] articulationList)
		{
			articulations = new Dictionary<int, Vector3>();
			SetArticulations(articulationList);
		}

		public Register(Dictionary<int, Vector3> articulations)
		{
			this.articulations = articulations;
		}

		// Define quais articulações estarão no dicionário
		public void SetArticulations(int[] articulationList)
		{
			foreach (int articulation in articulationList)
			{
				if (articulation < 1 || articulation > 20)
				{
					articulations.Clear();
					throw new IndexOutOfRangeException("Articulations can't be smaller than 1 or greater than 20");
				}
				try
				{
					articulations.Add(articulation, new Vector3());
				}
				catch (ArgumentException)
				{
					throw new RepeatedArticulationException("Duplicate articulation in list", articulationList);
				}
			}
		}

		public void SetArticulationRotations(int articulation, Vector3 rotations)
		{
			articulations[articulation] = rotations;
		}

		public Vector3 GetArticulationRotations(int articulation)
		{
			return articulations[articulation];
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
	}
}

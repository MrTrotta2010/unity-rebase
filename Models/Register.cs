using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ReBase
{
	[Serializable]
	public class Register
	{
		private Dictionary<string, Rotation> _articulations;

		public int articulationCount { get => _articulations.Count; }
		public string[] articulations { get => _articulations.Keys.ToArray(); }
		public bool isEmpty { get => _articulations.Count == 0; }

		public Register()
		{
			_articulations = new Dictionary<string, Rotation>();
		}

		public Register(string[] articulationList)
		{
			_articulations = new Dictionary<string, Rotation>();
			SetArticulations(articulationList);
		}

		public Register(Dictionary<string, Rotation> articulations)
		{
			this._articulations = articulations;
		}

		// Define quais articulações estarão no dicionário
		public void SetArticulations(string[] articulationList)
		{
			foreach (string articulation in articulationList)
			{
				try
				{
					_articulations.Add(articulation, null);
				}
				catch (ArgumentException)
				{
					throw new RepeatedArticulationException("Duplicate articulation in list", articulationList);
				}
			}
		}

		public void SetArticulationRotations(string articulation, Rotation rotations)
		{
			_articulations[articulation] = rotations;
		}

		public Rotation GetArticulationRotations(string articulation)
		{
			return _articulations[articulation];
		}

		override public string ToString()
		{
			string[] list = new string[_articulations.Count];
			int i = 0;
			foreach (KeyValuePair<string, Rotation> kvp in _articulations)
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

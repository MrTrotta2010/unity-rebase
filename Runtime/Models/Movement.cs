// Copyright © 2023 Tiago Trotta

// This file is part of Unity ReBase.

// Unity ReBase is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Unity ReBase is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Unity ReBase.  If not, see <https://www.gnu.org/licenses/>

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace ReBase
{
	[Serializable]
	public class Movement
	{
		private string _id;
		private string _label;
		private string _description;
		private string _device;
		private string[] _articulations;
		private float _fps;
		private float _duration;
		private int _numberOfRegisters;
		private string _insertionDate;
		private string _updateDate;

		private string _sessionId;
		private string _professionalId;
		private string _patientId;

		private int _appCode;
		private string _appData;


		private List<Register> _registers;

		public string id { get => _id; }
		public string label { get => _label; set => _label = value; }
		public string description { get => _description; set => _description = value; }
		public string device { get => _device; set => _device = value; }
		public float fps { get => _fps; set => _fps = value; }
		public float duration { get => _duration; set => _duration = value; }
		public int numberOfRegisters { get => _numberOfRegisters; set => _numberOfRegisters = value; }
		public string[] articulations { get => _articulations; set => _articulations = value; }
		public string insertionDate { get => _insertionDate; }
		public string updateDate { get => _updateDate; }
		public string sessionId { get => _sessionId; }
		public string professionalId { get => _professionalId; set => _professionalId = value; }
		public int appCode { get => _appCode; set => _appCode = value; }
		public string appData { get => _appData; set => _appData = value; }
		public string patientId { get => _patientId; set => _patientId = value; }
		public List<Register> registers
		{
			get => _registers;
			set
			{
				_registers = value;
				numberOfRegisters = _registers.Count;
			}
		}

		public Movement(string label = "", string description = "", string device = "", float fps = 30f, string[] articulations = null,
						string sessionId = "",string professionalId = "", int appCode = 0, string appData = "",
						string patientId = "", Register[] registers = null)
		{
			_label = label;
			_description = description;
			_device = device;
			_fps = fps;

			_articulations = articulations ?? new string[] { };
			ValidateArticulationList();

			_sessionId = sessionId;
			_professionalId = professionalId;
			_appCode = appCode;
			_appData = appData;
			_patientId = patientId;

			_duration = 0f;
			_numberOfRegisters = 0;

			if (registers != null)
			{
				foreach (Register register in registers)
					ValidateRegisters(register.articulations);

				_registers = new List<Register>(registers);
				_numberOfRegisters = _registers.Count;
			}
			else
			{
				_registers = new List<Register>();
			}
		}

		public Movement(SerializableMovement movement)
		{
			ConvertSerializableMovement(movement);
		}

		public Movement(string movementJson)
		{
			SerializableMovement auxMovement = Serializer.Deserialize<SerializableMovement>(movementJson);
			ConvertSerializableMovement(auxMovement);
		}

		public void AddRegister(Register register)
		{
			ValidateRegisters(register.articulations);

			_registers.Add(register);
			_numberOfRegisters += 1;
			_duration = _numberOfRegisters / _fps;
		}

		public string ToJson(bool update = false)
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			if (update)
			{
				return $"{{\"movement\":{{\"label\":\"{_label}\"," +
					$"\"description\":\"{_description}\"," +
					$"\"device\":\"{_device}\"," +
					$"\"sessionId\":\"{_sessionId}\"," +
					"\"app\":{" +
					$"\"code\":{_appCode}," +
					$"\"data\":\"{_appData}\"}}}}}}";
			}
			return $"{{\"movement\":{{\"label\":\"{_label}\"," +
				$"\"device\":\"{_device}\"," +
				$"\"description\":\"{_description}\"," +
				$"\"fps\":\"{_fps}\"," +
				$"\"duration\":{_duration}," +
				$"\"numberOfRegisters\":{_numberOfRegisters}," +
				$"\"sessionId\":\"{_sessionId}\"," +
				"\"app\":{" +
				$"\"code\":{_appCode}," +
				$"\"data\":\"{_appData}\"}}," +
				$"\"registers\":{SerializeRegisters()}}}}}";
		}

		public string ToCreateSessionJson()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			return $"{{\"label\":\"{_label}\"," +
				$"\"device\":\"{_device}\"," +
				$"\"description\":\"{_description}\"," +
				$"\"fps\":\"{_fps}\"," +
				$"\"duration\":{_duration}," +
				$"\"numberOfRegisters\":{_numberOfRegisters}," +
				$"\"sessionId\":\"{_sessionId}\"," +
				"\"app\":{" +
				$"\"code\":{_appCode}," +
				$"\"data\":\"{_appData}\"}}," +
				$"\"registers\":{SerializeRegisters()}}}";
		}

		public override string ToString()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			return $"{{\"movement\":{{\"id\":\"{_id}\"," +
				$"\"label\":\"{_label}\"," +
				$"\"description\":\"{_description}\"," +
				$"\"device\":\"{_device}\"," +
				$"\"articulations\":[{string.Join(",", _articulations)}]," +
				$"\"insertionDate\":\"{_insertionDate}\"," +
				$"\"updateDate\":\"{_updateDate}\"," +
				$"\"duration\":{_duration}," +
				$"\"numberOfRegisters\":{_numberOfRegisters}," +
				$"\"sessionId\":\"{_sessionId}\"," +
				$"\"patientId\":\"{_patientId}\"," +
				$"\"professionalId\":\"{_professionalId}\"," +
				"\"app\":{" +
				$"\"code\":{_appCode}," +
				$"\"data\":\"{_appData}\"}}," +
				$"\"registers\":{SerializeRegisters()}}}}}";
		}

		private void ValidateRegisters(string[] registerArticulations)
		{
			if (!CompareArticulationLists(_articulations, registerArticulations))
				throw new MismatchedArticulationsException("Articulation lists do not match", _articulations, registerArticulations);
		}

		private void ConvertSerializableMovement(SerializableMovement movement)
		{
			if (movement.id != null) _id = movement.id;
			if (movement.label != null) _label = movement.label;
			if (movement.description != null) _description = movement.description;
			if (movement.device != null) _device = movement.device;
			if (movement.insertionDate != null) _insertionDate = movement.insertionDate;
			if (movement.updateDate != null) _updateDate = movement.updateDate;
			_fps = movement.fps;
			_duration = movement.duration;
			_numberOfRegisters = movement.numberOfRegisters;

			if (movement.articulations != null)
			{
				_articulations = movement.articulations;
				ValidateArticulationList();
			}
			if (movement.sessionId != null) _sessionId = movement.sessionId;
			if (movement.professionalId != null) _professionalId = movement.professionalId;
			if (movement.app != null)
			{
				_appCode = movement.app.code;
				if (movement.app.data != null) _appData = movement.app.data;
			}
			if (movement.patientId != null) _patientId = movement.patientId;

			_registers = BuildRegisterList(movement.registers);
		}

		private string SerializeRegisters()
		{
			string[] serializedRegisters = new string[numberOfRegisters];

			for (int i = 0; i < _registers.Count; i++)
			{
				string[] strRegisterList = new string[_registers[i].articulationCount];
				int j = 0;

				foreach (string articulation in _registers[i].articulations)
				{
					strRegisterList[j] = $"\"{articulation}\": [{string.Join(", ", _registers[i].GetArticulationRotations(articulation).ToArray())}]";
					j++;
				}
				serializedRegisters[i] = $"{{{string.Join(", ", strRegisterList)}}}";
			}

			return $"[{string.Join(", ", serializedRegisters)}]";
		}

		private List<Register> BuildRegisterList(Dictionary<string, float[]>[] registers)
		{
			List<Register> registerList = new List<Register>();

			foreach (Dictionary<string, float[]> register in registers)
			{
				string[] articulations = register.Keys.ToArray();
				Register registerObj = new Register(articulations);
				foreach (string articulation in articulations)
					registerObj.SetArticulationRotations(articulation, new Rotation(register[articulation]));
				registerList.Add(registerObj);
			}

			return registerList;
		}

		private void ValidateArticulationList()
		{
			foreach (string articulationA in _articulations)
			{
				int count = 0;
				foreach (string articulationB in _articulations)
				{
					if (articulationA == articulationB)
					{
						count++;
						if (count > 1)
						{
							throw new RepeatedArticulationException("Repeated articulation in list", _articulations);
						}
					}
				}
			}
		}

		public static bool CompareArticulationLists(string[] listA, string[] listB)
		{
			if (listA.Length != listB.Length) return false;
			for (int i = 0; i < listA.Length; i++)
			{
				if (listA[i] != listB[i]) return false;
			}
			return true;
		}
	}
}
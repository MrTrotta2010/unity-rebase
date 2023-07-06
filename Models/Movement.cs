using System;
using System.Globalization;
using System.Collections.Generic;

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


		private List<Register> _articulationData;

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
		public List<Register> articulationData
		{
			get => _articulationData;
			set
			{
				_articulationData = value;
				numberOfRegisters = _articulationData.Count;
			}
		}

		public Movement(string label = "", string description = "", string device = "", float fps = 30f, string[] articulations = null,
						string sessionId = "",string professionalId = "", int appCode = 0, string appData = "",
						string patientId = "", Register[] articulationData = null)
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

			if (articulationData != null)
			{
				foreach (Register register in articulationData)
					ValidateArticulationData(register.articulations);

				_articulationData = new List<Register>(articulationData);
			}
			else
			{
				_articulationData = new List<Register>();
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
			ValidateArticulationData(register.articulations);

			_articulationData.Add(register);
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
				$"\"articulationData\":{SerializeArticulationData()}}}}}";
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
				$"\"articulationData\":{SerializeArticulationData()}}}";
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
				$"\"articulationData\":{SerializeArticulationData()}}}}}";
		}

		private void ValidateArticulationData(string[] registerArticulations)
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

			_articulationData = ArticulationDataToRegisterList(movement.articulationData);
		}

		private string SerializeArticulationData()
		{
			Dictionary<string, List<Rotation>> aritulationDataDict = new Dictionary<string, List<Rotation>>();

			foreach (string articulation in _articulations)
			{
				aritulationDataDict.Add(articulation, new List<Rotation>());
			}
			foreach (Register register in _articulationData)
			{
				foreach (string articulation in aritulationDataDict.Keys)
				{
					aritulationDataDict[articulation].Add(register.GetArticulationRotations(articulation));
				}
			}

			string serializedList = "[";
			foreach (KeyValuePair<string, List<Rotation>> items in aritulationDataDict)
			{
				serializedList += $"{{\"articulation\":{items.Key},\"data\":[";
				for (int i = 0; i < items.Value.Count; i++)
				{
					Rotation coordinates = items.Value[i];
					if (coordinates == null) continue;

					serializedList += $"[{coordinates.x},{coordinates.y},{coordinates.z}]";
					if (i < items.Value.Count - 1) serializedList += ",";
				}
				serializedList += "]},";
			}

			return serializedList.TrimEnd(',') + "]";
		}

		private List<Register> ArticulationDataToRegisterList(SerializableMovement.ArticulationData[] articulationData)
		{
			int length = articulationData?.Length > 0 ? (articulationData[0].data?.Length ?? 0) : 0;
			List<Register> registerList = new List<Register>();

			for (int j = 0; j < length; j++)
			{
				Register register = new Register(_articulations);
				foreach (SerializableMovement.ArticulationData dataObject in articulationData)
				{
					if (dataObject.data.Length == 0) continue;

					try
					{
						Rotation rotations = new Rotation(dataObject.data[j][0], dataObject.data[j][1], dataObject.data[j][2]);
						register.SetArticulationRotations(dataObject.articulation, rotations);
					}
					catch (Exception ex) when (ex is ArgumentException || ex is NullReferenceException)
					{
						continue;
					}
				}
				if (register.isEmpty) continue;
				registerList.Add(register);
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
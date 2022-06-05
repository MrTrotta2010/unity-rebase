using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace ReBase
{
	[Serializable]
	public class Movement
	{
		private string _id;
		private string _label;
		private string _device;
		private int[] _articulations;
		private float _fps;
		private float _duration;
		private int _numberOfRegisters;
		private string _insertionDate;
		private string _updateDate;

		private string _sessionId;
		private string _title;
		private string _description;
		private string _professionalId;
		private int _patientSessionNumber;

		private int _appCode;
		private string _appData;

		private string _patientId;
		private int _patientAge;
		private float _patientHeight;
		private float _patientWeight;
		private string _mainComplaint;
		private string _historyOfCurrentDesease;
		private string _historyOfPastDesease;
		private string _diagnosis;
		private string _relatedDeseases;
		private string _medications;
		private string _physicalEvaluation;

		private List<Register> _articulationData;

		public string id { get => _id; }
		public string label { get => _label; set => _label = value; }
		public string device { get => _device; set => _device = value; }
		public float fps { get => _fps; set => _fps = value; }
		public float duration { get => _duration; set => _duration = value; }
		public int numberOfRegisters { get => _numberOfRegisters; set => _numberOfRegisters = value; }
		public int[] articulations { get => _articulations; set => _articulations = value; }
		public string insertionDate { get => _insertionDate; }
		public string updateDate { get => _updateDate; }
		public string sessionId { get => _sessionId; }
		public string title { get => _title; set => _title = value; }
		public string description { get => _description; set => _description = value; }
		public string professionalId { get => _professionalId; set => _professionalId = value; }
		public int patientSessionNumber { get => _patientSessionNumber; set => _patientSessionNumber = value; }
		public int appCode { get => _appCode; set => _appCode = value; }
		public string appData { get => _appData; set => _appData = value; }
		public string patientId { get => _patientId; set => _patientId = value; }
		public int patientAge { get => _patientAge; set => _patientAge = value; }
		public float patientHeight { get => _patientHeight; set => _patientHeight = value; }
		public float patientWeight { get => _patientWeight; set => _patientWeight = value; }
		public string mainComplaint { get => _mainComplaint; set => _mainComplaint = value; }
		public string historyOfCurrentDesease { get => _historyOfCurrentDesease; set => _historyOfCurrentDesease = value; }
		public string historyOfPastDesease { get => _historyOfPastDesease; set => _historyOfPastDesease = value; }
		public string diagnosis { get => _diagnosis; set => _diagnosis = value; }
		public string relatedDeseases { get => _relatedDeseases; set => _relatedDeseases = value; }
		public string medications { get => _medications; set => _medications = value; }
		public string physicalEvaluation { get => _physicalEvaluation; set => _physicalEvaluation = value; }
		public List<Register> articulationData
		{
			get => _articulationData;
			set
			{
				_articulationData = value;
				numberOfRegisters = _articulationData.Count;
			}
		}

		public Movement(string label = "", string device = "", float fps = 30f, int[] articulations = null, string sessionId = "", string title = "", string description = "", string professionalId = "",
						int patientSessionNumber = 0, int appCode = 0, string appData = "", string patientId = "", int patientAge = 0, float patientHeight = 0f, float patientWeight = 0f,
						string mainComplaint = "", string historyOfCurrentDesease = "", string historyOfPastDesease = "", string diagnosis = "", string relatedDeseases = "",
						string medications = "", string physicalEvaluation = "")
		{
			_label = label;
			_device = device;
			_fps = fps;

			_articulations = articulations ?? new int[] { };
			ValidateArticulationList();

			_sessionId = sessionId;
			_title = title;
			_description = description;
			_professionalId = professionalId;
			_patientSessionNumber = patientSessionNumber;
			_appCode = appCode;
			_appData = appData;
			_patientId = patientId;
			_patientAge = patientAge;
			_patientHeight = patientHeight;
			_patientWeight = patientWeight;
			_mainComplaint = mainComplaint;
			_historyOfCurrentDesease = historyOfCurrentDesease;
			_historyOfPastDesease = historyOfPastDesease;
			_diagnosis = diagnosis;
			_relatedDeseases = relatedDeseases;
			_medications = medications;
			_physicalEvaluation = physicalEvaluation;

			_duration = 0f;
			_numberOfRegisters = 0;
			_articulationData = new List<Register>();
		}

		public Movement(SerializableMovement movement)
		{
			ConvertSerializableMovement(movement);
		}

		public Movement(string movementJson)
		{
			SerializableMovement auxMovement = JsonUtility.FromJson<SerializableMovement>(movementJson);
			ConvertSerializableMovement(auxMovement);
		}

		public void SetNewSession(string label = "", string device = "", float fps = 30f, int[] articulations = null, string sessionId = "", string title = "", string description = "", string professionalId = "",
									int patientSessionNumber = 0, int appCode = 0, string appData = "", string patientId = "", int patientAge = 0, float patientHeight = 0f, float patientWeight = 0f,
									string mainComplaint = "", string historyOfCurrentDesease = "", string historyOfPastDesease = "", string diagnosis = "", string relatedDeseases = "",
									string medications = "", string physicalEvaluation = "")
		{
			_label = label;
			_device = device;
			_fps = fps;

			_articulations = articulations ?? new int[] { };
			ValidateArticulationList();

			_sessionId = sessionId;
			_title = title;
			_description = description;
			_professionalId = professionalId;
			_patientSessionNumber = patientSessionNumber;
			_appCode = appCode;
			_appData = appData;
			_patientId = patientId;
			_patientAge = patientAge;
			_patientHeight = patientHeight;
			_patientWeight = patientWeight;
			_mainComplaint = mainComplaint;
			_historyOfCurrentDesease = historyOfCurrentDesease;
			_historyOfPastDesease = historyOfPastDesease;
			_diagnosis = diagnosis;
			_relatedDeseases = relatedDeseases;
			_medications = medications;
			_physicalEvaluation = physicalEvaluation;

			_duration = 0f;
			_numberOfRegisters = 0;
			_articulationData.Clear();
		}

		public void AddRegister(Register register)
		{
			int[] registerArticulations = register.Articulations;

			if (Articulation.CompareArticulationLists(_articulations, registerArticulations))
			{
				_articulationData.Add(register);
				_numberOfRegisters += 1;
				_duration = _numberOfRegisters / _fps;
			}
			else
			{
				throw new MismatchedArticulationsExcpetion("Articulation lists do not match", _articulations, registerArticulations);
			}
		}

		public string ToJson(bool update = false)
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			if (update)
			{
				return $"{{\"movement\":{{\"label\":\"{_label}\"," +
					$"\"device\":\"{_device}\"," +
					$"\"artIndexPattern\":\"{string.Join(";", _articulations)}\"," +
					"\"session\":{" +
					$"\"title\":\"{_title}\"," +
					$"\"description\":\"{_description}\"," +
					$"\"professionalId\":\"{_professionalId}\"," +
					$"\"patientSessionNumber\":{_patientSessionNumber}}}," +
					"\"app\":{" +
					$"\"code\":{_appCode}," +
					$"\"data\":\"{_appData}\"}}," +
					"\"patient\":{" +
					$"\"id\":\"{_patientId}\"," +
					$"\"age\":{_patientAge}," +
					$"\"height\":{_patientHeight}," +
					$"\"weight\":{_patientWeight}}}," +
					"\"medicalData\":{" +
					$"\"mainComplaint\":\"{_mainComplaint}\"," +
					$"\"historyOfCurrentDesease\":\"{_historyOfCurrentDesease}\"," +
					$"\"historyOfPastDesease\":\"{_historyOfPastDesease}\"," +
					$"\"diagnosis\":\"{_diagnosis}\"," +
					$"\"relatedDeseases\":\"{_relatedDeseases}\"," +
					$"\"medications\":\"{_medications}\"," +
					$"\"physicalEvaluation\":\"{_physicalEvaluation}\"}}," +
					$"\"articulationData\":{SerializeArticulationData()}}}}}";
			}
			return $"{{\"movement\":{{\"label\":\"{_label}\"," +
				$"\"device\":\"{_device}\"," +
				$"\"artIndexPattern\":\"{string.Join(";", _articulations)}\"," +
				$"\"fps\":\"{_fps}\"," +
				$"\"duration\":{_duration}," +
				$"\"numberOfRegisters\":{_numberOfRegisters}," +
				"\"session\":{" +
				$"\"id\":\"{_sessionId}\"," +
				$"\"title\":\"{_title}\"," +
				$"\"description\":\"{_description}\"," +
				$"\"professionalId\":\"{_professionalId}\"," +
				$"\"patientSessionNumber\":{_patientSessionNumber}}}," +
				"\"app\":{" +
				$"\"code\":{_appCode}," +
				$"\"data\":\"{_appData}\"}}," +
				"\"patient\":{" +
				$"\"id\":\"{_patientId}\"," +
				$"\"age\":{_patientAge}," +
				$"\"height\":{_patientHeight}," +
				$"\"weight\":{_patientWeight}}}," +
				"\"medicalData\":{" +
				$"\"mainComplaint\":\"{_mainComplaint}\"," +
				$"\"historyOfCurrentDesease\":\"{_historyOfCurrentDesease}\"," +
				$"\"historyOfPastDesease\":\"{_historyOfPastDesease}\"," +
				$"\"diagnosis\":\"{_diagnosis}\"," +
				$"\"relatedDeseases\":\"{_relatedDeseases}\"," +
				$"\"medications\":\"{_medications}\"," +
				$"\"physicalEvaluation\":\"{_physicalEvaluation}\"}}," +
				$"\"articulationData\":{SerializeArticulationData()}}}}}";
		}

		public override string ToString()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			return $"{{\"movement\":{{\"id\":\"{_id}\"," +
				$"\"label\":\"{_label}\"," +
				$"\"device\":\"{_device}\"," +
				$"\"artIndexPattern\":\"{string.Join(";", _articulations)}\"," +
				$"\"insertionDate\":\"{_insertionDate}\"," +
				$"\"updateDate\":\"{_updateDate}\"," +
				$"\"duration\":{_duration}," +
				$"\"numberOfRegisters\":{_numberOfRegisters}," +
				"\"session\":{" +
				$"\"id\":\"{_sessionId}\"," +
				$"\"title\":\"{_title}\"," +
				$"\"description\":\"{_description}\"," +
				$"\"professionalId\":\"{_professionalId}\"," +
				$"\"patientSessionNumber\":{_patientSessionNumber}}}," +
				"\"app\":{" +
				$"\"code\":{_appCode}," +
				$"\"data\":\"{_appData}\"}}," +
				"\"patient\":{" +
				$"\"id\":\"{_patientId}\"," +
				$"\"age\":{_patientAge}," +
				$"\"height\":{_patientHeight}," +
				$"\"weight\":{_patientWeight}}}," +
				"\"medicalData\":{" +
				$"\"mainComplaint\":\"{_mainComplaint}\"," +
				$"\"historyOfCurrentDesease\":\"{_historyOfCurrentDesease}\"," +
				$"\"historyOfPastDesease\":\"{_historyOfPastDesease}\"," +
				$"\"diagnosis\":\"{_diagnosis}\"," +
				$"\"relatedDeseases\":\"{_relatedDeseases}\"," +
				$"\"medications\":\"{_medications}\"," +
				$"\"physicalEvaluation\":\"{_physicalEvaluation}\"}}," +
				$"\"articulationData\":{SerializeArticulationData()}}}}}";
		}

		private void ConvertSerializableMovement(SerializableMovement movement)
		{
			if (movement.id != null) _id = movement.id;
			if (movement.label != null) _label = movement.label;
			if (movement.device != null) _device = movement.device;
			if (movement.insertionDate != null) _insertionDate = movement.insertionDate;
			if (movement.updateDate != null) _updateDate = movement.updateDate;
			_fps = movement.fps;
			_duration = movement.duration;
			_numberOfRegisters = movement.numberOfRegisters;

			if (movement.artIndexPattern != null)
			{
				string[] splitArtIndexPatter = movement.artIndexPattern.Split(';');
				_articulations = new int[splitArtIndexPatter.Length];
				for (int i = 0; i < splitArtIndexPatter.Length; i++)
				{
					_articulations[i] = int.Parse(splitArtIndexPatter[i]);
				}
				ValidateArticulationList();
			}

			if (movement.session != null)
			{
				if (movement.session.id != null) _sessionId = movement.session.id;
				if (movement.session != null && movement.session.title != null) _title = movement.session.title;
				if (movement.session != null && movement.session.description != null) _description = movement.session.description;
				if (movement.session != null && movement.session.professionalId != null) _professionalId = movement.session.professionalId;
				_patientSessionNumber = movement.session.patientSessionNumber;
			}
			if (movement.app != null)
			{
				_appCode = movement.app.code;
				if (movement.app.data != null) _appData = movement.app.data;
			}
			if (movement.patient != null)
			{
				if (movement.patient.id != null) _patientId = movement.patient.id;
				if (movement.patient != null) _patientAge = movement.patient.age;
				if (movement.patient != null) _patientHeight = movement.patient.height;
				if (movement.patient != null) _patientWeight = movement.patient.weight;
			}
			if (movement.medicalData != null)
			{
				if (movement.medicalData.mainComplaint != null) _mainComplaint = movement.medicalData.mainComplaint;
				if (movement.medicalData != null && movement.medicalData.historyOfCurrentDesease != null) _historyOfCurrentDesease = movement.medicalData.historyOfCurrentDesease;
				if (movement.medicalData != null && movement.medicalData.historyOfPastDesease != null) _historyOfPastDesease = movement.medicalData.historyOfPastDesease;
				if (movement.medicalData != null && movement.medicalData.diagnosis != null) _diagnosis = movement.medicalData.diagnosis;
				if (movement.medicalData != null && movement.medicalData.relatedDeseases != null) _relatedDeseases = movement.medicalData.relatedDeseases;
				if (movement.medicalData != null && movement.medicalData.medications != null) _medications = movement.medicalData.medications;
				if (movement.medicalData != null && movement.medicalData.physicalEvaluation != null) _physicalEvaluation = movement.medicalData.physicalEvaluation;
			}

			_articulationData = ArticulationDataToRegisterList(movement.articulationData);
		}

		private string SerializeArticulationData()
		{
			Dictionary<int, List<Vector3>> aritulationDataDict = new Dictionary<int, List<Vector3>>();

			foreach (int articulation in _articulations)
			{
				aritulationDataDict.Add(articulation, new List<Vector3>());
			}
			foreach (Register register in _articulationData)
			{
				foreach (int articulation in aritulationDataDict.Keys)
				{
					aritulationDataDict[articulation].Add(register.GetArticulationRotations(articulation));
				}
			}

			string serializedList = "[";
			foreach (KeyValuePair<int, List<Vector3>> items in aritulationDataDict)
			{
				serializedList += $"{{\"articulation\":{items.Key},\"data\":[";
				for (int i = 0; i < items.Value.Count; i++)
				{
					Vector3 coordinates = items.Value[i];
					serializedList += $"{coordinates.x},{coordinates.y},{coordinates.z}";
					if (i < items.Value.Count - 1) serializedList += ",";
				}
				serializedList += "]},";
			}

			return serializedList.TrimEnd(',') + "]";
		}

		private List<Register> ArticulationDataToRegisterList(SerializableMovement.ArticulationData[] articulationData)
		{
			int length = 0;
			foreach (SerializableMovement.ArticulationData dataObject in articulationData)
			{
				length = dataObject.data.Length;
				break;
			}

			List<Register> registerList = new List<Register>();

			for (int j = 0; j < length; j += 3)
			{
				Register register = new Register(_articulations);
				foreach (SerializableMovement.ArticulationData dataObject in articulationData)
				{

					try
					{
						Vector3 rotations = new Vector3(dataObject.data[j], dataObject.data[j + 1], dataObject.data[j + 2]);
						register.SetArticulationRotations(dataObject.articulation, rotations);
					}
					catch (ArgumentException)
					{
						continue;
					}
				}
				registerList.Add(register);
			}
			return registerList;
		}

		private void ValidateArticulationList()
		{
			foreach (int articulationA in _articulations)
			{
				int count = 0;
				foreach (int articulationB in _articulations)
				{
					if (articulationA < 1 || articulationA > 20) throw new IndexOutOfRangeException("Articulations can't be smaller than 1 or greater than 20");
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
	}
}
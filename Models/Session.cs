using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

namespace ReBase
{
	[Serializable]
	public class Session
	{
		private string _id;
		private string _title;
		private string _description;
		private string _professionalId;
		private int _patientSessionNumber;
		private string _insertionDate;
		private string _updateDate;

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

		private int _numberOfMovements;
		private List<Movement> _movements;


		public string id { get => _id; set => _id = value; }
		public string title { get => _title; set => _title = value; }
		public string description { get => _description; set => _description = value; }
		public string professionalId { get => _professionalId; set => _professionalId = value; }
		public int patientSessionNumber { get => _patientSessionNumber; set => _patientSessionNumber = value; }
		public string insertionDate { get => _insertionDate; set => _insertionDate = value; }
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
		public int numberOfMovements{ get => _numberOfMovements; set => _numberOfMovements = value; }
		public List<Movement> movements
		{
			get => _movements;
			set => _movements = value;
		}

		public Session(string title = "", string description = "", string professionalId = "", Movement[] movements = null, int patientSessionNumber = 0,
				string insertionDate ="", string updateDate = "", string patientId = "", int patientAge = 0, float patientHeight = 0f, float patientWeight = 0f,
				string mainComplaint = "", string historyOfCurrentDesease = "", string historyOfPastDesease = "", string diagnosis = "",
				string relatedDeseases = "", string medications = "", string physicalEvaluation = "", string id = "", int numberOfMovements = 0)
		{
			_id = id;
			_title = title;
			_description = description;
			_professionalId = professionalId;
			_patientSessionNumber = patientSessionNumber;
			_insertionDate = insertionDate;
			_updateDate = updateDate;
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
			_numberOfMovements = numberOfMovements;

			if (movements != null) _movements = new List<Movement>(movements);
			else _movements = new List<Movement>();
		}

		public Session(SerializableSession session)
		{
			ConvertSerializableSession(session);
		}

		public Session(string sessionJson)
		{
			SerializableSession auxSession= JsonUtility.FromJson<SerializableSession>(sessionJson);
			ConvertSerializableSession(auxSession);
		}

		//public Session(
		//SerializableSession movement)
		//{
		//	ConvertSeri
		//leSession(movement);
		//}

		//public Session(Legacy sessionJson, bool legacySession = false)
		//{
		//	if (legacySession)
		//	{
		//		SerializableSession auxSession = JsonUtility.FromJson<SerializableSession>(movementJson);
		//	}
		//	else
		//	{
		//		LegacySerializableSession auxSession = JsonUtility.FromJson<SerializableSession>(movementJson);
		//		ConvertSerializableSession(auxSession, legacySessio);
		//	}
		//}

		private void ConvertSerializableSession(SerializableSession session)
		{
			if (session.id != null) _id = session.id;
			if (session.title != "") _title = session.title;
			if (session.description != "") _description = session.description;
			if (session.professionalId != "") _professionalId = session.professionalId;
			_patientSessionNumber = session.patientSessionNumber;
			_insertionDate = session.insertionDate;
			_updateDate = session.updateDate;

			if (session.patient != null)
			{
				if (session.patient.id != null) _patientId = session.patient.id;
				if (session.patient != null) _patientAge = session.patient.age;
				if (session.patient != null) _patientHeight = session.patient.height;
				if (session.patient != null) _patientWeight = session.patient.weight;
			}
			if (session.medicalData != null)
			{
				if (session.medicalData.mainComplaint != null) _mainComplaint = session.medicalData.mainComplaint;
				if (session.medicalData != null && session.medicalData.historyOfCurrentDesease != null) _historyOfCurrentDesease = session.medicalData.historyOfCurrentDesease;
				if (session.medicalData != null && session.medicalData.historyOfPastDesease != null) _historyOfPastDesease = session.medicalData.historyOfPastDesease;
				if (session.medicalData != null && session.medicalData.diagnosis != null) _diagnosis = session.medicalData.diagnosis;
				if (session.medicalData != null && session.medicalData.relatedDeseases != null) _relatedDeseases = session.medicalData.relatedDeseases;
				if (session.medicalData != null && session.medicalData.medications != null) _medications = session.medicalData.medications;
				if (session.medicalData != null && session.medicalData.physicalEvaluation != null) _physicalEvaluation = session.medicalData.physicalEvaluation;
			}

			_movements = new List<Movement>();
			foreach (SerializableMovement movement in session.movements)
				_movements.Add(new Movement(movement));
		}

		public string ToJson(bool update = false)
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			if (update)
			{
				return $"{{\"session\":{{\"title\":\"{_title}\"," +
					$"\"description\":\"{_description}\"," +
					$"\"professionalId\":\"{_professionalId}\"," +
					$"\"patientSessionNumber\":{_patientSessionNumber}," +
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
					$"\"physicalEvaluation\":\"{_physicalEvaluation}\"}}}}}}";
			}
			else
			{
				string strMovements = "[";
				foreach (Movement movement in _movements)
				{
					strMovements += $"{movement.ToCreateSessionJson()},";
				}
				strMovements = $"{strMovements.TrimEnd(',')}]";

				return $"{{\"session\":{{\"title\":\"{_title}\"," +
					$"\"description\":\"{_description}\"," +
					$"\"professionalId\":\"{_professionalId}\"," +
					$"\"patientSessionNumber\":{_patientSessionNumber}," +
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
					$"\"numberOfMovements\":{_numberOfMovements}," +
					$"\"movements\":{strMovements}}}}}";
			}
		}

		public override string ToString()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			string strMovements = "[";

			foreach (Movement movement in _movements)
			{
				strMovements += $"{movement},";
			}
			strMovements = $"{strMovements.TrimEnd(',')}]";

			return $"{{\"session\":{{\"_id\":\"{_id}\"," +
				$"\"title\":\"{_title}\"," +
				$"\"description\":\"{_description}\"," +
				$"\"professionalId\":\"{_professionalId}\"," +
				$"\"patientSessionNumber\":{_patientSessionNumber}," +
				$"\"insertionDate\":\"{_insertionDate}\"," +
				$"\"updateDate\":\"{_updateDate}\"," +
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
				$"\"numberOfMovements\":{_numberOfMovements}," +
				$"\"articulationData\":{strMovements}}}}}";
		}
	}
}
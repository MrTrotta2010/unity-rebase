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
		private string _historyOfCurrentDisease;
		private string _historyOfPastDisease;
		private string _diagnosis;
		private string _relatedDiseases;
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
		public string updateDate { get => _updateDate; set => _updateDate = value; }
		public string patientId { get => _patientId; set => _patientId = value; }
		public int patientAge { get => _patientAge; set => _patientAge = value; }
		public float patientHeight { get => _patientHeight; set => _patientHeight = value; }
		public float patientWeight { get => _patientWeight; set => _patientWeight = value; }
		public string mainComplaint { get => _mainComplaint; set => _mainComplaint = value; }
		public string historyOfCurrentDisease { get => _historyOfCurrentDisease; set => _historyOfCurrentDisease = value; }
		public string historyOfPastDisease { get => _historyOfPastDisease; set => _historyOfPastDisease = value; }
		public string diagnosis { get => _diagnosis; set => _diagnosis = value; }
		public string relatedDiseases { get => _relatedDiseases; set => _relatedDiseases = value; }
		public string medications { get => _medications; set => _medications = value; }
		public string physicalEvaluation { get => _physicalEvaluation; set => _physicalEvaluation = value; }
		public int numberOfMovements{ get => _numberOfMovements; set => _numberOfMovements = value; }
		public float duration
		{
			get
			{
				float sum = 0f;
				foreach (Movement movement in _movements) sum += movement.duration;
				return sum;
			}
		}
		public List<Movement> movements
		{
			get => _movements;
			set => _movements = value;
		}

		public Session(string title = "", string description = "", string professionalId = "", Movement[] movements = null, int patientSessionNumber = 0,
				string insertionDate ="", string updateDate = "", string patientId = "", int patientAge = 0, float patientHeight = 0f, float patientWeight = 0f,
				string mainComplaint = "", string historyOfCurrentDisease = "", string historyOfPastDisease = "", string diagnosis = "",
				string relatedDiseases = "", string medications = "", string physicalEvaluation = "", string id = "", int numberOfMovements = 0)
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
			_historyOfCurrentDisease = historyOfCurrentDisease;
			_historyOfPastDisease = historyOfPastDisease;
			_diagnosis = diagnosis;
			_relatedDiseases = relatedDiseases;
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
			SerializableSession auxSession = Serializer.Deserialize<SerializableSession>(sessionJson);
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
				if (session.medicalData != null && session.medicalData.historyOfCurrentDisease != null) _historyOfCurrentDisease = session.medicalData.historyOfCurrentDisease;
				if (session.medicalData != null && session.medicalData.historyOfPastDisease != null) _historyOfPastDisease = session.medicalData.historyOfPastDisease;
				if (session.medicalData != null && session.medicalData.diagnosis != null) _diagnosis = session.medicalData.diagnosis;
				if (session.medicalData != null && session.medicalData.relatedDiseases != null) _relatedDiseases = session.medicalData.relatedDiseases;
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
					$"\"historyOfCurrentDisease\":\"{_historyOfCurrentDisease}\"," +
					$"\"historyOfPastDisease\":\"{_historyOfPastDisease}\"," +
					$"\"diagnosis\":\"{_diagnosis}\"," +
					$"\"relatedDiseases\":\"{_relatedDiseases}\"," +
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
					$"\"historyOfCurrentDisease\":\"{_historyOfCurrentDisease}\"," +
					$"\"historyOfPastDisease\":\"{_historyOfPastDisease}\"," +
					$"\"diagnosis\":\"{_diagnosis}\"," +
					$"\"relatedDiseases\":\"{_relatedDiseases}\"," +
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
				$"\"historyOfCurrentDisease\":\"{_historyOfCurrentDisease}\"," +
				$"\"historyOfPastDisease\":\"{_historyOfPastDisease}\"," +
				$"\"diagnosis\":\"{_diagnosis}\"," +
				$"\"relatedDiseases\":\"{_relatedDiseases}\"," +
				$"\"medications\":\"{_medications}\"," +
				$"\"physicalEvaluation\":\"{_physicalEvaluation}\"}}," +
				$"\"numberOfMovements\":{_numberOfMovements}," +
				$"\"movements\":{strMovements}}}}}";
		}
	}
}
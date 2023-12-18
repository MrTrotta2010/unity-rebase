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
using System.Collections.Generic;

namespace ReBase
{
	[Serializable]
	public class SerializableMovement
	{
		[Serializable]
		public class AppData
		{
			public int code;
			public string data;
		}

		public string _id;
		public string label;
		public string description;
		public string device;
		public float fps;
		public float duration;
		public int numberOfRegisters;
		public string[] articulations;
		public string sessionId;
		public string patientId;
		public string professionalId;
		public string insertionDate;
		public string updateDate;
		public AppData app;
		public Dictionary<string, float[]>[] registers;

		public string id { get => _id; set => _id = value; }

		public override string ToString()
		{
			string registers_str = "";
			foreach (Dictionary<string, float[]> register in registers)
			{
				registers_str += "\n\t\t\t{ ";
				foreach (KeyValuePair<string, float[]> kvp in register)
				{
					registers_str += $"\"{kvp.Key}\": [{(string.Join("; ", kvp.Value)).Replace(',', '.').Replace(';', ',')}], ";
				}
				registers_str.TrimEnd(' ');
				registers_str.TrimEnd(',');
				registers_str += " }";
			}
			return $"{{\n\tid: \"{_id}\",\n\tlabel: \"{label}\",\n\tdescription: \"{description}\",\n\tsessionId: \"{sessionId}\",\n\tprofessionalId: \"{professionalId}\",\n\tpatientId: \"{patientId}\",\n\tdevice: \"{device}\",\n\tfps: {fps},\n\tduration: {duration},\n\tnumberOfRegisters: {numberOfRegisters},\n\tinsertionDate: {insertionDate},\n\tupdateDate: {updateDate},\n\articulations: \"{(articulations == null ? "" : string.Join(",", articulations))}\",\n\registers: [{(registers_str == "" ? "" : $"\n\t\t{registers_str}")}\n\t]\n}}";
		}
	}

	[Serializable]
	public class SerializableSession
	{
		[Serializable]
		public class PatientData
		{
			public string id;
			public int age;
			public float height;
			public float weight;
		}
		[Serializable]
		public class MedicalData
		{
			public string mainComplaint;
			public string historyOfCurrentDisease;
			public string historyOfPastDisease;
			public string diagnosis;
			public string relatedDiseases;
			public string medications;
			public string physicalEvaluation;
		}

		public string _id;
		public string title;
		public string description;
		public string professionalId;
		public int patientSessionNumber;
		public int numberOfMovements;
		public float duration;
		public string insertionDate;
		public string updateDate;

		public PatientData patient;
		public MedicalData medicalData;

		public SerializableMovement[] movements;
		public string[] movementIds;

		public string id { get => _id; set => _id = value; }

		public override string ToString()
		{
			return $"{{\n\tid: \"{_id}\",\n\ttitle: \"{title}\",\n\tdescription: {description},\n\tnumberOfMovements: {numberOfMovements},\n\tmovementIds: [{(movementIds == null ? "" : $"\n\t\t{string.Join<string>(",\n\t\t\t\t", movementIds)}\n\t\t")}\n\t\t]\n\tmovements: [{(movements == null ? "" : $"\n\t\t{string.Join<SerializableMovement>(",\n\t\t\t\t", movements)}\n\t\t")}\n\t\t]\n}}";
		}
	}

	[Serializable]
	public class MetaData
	{
		public int current_page;
		public int next_page;
		public int total_count;
		public int total_page_count;

		public int currentPage { get => current_page; }
		public int nextPage { get => next_page; }
		public int totalCount { get => total_count; }
		public int totalPageCount { get => total_page_count; }

		public override string ToString()
		{
			return $"{{ currentPage: {current_page}, nextPage: {next_page}, totalCount: {total_count}, totalPageCount: {total_page_count} }}";
		}
	}
}

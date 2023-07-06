using System;

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
		[Serializable]
		public class ArticulationData
		{
			public string articulation;
			public float[][] data;

			private string JoinData()
			{
				string[] str = new string[data.Length];
				for (int i = 0; i < data.Length; i++)
				{
					str[i] = $"[{string.Join(", ", data[i])}]";
				}
				return string.Join(", ", str);
			}

			public override string ToString()
			{
				return $"{{ articulation: {articulation}, data: [{(data != null ? JoinData() : "")}] }}";
			}
		}
		[Serializable]
		public class DataFrame
		{
			public float[] data;

			//public float this[int i]
			//{
			//	get { return data[i]; }
			//	set { data[i] = value; }
			//}

			public override string ToString()
			{
				return data != null ? $"[{(string.Join(", ", data))}]" : "";
			}
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
		public ArticulationData[] articulationData;

		public string id { get => _id; set => _id = value; }

		public override string ToString()
		{
			return $"{{\n\tid: \"{_id}\",\n\tlabel: \"{label}\",\n\tdescription: \"{description}\",\n\tsessionId: \"{sessionId}\",\n\tprofessionalId: \"{professionalId}\",\n\tpatientId: \"{patientId}\",\n\tdevice: \"{device}\",\n\tfps: {fps},\n\tduration: {duration},\n\tnumberOfRegisters: {numberOfRegisters},\n\tinsertionDate: {insertionDate},\n\tupdateDate: {updateDate},\n\articulations: \"{(articulations == null ? "" : string.Join(",", articulations))}\",\n\tarticulationData: [{(articulationData == null ? "" : $"\n\t\t{string.Join<ArticulationData>(",\n\t\t", articulationData)}")}\n\t]\n}}";
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
			public string historyOfCurrentDesease;
			public string historyOfPastDesease;
			public string diagnosis;
			public string relatedDeseases;
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

		public string id { get => _id; set => _id = value; }

		public override string ToString()
		{
			return $"{{\n\tid: \"{_id}\",\n\ttitle: \"{title}\",\n\tdescription: {description},\n\tnumberOfMovements: {numberOfMovements},\n\tmovements: [{(movements == null ? "" : $"\n\t\t{string.Join<SerializableMovement>(",\n\t\t\t\t", movements)}\n\t\t")}\n\t\t]\n}}";
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

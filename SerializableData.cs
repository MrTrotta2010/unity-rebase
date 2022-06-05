using System;

namespace ReBase
{
	[Serializable]
	public class SerializableMovement
	{
		[Serializable]
		public class SessionData
		{
			public string id;
			public string title;
			public string description;
			public string professionalId;
			public int patientSessionNumber;
			public int duration;
			public int numberOfRegisters;
		}
		[Serializable]
		public class AppData
		{
			public int code;
			public string data;
		}
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
		[Serializable]
		public class ArticulationData
		{
			public int articulation;
			public float[] data;

			public override string ToString()
			{
				return $"{{ articulation: {articulation}, data: [{(data != null ? string.Join(", ", data) : "")}] }}";
			}
		}

		public string _id;
		public string label;
		public string device;
		public string artIndexPattern;
		public DateTime insertionDate;
		public SessionData session;
		public AppData app;
		public PatientData patient;
		public MedicalData medicalData;
		public ArticulationData[] articulationData;

		public string id { get => _id; set => _id = value; }

		public override string ToString()
		{
			return $"{{ id: {_id}, label: {label}, title: {session.title}, description: {session.description}, device: {device}, artIndexPattern: {artIndexPattern}, articulationData: [{(articulationData == null ? "" : string.Join<ArticulationData>(", ", articulationData))}] }}";
		}
	}
}

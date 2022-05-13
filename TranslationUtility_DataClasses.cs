using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public partial class TranslationUtility
{
	public class SerializableMovement
	{
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
		public class AppData
		{
			public int code;
			public string data;
		}
		public class PatientData
		{
			public string id;
			public int age;
			public float height;
			public float weight;
		}
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

		public string label;
		public string device;
		public string artIndexPattern;
		public DateTime insertionDate;
		public SessionData session;
		public AppData app;
		public PatientData patient;
		public MedicalData medicalData;
		public Dictionary<int, List<Vector3>> articulationData;
	}
}

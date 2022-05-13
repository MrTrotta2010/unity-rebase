using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Movement
{
	private string label;
	private string device;
	private int[] articulations;
	private DateTime insertionDate;

	private string sessionId;
    private string title;
    private string description;
    private string professionalId;
	private int patientSessionNumber;
	private int sessionDuration;
	private int numberOfRegisters;

	private int appCode;
	private string appData;

	private string patientId;
	private int patientAge;
	private float patientHeight;
	private float patientWeight;
	private string mainComplaint;
	private string historyOfCurrentDesease;
	private string historyOfPastDesease;
	private string diagnosis;
	private string relatedDeseases;
	private string medications;
	private string physicalEvaluation;

	private List<Register> articulationData;

	public string Label { get => label; set => label = value; }
	public string Device { get => device; set => device = value; }
	public int[] Articulations { get => articulations; set => articulations = value; }
	public string SessionId { get => sessionId; set => sessionId = value; }
	public string Title { get => title; set => title = value; }
	public string Description { get => description; set => description = value; }
	public string ProfessionalId { get => professionalId; set => professionalId = value; }
	public int PatientSessionNumber { get => patientSessionNumber; set => patientSessionNumber = value; }
	public int SessionDuration { get => sessionDuration; set => sessionDuration = value; }
	public int NumberOfRegisters { get => numberOfRegisters; set => numberOfRegisters = value; }
	public int AppCode { get => appCode; set => appCode = value; }
	public string AppData { get => appData; set => appData = value; }
	public string PatientId { get => patientId; set => patientId = value; }
	public int PatientAge { get => patientAge; set => patientAge = value; }
	public float PatientHeight { get => patientHeight; set => patientHeight = value; }
	public float PatientWeight { get => patientWeight; set => patientWeight = value; }
	public string MainComplaint { get => mainComplaint; set => mainComplaint = value; }
	public string HistoryOfCurrentDesease { get => historyOfCurrentDesease; set => historyOfCurrentDesease = value; }
	public string HistoryOfPastDesease { get => historyOfPastDesease; set => historyOfPastDesease = value; }
	public string Diagnosis { get => diagnosis; set => diagnosis = value; }
	public string RelatedDeseases { get => relatedDeseases; set => relatedDeseases = value; }
	public string Medications { get => medications; set => medications = value; }
	public string PhysicalEvaluation { get => physicalEvaluation; set => physicalEvaluation = value; }
	public List<Register> ArticulationData
	{
		get => articulationData;
		set
		{
			articulationData = value;
			numberOfRegisters = articulationData.Count;
		}
	}

	public Movement(string label = "", string device = "", int[] articulations = null, string sessionId = "", string title = "", string description = "", string professionalId = "",
					int patientSessionNumber = 0, int appCode = 0, string appData = "", string patientId = "", int patientAge = 0, float patientHeight = 0f, float patientWeight = 0f,
					string mainComplaint = "", string historyOfCurrentDesease = "", string historyOfPastDesease = "", string diagnosis = "", string relatedDeseases = "",
					string medications = "", string physicalEvaluation = "")
	{
		this.label = label;
		this.device = device;

		this.articulations = articulations ?? new int[] { };
		ValidateArticulationList();

		this.sessionId = sessionId;
		this.title = title;
		this.description = description;
		this.professionalId = professionalId;
		this.patientSessionNumber = patientSessionNumber;
		this.appCode = appCode;
		this.appData = appData;
		this.patientId = patientId;
		this.patientAge = patientAge;
		this.patientHeight = patientHeight;
		this.patientWeight = patientWeight;
		this.mainComplaint = mainComplaint;
		this.historyOfCurrentDesease = historyOfCurrentDesease;
		this.historyOfPastDesease = historyOfPastDesease;
		this.diagnosis = diagnosis;
		this.relatedDeseases = relatedDeseases;
		this.medications = medications;
		this.physicalEvaluation = physicalEvaluation;

		sessionDuration = 0;
		numberOfRegisters = 0;
		articulationData = new List<Register>();
	}

	public Movement(Session session)
	{
		title = session.GetTitle();
		device = session.GetDevice();
		description = session.GetDescription();
		professionalId = session.GetProfessionalID();
		patientId = session.GetPatientID();
		label = session.GetMovementLabel();
		mainComplaint = session.GetMainComplaint();
		historyOfCurrentDesease = session.GetHistoryOfCurrentDesease();
		historyOfPastDesease = session.GetHistoryOfPastDesease();
		diagnosis = session.GetDiagnosis();
		relatedDeseases = session.GetRelatedDeseases();
		medications = session.GetMedications();
		physicalEvaluation = session.GetPhysicalEvaluation();
		patientAge = session.GetPatientAge();
		patientHeight = session.GetPatientHeight();
		patientWeight = session.GetPatientWeight();
		patientSessionNumber = session.GetPatientSessionNumber();
		sessionDuration = session.GetSessionDuration();
		articulations = session.GetArticulationList();
		articulationData = session.GetRegisterList();
		numberOfRegisters = session.GetNumberOfRegisters();
	}

	public Movement(TranslationUtility.SerializableMovement movement)
	{
		label = movement.label;
		device = movement.device;
		
		string[] splitArtIndexPatter = movement.artIndexPattern.Split(';');
		articulations = new int[splitArtIndexPatter.Length];
		for (int i = 0; i < splitArtIndexPatter.Length; i++)
		{
			articulations[i] = int.Parse(splitArtIndexPatter[i]);
		}
		ValidateArticulationList();

		sessionId = movement.session.id;
		title = movement.session.title;
		description = movement.session.description;
		professionalId = movement.session.professionalId;
		patientSessionNumber = movement.session.patientSessionNumber;
		sessionDuration = movement.session.duration;
		numberOfRegisters = movement.session.numberOfRegisters;
		appCode = movement.app.code;
		appData = movement.app.data;
		patientId = movement.patient.id;
		patientAge = movement.patient.age;
		patientHeight = movement.patient.height;
		patientWeight = movement.patient.weight;
		mainComplaint = movement.medicalData.mainComplaint;
		historyOfCurrentDesease = movement.medicalData.historyOfCurrentDesease;
		historyOfPastDesease = movement.medicalData.historyOfPastDesease;
		diagnosis = movement.medicalData.diagnosis;
		relatedDeseases = movement.medicalData.relatedDeseases;
		medications = movement.medicalData.medications;
		physicalEvaluation = movement.medicalData.physicalEvaluation;

		articulationData = DataDictionaryToRegisterList(movement.articulationData);
	}

	public void SetNewSession(string label = "", string device = "", int[] articulations = null, string sessionId = "", string title = "", string description = "", string professionalId = "",
								int patientSessionNumber = 0, int appCode = 0, string appData = "", string patientId = "", int patientAge = 0, float patientHeight = 0f, float patientWeight = 0f,
								string mainComplaint = "", string historyOfCurrentDesease = "", string historyOfPastDesease = "", string diagnosis = "", string relatedDeseases = "",
								string medications = "", string physicalEvaluation = "")
	{
		this.label = label;
		this.device = device;

		this.articulations = articulations ?? new int[] { };
		ValidateArticulationList();

		this.sessionId = sessionId;
		this.title = title;
		this.description = description;
		this.professionalId = professionalId;
		this.patientSessionNumber = patientSessionNumber;
		this.appCode = appCode;
		this.appData = appData;
		this.patientId = patientId;
		this.patientAge = patientAge;
		this.patientHeight = patientHeight;
		this.patientWeight = patientWeight;
		this.mainComplaint = mainComplaint;
		this.historyOfCurrentDesease = historyOfCurrentDesease;
		this.historyOfPastDesease = historyOfPastDesease;
		this.diagnosis = diagnosis;
		this.relatedDeseases = relatedDeseases;
		this.medications = medications;
		this.physicalEvaluation = physicalEvaluation;

		sessionDuration = 0;
		numberOfRegisters = 0;
		articulationData.Clear();
	}

	public void AddRegister(Register register)
	{
		int[] registerArticulations = register.Articulations;

		if (Articulation.CompareArticulationLists(articulations, registerArticulations))
		{
			articulationData.Add(register);
			numberOfRegisters += 1;
		}
		else
		{
			throw new MismatchedArticulationsExcpetion("Articulation lists do not match", articulations, registerArticulations);
		}
	}

	public string ToJson()
	{
		return $"{{\"movement\":{{\"label\":\"{label}\"," +
			$"\"device\":\"{device}\"," +
			$"\"artIndexPattern\":\"{string.Join(";", articulations)}\"," +
			"\"session\":{" +
			$"\"id\":\"{sessionId}\"," +
			$"\"title\":\"{title}\"," +
			$"\"description\":\"{description}\"," +
			$"\"professionalId\":\"{professionalId}\"," +
			$"\"patientSessionNumber\":{patientSessionNumber}," +
			$"\"duration\":{sessionDuration}," +
			$"\"numberOfRegisters\":{numberOfRegisters}}}," +
			"\"app\":{" +
			$"\"code\":{appCode}," +
			$"\"data\":\"{appData}\"}}," +
			"\"patient\":{" +
			$"\"id\":\"{patientId}\"," +
			$"\"age\":{patientAge}," +
			$"\"height\":{patientHeight}," +
			$"\"weight\":{patientWeight}}}," +
			"\"medicalData\":{" +
			$"\"mainComplaint\":\"{mainComplaint}\"," +
			$"\"historyOfCurrentDesease\":\"{historyOfCurrentDesease}\"," +
			$"\"historyOfPastDesease\":\"{historyOfPastDesease}\"," +
			$"\"diagnosis\":\"{diagnosis}\"," +
			$"\"relatedDeseases\":\"{relatedDeseases}\"," +
			$"\"medications\":\"{medications}\"," +
			$"\"physicalEvaluation\":\"{physicalEvaluation}\"}}," +
			$"\"articulationData\":{SerializeDataDictionary()}}}}}";
	}

	private string SerializeDataDictionary()
	{
		Dictionary<int, List<Vector3>> aritulationDataDict = new Dictionary<int, List<Vector3>>();

		foreach (int articulation in articulations)
		{
			aritulationDataDict.Add(articulation, new List<Vector3>());
		}
		foreach (Register register in articulationData)
		{
			foreach (int articulation in aritulationDataDict.Keys)
			{
				aritulationDataDict[articulation].Add(register.GetArticulationRotations(articulation));
			}
		}

		string serializedDictionary = "{";
		foreach (KeyValuePair<int, List<Vector3>> items in aritulationDataDict)
		{
			serializedDictionary += $"\"{items.Key}\":[";
			for (int i = 0; i < items.Value.Count; i++)
			{
				Vector3 coordinates = items.Value[i];
				serializedDictionary += $"[{coordinates.x},{coordinates.y},{coordinates.z}]";
				if (i < items.Value.Count - 1) serializedDictionary += ",";
			}
			serializedDictionary += "],";
		}

		return serializedDictionary.TrimEnd(',') + "}";
	}

	private List<Register> DataDictionaryToRegisterList(Dictionary<int, List<Vector3>> dictionary)
	{
		int length = 0;
		foreach (int key in dictionary.Keys)
		{
			length = dictionary[key].Count;
			break;
		}

		List<Register> registerList = new List<Register>();

		for (int j = 0; j < length; j++)
		{
			Register register = new Register(articulations);
			foreach (int articulation in dictionary.Keys)
			{
				register.SetArticulationRotations(articulation, dictionary[articulation][j]);
			}
			registerList.Add(register);
		}
		return registerList;
	}

	private void ValidateArticulationList()
	{
		foreach (int articulationA in articulations)
		{
			int count = 0;
			foreach (int articulationB in articulations)
			{
				if (articulationA < 1 || articulationA > 20) throw new IndexOutOfRangeException("Articulations can't be smaller than 1 or greater than 20");
				if (articulationA == articulationB)
				{
					count++;
					if (count > 1)
					{
						throw new RepeatedArticulationException("Repeated articulation in list", articulations);
					}
				}
			}
		}
	}
}
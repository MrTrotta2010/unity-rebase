using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Session
{
	private string title;
	private string device;
	private string description;
	private string professionalid;
	private string patientid;
	private string movementlabel;

	private string maincomplaint;
	private string historyofcurrentdesease;
	private string historyofpastdesease;
	private string diagnosis;
	private string relateddeseases;
	private string medications;
	private string physicalevaluation;

	private int patientage;
	private float patientheight;
	private float patientweight;
	private int patientsessionnumber;
	private int sessionduration;
	private int numberofregisters;
	private string artindexpattern;
	private List<Register> registerList;

	public Session(string title = "", string device = "", string description = "", string professionalid = "",
					string patientid = "", string movementlabel = "", int[] articulations = null, int patientage = 0,
					float patientheight = 0.0f, float patientweight = 0.0f, string maincomplaint = "",
					string historyofcurrentdesease = "", string historyofpastdesease = "", string diagnosis = "",
					string relateddeseases = "", string medications = "", string physicalevaluation = "",
					int patientsessionnumber = 0, int sessionduration = 0)
	{
		this.title = title;
		this.device = device;
		this.description = description;
		this.professionalid = professionalid;
		this.patientid = patientid;
		this.movementlabel = movementlabel;
		this.maincomplaint = maincomplaint;
		this.historyofcurrentdesease = historyofcurrentdesease;
		this.historyofpastdesease = historyofpastdesease;
		this.diagnosis = diagnosis;
		this.relateddeseases = relateddeseases;
		this.medications = medications;
		this.physicalevaluation = physicalevaluation;
		this.patientage = patientage;
		this.patientheight = patientheight;
		this.patientweight = patientweight;
		this.patientsessionnumber = patientsessionnumber;
		this.sessionduration = sessionduration;

		if (articulations == null)
		{
			artindexpattern = "";
		}
		else
		{
			SetArtIndexPattern(articulations);
		}

		numberofregisters = 0;
		registerList = new List<Register>();
	}

	public Session(string title = "", string device = "", string description = "", string professionalid = "",
					string patientid = "", string movementlabel = "", string artindexpattern = "", int patientage = 0,
					float patientheight = 0.0f, float patientweight = 0.0f, string maincomplaint = "",
					string historyofcurrentdesease = "", string historyofpastdesease = "", string diagnosis = "",
					string relateddeseases = "", string medications = "", string physicalevaluation = "",
					int patientsessionnumber = 0, int sessionduration = 0)
	{
		this.title = title;
		this.device = device;
		this.description = description;
		this.professionalid = professionalid;
		this.patientid = patientid;
		this.movementlabel = movementlabel;
		this.maincomplaint = maincomplaint;
		this.historyofcurrentdesease = historyofcurrentdesease;
		this.historyofpastdesease = historyofpastdesease;
		this.diagnosis = diagnosis;
		this.relateddeseases = relateddeseases;
		this.medications = medications;
		this.physicalevaluation = physicalevaluation;
		this.patientage = patientage;
		this.patientheight = patientheight;
		this.patientweight = patientweight;
		this.patientsessionnumber = patientsessionnumber;
		this.sessionduration = sessionduration;
		this.artindexpattern = artindexpattern;

		numberofregisters = 0;
		registerList = new List<Register>();
	}

	public Session(Session session)
	{
		title = session.GetTitle();
		device = session.GetDevice();
		description = session.GetDescription();
		professionalid = session.GetProfessionalID();
		patientid = session.GetPatientID();
		movementlabel = session.GetMovementLabel();
		maincomplaint = session.GetMainComplaint();
		historyofcurrentdesease = session.GetHistoryOfCurrentDesease();
		historyofpastdesease = session.GetHistoryOfPastDesease();
		diagnosis = session.GetDiagnosis();
		relateddeseases = session.GetRelatedDeseases();
		medications = session.GetMedications();
		physicalevaluation = session.GetPhysicalEvaluation();
		patientage = session.GetPatientAge();
		patientheight = session.GetPatientHeight();
		patientweight = session.GetPatientWeight();
		patientsessionnumber = session.GetPatientSessionNumber();
		sessionduration = session.GetSessionDuration();
		artindexpattern = session.GetArtIndexPattern();

		registerList = new List<Register>();
		numberofregisters = 0;
	}

	public void SetNewSession(string title = "", string device = "", string description = "", string professionalid = "",
								string patientid = "", string movementlabel = "", int[] articulations = null, int patientage = 0,
								float patientheight = 0.0f, float patientweight = 0.0f, string maincomplaint = "",
								string historyofcurrentdesease = "", string historyofpastdesease = "", string diagnosis = "",
								string relateddeseases = "", string medications = "", string physicalevaluation = "",
								int patientsessionnumber = 0, int sessionduration = 0)
	{
		this.title = title;
		this.device = device;
		this.description = description;
		this.professionalid = professionalid;
		this.patientid = patientid;
		this.movementlabel = movementlabel;
		this.maincomplaint = maincomplaint;
		this.historyofcurrentdesease = historyofcurrentdesease;
		this.historyofpastdesease = historyofpastdesease;
		this.diagnosis = diagnosis;
		this.relateddeseases = relateddeseases;
		this.medications = medications;
		this.physicalevaluation = physicalevaluation;
		this.patientage = patientage;
		this.patientheight = patientheight;
		this.patientweight = patientweight;
		this.patientsessionnumber = patientsessionnumber;
		this.sessionduration = sessionduration;

		if (articulations == null)
		{
			artindexpattern = "";
		}
		else
		{
			SetArtIndexPattern(articulations);
		}

		numberofregisters = 0;
		registerList.Clear();
	}

	public void SetNewSession(string title = "", string device = "", string description = "", string professionalid = "",
								string patientid = "", string movementlabel = "", string artindexpattern = "", int patientage = 0,
								float patientheight = 0.0f, float patientweight = 0.0f, string maincomplaint = "",
								string historyofcurrentdesease = "", string historyofpastdesease = "", string diagnosis = "",
								string relateddeseases = "", string medications = "", string physicalevaluation = "",
								int patientsessionnumber = 0, int sessionduration = 0)
	{
		this.title = title;
		this.device = device;
		this.description = description;
		this.professionalid = professionalid;
		this.patientid = patientid;
		this.movementlabel = movementlabel;
		this.maincomplaint = maincomplaint;
		this.historyofcurrentdesease = historyofcurrentdesease;
		this.historyofpastdesease = historyofpastdesease;
		this.diagnosis = diagnosis;
		this.relateddeseases = relateddeseases;
		this.medications = medications;
		this.physicalevaluation = physicalevaluation;
		this.patientage = patientage;
		this.patientheight = patientheight;
		this.patientweight = patientweight;
		this.patientsessionnumber = patientsessionnumber;
		this.sessionduration = sessionduration;
		this.artindexpattern = artindexpattern;

		numberofregisters = 0;
		registerList.Clear();
	}

	public void AddRegister(Register register)
	{
		if (artindexpattern == register.GetArticulationIndexPattern())
		{
			registerList.Add(register);
			numberofregisters += 1;
		}
		else
		{
			throw new MismatchedArticulationsExcpetion("Articulation lists don't match", new int[] { }, register.GetArticulationList());
		}
	}

	private string GetArtIndexPatternFromArray(int[] array)
	{
		if (CheckForRepetitionInArray(array) == 0)
		{
			string pattern = "";
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (1 <= array[i] && array[i] <= 20)
				{
					pattern += "a" + array[i] + ";";
				}
				else
				{
					throw new IndexOutOfRangeException("Articulations can't be smaller than 1 or greater than 20");
				}
			}
			if (1 <= array[array.Length - 1] && array[array.Length - 1] <= 20)
			{
				pattern += "a" + array[array.Length - 1];
			}
			else
			{
				throw new IndexOutOfRangeException("Articulations can't be smaller than 1 or greater than 20");
			}
			return pattern;
		}
		else
		{
			throw new ArgumentException("Duplicate articulation in list");
		}
	}

	// Retorna 0 se não houver repetição do array e 1 se houver
	private int CheckForRepetitionInArray(int[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			int count = 0;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[i] == array[j])
				{
					count++;
				}
			}
			if (count > 1)
			{
				return 1;
			}
		}
		return 0;
	}

	public string GetTitle()
	{
		return title;
	}

	public void SetTitle(string value)
	{
		title = value;
	}

	public string GetDevice()
	{
		return device;
	}

	public void SetDevice(string value)
	{
		device = value;
	}

	public string GetDescription()
	{
		return description;
	}

	public void SetDescription(string value)
	{
		description = value;
	}

	public string GetProfessionalID()
	{
		return professionalid;
	}

	public void SetProfessionalID(string value)
	{
		professionalid = value;
	}

	public string GetPatientID()
	{
		return patientid;
	}

	public void SetPatientID(string value)
	{
		patientid = value;
	}


	public string GetMovementLabel()
	{
		return movementlabel;
	}

	public void SetMovementLabel(string value)
	{
		movementlabel = value;
	}

	public string GetHistoryOfCurrentDesease()
	{
		return historyofcurrentdesease;
	}

	public void SetHistoryOfCurrentDesease(string value)
	{
		historyofcurrentdesease = value;
	}

	public string GetHistoryOfPastDesease()
	{
		return historyofpastdesease;
	}

	public void SetHistoryOfPastDesease(string value)
	{
		historyofpastdesease = value;
	}

	public string GetDiagnosis()
	{
		return diagnosis;
	}

	public void SetDiagnosis(string value)
	{
		diagnosis = value;
	}

	public string GetRelatedDeseases()
	{
		return relateddeseases;
	}

	public void SetRelatedDeseases(string value)
	{
		relateddeseases = value;
	}

	public string GetMedications()
	{
		return medications;
	}

	public void SetMedications(string value)
	{
		medications = value;
	}

	public string GetPhysicalEvaluation()
	{
		return physicalevaluation;
	}

	public void SetPhysicalEvaluation(string value)
	{
		physicalevaluation = value;
	}

	public int GetPatientAge()
	{
		return patientage;
	}

	public void SetPatientAge(int value)
	{
		patientage = value;
	}

	public float GetPatientHeight()
	{
		return patientheight;
	}

	public void SetPatientHeight(float value)
	{
		patientheight = value;
	}

	public float GetPatientWeight()
	{
		return patientweight;
	}

	public void SetPatientWeight(float value)
	{
		patientweight = value;
	}

	public int GetPatientSessionNumber()
	{
		return patientsessionnumber;
	}

	public void SetPatientSessionNumber(int value)
	{
		patientsessionnumber = value;
	}

	public int GetSessionDuration()
	{
		return sessionduration;
	}

	public void SetSessionDuration(int value)
	{
		sessionduration = value;
	}

	public string GetArtIndexPattern()
	{
		return artindexpattern;
	}

	public int[] GetArticulationList()
	{
		List<int> articulationList = new List<int>();

		foreach (string art in artindexpattern.Split(';'))
		{
			articulationList.Add(int.Parse(art.Replace("a", "")));
		}

		return articulationList.ToArray();
	}

	public void SetArtIndexPattern(string value)
	{
		artindexpattern = value;
	}

	public void SetArtIndexPattern(int[] array)
	{
		try
		{
			artindexpattern = GetArtIndexPatternFromArray(array);
		}
		catch (IndexOutOfRangeException)
		{
			throw new IndexOutOfRangeException("Articulations can't be smaller than 1 or greater than 20");
		}
		catch (ArgumentException)
		{
			throw new ArgumentException("Duplicate articulation in list");
		}
	}

	public int GetNumberOfRegisters()
	{
		return numberofregisters;
	}

	public List<Register> GetRegisterList()
	{
		return registerList;
	}

	public void SetRegisterList(List<Register> value)
	{
		registerList = new List<Register>(value);
		numberofregisters = registerList.Count;
	}

	public string GetMainComplaint()
	{
		return maincomplaint;
	}

	public void SetMainComplaint(string value)
	{
		maincomplaint = value;
	}
}
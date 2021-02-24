[System.Serializable]
public class SerializableSession
{
	public string id;

	public string title;
	public string device;
	public string description;
	public string professionalid;
	public string patientname;

	public string maincomplaint;
	public string historyofcurrentdesease;
	public string historyofpastdesease;
	public string diagnosis;
	public string relateddeseases;
	public string medications;
	public string physicalevaluation;

	public int patientage;
	public float patientheight;
	public float patientweight;
	public int patientsessionnumber;
	public int sessionduration;
	public int numberofregisters;
	public string artindexpattern;
	public SessionData sessiondata;
	public string insertiondate;
}
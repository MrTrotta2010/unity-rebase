using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class TranslationUtility
{
	public static string SessionToJson(Session session, string sessionID)
	{
		CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

		string[] stringArray = (string[])session.GetRegisterList().ToArray(typeof(string));
		if (stringArray == null)
		{
			stringArray = new string[] { "" };
		}

		return "{\"id\":\"" + sessionID + "\"," +
			   "\"title\":\"" + session.GetTitle() + "\"," +
			   "\"device\":\"" + session.GetDevice() + "\"," +
			   "\"description\":\"" + session.GetDescription() + "\"," +
			   "\"professionalid\":\"" + session.GetProfessionalID() + "\"," +
			   "\"patientid\":\"" + session.GetPatientID() + "\"," +
			   "\"movementlabel\":\"" + session.GetMovementLabel() + "\"," +
			   "\"maincomplaint\":\"" + session.GetMainComplaint() + "\"," +
			   "\"historyofcurrentdesease\":\"" + session.GetHistoryOfCurrentDesease() + "\"," +
			   "\"historyofpastdesease\":\"" + session.GetHistoryOfPastDesease() + "\"," +
			   "\"diagnosis\":\"" + session.GetDiagnosis() + "\"," +
			   "\"relateddeseases\":\"" + session.GetRelatedDeseases() + "\"," +
			   "\"medications\":\"" + session.GetMedications() + "\"," +
			   "\"physicalevaluation\":\"" + session.GetPhysicalEvaluation() + "\"," +
			   "\"patientage\":" + session.GetPatientAge() + "," +
			   "\"patientheight\":" + session.GetPatientHeight() + "," +
			   "\"patientweight\":" + session.GetPatientWeight() + "," +
			   "\"patientsessionnumber\":" + session.GetPatientSessionNumber() + "," +
			   "\"sessionduration\":" + session.GetSessionDuration() + "," +
			   "\"numberofregisters\":" + session.GetNumberOfRegisters() + "," +
			   "\"artindexpattern\":\"" + session.GetArtIndexPattern() + "\"," +
			   "\"sessiondata\":\"" + Convert.ToBase64String((CompressionUtility.Compress(string.Join("/", stringArray)))) + "\"}";
	}

	public static SessionList ParseSessionList(string json)
	{
		if (json != "[]")
		{
			string[] stringSessionList = (json.Trim('[', ']')).Split(new string[] { "},{" }, StringSplitOptions.None);
			List<SerializableSession> list = new List<SerializableSession>(stringSessionList.Length);

			for (int i = 0; i < stringSessionList.Length; i++)
			{
				if (stringSessionList[i][0] != '{')
					stringSessionList[i] = "{" + stringSessionList[i];
				if (stringSessionList[i][stringSessionList[i].Length - 1] != '}')
					stringSessionList[i] += "}";

				list.Add(JsonUtility.FromJson<SerializableSession>(stringSessionList[i]));
			}

			return new SessionList(list);
		}
		return null;
	}

	public static void TranslateSessionListData(SessionList sessionList)
	{
		foreach (SerializableSession session in sessionList.list)
		{
			Debug.Log(JsonUtility.ToJson(session));
			byte[] translatedBytes = new byte[session.sessiondata.data.Count];

			for (int i = 0; i < translatedBytes.Length; i++)
				translatedBytes[i] = Convert.ToByte(Convert.ToInt32(session.sessiondata.data[i]));

			string base64string = Encoding.UTF8.GetString(translatedBytes);
			translatedBytes = Convert.FromBase64String(base64string);
			session.sessiondata.translateddata = CompressionUtility.Decompress(translatedBytes);
		}
	}

	public static void TranslateSessionData(SerializableSession session)
	{
		byte[] translatedBytes = new byte[session.sessiondata.data.Count];

		for (int i = 0; i < translatedBytes.Length; i++)
			translatedBytes[i] = Convert.ToByte(Convert.ToInt32(session.sessiondata.data[i]));

		string base64string = Encoding.UTF8.GetString(translatedBytes);
		translatedBytes = Convert.FromBase64String(base64string);
		session.sessiondata.translateddata = CompressionUtility.Decompress(translatedBytes);
	}
}

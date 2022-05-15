using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace ReBase
{
	public class TranslationUtility
	{
		public static string SessionToJson(Session session, string sessionID)
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

			Register[] stringArray = session.GetRegisterList().ToArray();
			if (stringArray == null)
			{
				stringArray = new Register[] { };
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
				   "\"artindexpattern\":\"" + session.GetArtIndexPattern() + "\",";
			//"\"sessiondata\":\"" + Convert.ToBase64String(CompressionUtility.Compress(string.Join<>("/", stringArray))) + "\"}";
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

		public static Movement ParseMovement(string movementJson)
		{
			SerializableMovement auxMovement = JsonUtility.FromJson<SerializableMovement>(movementJson);
			return new Movement(auxMovement);
		}

		public static BaseResponse ParseAPIResponse(string response, int responseCode)
		{
			return JsonUtility.FromJson<BaseResponse>(response);
		}

		public static FetchMovementResponse ParseFetchMovementResponse(string response, long responseCode)
		{
			FetchMovementResponse responseObj = JsonUtility.FromJson<FetchMovementResponse>(response);
			responseObj.code = responseCode;
			return responseObj;
		}

		public static InsertMovementResponse ParseInsertMovementResponse(string response, long responseCode)
		{
			InsertMovementResponse responseObj = JsonUtility.FromJson<InsertMovementResponse>(response);
			responseObj.code = responseCode;
			return responseObj;
		}
	}
}

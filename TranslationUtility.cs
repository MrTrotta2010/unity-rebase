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

		public static Movement ParseMovement(string movementJson)
		{
			SerializableMovement auxMovement = JsonUtility.FromJson<SerializableMovement>(movementJson);
			return new Movement(auxMovement);
		}

		public static APIResponse ParseAPIResponse(APIResponse.ResponseType responseType, string response, long responseCode)
		{
			APIResponse responseObject = JsonUtility.FromJson<APIResponse>(response);
			responseObject.responseType = responseType;
			responseObject.code = responseCode;
			return responseObject;
		}
	}
}

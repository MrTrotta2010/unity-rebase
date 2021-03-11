using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Security.Cryptography;

public class RESTClient
{
	private static readonly RESTClient instance = new RESTClient();
	public static RESTClient Instance { get { return instance; } }

	// private string WEB_URL = "http://ec2-3-16-40-77.us-east-2.compute.amazonaws.com:3000";
	// private string WEB_URL = "http://192.168.0.113:3000";
	// private string WEB_URL = "http://brainnvr.ddns.net:3000";
	[SerializeField] private string WEB_URL = "http://200.145.46.239:3000";
	private UnityWebRequest www;
	private string sessionId;

	private RESTClient()
	{
		GenerateNewSessionID();
	}

	public IEnumerator DownloadAllSessions(Action<bool, string> callback, string professionalId = "", string patientName = "")
	{
		string response = "Request could not be completed properly";
		bool success = false;

		string fullUrl = WEB_URL + "/get";
		if (professionalId != "" && patientName != "")
        {
			fullUrl += "/professionalpatient/" + professionalId + "/" + patientName;
        }
		else if (professionalId != "")
        {
			fullUrl += "/professionalid/" + professionalId;
		}
		else if (patientName != "")
		{
			fullUrl += "/patientname/" + patientName;
		}

		www = UnityWebRequest.Get(fullUrl);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			success = false;
			response = www.error;
		}
		else if (www.isDone)
		{
			while (!www.downloadHandler.isDone) { } // Aguarda caso o download handler não tenha completado os processamentos
			success = true;
			response = www.downloadHandler.text;
		}
		www.Dispose();

		callback(success, response);
	}

	public IEnumerator UploadSession(Session session, Action<bool, string> callback)
	{
		string json = TranslationUtility.SessionToJson(session, sessionId);
		string response = "Request could not be completed properly";
		bool success = false;

		using (UnityWebRequest www = UnityWebRequest.Post(WEB_URL + "/post", json))
		{
			www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
			www.uploadHandler.contentType = "application/json";

			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				success = false;
				response = www.error;
			}
			else if (www.isDone)
			{
				while (!www.downloadHandler.isDone) { } // Aguarda caso o download handler não tenha completado os processamentos
				response = www.downloadHandler.text;
				success = true;
				if (response != "{\"[applied]\":true}")
				{
					success = false;
					response = "Failed to store session with duplicate ID. Try again!";
				}
			}
			www.Dispose();
		}
		callback(success, response);
	}

	public IEnumerator DeleteAll(Action<bool, string> callback)
	{
		using (UnityWebRequest www = UnityWebRequest.Delete(WEB_URL + "/deleteall"))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				string errorString = www.error;
				www.Dispose();
				callback(false, errorString);
			}
			else if (www.isDone)
			{
				www.Dispose();
				callback(true, "");
			}
		}
	}

	public string GenerateNewSessionID()
	{
		char letter;
		int shift;
		StringBuilder str_build = new StringBuilder();
		System.Random random = new System.Random();

		for (int i = 0; i < 50; i++)
		{
			double flt = random.NextDouble();
			int r = random.Next(3);

			switch (r)
			{
				case 0: // Número
					shift = Convert.ToInt32(Math.Floor(9 * flt));
					letter = Convert.ToChar(shift + 48);
					break;

				case 1: // Letra máiúscula
					shift = Convert.ToInt32(Math.Floor(25 * flt));
					letter = Convert.ToChar(shift + 65);
					break;

				default: // Letra minúscula
					shift = Convert.ToInt32(Math.Floor(25 * flt));
					letter = Convert.ToChar(shift + 97);
					break;
			}
			str_build.Append(letter);
		}
		return GetHashString(str_build.ToString());
	}

	private byte[] GetHash(string inputString)
	{
		using (HashAlgorithm algorithm = SHA256.Create())
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
	}

	private string GetHashString(string inputString)
	{
		StringBuilder sb = new StringBuilder();
		foreach (byte b in GetHash(inputString))
			sb.Append(b.ToString("X2"));

		return sb.ToString();
	}
}

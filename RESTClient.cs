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

	private string WEB_URL = "http://200.145.46.239:3030";
	//private UnityWebRequest www;
	private string sessionId;

	private RESTClient()
	{
		GenerateNewSessionID();
	}

	public IEnumerator DownloadSessions(Action<string> callback, string professionalId = "", string patientId = "", string movementLabel = "",
										int[] articulations = null, bool legacy = false, int page = 0, int limit = 0)
	{
		int[] artList = articulations ?? new int[] { };

		string fullUrl = $"{WEB_URL}/session?professionalid={professionalId}&patientid={patientId}&movementLabel={movementLabel}&articulations={string.Join(",", artList)}&legacy={legacy}";
		if (page > 0) fullUrl += $"&page={page}";
		if (limit > 0) fullUrl += $"&limit={limit}";

		UnityWebRequest request = UnityWebRequest.Get(fullUrl);
		request.method = "GET";
		yield return request.SendWebRequest();

		long responseCode;
		string response = GetAPIResponse(request, out responseCode);

		request.Dispose();

		callback(response);
	}

	public IEnumerator FetchMovements(Action<TranslationUtility.FetchMovementResponse> callback, string professionalId = "", string patientId = "", string movementLabel = "",
										int[] articulations = null, bool legacy = false, int page = 0, int limit = 0)
	{
		int[] artList = articulations ?? new int[] { };

		string fullUrl = $"{WEB_URL}/movement?professionalid={professionalId}&patientid={patientId}&movementLabel={movementLabel}&articulations={string.Join(",", artList)}&legacy={legacy}";
		if (page > 0) fullUrl += $"&page={page}";
		if (limit > 0) fullUrl += $"&limit={limit}";

		Debug.Log($"Endpoint: {fullUrl}");

		UnityWebRequest request = UnityWebRequest.Get(fullUrl);
		request.method = "GET";
		yield return request.SendWebRequest();

		long responseCode;
		string responseStr = GetAPIResponse(request, out responseCode);

		TranslationUtility.FetchMovementResponse response = TranslationUtility.ParseFetchMovementResponse(responseStr, responseCode);
		request.Dispose();

		callback(response);
	}

	public IEnumerator InsertMovement(Action<string> callback, Movement movement)
	{
		string json = movement.ToJson();

		using (UnityWebRequest request = UnityWebRequest.Post(WEB_URL + "/movement", json))
		{
			request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
			request.uploadHandler.contentType = "application/json";
			request.method = "POST";

			yield return request.SendWebRequest();

			long responseCode;
			string response = GetAPIResponse(request, out responseCode);

			request.Dispose();
			callback(response);
		}
	}

	public IEnumerator UploadSession(Session session, Action<bool, string> callback)
	{
		if (session.GetMovementLabel().Contains(","))
        {
			callback(false, "Caractere inválido no campo \"movementlabel\": ','");
        }
		else
        {
			string json = TranslationUtility.SessionToJson(session, sessionId);
			string response = "Request could not be completed properly";
			bool success = false;

			using (UnityWebRequest www = UnityWebRequest.Post(WEB_URL + "/post", json))
			{
				www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
				www.uploadHandler.contentType = "application/json";
				www.method = "POST";

				yield return www.SendWebRequest();

				if (IsNetworkError(www) || IsHTTPError(www))
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
	}

	public IEnumerator UploadSession(string sessionJson, Action<bool, string> callback)
	{
		string response = "Request could not be completed properly";
		bool success = false;

		using (UnityWebRequest www = UnityWebRequest.Post(WEB_URL + "/post", sessionJson))
		{
			www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(sessionJson));
			www.uploadHandler.contentType = "application/json";
			www.method = "POST";

			yield return www.SendWebRequest();

			if (IsNetworkError(www) || IsHTTPError(www))
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

	public IEnumerator UpdateSession(string id, string insertiondate, Session session, Action<bool, string> callback)
	{
		string json = TranslationUtility.SessionToJson(session, sessionId);
		string response = "Request could not be completed properly";
		bool success = false;

		using (UnityWebRequest www = UnityWebRequest.Put(WEB_URL + "/put/movement/" + id + "/" + session.GetMovementLabel() + "/" + insertiondate, json))
		{
			www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
			www.uploadHandler.contentType = "application/json";
			www.method = "PUT";

			yield return www.SendWebRequest();

			if (IsNetworkError(www) || IsHTTPError(www))
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
		}
		callback(success, response);
	}

	public IEnumerator UpdateSession(SerializableSession session, Action<bool, string> callback)
	{
		string json = JsonUtility.ToJson(session);
		string response = "Request could not be completed properly";
		bool success = false;

		using (UnityWebRequest www = UnityWebRequest.Put(WEB_URL + "/put/movement/" + session.id + "/" + session.movementlabel + "/" + session.insertiondate, json))
		{
			www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
			www.uploadHandler.contentType = "application/json";
			www.method = "PUT";

			yield return www.SendWebRequest();

			if (IsNetworkError(www) || IsHTTPError(www))
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
		}
		callback(success, response);
	}

	public IEnumerator DeleteAll(Action<bool, string> callback)
	{
		using (UnityWebRequest www = UnityWebRequest.Delete(WEB_URL + "/deleteall"))
		{
			www.method = "DELETE";
			yield return www.SendWebRequest();

			if (IsNetworkError(www) || IsHTTPError(www))
			{
				string errorString = www.error;
				www.Dispose();
				callback(false, errorString);
			}
			else if (www.isDone)
			{
				string response = www.downloadHandler.text;
				www.Dispose();
				callback(true, response);
			}
		}
	}

	public IEnumerator DeleteSession(string id, string movementlabel, string insertiondate, Action<bool, string> callback)
	{
		using (UnityWebRequest www = UnityWebRequest.Delete(WEB_URL + "/delete/movement/" + id + "/"+ movementlabel + "/" + insertiondate))
		{
			www.downloadHandler = new DownloadHandlerBuffer();
			www.method = "DELETE";
			yield return www.SendWebRequest();

			if (IsNetworkError(www) || IsHTTPError(www))
			{
				string errorString = www.error;
				www.Dispose();
				callback(false, errorString);
			}
			else if (www.isDone)
			{
				while (!www.downloadHandler.isDone) { } // Aguarda caso o download handler não tenha completado os processamentos
				string response = www.downloadHandler.text;
				www.Dispose();
				callback(true, response);
			}
		}
	}

	public IEnumerator DeleteSession(SerializableSession session, Action<bool, string> callback)
	{
		using (UnityWebRequest www = UnityWebRequest.Delete(WEB_URL + "/delete/movement/" + session.id + "/"+ session.movementlabel + "/" + session.insertiondate))
		{
			www.downloadHandler = new DownloadHandlerBuffer();
			www.method = "DELETE";
			yield return www.SendWebRequest();

			if (IsNetworkError(www) || IsHTTPError(www))
			{
				string errorString = www.error;
				www.Dispose();
				callback(false, errorString);
			}
			else if (www.isDone)
			{
				while (!www.downloadHandler.isDone) { } // Aguarda caso o download handler não tenha completado os processamentos
				string response = www.downloadHandler.text;
				www.Dispose();
				callback(true, response);
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
		sessionId = GetHashString(str_build.ToString());
		return sessionId;
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

	private bool IsHTTPError(UnityWebRequest request)
	{
		return request.result == UnityWebRequest.Result.ProtocolError;
	}

	private bool IsNetworkError(UnityWebRequest request)
	{
		return request.result == UnityWebRequest.Result.ConnectionError;
	}

	private string GetAPIResponse(UnityWebRequest request, out long responseCode)
	{
		if (IsNetworkError(request) || IsHTTPError(request))
		{
			responseCode = request.responseCode;
			return request.downloadHandler.text;
		}
		if (!request.isDone)
		{
			responseCode = 0;
			return "";
		}

		while (!request.downloadHandler.isDone) { } // Aguarda caso o download handler não tenha completado os processamentos

		responseCode = request.responseCode;
		return request.downloadHandler.text;
	}
}

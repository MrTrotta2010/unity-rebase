using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Security.Cryptography;

namespace ReBase
{
	public class RESTClient
	{
		private static readonly RESTClient instance = new RESTClient();
		public static RESTClient Instance { get { return instance; } }

		private string WEB_URL = "http://200.145.46.239:3030";
		//private UnityWebRequest www;

		public IEnumerator DownloadSessions(Action<APIResponse> callback, string professionalId = "", string patientId = "", string movementLabel = "",
											int[] articulations = null, bool legacy = false, int page = 0, int limit = 0)
		{
			int[] artList = articulations ?? new int[] { };

			string fullUrl = $"{WEB_URL}/session?professionalid={professionalId}&patientid={patientId}&movementLabel={movementLabel}&articulations={string.Join(",", artList)}&legacy={legacy}";
			if (page > 0) fullUrl += $"&page={page}";
			if (limit > 0) fullUrl += $"&limit={limit}";

			UnityWebRequest request = UnityWebRequest.Get(fullUrl);
			request.method = "GET";
			yield return request.SendWebRequest();

			APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.FetchMovements);
			request.Dispose();

			callback(response);
		}

		public IEnumerator FetchMovements(Action<APIResponse> callback, string professionalId = "", string patientId = "", string movementLabel = "",
											int[] articulations = null, int page = 0, int limit = 0)
		{
			int[] artList = articulations ?? new int[] { };

			string fullUrl = $"{WEB_URL}/movement?professionalid={professionalId}&patientid={patientId}&movementLabel={movementLabel}&articulations={string.Join(",", artList)}";
			if (page > 0) fullUrl += $"&page={page}";
			if (limit > 0) fullUrl += $"&limit={limit}";

			UnityWebRequest request = UnityWebRequest.Get(fullUrl);
			request.method = "GET";
			yield return request.SendWebRequest();

			APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.FetchMovements);
			request.Dispose();

			callback(response);
		}

		public IEnumerator FindMovement(Action<APIResponse> callback, string id)
		{
			UnityWebRequest request = UnityWebRequest.Get($"{WEB_URL}/movement/{id}");
			request.method = "GET";
			yield return request.SendWebRequest();

			APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.FindMovement);
			request.Dispose();

			callback(response);
		}

		public IEnumerator InsertMovement(Action<APIResponse> callback, Movement movement)
		{
			string json = movement.ToJson();

			using (UnityWebRequest request = UnityWebRequest.Post($"{WEB_URL}/movement", json))
			{
				request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
				request.uploadHandler.contentType = "application/json";
				request.method = "POST";

				yield return request.SendWebRequest();

				APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.InsertMovement);
				request.Dispose();

				callback(response);
			}
		}

		public IEnumerator UpdateMovement(Action<APIResponse> callback, string id, Movement movement)
		{
			string json = movement.ToJson(update: true);

			using (UnityWebRequest request = UnityWebRequest.Put($"{WEB_URL}/movement/{id}", json))
			{
				request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
				request.uploadHandler.contentType = "application/json";
				request.method = "PUT";

				yield return request.SendWebRequest();

				APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.UpdateMovement);
				request.Dispose();

				callback(response);
			}
		}

		public IEnumerator UpdateMovement(Action<APIResponse> callback, Movement movement)
		{
			string json = movement.ToJson(update: true);

			using (UnityWebRequest request = UnityWebRequest.Put($"{WEB_URL}/movement/{movement.id}", json))
			{
				request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
				request.uploadHandler.contentType = "application/json";
				request.method = "PUT";

				yield return request.SendWebRequest();

				APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.UpdateMovement);
				request.Dispose();

				callback(response);
			}
		}

		public IEnumerator DeleteMovement(Action<APIResponse> callback, string id)
		{
			using (UnityWebRequest request = UnityWebRequest.Delete($"{WEB_URL}/movement/{id}"))
			{
				request.downloadHandler = new DownloadHandlerBuffer();
				request.method = "DELETE";
				yield return request.SendWebRequest();

				APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.DeleteMovement);
				request.Dispose();

				callback(response);
			}
		}

		public IEnumerator FindSession(Action<APIResponse> callback, string id)
		{
			UnityWebRequest request = UnityWebRequest.Get($"{WEB_URL}/session/{id}");
			request.method = "GET";
			yield return request.SendWebRequest();

			APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.FindSession);
			request.Dispose();

			callback(response);
		}

		public IEnumerator FetchSessions(Action<APIResponse> callback, string professionalId = "", string patientId = "", string movementLabel = "",
											int[] articulations = null, int page = 0, int limit = 0)
		{
			int[] artList = articulations ?? new int[] { };

			string fullUrl = $"{WEB_URL}/session?professionalid={professionalId}&patientid={patientId}&movementLabel={movementLabel}&articulations={string.Join(",", artList)}";
			if (page > 0) fullUrl += $"&page={page}";
			if (limit > 0) fullUrl += $"&limit={limit}";

			UnityWebRequest request = UnityWebRequest.Get(fullUrl);
			request.method = "GET";
			yield return request.SendWebRequest();

			APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.FetchSessions);
			request.Dispose();

			callback(response);
		}

		public IEnumerator UpdateSession(Action<APIResponse> callback, Session session)
		{
			string json = session.ToJson();

			using (UnityWebRequest request = UnityWebRequest.Put($"{WEB_URL}/session/{session.id}", json))
			{
				request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
				request.uploadHandler.contentType = "application/json";
				request.method = "PUT";

				yield return request.SendWebRequest();

				APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.UpdateSession);
				request.Dispose();

				callback(response);
			}
		}

		public IEnumerator DeleteSession(Action<APIResponse> callback, string id)
		{
			using (UnityWebRequest request = UnityWebRequest.Delete($"{WEB_URL}/session/{id}"))
			{
				request.downloadHandler = new DownloadHandlerBuffer();
				request.method = "DELETE";
				yield return request.SendWebRequest();

				APIResponse response = ParseAPIResponse(request, APIResponse.ResponseType.DeleteSession);
				request.Dispose();

				callback(response);
			}
		}

		private bool IsHTTPError(UnityWebRequest request)
		{
			return request.result == UnityWebRequest.Result.ProtocolError;
		}

		private bool IsNetworkError(UnityWebRequest request)
		{
			return request.result == UnityWebRequest.Result.ConnectionError;
		}

		private APIResponse ParseAPIResponse(UnityWebRequest request, APIResponse.ResponseType responseType)
		{
			if (IsNetworkError(request) || IsHTTPError(request))
			{
				return NewAPIResponse(APIResponse.ResponseType.APIError, request.downloadHandler.text, request.responseCode);
			}
			if (!request.isDone)
			{
				return new APIResponse();
			}

			while (!request.downloadHandler.isDone) { } // Aguarda caso o download handler não tenha completado os processamentos

			return NewAPIResponse(responseType, request.downloadHandler.text, request.responseCode);
		}

		private APIResponse NewAPIResponse(APIResponse.ResponseType responseType, string response, long responseCode)
		{
			Debug.Log(response);
			APIResponse responseObject;

			try
            {
				responseObject = JsonUtility.FromJson<APIResponse>(response);
				responseObject.responseType = responseType;
            }
			catch (ArgumentException)
            {
				responseObject = new APIResponse();
				responseObject.responseType = APIResponse.ResponseType.APIError;
				responseObject.HTMLError = response;
			}

			responseObject.code = responseCode;
			return responseObject;
		}
	}
}

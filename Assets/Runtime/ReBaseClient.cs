﻿using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace ReBase
{
	public partial class ReBaseClient
	{
		private static readonly ReBaseClient instance = new ReBaseClient();
		public static ReBaseClient Instance { get { return instance; } }

		private string WEB_URL = "http://200.145.46.235:3030";

		private static readonly HttpClient client = new HttpClient();

		private enum Resource { Movement = 0, Session = 1 }

		private enum Method { GET = 0, POST = 1, PUT = 2, DELETE = 3 }

		public async Task<APIResponse> FetchMovements(string professionalId = "", string patientId = "", string movementLabel = "",
													  string[] articulations = null, bool legacy = false, int page = 0, int per = 0, string previousId = "")
		{
			return await SendRequest(Method.GET, Resource.Movement, APIResponse.ResponseType.FetchMovements,
									 query: FormatQueryParams(professionalId, patientId, movementLabel, articulations, page, per, previousId, legacy));
		}

		public async Task<APIResponse> FindMovement(string id, bool legacy = false)
		{
			if (id == default) throw new MissingAttributeException("movement id");

			return await SendRequest(Method.GET, Resource.Movement, APIResponse.ResponseType.FindMovement,
									 id, FormatQueryParams(legacy: legacy));
		}

		public async Task<APIResponse> InsertMovement(Movement movement)
		{
			return await SendRequest(Method.POST, Resource.Movement, APIResponse.ResponseType.InsertMovement,
									 body: movement.ToJson());
		}

		public async Task<APIResponse> UpdateMovement(string id, Movement movement)
		{
			if (id == default) throw new MissingAttributeException("movement id");

			return await SendRequest(Method.PUT, Resource.Movement, APIResponse.ResponseType.UpdateMovement,
									 id, body: movement.ToJson(true));
		}

		public async Task<APIResponse> UpdateMovement(Movement movement)
		{
			if (movement.id == default) throw new MissingAttributeException("movement id");

			return await SendRequest(Method.PUT, Resource.Movement, APIResponse.ResponseType.UpdateMovement,
									 movement.id, body: movement.ToJson(true));
		}

		public async Task<APIResponse> DeleteMovement(string id)
		{
			if (id == default) throw new MissingAttributeException("movement id");

			return await SendRequest(Method.DELETE, Resource.Movement, APIResponse.ResponseType.DeleteMovement, id);
		}

		public async Task<APIResponse> FetchSessions(string professionalId = "", string patientId = "", string movementLabel = "",
											string[] articulations = null, bool legacy = false, int page = 0, int per = 0, string previousId = "")
		{
			return await SendRequest(Method.GET, Resource.Session, APIResponse.ResponseType.FetchSessions,
									 query: FormatQueryParams(professionalId, patientId, movementLabel, articulations, page, per, previousId, legacy));
		}

		public async Task<APIResponse> FindSession(string id, bool legacy = false)
		{
			if (id == default) throw new MissingAttributeException("session id");

			return await SendRequest(Method.GET, Resource.Session, APIResponse.ResponseType.FindSession,
									 id, FormatQueryParams(legacy: legacy));
		}

		public async Task<APIResponse> InsertSession(Session session)
		{
			return await SendRequest(Method.POST, Resource.Session, APIResponse.ResponseType.InsertSession,
									 body: session.ToJson());
		}

		public async Task<APIResponse> UpdateSession(Session session)
		{
			if (session.id == default) throw new MissingAttributeException("session id");

			return await SendRequest(Method.PUT, Resource.Session, APIResponse.ResponseType.UpdateSession,
									 session.id, body: session.ToJson(true));
		}

		public async Task<APIResponse> DeleteSession(string id, bool deep = false)
		{
			if (id == default) throw new MissingAttributeException("session id");

			return await SendRequest(Method.DELETE, Resource.Session, APIResponse.ResponseType.DeleteSession,
									 id, FormatQueryParams(deep: deep));
		}

		private string FormatQueryParams(string professionalId = "", string patientId = "", string movementLabel = "",
										 string[] articulations = null, int page = -1, int per = -1, string previousId = "",
										 bool legacy = false, bool deep = false)
		{
			string query = "";
			List<string> filters = new List<string>();

			if (professionalId != "") filters.Add($"professionalid={professionalId}");
			if (patientId != "") filters.Add($"patientid={patientId}");
			if (movementLabel != "") filters.Add($"movementlabel={movementLabel}");
			if (articulations != null && articulations.Length > 0)
			{
				bool valid = true;
				foreach (string art in articulations)
				{
					if (art == "")
					{
						valid = false;
						break;
					}
				}
				if (valid) filters.Add($"articulations={string.Join(",", articulations)}");
			}
			if (page > 0) filters.Add($"page={page}");
			if (per > 0) filters.Add($"per={per}");
			if (previousId != "") filters.Add($"previous_id={previousId}");
			if (legacy) filters.Add($"legacy=true");
			if (deep) filters.Add($"deep=true");

			if (filters.Count > 0) query += $"?{string.Join("&", filters)}";
			return query;
		}

		private async Task<APIResponse> SendRequest(Method method, Resource resource, APIResponse.ResponseType responseType, string resourceId = null, string query = "", string body = "")
		{
			string url = resource == Resource.Movement ? $"{WEB_URL}/movement" : $"{WEB_URL}/session";
			if (resourceId != null) url += $"/{resourceId}";
			url += query;

			HttpResponseMessage httpResponse = null;

			try
			{
				switch (method)
				{
					case Method.GET:
						httpResponse = await client.GetAsync(url);
						break;
					case Method.POST:
						httpResponse = await client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
						break;
					case Method.PUT:
						httpResponse = await client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));
						break;
					case Method.DELETE:
						httpResponse = await client.DeleteAsync(url);
						break;
				}
			}
			catch (HttpRequestException e)
			{
				return NewAPIErrorResponse(e.Message);
			}

			return await ParseAPIResponse(httpResponse, responseType);
		}

		private async Task<APIResponse> ParseAPIResponse(HttpResponseMessage response, APIResponse.ResponseType responseType)
		{
			if (!response.IsSuccessStatusCode)
			{
				return NewAPIResponse(APIResponse.ResponseType.APIError, await response.Content.ReadAsStringAsync(), (int)response.StatusCode, 1);
			}

			return NewAPIResponse(responseType, await response.Content.ReadAsStringAsync(), (int)response.StatusCode, 0);
		}

		private APIResponse NewAPIResponse(APIResponse.ResponseType responseType, string response, long responseCode, int responseStatus)
		{
			Debug.Log(response);
			APIResponse responseObject;

			try
			{
				responseObject = Serializer.Deserialize<APIResponse>(response);
				if (responseObject == null) responseObject = NewAPIErrorResponse(response);
				responseObject.responseType = responseType;
			}
			catch (ArgumentException)
			{
				responseObject = NewAPIErrorResponse(response);
			}
			catch (NullReferenceException)
            {
				responseObject = NewAPIErrorResponse(response);
			}

			responseObject.code = responseCode;
			responseObject.status = responseStatus;
			return responseObject;
		}

		private APIResponse NewAPIErrorResponse(string response)
		{
			APIResponse reponseObject = new APIResponse();
			reponseObject.responseType = APIResponse.ResponseType.APIError;
			reponseObject.HTMLError = response;
			return reponseObject;
		}
	}
}
// Copyright © 2023 Tiago Trotta

// This file is part of Unity ReBase.

// Unity ReBase is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Unity ReBase is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Unity ReBase.  If not, see <https://www.gnu.org/licenses/>

namespace ReBase
{
	[System.Serializable]
	public class APIResponse
	{
		public enum ResponseType {
			FetchMovements = 0,
			FindMovement = 1,
			InsertMovement = 3,
			UpdateMovement = 4,
			DeleteMovement = 5,
			FetchSessions = 6,
			FindSession = 7,
			InsertSession = 8,
			UpdateSession = 9,
			DeleteSession = 10,
			APIError = 11,
			Authentication = 12
		}

		public ResponseType responseType;
		public int status;
		public long code;
		public string message;
		public string HTMLError;
		public string[] error;
		public string[] warning;

		public SerializableMovement[] movements;
		public SerializableMovement movement;
		public SerializableSession[] sessions;
		public SerializableSession session;
		public string deletedId;
		public int deletedCount;

		public MetaData meta;

		public bool success { get { return status == 0; } }

		public override string ToString()
		{
			string str = $"{{ type: {responseType}, code: {code}, status: {status}";
			if (message != null) str += $", message: {message}";
			if (error != null) str += $", error: [{string.Join(", ", error)}]";
			if (warning != null) str += $", warning: [{string.Join(", ", warning)}]";

			switch (responseType)
			{
				case ResponseType.FetchMovements:
					if (movements != null && movements.Length > 0) str += $", movements: [{string.Join<SerializableMovement>(", ", movements)}]";
					break;
                case ResponseType.FindMovement:
                case ResponseType.InsertMovement:
                case ResponseType.UpdateMovement:
					if (movement != null && movement.id != null) str += $", movement: {movement}";
					break;
				case ResponseType.DeleteMovement:
					if (deletedId != null) str += $", deletedId: {deletedId}";
					break;
				case ResponseType.DeleteSession:
					if (deletedId != null) str += $", deletedId: {deletedId}, deletedCount: {deletedCount}";
					break;
				case ResponseType.FetchSessions:
					if (sessions != null && sessions.Length > 0) str += $", sessions: [{string.Join<SerializableSession>(", ", sessions)}]";
					break;
				case ResponseType.InsertSession:
				case ResponseType.FindSession:
                case ResponseType.UpdateSession:
					if (session != null && session._id != null) str += $", session: {session}";
					break;
                default:
					if (HTMLError != null) str += $", HTMLError: \"{HTMLError}\"";
					break;
			}

			if (meta != null)
				str += $",\n{meta}";

			return str + " }"; ;
		}
	}
}

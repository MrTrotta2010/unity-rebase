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
			APIError = 11
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

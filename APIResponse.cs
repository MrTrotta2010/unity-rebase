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
			DeleteMovement = 5
		}

		public ResponseType responseType;
		public int status;
		public long code;
		public string message;
		public string[] error;
		public string[] warning;

		public SerializableMovement[] movements;
		public SerializableMovement movement;
		public SerializableMovement created;
		public SerializableMovement updated;
		public string deletedId;

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
					if (movement != null && movement.id != null) str += $", movement: [{movement}]";
					break;
				case ResponseType.InsertMovement:
					if (created != null && created.id != null) str += $", created: {created}";
					break;
				case ResponseType.UpdateMovement:
					if (updated != null && updated.id != null) str += $", updated: {updated}";
					break;
				case ResponseType.DeleteMovement:
					if (deletedId != null) str += $", deletedId: {deletedId}";
					break;
				default:
					break;
			}
			return str + " }";
		}
	}
}

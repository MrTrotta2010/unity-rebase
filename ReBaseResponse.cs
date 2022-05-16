namespace ReBase
{
	[System.Serializable]
	public class ReBaseResponse
	{
		public enum ResponseType {
			FetchMovements = 0,
			ShowMovement = 1,
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
		public SerializableMovement created;
		public SerializableMovement updated;

		public override string ToString()
		{
			string str = $"{{ type: {responseType}, code: {code}, status: {status}";
			if (message != null) str += $", message: {message}";
			if (error != null) str += $", error: [{string.Join(", ", error)}]";
			if (warning != null) str += $", warning: [{string.Join(", ", warning)}]";

			switch (responseType)
			{
				case ResponseType.FetchMovements:
					str += $", movements: [{(movements == null ? "" : string.Join<SerializableMovement>(", ", movements))}]";
					break;
				case ResponseType.InsertMovement:
					str += $", created: {created.ToString() ?? ""}";
					break;
				case ResponseType.UpdateMovement:
					str += $", updated: {updated.ToString() ?? ""}";
					break;
				default:
					break;
			}
			return str + " }";
		}
	}
}

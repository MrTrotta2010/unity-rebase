using System.Collections.Generic;

public partial class TranslationUtility
{
	public class BaseResponse
	{
		public int status;
		public long code;
		public string message;
		public string[] error;
		public string[] warning;

		public override string ToString()
		{
			return $"{{ code: {code}, status: {status}, message: {message}, error: [{string.Join(", ", error)}], warning: [{string.Join(", ", warning)}] }}";
		}
	}

	public class FetchMovementResponse : BaseResponse
	{
		public List<SerializableMovement> movements;

		public override string ToString()
		{
			string str = $"{{ code: {code}, status: {status}, movements: {(movements == null ? "NULL" : movements.Count.ToString())}";
			if (message != null) str += $", message: {message}";
			if (error != null) str += $", error: [{string.Join(", ", error)}]";
			if (warning != null) str += $", warning: [{string.Join(", ", warning)}]";
			return str + " }";
		}
	}

	public class InsertMovementResponse : BaseResponse
	{
		public Movement movement;
	}
}

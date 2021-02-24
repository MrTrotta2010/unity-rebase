using System.Collections.Generic;

[System.Serializable]
public class SessionList
{
	public List<SerializableSession> list;

	public SessionList(List<SerializableSession> sessionList)
	{
		list = sessionList;
	}
}

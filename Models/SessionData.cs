using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SessionData
{
	public string type;
	public List<int> data;
	public string translateddata;

	override public string ToString()
	{
		int[] intArray = data.ToArray();
		if (intArray != null)
		{
			return "type: " + type + ", data: [" + string.Join(",", intArray) + "]";
		}
		else
		{
			return "type: " + type + ", data: [\"\"]";
		}
	}
	
	public string ToStringTranslated()
	{
		return "type: " + type + ", data: " + translateddata;
	}

	public string GetJsonString()
    {
		int[] intArray = data.ToArray();
		if (intArray != null)
		{
			return "\"type\":\"" + type + "\"," +
					"\"data\":[" + string.Join(",", intArray) + "]," + 
					"\"translateddata\":\"" + translateddata + "\"";
		}
		return "\"type\":\"" + type + "\"," +
				"\"data\":[]," +
				"\"translateddata\":\"" + translateddata + "\"";
	}
}

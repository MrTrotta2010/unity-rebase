using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

public class CompressionUtility
{
	private static void CopyTo(Stream src, Stream dest)
	{
		byte[] bytes = new byte[4096];

		int cnt;

		while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
		{
			dest.Write(bytes, 0, cnt);
		}
	}

	public static byte[] Compress(string str)
	{
		var bytes = Encoding.UTF8.GetBytes(str);

		using (var msi = new MemoryStream(bytes))
		using (var mso = new MemoryStream())
		{
			using (var gs = new DeflateStream(mso, CompressionMode.Compress))
			{
				//msi.CopyTo(gs);
				CopyTo(msi, gs);
			}

			return mso.ToArray();
		}
	}

	public static string Decompress(byte[] bytes)
	{
		using (var msi = new MemoryStream(bytes))
		using (var mso = new MemoryStream())
		{
			using (var gs = new DeflateStream(msi, CompressionMode.Decompress))
			{
				//gs.CopyTo(mso);
				CopyTo(gs, mso);
			}

			return Encoding.UTF8.GetString(mso.ToArray());
		}
	}
}

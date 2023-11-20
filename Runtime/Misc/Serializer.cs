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

using Newtonsoft.Json;

namespace ReBase
{
    public static class Serializer
    {
		private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			MissingMemberHandling = MissingMemberHandling.Ignore
		};

		public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
		}
    }
}

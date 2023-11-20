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

using System;

namespace ReBase
{
    public class RepeatedArticulationException : Exception
    {
        public string Patterns { get; }
        public RepeatedArticulationException() { }

        public RepeatedArticulationException(string message)
            : base(message) { }

        public RepeatedArticulationException(string message, Exception inner)
            : base(message, inner) { }

        public RepeatedArticulationException(string message, string[] articulations)
        : this(message)
        {
            Patterns = $"[{string.Join(", ", articulations)}]";
        }
    }
}
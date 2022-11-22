﻿using Newtonsoft.Json;

namespace SharedObjects
{
    internal class CleanJsonWriter : JsonTextWriter
    {
        public CleanJsonWriter(TextWriter writer) : base(writer) { }

        public override void WritePropertyName(string name)
        {
            string result = name;

            if (name.StartsWith("@") || name.StartsWith("#"))
                result = name[1..];

            if (result.Contains(':'))
                result = result.Split(":".ToCharArray()).Last();

            base.WritePropertyName(result);
        }
    }
}
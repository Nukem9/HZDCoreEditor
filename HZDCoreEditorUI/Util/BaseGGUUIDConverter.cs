﻿namespace HZDCoreEditorUI.Util;

using System;
using System.Diagnostics.CodeAnalysis;
using Decima;
using Newtonsoft.Json;

public class BaseGGUUIDConverter : JsonConverter<BaseGGUUID>
{
    public override BaseGGUUID ReadJson(JsonReader reader, Type objectType, [AllowNull] BaseGGUUID existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var data = reader.Value as string;
        return data;
    }

    public override void WriteJson(JsonWriter writer, [AllowNull] BaseGGUUID value, JsonSerializer serializer)
    {
        if (value != null)
        {
            writer.WriteValue(value.ToString());
        }
        else
        {
            writer.WriteNull();
        }
    }
}

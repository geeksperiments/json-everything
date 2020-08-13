﻿using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Schema
{
	[SchemaKeyword(Name)]
	[JsonConverter(typeof(MaxPropertiesKeywordJsonConverter))]
	public class MaxPropertiesKeyword : IJsonSchemaKeyword
	{
		internal const string Name = "maxProperties";

		public decimal Value { get; }

		public MaxPropertiesKeyword(decimal value)
		{
			Value = value;
		}

		public ValidationResults Validate(ValidationContext context)
		{
			if (context.Instance.ValueKind != JsonValueKind.Object)
				return null;

			var number = context.Instance.EnumerateObject().Count();
			return Value >= number
				? ValidationResults.Success(context)
				: ValidationResults.Fail(context, $"Value has more than {Value} properties");
		}
	}

	public class MaxPropertiesKeywordJsonConverter : JsonConverter<MaxPropertiesKeyword>
	{
		public override MaxPropertiesKeyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.Number)
				throw new JsonException("Expected number");

			var number = reader.GetDecimal();

			return new MaxPropertiesKeyword(number);
		}
		public override void Write(Utf8JsonWriter writer, MaxPropertiesKeyword value, JsonSerializerOptions options)
		{
			writer.WriteNumber(MaxPropertiesKeyword.Name, value.Value);
		}
	}
}  
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Schema
{
	[SchemaPriority(10)]
	[SchemaKeyword(Name)]
	[JsonConverter(typeof(AdditionalItemsKeywordJsonConverter))]
	public class AdditionalItemsKeyword : IJsonSchemaKeyword
	{
		internal const string Name = "additionalItems";

		public JsonSchema Value { get; }

		static AdditionalItemsKeyword()
		{
			ValidationContext.RegisterConsolidationMethod(ConsolidateAnnotations);
		}
		public AdditionalItemsKeyword(JsonSchema value)
		{
			Value = value;
		}

		public ValidationResults Validate(ValidationContext context)
		{
			if (context.Instance.ValueKind != JsonValueKind.Array)
				return null;

			var subResults = new List<ValidationResults>();
			var overallResult = true;
			int startIndex = 0;
			var annotation = context.TryGetAnnotation(ItemsKeyword.Name);
			if (annotation != null)
			{
				if (annotation is bool) return null; // is only ever true or a number
				startIndex = (int) annotation;
			}
			foreach (var item in context.Instance.EnumerateArray().Skip(startIndex))
			{
				var results = Value.Validate(item);
				overallResult &= results.IsValid;
				subResults.Add(results);
			}

			context.Annotations[Name] = true;
			var result = overallResult
				? ValidationResults.Success(context)
				: ValidationResults.Fail(context);

			result.AddNestedResults(subResults);
			return result;
		}

		private static void ConsolidateAnnotations(IEnumerable<ValidationContext> sourceContexts, ValidationContext destContext)
		{
			if (sourceContexts.Select(c => c.TryGetAnnotation(Name)).OfType<bool>().Any())
				destContext.Annotations[Name] = true;
		}
	}

	public class AdditionalItemsKeywordJsonConverter : JsonConverter<AdditionalItemsKeyword>
	{
		public override AdditionalItemsKeyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var schema = JsonSerializer.Deserialize<JsonSchema>(ref reader, options);

			return new AdditionalItemsKeyword(schema);
		}
		public override void Write(Utf8JsonWriter writer, AdditionalItemsKeyword value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(AdditionalItemsKeyword.Name);
			JsonSerializer.Serialize(writer, value.Value, options);
		}
	}
}
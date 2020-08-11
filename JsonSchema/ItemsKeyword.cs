﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Schema
{
	[SchemaKeyword(Name)]
	[JsonConverter(typeof(ItemsKeywordJsonConverter))]
	public class ItemsKeyword : IJsonSchemaKeyword
	{
		internal const string Name = "items";

		public JsonSchema SingleValue { get; }
		public List<JsonSchema> ArrayValues { get; }

		static ItemsKeyword()
		{
			ValidationContext.RegisterConsolidationMethod(ConsolidateAnnotations);
		}
		public ItemsKeyword(JsonSchema value)
		{
			SingleValue = value;
		}

		public ItemsKeyword(params JsonSchema[] values)
		{
			ArrayValues = values.ToList();
		}

		public ItemsKeyword(IEnumerable<JsonSchema> values)
		{
			ArrayValues = values.ToList();
		}

		public ValidationResults Validate(ValidationContext context)
		{
			if (context.Instance.ValueKind != JsonValueKind.Array)
				return null;

			if (SingleValue != null)
			{
				var subResults = new List<ValidationResults>();
				var overallResult = true;
				foreach (var item in context.Instance.EnumerateArray())
				{
					var results = SingleValue.Validate(item);
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
			else // array
			{
				var subResults = new List<ValidationResults>();
				var overallResult = true;
				var maxEvaluations = Math.Min(ArrayValues.Count, context.Instance.GetArrayLength());
				for (int i = 0; i < maxEvaluations; i++)
				{
					var schema = ArrayValues[i];
					var item = context.Instance[i];
					var results = schema.Validate(item);
					overallResult &= results.IsValid;
					subResults.Add(results);
				}

				context.Annotations[Name] = maxEvaluations;
				var result = overallResult
					? ValidationResults.Success(context)
					: ValidationResults.Fail(context);
				result.AddNestedResults(subResults);
				return result;
			}
		}

		private static void ConsolidateAnnotations(IEnumerable<ValidationContext> sourceContexts, ValidationContext destContext)
		{
			object value;
			var allAnnotations = sourceContexts.Select(c => c.TryGetAnnotation(Name))
				.Where(a => a != null)
				.ToList();
			if (allAnnotations.OfType<bool>().Any())
				value = true;
			else
				value = allAnnotations.OfType<int>().DefaultIfEmpty(-1).Max();
			if (!Equals(value, -1))
				destContext.Annotations[Name] = value;
		}
	}

	public class ItemsKeywordJsonConverter : JsonConverter<ItemsKeyword>
	{
		public override ItemsKeyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.StartArray)
			{
				var schemas = JsonSerializer.Deserialize<List<JsonSchema>>(ref reader, options);
				return new ItemsKeyword(schemas);
			}
			
			var schema = JsonSerializer.Deserialize<JsonSchema>(ref reader, options);
			return new ItemsKeyword(schema);
		}
		public override void Write(Utf8JsonWriter writer, ItemsKeyword value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(ItemsKeyword.Name);
			if (value.SingleValue != null)
				JsonSerializer.Serialize(writer, value.SingleValue, options);
			else
			{
				writer.WriteStartArray();
				foreach (var schema in value.ArrayValues)
				{
					JsonSerializer.Serialize(writer, schema, options);
				}
				writer.WriteEndArray();
			}
		}
	}
}
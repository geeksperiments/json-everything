﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.More
{
	/// <summary>
	/// Enum to string converter.
	/// </summary>
	/// <typeparam name="T">The supported enum.</typeparam>
	/// <remarks>
	///	This serializer supports the <see cref="DescriptionAttribute"/> to indicate custom value naming.
	///
	/// The <see cref="FlagsAttribute"/> is supported via serializing to an array of base values.  Inclusion
	/// of composite values is not supported.
	/// </remarks>
	/// <example>
	/// public class MyClass {
	///	    [JsonConverter(typeof(EnumStringConverter&lt;MyEnum&gt;))]
	///	    public MyEnum Value { get; set; }
	/// }
	/// 
	/// public enum MyEnum {
	///     Foo,
	///     Bar
	/// }
	/// </example>
	public class EnumStringConverter<T> : JsonConverter<T>
		where T : Enum
	{
		private static Dictionary<string, T> _readValues;
		private static Dictionary<T, string> _writeValues;
		private static Func<T, T, T> _aggregator;
		// ReSharper disable once StaticMemberInGenericType
		private static readonly object _lock = new object();

		/// <summary>Reads and converts the JSON to type <typeparamref name="T" />.</summary>
		/// <param name="reader">The reader.</param>
		/// <param name="typeToConvert">The type to convert.</param>
		/// <param name="options">An object that specifies serialization options to use.</param>
		/// <returns>The converted value.</returns>
		/// <exception cref="JsonException">Element was not a string or could not identify the JSON value as a known enum value.</exception>
		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			EnsureMap();

			string str;
			if (reader.TokenType != JsonTokenType.String)
			{
				if (typeToConvert.GetCustomAttribute<FlagsAttribute>() != null && reader.TokenType == JsonTokenType.StartArray)
				{
					reader.Read(); // waste the start array
					var values = new List<T>();
					while (reader.TokenType != JsonTokenType.EndArray)
					{
						str = reader.GetString();

						if (!_readValues.TryGetValue(str, out var immediate))
							throw new JsonException($"Could not find appropriate value for {str} in type {typeToConvert.Name}");

						values.Add(immediate);
						reader.Read();
					}
					reader.Read(); // waste the end array

					return ToCombined(values);
				}
				throw new JsonException("Expected string");
			}

			str = reader.GetString();

			return _readValues.TryGetValue(str, out var value)
				? value
				: throw new JsonException($"Could not find appropriate value for {str} in type {typeToConvert.Name}");
		}

		/// <summary>Writes a specified value as JSON.</summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="value">The value to convert to JSON.</param>
		/// <param name="options">An object that specifies serialization options to use.</param>
		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
		{
			EnsureMap();

			if (typeof(T).GetCustomAttribute<FlagsAttribute>() != null && !_writeValues.ContainsKey(value))
			{
				writer.WriteStartArray();
				foreach (var name in _writeValues.Keys)
				{
					if (value.HasFlag(name))
						writer.WriteStringValue(_writeValues[name]);
				}
				writer.WriteEndArray();
				return;
			}

			writer.WriteStringValue(_writeValues[value]);
		}

		private static T ToCombined(IEnumerable<T> list)
		{
			if (_aggregator == null)
			{
				var underlyingType = Enum.GetUnderlyingType(typeof(T));
				var currentParameter = Expression.Parameter(typeof(T), "current");
				var nextParameter = Expression.Parameter(typeof(T), "next");

				_aggregator = Expression.Lambda<Func<T, T, T>>(
					Expression.Convert(
						Expression.Or(
							Expression.Convert(currentParameter, underlyingType),
							Expression.Convert(nextParameter, underlyingType)
						),
						typeof(T)
					),
					currentParameter,
					nextParameter
				).Compile();
			}

			return list.Aggregate(_aggregator);
		}

		private static void EnsureMap()
		{
			if (_readValues != null) return;
			lock (_lock)
			{
				if (_readValues != null) return;

				var map = typeof(T).GetFields()
					.Where(f => !f.IsSpecialName)
					.Select(f => new
					{
						Value = (T) Enum.Parse(typeof(T), f.Name),
						Description = f.GetCustomAttribute<DescriptionAttribute>()?.Description ?? f.Name
					})
					.ToList();
				_readValues = map.ToDictionary(v => v.Description, v => v.Value);
				_writeValues = map.ToDictionary(v => v.Value, v => v.Description);
			}
		}
	}
}
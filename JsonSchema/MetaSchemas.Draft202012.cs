﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using Json.More;

namespace Json.Schema
{
	public static partial class MetaSchemas
	{
		internal const string Draft202012IdValue = "https://json-schema.org/draft/2020-12/schema";

		/// <summary>
		/// The Draft 2020-12 meta-schema ID.
		/// </summary>
		public static readonly Uri Draft202012Id = new Uri(Draft202012IdValue);
		
		/// <summary>
		/// The Draft 2020-12 Core meta-schema ID.
		/// </summary>
		public static readonly Uri Core202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/core");
		/// <summary>
		/// The Draft 2020-12 Dynamic meta-schema ID.
		/// </summary>
		public static readonly Uri Dynamic202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/dynamic");
		/// <summary>
		/// The Draft 2020-12 Applicator meta-schema ID.
		/// </summary>
		public static readonly Uri Applicator202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/applicator");
		/// <summary>
		/// The Draft 2020-12 Validation meta-schema ID.
		/// </summary>
		public static readonly Uri Validation202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/validation");
		/// <summary>
		/// The Draft 2020-12 Metadata meta-schema ID.
		/// </summary>
		public static readonly Uri Metadata202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/meta-data");
		/// <summary>
		/// The Draft 2020-12 Format-Annotation meta-schema ID.
		/// </summary>
		public static readonly Uri FormatAnnotation202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/format-annotation");
		/// <summary>
		/// The Draft 2020-12 Format-Assertion meta-schema ID.
		/// </summary>
		public static readonly Uri FormatAssertion202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/format-assertion");
		/// <summary>
		/// The Draft 2020-12 Content meta-schema ID.
		/// </summary>
		public static readonly Uri Content202012Id = new Uri("https://json-schema.org/draft/2020-12/meta/content");

		/// <summary>
		/// The Draft 2020-12 meta-schema.
		/// </summary>
		public static readonly JsonSchema Draft202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(Draft202012Id)
				.Vocabulary(
					(Vocabularies.Core202012Id, true),
					(Vocabularies.Dynamic202012Id, true),
					(Vocabularies.Applicator202012Id, true),
					(Vocabularies.Validation202012Id, true),
					(Vocabularies.Metadata202012Id, true),
					(Vocabularies.FormatAnnotation202012Id, false),
					(Vocabularies.Content202012Id, true)
				)
				.RecursiveAnchor()
				.Title("Core and Validation specifications meta-schema")
				.AllOf(
					new JsonSchemaBuilder().Ref("meta/core"),
					new JsonSchemaBuilder().Ref("meta/dynamic"),
					new JsonSchemaBuilder().Ref("meta/applicator"),
					new JsonSchemaBuilder().Ref("meta/validation"),
					new JsonSchemaBuilder().Ref("meta/meta-data"),
					new JsonSchemaBuilder().Ref("meta/format-annotation"),
					new JsonSchemaBuilder().Ref("meta/content")
				)
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(DefinitionsKeyword.Name, new JsonSchemaBuilder()
						.Comment("While no longer an official keyword as it is replaced by $defs, this keyword is retained in the meta-schema to prevent incompatible extensions as it remains in common use.")
						.Type(SchemaValueType.Object)
						.AdditionalProperties(JsonSchemaBuilder.RecursiveRefRoot())
						.Default(new Dictionary<string, JsonElement>().AsJsonElement())
					),
					(DependenciesKeyword.Name, new JsonSchemaBuilder()
						.Comment("\"dependencies\" is no longer a keyword, but schema authors should avoid redefining it to facilitate a smooth transition to \"dependentSchemas\" and \"dependentRequired\"")
						.Type(SchemaValueType.Object)
						.AdditionalProperties(new JsonSchemaBuilder()
							.AnyOf(
								JsonSchemaBuilder.RecursiveRefRoot(),
								new JsonSchemaBuilder().Ref("meta/validation#/$defs/stringArray")
							)
						)
					)
				);

		/// <summary>
		/// The Draft 2020-12 Core meta-schema.
		/// </summary>
		public static readonly JsonSchema Core202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(Core202012Id)
				.Vocabulary((Vocabularies.Core202012Id, true))
				.RecursiveAnchor()
				.Title("Core vocabulary meta-schema")
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(IdKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
						.Format(Formats.UriReference)
						.Comment("Non-empty fragments not allowed.")
						.Pattern("^[^#]*#?$")
					),
					(SchemaKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
						.Format(Formats.Uri)
					),
					(AnchorKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
						.Pattern(AnchorKeyword.AnchorPattern)
					),
					(RefKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
						.Format(Formats.UriReference)
					),
					(VocabularyKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Object)
						.PropertyNames(new JsonSchemaBuilder()
							.Type(SchemaValueType.String)
							.Format(Formats.Uri)
						)
						.AdditionalProperties(new JsonSchemaBuilder()
							.Type(SchemaValueType.Boolean)
						)
					),
					(CommentKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
					),
					(DefsKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Object)
						.AdditionalProperties(JsonSchemaBuilder.RecursiveRefRoot())
						.Default(new Dictionary<string, JsonElement>().AsJsonElement())
					)
				);

		/// <summary>
		/// The Draft 2020-12 Core meta-schema.
		/// </summary>
		public static readonly JsonSchema Dynamic202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(Dynamic202012Id)
				.Vocabulary((Vocabularies.Dynamic202012Id, true))
				.RecursiveAnchor()
				.Title("Dynamic vocabulary meta-schema")
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(RecursiveRefKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
						.Format(Formats.UriReference)
					),
					(RecursiveAnchorKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Boolean)
						.Default(false.AsJsonElement())
					)
				);

		/// <summary>
		/// The Draft 2020-12 Applicator meta-schema.
		/// </summary>
		public static readonly JsonSchema Applicator202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(Applicator202012Id)
				.Vocabulary((Vocabularies.Applicator202012Id, true))
				.RecursiveAnchor()
				.Title("Applicator vocabulary meta-schema")
				.Properties(
					(AdditionalItemsKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(UnevaluatedItemsKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(ItemsKeyword.Name, new JsonSchemaBuilder()
						.AnyOf(
							JsonSchemaBuilder.RefRoot(),
							new JsonSchemaBuilder().Ref("#/defs/schemaArray")
						)
					),
					(ContainsKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(AdditionalPropertiesKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(UnevaluatedPropertiesKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(PropertiesKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Object)
						.AdditionalProperties(JsonSchemaBuilder.RecursiveRefRoot())
						.Default(new Dictionary<string, JsonElement>().AsJsonElement())
					),
					(PatternPropertiesKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Object)
						.AdditionalProperties(JsonSchemaBuilder.RecursiveRefRoot())
						.PropertyNames(new JsonSchemaBuilder()
							.Format(Formats.Regex)
						)
						.Default(new Dictionary<string, JsonElement>().AsJsonElement())
					),
					(DependentSchemasKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Object)
						.AdditionalProperties(JsonSchemaBuilder.RecursiveRefRoot())
					),
					(PropertyNamesKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(IfKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(ThenKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(ElseKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot()),
					(AllOfKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/schemaArray")
					),
					(AnyOfKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/schemaArray")
					),
					(OneOfKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/schemaArray")
					),
					(NotKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot())
				)
				.Defs(
					("schemaArray", new JsonSchemaBuilder()
						.Type(SchemaValueType.Array)
						.MinItems(1)
						.Items(JsonSchemaBuilder.RecursiveRefRoot())
					)
				);

		/// <summary>
		/// The Draft 2020-12 Validation meta-schema.
		/// </summary>
		public static readonly JsonSchema Validation202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(Validation202012Id)
				.Vocabulary((Vocabularies.Validation202012Id, true))
				.RecursiveAnchor()
				.Title("Validation vocabulary meta-schema")
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(MultipleOfKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Number)
						.ExclusiveMinimum(0)
					),
					(MaximumKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Number)
					),
					(ExclusiveMaximumKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Number)
					),
					(MinimumKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Number)
					),
					(ExclusiveMinimumKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Number)
					),
					(MaxLengthKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeInteger")
					),
					(MinLengthKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeIntegerDefault0")
					),
					(PatternKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
						.Format(Formats.Regex)
					),
					(MaxItemsKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeInteger")
					),
					(MinItemsKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeIntegerDefault0")
					),
					(UniqueItemsKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Boolean)
						.Default(false.AsJsonElement())
					),
					(MaxContainsKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeInteger")
					),
					(MinContainsKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeInteger")
						.Default(1.AsJsonElement())
					),
					(MaxPropertiesKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeInteger")
					),
					(MinPropertiesKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeIntegerDefault0")
					),
					(RequiredKeyword.Name, new JsonSchemaBuilder()
						.Ref("#/$defs/stringArray")
					),
					(DependentRequiredKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Object)
						.AdditionalProperties(new JsonSchemaBuilder()
							.Ref("#/$defs/stringArray")
						)
					),
					(ConstKeyword.Name, true),
					(EnumKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Array)
						.Items(true)
					),
					(TypeKeyword.Name, new JsonSchemaBuilder()
						.AnyOf(
							new JsonSchemaBuilder().Ref("#/$defs/simpleTypes"),
							new JsonSchemaBuilder()
								.Type(SchemaValueType.Array)
								.Items(new JsonSchemaBuilder().Ref("#/$defs/simpleTypes"))
								.MinItems(1)
								.UniqueItems(true)
						)
					)
				)
				.Defs(
					("nonNegativeInteger", new JsonSchemaBuilder()
						.Type(SchemaValueType.Integer)
						.Minimum(0)
					),
					("nonNegativeIntegerDefault0", new JsonSchemaBuilder()
						.Ref("#/$defs/nonNegativeInteger")
						.Default(0.AsJsonElement())
					),
					("simpleTypes", new JsonSchemaBuilder()
						.Enum(
							"array".AsJsonElement(),
							"boolean".AsJsonElement(),
							"integer".AsJsonElement(),
							"null".AsJsonElement(),
							"number".AsJsonElement(),
							"object".AsJsonElement(),
							"string".AsJsonElement()
						)
					),
					("stringArray", new JsonSchemaBuilder()
						.Type(SchemaValueType.Array)
						.Items(new JsonSchemaBuilder().Type(SchemaValueType.String))
						.UniqueItems(true)
						.Default(new JsonElement[0].AsJsonElement())
					)
				);

		/// <summary>
		/// The Draft 2020-12 Metadata meta-schema.
		/// </summary>
		public static readonly JsonSchema Metadata202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(Metadata202012Id)
				.Vocabulary((Vocabularies.Metadata202012Id, true))
				.RecursiveAnchor()
				.Title("Meta-data vocabulary meta-schema")
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(TitleKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
					),
					(DescriptionKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
					),
					(DefaultKeyword.Name, true),
					(DeprecatedKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Boolean)
						.Default(false.AsJsonElement())
					),
					(ReadOnlyKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Boolean)
						.Default(false.AsJsonElement())
					),
					(WriteOnlyKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Boolean)
						.Default(false.AsJsonElement())
					),
					(ExamplesKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.Array)
					)
				);

		/// <summary>
		/// The Draft 2020-12 Format-Annotation meta-schema.
		/// </summary>
		public static readonly JsonSchema FormatAnnotation202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(FormatAnnotation202012Id)
				.Vocabulary((Vocabularies.FormatAnnotation202012Id, true))
				.RecursiveAnchor()
				.Title("Format-annotation vocabulary meta-schema")
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(FormatKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
					)
				);

		/// <summary>
		/// The Draft 2020-12 Format-Assertion meta-schema.
		/// </summary>
		public static readonly JsonSchema FormatAssertion202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(FormatAssertion202012Id)
				.Vocabulary((Vocabularies.FormatAssertion202012Id, true))
				.RecursiveAnchor()
				.Title("Format-assertion vocabulary meta-schema")
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(FormatKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
					)
				);

		/// <summary>
		/// The Draft 2020-12 Content meta-schema.
		/// </summary>
		public static readonly JsonSchema Content202012 =
			new JsonSchemaBuilder()
				.Schema(Draft202012Id)
				.Id(Content202012Id)
				.Vocabulary((Vocabularies.Content202012Id, true))
				.RecursiveAnchor()
				.Title("Content vocabulary meta-schema")
				.Type(SchemaValueType.Object | SchemaValueType.Boolean)
				.Properties(
					(ContentMediaTypeKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
					),
					(ContentMediaEncodingKeyword.Name, new JsonSchemaBuilder()
						.Type(SchemaValueType.String)
					),
					(ContentSchemaKeyword.Name, JsonSchemaBuilder.RecursiveRefRoot())
				);
	}
}
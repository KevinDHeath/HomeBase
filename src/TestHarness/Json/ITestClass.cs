using System;
using System.Text.Json.Serialization;

namespace TestHarness;

#nullable enable

/// <summary>
/// Interface for an TestClass implementation.
/// </summary>
public interface ITestClass
{
	#region Properties

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	bool? Boolean { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	byte? Byte { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	DateOnly? DateOnly { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	DateTime? DateTime { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	DateTimeOffset? DateTimeOffset { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	decimal? Decimal { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	double? Double { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	float? Float { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	Guid? Guid { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	int? Integer { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	long? Long { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	sbyte? SByte { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	short? Short { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string String { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	string? StringNull { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	TimeOnly? TimeOnly { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	TimeSpan? TimeSpan { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	uint? UInt { get; set; }

	[JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingNull )]
	ulong? ULong { get; set; }

	#endregion
}
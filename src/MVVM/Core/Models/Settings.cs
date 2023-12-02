using Common.Core.Classes;

namespace MVVM.Core.Models;

/// <summary>Application settings.</summary>
public class Settings : ModelBase
{
	private int? _fontSize;

	/// <summary>Gets or sets the default font size.</summary>
	public int? FontSize
	{
		get => _fontSize;
		set
		{
			if( value is not null && value.Equals( _fontSize ) ) { return; }
			_fontSize = value;
			OnPropertyChanged();
		}
	}

	private string? _extCompanies;

	/// <summary>Gets or sets the external data file for companies.</summary>
	public string? ExtCompanies
	{
		get => _extCompanies;
		set
		{
			if( value is not null && value.Equals( _extCompanies ) ) { return; }
			_extCompanies = value;
			OnPropertyChanged();
		}
	}

	private string? _extPeople;

	/// <summary>Gets or sets the external data file for people.</summary>
	public string? ExtPeople
	{
		get => _extPeople;
		set
		{
			if( value is not null && value.Equals( _extPeople ) ) { return; }
			_extPeople = value;
			OnPropertyChanged();
		}
	}

	private string? _externalData;

	/// <summary>Gets or sets the folder containing external data.</summary>
	public string? ExternalData
	{
		get => _externalData;
		set
		{
			if( value is not null && value.Equals( _externalData ) ) { return; }
			_externalData = value;
			OnPropertyChanged();
		}
	}

	private int? _maxCompanies;

	/// <summary>Gets or sets the maximum number of companies to load.</summary>
	public int? MaxCompanies
	{
		get => _maxCompanies;
		set
		{
			if( value is not null && value.Equals( _maxCompanies ) ) { return; }
			_maxCompanies = value;
			OnPropertyChanged();
		}
	}

	private int? _maxPeople;

	/// <summary>Gets or sets the maximum number of people to load.</summary>
	public int? MaxPeople
	{
		get => _maxPeople;
		set
		{
			if( value is not null && value.Equals( _maxPeople ) ) { return; }
			_maxPeople = value;
			OnPropertyChanged();
		}
	}

	private bool? _useExternal;

	/// <summary>Indicates whether to use external data files.</summary>
	public bool? UseExternal
	{
		get => _useExternal;
		set
		{
			if( value is not null && value.Equals( _useExternal ) ) { return; }
			_useExternal = value;
			OnPropertyChanged();
		}
	}

	/// <summary>Gets or set the collection of connection strings.</summary>
	public Dictionary<string, string> ConnectionStrings { get; set; } = [];

	/// <summary>Gets or set the collection of endpoint strings.</summary>
	public Dictionary<string, string> Endpoints { get; set; } = [];
}
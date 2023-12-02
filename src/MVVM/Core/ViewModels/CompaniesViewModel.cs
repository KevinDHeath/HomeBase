// Ignore Spelling: Naics Bal

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Common.Core.Interfaces;
using Common.Core.Models;
using MVVM.Core.Stores;
using MVVM.Core.Validations;

namespace MVVM.Core.ViewModels;

/// <summary>Companies view model.</summary>
public sealed partial class CompaniesViewModel : AddressViewModel
{
	#region Companies Properties

	/// <summary>Gets or sets the number of Companies in the collection.</summary>
	[ObservableProperty]
	[NotifyCanExecuteChangedFor( nameof( AddCompanyCommand ) )]
	private int _count;

	/// <summary>Gets or sets the currently selected Company.</summary>
	public ICompany? CurrentCompany { get => _store.CurrentCompany; set { _store.CurrentCompany = value; } }

	/// <summary>Gets the collection of Companies.</summary>
	public ObservableCollection<ICompany> Companies => _store.Companies;

	/// <summary>Gets the external data path.</summary>
	public string DataPath => _store.DataPath;

	/// <summary>Indicates whether the Current Company has been set.</summary>
	public bool IsCurrentCompany => CurrentCompany is not null;

	#endregion

	#region Commands

	/// <summary>Command to add another company to the collection.</summary>
	public IRelayCommand AddCompanyCommand { get; }

	/// <summary>Command to export company data.</summary>
	public IRelayCommand<string?> ExportDataCommand { get; }

	/// <summary>Command to cancel the company changes.</summary>
	public IRelayCommand CancelCompanyEditCommand { get; }

	/// <summary>Command to apply the company changes.</summary>
	public IRelayCommand SaveCompanyCommand { get; }

	#endregion

	#region Constructor and Variables

	private readonly CompaniesStore _store;
	private Company _dc;
	private bool _changes;

	/// <summary>Initializes a new instance of the CompaniesViewModel class.</summary>
	/// <param name="store">Companies storage.</param>
	public CompaniesViewModel( CompaniesStore store )
	{
		_store = store;
		_store.CurrentCompanyChanged += RollbackCompany;

		_count = store.Companies.Count;
		_dc = new Company();
		_ad = _dc.Address;

		AddCompanyCommand = new RelayCommand( AddCompany, AllowAdd );
		ExportDataCommand = new RelayCommand<string?>( ExportData, AllowExport );
		CancelCompanyEditCommand = new RelayCommand( RollbackCompany, AllowRollback );
		SaveCompanyCommand = new RelayCommand( SaveCompany, AllowApply );
	}

	#endregion

	#region Overridden Methods

	/// <inheritdoc/>
	public override void Dispose()
	{
		_store.CurrentCompanyChanged -= RollbackCompany;
		base.Dispose();
	}

	#endregion

	#region Private Methods

	private void OnAddressChanged( object? sender, PropertyChangedEventArgs e )
	{
		if( e.PropertyName == nameof( Country ) )
		{
			// Re-validate Phone if Country has changed
			_validation.ValidateProperty( PrimaryPhone, nameof( PrimaryPhone ) );
			_validation.ValidateProperty( SecondaryPhone, nameof( SecondaryPhone ) );
		}

		if( e.PropertyName != "FullAddress" ) // Ignore full address
		{
			EvaluateCommands();
		}
	}

	private void RefreshAllProperties()
	{
		_validation.ClearErrors();
		OnPropertyChanged( string.Empty );
		EvaluateCommands();
	}

	private void EvaluateCommands()
	{
		CancelCompanyEditCommand.NotifyCanExecuteChanged();
		SaveCompanyCommand.NotifyCanExecuteChanged();
	}

	private bool AllowAdd() => _store.Count > Count;

	private void AddCompany()
	{
		_store.AddCompany( 1 );
		Count++;
	}

	private bool AllowExport( string? fileName ) => _store.Count > 0;

	private void ExportData( string? fileName )
	{
		if( string.IsNullOrWhiteSpace( fileName ) ) { return; }

		_store.Export( Path.GetDirectoryName( fileName ), Path.GetFileName( fileName ) );
	}

	private bool AllowRollback()
	{
		if( CurrentCompany is null ) return false;
		_changes = !_dc.Equals( CurrentCompany );
		return _changes;
	}

	private void RollbackCompany()
	{
		if( CurrentCompany is not null )
		{
			_dc = (Company)( (Company)CurrentCompany ).Clone();
			_ad.PropertyChanged -= OnAddressChanged;
			_ad = _dc.Address;
			_ad.PropertyChanged += OnAddressChanged;
		}
		RefreshAllProperties();
		_validation.ValidateAllProperties();

		OnPropertyChanged( nameof( IsCurrentCompany ) );
	}

	private bool AllowApply() => !_validation.HasErrors & _changes;

	private void SaveCompany()
	{
		_store.UpdateCompany( _dc );
		RefreshAllProperties();
	}

	#endregion
}

#region ICompany Implementation

public sealed partial class CompaniesViewModel : ICompany
{
	/// <summary>Gets or sets the unique identifier.</summary>
	public int Id { get => _dc.Id; set { if( _dc is not null ) _dc.Id = value; } }

	/// <summary>Gets or sets the Company Name.</summary>
	[Display( Name = "Name" )]
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[StringLength( 100, ErrorMessage = "{0} cannot be greater than 100 chars." )]
	public string Name
	{
		get => ( _dc.Name is null ) ? string.Empty : _dc.Name;
		set
		{
			if( !value.Equals( _dc.Name ) )
			{
				_validation.ValidateProperty( value );
				_dc.Name = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Primary Address.</summary>
	public Address Address
	{
		get => _ad;
		set => _ad = value;
	}

	/// <summary>Gets or sets the Government Number.</summary>
	[Required( ErrorMessage = "EIN must be provided." )]
	[RegularExpression( cEINRegex, ErrorMessage = "Format not valid." )]
	public string? GovernmentNumber
	{
		get => _dc.GovernmentNumber;
		set
		{
			if( value != _dc.GovernmentNumber )
			{
				_validation.ValidateProperty( value );
				_dc.GovernmentNumber = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Primary Phone Number.</summary>
	[PhoneNumber]
	public string? PrimaryPhone
	{
		get => _dc.PrimaryPhone;
		set
		{
			if( value != _dc.PrimaryPhone )
			{
				_validation.ValidateProperty( value );
				_dc.PrimaryPhone = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Secondary Phone Number.</summary>
	[PhoneNumber]
	public string? SecondaryPhone
	{
		get => _dc.SecondaryPhone;
		set
		{
			if( value != _dc.SecondaryPhone )
			{
				_validation.ValidateProperty( value );
				_dc.SecondaryPhone = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Primary Email.</summary>
	[EmailAddress( ErrorMessage = "Format not valid." )]
	public string? Email
	{
		get => _dc.Email;
		set
		{
			if( value != _dc.Email )
			{
				_validation.ValidateProperty( value );
				_dc.Email = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the NAICS Code.</summary>
	public string? NaicsCode
	{
		get => _dc.NaicsCode;
		set
		{
			if( value != _dc.NaicsCode )
			{
				_dc.NaicsCode = value;
				EvaluateCommands();
			}
		}
	}


	/// <summary>Gets or sets the Private Company indicator.</summary>
	public bool? Private
	{
		get => _dc.Private;
		set
		{
			if( value != _dc.Private )
			{
				_dc.Private = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the number of Deposit Accounts.</summary>
	public int? DepositsCount
	{
		get => _dc.DepositsCount;
		set
		{
			if( value != _dc.DepositsCount )
			{
				_dc.DepositsCount = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Deposit Accounts Balance.</summary>
	public decimal? DepositsBal
	{
		get => _dc.DepositsBal;
		set
		{
			if( value != _dc.DepositsBal )
			{
				_dc.DepositsBal = value;
				EvaluateCommands();
			}
		}
	}
}

#endregion
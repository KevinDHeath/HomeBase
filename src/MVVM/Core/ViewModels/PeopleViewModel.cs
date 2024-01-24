global using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Common.Core.Classes;
using Common.Core.Interfaces;
using Common.Core.Models;
using MVVM.Core.Stores;
using MVVM.Core.Validations;

namespace MVVM.Core.ViewModels;

/// <summary>People view model.</summary>
public sealed partial class PeopleViewModel : AddressViewModel
{
	#region People Properties

	/// <summary>Gets or sets the number of people in the collection.</summary>
	[ObservableProperty]
	[NotifyCanExecuteChangedFor( nameof( AddPersonCommand ) )]
	private int _count;

	/// <summary>Gets or sets the currently selected Person.</summary>
	public IPerson? CurrentPerson { get => _store.CurrentPerson; set { _store.CurrentPerson = value; } }

	/// <summary>Gets the collection of People.</summary>
	public ObservableCollection<IPerson> People => _store.People;

	/// <summary>Gets the external data path.</summary>
	public string DataPath => _store.DataPath;

	/// <summary>Indicates whether the Current Person has been set.</summary>
	public bool IsCurrentPerson => CurrentPerson is not null;

	#endregion

	#region Commands

	/// <summary>Command to add another person to the collection.</summary>
	public IRelayCommand AddPersonCommand { get; }

	/// <summary>Command to export person data.</summary>
	public IRelayCommand<string?> ExportDataCommand { get; }

	/// <summary>Command to cancel the person changes.</summary>
	public IRelayCommand CancelPersonEditCommand { get; }

	/// <summary>Command to apply the person changes.</summary>
	public IRelayCommand SavePersonCommand { get; }

	#endregion

	#region Constructor and Variables

	private readonly PeopleStore _store;
	private Person _dc;
	private bool _changes;

	/// <summary>Initializes a new instance of the PeopleViewModel class.</summary>
	/// <param name="store">People storage.</param>
	public PeopleViewModel( PeopleStore store )
	{
		_store = store;
		_store.CurrentPersonChanged += RollbackPerson;

		_count = store.People.Count;
		_dc = new Person();
		EntityAddress = _dc.Address;

		AddPersonCommand = new RelayCommand( AddPerson, AllowAdd );
		ExportDataCommand = new RelayCommand<string?>( ExportData, AllowExport );
		CancelPersonEditCommand = new RelayCommand( RollbackPerson, AllowRollback );
		SavePersonCommand = new RelayCommand( SavePerson, AllowApply );
	}

	#endregion

	#region Overridden Methods

	/// <inheritdoc/>
	public override void Dispose()
	{
		_store.CurrentPersonChanged -= RollbackPerson;
		base.Dispose();
	}

	#endregion

	#region Private Methods

	private void OnAddressChanged( object? sender, PropertyChangedEventArgs e )
	{
		if( e.PropertyName == nameof( Country ) )
		{
			// Re-validate Phone if Country has changed
			_validation.ValidateProperty( HomePhone, nameof( HomePhone ) );
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
		CancelPersonEditCommand.NotifyCanExecuteChanged();
		SavePersonCommand.NotifyCanExecuteChanged();
	}

	private bool AllowAdd() => _store.Count > Count;

	private void AddPerson()
	{
		_store.AddPerson( 1 );
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
		if( CurrentPerson is null ) return false;
		_changes = !_dc.Equals( CurrentPerson );
		return _changes;
	}

	private void RollbackPerson()
	{
		if( CurrentPerson is not null )
		{
			_dc = (Person)( (Person)CurrentPerson ).Clone();
			EntityAddress.PropertyChanged -= OnAddressChanged;
			EntityAddress = _dc.Address;
			EntityAddress.PropertyChanged += OnAddressChanged;
		}
		RefreshAllProperties();
		_validation.ValidateAllProperties();

		OnPropertyChanged( nameof( IsCurrentPerson ) );
	}

	private bool AllowApply() => !_validation.HasErrors & _changes;

	private void SavePerson()
	{
		_store.UpdatePerson( _dc );
		RefreshAllProperties();
	}

	#endregion
}

#region IPerson Implementation

public sealed partial class PeopleViewModel : IPerson
{
	/// <summary>Gets the unique identifier.</summary>
	public int Id { get => _dc.Id; set { if( _dc is not null ) _dc.Id = value; } }

	/// <summary>Gets or sets the First Name.</summary>
	[Display( Name = "First name" )]
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[StringLength( 50, ErrorMessage = "{0} cannot be greater than {1} chars." )]
	public string FirstName
	{
		get => ( _dc.FirstName is not null ) ? _dc.FirstName : string.Empty;
		set
		{
			if( !value.Equals( _dc.FirstName ) )
			{
				_validation.ValidateProperty( value );
				_dc.FirstName = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Middle Name.</summary>
	public string? MiddleName
	{
		get => _dc.MiddleName;
		set
		{
			if( value != _dc.MiddleName )
			{
				_dc.MiddleName = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Last Name.</summary>
	[Display( Name = "Last name" )]
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[StringLength( 50, ErrorMessage = "{0} cannot be greater than {1} chars." )]
	public string LastName
	{
		get => ( _dc.LastName is not null ) ? _dc.LastName : string.Empty;
		set
		{
			if( !value.Equals( _dc.LastName ) )
			{
				_validation.ValidateProperty( value );
				_dc.LastName = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Primary Address.</summary>
	public Address Address
	{
		get => EntityAddress;
		set => EntityAddress = value;
	}

	/// <summary>Gets or sets the Government Number.</summary>
	[RegularExpression( cSSNRegex, ErrorMessage = "Format not valid.")]
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

	/// <summary>Gets or sets the Identification Province.</summary>
	[Province()]
	public string? IdProvince
	{
		get => _dc.IdProvince;
		set
		{
			if( value != _dc.IdProvince )
			{
				_validation.ValidateProperty( value );
				_dc.IdProvince = value?.ToUpper();
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Identification Number.</summary>
	public string? IdNumber
	{
		get => _dc.IdNumber;
		set
		{
			if( value != _dc.IdNumber )
			{
				_dc.IdNumber = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Home Phone Number.</summary>
	[PhoneNumber]
	public string? HomePhone
	{
		get => _dc.HomePhone;
		set
		{
			if( value != _dc.HomePhone )
			{
				_validation.ValidateProperty( value );
				_dc.HomePhone = value;
				EvaluateCommands();
			}
		}
	}

	/// <summary>Gets or sets the Date of Birth.</summary>
	[Display( Name = "Birth Date" )]
	[Required( ErrorMessage = "{0} cannot be empty." )]
	[NoFutureDate]
	public DateOnly? BirthDate
	{
		get => _dc.BirthDate;
		set
		{
			if( value != _dc.BirthDate )
			{
				_validation.ValidateProperty( value );
				_dc.BirthDate = value;
				EvaluateCommands();
				OnPropertyChanged( nameof( Age ) );
			}
		}
	}

	/// <summary>Gets the Full Name.</summary>
	public string FullName { get => _dc.FullName; }

	/// <summary>Gets the persons age.</summary>
	public int? Age => ModelBase.CalculateAge( BirthDate );
}

#endregion
namespace Library.Core.Commands;

/// <summary>Defines a command.</summary>
public abstract class CommandBase : ICommand
{
	/// <summary>Occurs when changes take place that affect whether or not the command should execute.</summary>
#pragma warning disable 0067 // The event is never used
	public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

	/// <summary>Determines whether the command can execute in its current state.</summary>
	/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
	/// <returns>true if this command can be executed; otherwise, false.</returns>
	public virtual bool CanExecute( object? parameter ) => true;

	/// <summary>Defines the method to be called when the command is invoked.</summary>
	/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
	public abstract void Execute( object? parameter );
}
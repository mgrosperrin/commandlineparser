using System;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.CommandLineParser.Command;

/// <summary>
///     Defines an base abstraction for the commands. It adds the implementation of the <seealso cref="CommandData.Arguments" />
///     property, and a
///     <seealso
///         cref="HelpedCommandData.Help" />
///     option.
/// </summary>
public abstract class CommandBase<TCommandData> : ICommandHandler<TCommandData>
    where TCommandData : HelpedCommandData, new()
{
    /// <summary>
    ///     Initializes a new instance of a <see cref="CommandBase{TCommandData}" /> .
    /// </summary>
    protected CommandBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    ///     Gets the console used by the parser (if the command needs to writes something).
    /// </summary>
    protected IConsole Console => ServiceProvider.GetRequiredService<IConsole>();

    /// <summary>
    ///     Gets the <see cref="IServiceProvider" /> of the parsing operation.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; private set; }

    ///// <summary>
    /////     Gets the <see cref="CommandType" /> of the command.
    ///// </summary>
    //protected ICommandType CommandType { get; private set; }


    /// <summary>
    ///     Executes the command.
    /// </summary>
    /// <returns> Return 0 is everything was right, an negative error code otherwise. </returns>
    public virtual Task<int> ExecuteAsync(TCommandData commandData)
    {
        if (commandData.Help)
        {
            var helpWriter = ServiceProvider.GetRequiredService<IHelpWriter>();
            helpWriter.WriteHelpForCommand(commandData.CommandType);
            return Task.FromResult(0);
        }
        return ExecuteCommandAsync(commandData);
    }

    ///// <summary>
    /////     Configure the command with the <see cref="ICommandType" /> representing the command.
    ///// </summary>
    ///// <param name="commandType">The <see cref="CommandType" /> of the command.</param>
    //public virtual void Configure(ICommandType commandType)
    //{
    //    CommandType = commandType;
    //}

    /// <summary>
    ///     Executes the command.
    /// </summary>
    /// <returns> Return 0 is everything was right, an negative error code otherwise. </returns>
    protected abstract Task<int> ExecuteCommandAsync(TCommandData commandData);
}
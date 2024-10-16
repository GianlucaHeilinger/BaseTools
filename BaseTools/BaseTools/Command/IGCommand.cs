using System.Windows.Input;

namespace BaseTools.Command
{
    /// <summary>
    /// Defines a command that can be executed with a parameter of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    public interface IGCommand<T> : ICommand
    {
        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">The parameter to be passed to the command.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        bool CanExecute(T parameter);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter to be passed to the command.</param>
        void Execute(T parameter);
    }

    /// <summary>
    /// Defines a command that can be executed without parameters.
    /// </summary>
    public interface IGCommand : ICommand
    {
    }
}
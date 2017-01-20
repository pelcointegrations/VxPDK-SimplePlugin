using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OverlayDrawings.Model
{
    /// <summary>
    /// This class represents a command or action that can be represented
    /// in a view.
    /// </summary>
    class CommandViewModel
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentException("Command is null");
            }

            DisplayName = displayName;
            Command = command;
        }

        /// <summary>
        /// The display name of the command.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// The command to be executed.
        /// </summary>
        public ICommand Command { get; private set; }
    }
}

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace BlackBoxAI.VSExtension.Commands
{
    internal sealed class BlackBoxAIToolWindowCommand
    {
        public const int CommandId = 0x0101;
        public static readonly Guid CommandSet = new Guid("12345678-1234-1234-1234-123456789013");
        private readonly AsyncPackage package;

        private BlackBoxAIToolWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(ShowToolWindow, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static BlackBoxAIToolWindowCommand Instance { get; private set; }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new BlackBoxAIToolWindowCommand(package, commandService);
        }

        private void ShowToolWindow(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            ToolWindows.BlackBoxAIWindow.ShowToolWindow(package);
        }
    }
}

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace BlackBoxAI.VSExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(ToolWindows.BlackBoxAIWindow))]
    [Guid(PackageGuidString)]
    public sealed class BlackBoxAIVSExtensionPackage : AsyncPackage
    {
        public const string PackageGuidString = "12345678-1234-1234-1234-123456789012";

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await Commands.BlackBoxAICommand.InitializeAsync(this);
            await Commands.BlackBoxAIToolWindowCommand.InitializeAsync(this);
        }
    }
}

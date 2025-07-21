using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace BlackBoxAI.VSExtension.ToolWindows
{
    [Guid("12345678-1234-1234-1234-123456789014")]
    public class BlackBoxAIWindow : ToolWindowPane
    {
        public BlackBoxAIWindow() : base(null)
        {
            this.Caption = "BlackBox AI Assistant";
            this.Content = new BlackBoxAIWindowControl();
        }

        public static void ShowToolWindow(AsyncPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            
            ToolWindowPane window = package.FindToolWindow(typeof(BlackBoxAIWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}

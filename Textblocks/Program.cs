using System;
using System.Resources;
using System.Windows.Forms;

[assembly: NeutralResourcesLanguageAttribute("de-DE")]

namespace cwsoft.Textblocks;

// Main entry point of this application.
// Catches exceptions not treated by the application itself.
internal static class Program
{
   /// <summary>
   /// Der Haupteinstiegspunkt für die Anwendung.
   /// </summary>
   [STAThread]
   private static void Main()
   {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // Deal with unhandled application exceptions when not in debug mode.
      Application.SetUnhandledExceptionMode(!System.Diagnostics.Debugger.IsAttached
          ? UnhandledExceptionMode.ThrowException
          : UnhandledExceptionMode.Automatic
      );

      // Register handler to deal with exceptions not captured by the main app (not recoverable).
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);

      Application.Run(new Gui.App());
   }

   private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
   {
      // Show exception message before quiting the application.
      try {
         if (args.ExceptionObject is Exception ex) {
            _ = MessageBox.Show($"Uups, ein unbehandelter Fehler ist aufgetreten.\n{ex.Message}\n\n" +
            $"Auslöser: {ex.TargetSite}",
            "Anwendungsfehler: Textblocks wird beendet", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         else {
            _ = MessageBox.Show($"Uups, ein unbehandelter Fehler ist aufgetreten.",
            "Anwendungsfehler: Textblocks wird beendet", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      finally {
         GC.Collect();
         GC.WaitForPendingFinalizers();
         Application.Exit();
      }
   }
}

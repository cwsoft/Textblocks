using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cwsoft.Textblocks.Gui;

// This class implements the main application GUI the user interacts with.
public partial class App: Form
{
   #region // Fields, Properties, Constructors
   // Private constants and fields.
   private const int _minimumRequiredSearchPatternLength = 3;
   private Catalog.CatalogManager? _catalogManager = null;
   private List<Model.Textblock> _activeTextblocks = new();
   private readonly Helper.ListFilter _filter = new();

   // Public properties. 
   public string InfoText { get => Rtb_Preview.Text; set => Rtb_Preview.Text = value; }
   public string StatusBarText { get => Lbl_StatusBar.Text; set => Lbl_StatusBar.Text = value; }

   // Constructor. 
   // Blocking tasks moved to App_Shown so we can provide App progress status on start-up.
   public App()
   {
      InitializeComponent();
      Font = new Font("Segoe UI", 9f, FontStyle.Regular);
      ApplyFormControlDefaults();
   }
   #endregion

   #region // App main entry point, shutdown handler and reset.
   // Initialize catalog manager and open last used catalog on startup.
   // Blocking tasks moved here so we can show progress info at startup.
   private void App_Shown(object sender, EventArgs e)
   {
      // Initialize catalog object.
      StatusBarText = "Initialisiere Textblocks ... bitte warten";
      using (new Helper.BlockingTask(Rtb_Preview, "Initialisiere Textblocks ... bitte warten")) {
         _catalogManager = new(infoControl: Rtb_Preview);
         // Shutdown Textblocks app if MS Word connection failed (e.g. MS Word not installed).
         if (!_catalogManager.IsInitialized) {
            ShutdownApp();
         }
      }
      // Try to open last used catalog and refresh UI.
      OpenLastUsedCatalogIfExists();
      RefreshUI(refreshTextblocks: true);
   }

   // Shutdown app in case no MS Word connection could be established.
   private void ShutdownApp()
   {
      using (new Helper.BlockingTask(Rtb_Preview, "Textblocks wird beendet ... bitte warten")) {
         _ = MessageBox.Show("Verbindung zu MS Word konnte nicht aufgebaut werden.\n" +
            "Um Textblocks zu nutzen, muss MS Word installiert sein.",
            "Textblocks wird beendet",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
         );
         Close();
      }
   }

   // Reset app into defined state in case InteropService.COMException was raised within RefreshUI.
   private void ResetApp()
   {
      // Close actual opened MS Word catalog and release combobox data binding.
      _catalogManager?.CloseCatalog();
      (Cbx_Categories.DataSource, Cbx_Textblocks.DataSource) = (null, null);

      _ = MessageBox.Show(
         "Die Verbindung zu MS-Word wurde unterbrochen.",
         "Fehler im Interop Service",
         MessageBoxButtons.OK,
         MessageBoxIcon.Warning
      );

      RefreshUI(refreshTextblocks: true);
   }
   #endregion

   #region // Internal API
   // Try to autoload last opened catalog if exists.
   private void OpenLastUsedCatalogIfExists()
   {
      // Open last used catalog document if exists.
      string lastDocumentPath = Properties.Settings.Default.LastUsedCatalog;
      using (new Helper.BlockingTask(Rtb_Preview, $"Lade letzten Katalog '{Catalog.CatalogManager.GetFilenameOrDefault(lastDocumentPath)}' ... bitte warten")) {
         if (_catalogManager is not null && _catalogManager.OpenCatalog(lastDocumentPath, allowCatalogSelection: false)) {
            LoadAndDisplayCatalogData(lastDocumentPath);
            return;
         }
      }

      // Unset last used catalog if not exists to skip check on next start-up.
      _catalogManager?.CloseCatalog();
      Properties.Settings.Default.LastUsedCatalog = string.Empty;
   }

   // Load specified catalog and refresh UI.
   private void LoadAndDisplayCatalogData(string catalogPath)
   {
      if (_catalogManager is null) {
         return;
      }

      // Reset filter as we won't apply filter settings of actual catalog on newly opened catalogs.
      _filter.ResetPattern();

      // Extract categories and textblocks from actual catalog file (either WordFile or DataFile).
      using (new Helper.BlockingTask()) {
         InfoText = $"Lade Katalog '{Catalog.CatalogManager.GetFilenameOrDefault(catalogPath)}' ... bitte warten";
         StatusBarText = $"Lade '{catalogPath}'";
         if (_catalogManager.OpenCatalog(catalogPath)) {
            StatusBarText = $"{_catalogManager.Data}";
            Properties.Settings.Default.LastUsedCatalog = catalogPath;
         }

         // Update category and textblock data binding then refresh UI.
         Cbx_Categories.DataSource = _catalogManager.Data.Categories.ToList();
         Cbx_Textblocks.DataSource = _catalogManager.Data.Textblocks.ToList();
         RefreshUI(refreshTextblocks: true);

         // Show a status message in case no valid category or textblock exists.
         if (_catalogManager.Data.Categories.Count == 0 || _catalogManager.Data.Textblocks.Count == 0) {
            StatusBarText = $"Katalog '{catalogPath}' ist fehlerhaft.";
            _ = MessageBox.Show("Die Katalogdatei ist leer oder fehlerhaft.\n" +
               "Bitte Katalogdatei und Word-Stilnamen überprüfen.",
               "Ungültige Katalogdatei (*.docx)",
               MessageBoxButtons.OK,
               MessageBoxIcon.Exclamation);
         }
      }
   }

   // Refresh UI to show textblocks for selected category matching optional filter critera.
   private void RefreshUI(bool refreshTextblocks = true)
   {
      try {
         // Only proceed if selected list box entry is a valid category object.
         if (Cbx_Categories.SelectedItem is not Model.Category category) {
            StatusBarText = (_catalogManager?.IsInitialized ?? false)
               ? "Keine gültige Katalogdatei geladen."
               : "Verbindung zu MS-Word wurde unterbrochen.";
            InfoText = $"Bitte gültigen Katalog über 'Datei -> Katalog öffnen' wählen, oder Programm beenden.";
            return;
         }

         // Get active textblocks for actual category matching optional filter criteria.
         FilterTextblocksByCategoryAndSearchPattern(refreshTextblocks, category);
         UpdateFilterControls(category);

         // Update preview panel with rich text of the actual selected textblock.
         UpdateTextblockPreview();
      }
      catch (System.Runtime.InteropServices.COMException) {
         // If we end up here, we most likely lost MS Word connection (e.g. killed process).
         // Reset app into defined state and let user decide how to proceed.
         ResetApp();
      }
   }

   // Filter textblocks for actual category matching optional filter criteria.
   private void FilterTextblocksByCategoryAndSearchPattern(bool refreshTextblocks, Model.Category category)
   {
      if (_catalogManager is not null && refreshTextblocks) {
         _activeTextblocks = _catalogManager.GetTextblocksByCategoryId(category.Id);
         _activeTextblocks = _filter.GetMatches(_activeTextblocks, propertyName: "Content", operatorAnd: Rbt_And.Checked);
         Cbx_Textblocks.DataSource = _activeTextblocks;
      }

      if (_activeTextblocks.Count == 0) {
         Cbx_Textblocks.Text = string.Empty;
         InfoText = "Für die ausgewählte Kategorie und Suchfilter sind keine Textblöcke vorhanden.";
      }
   }

   // Update preview with actual selected textblock content.
   private void UpdateTextblockPreview()
   {
      if (Cbx_Textblocks.SelectedItem is Model.Textblock textblock) {
         Lbl_Category.Text = $"Kategorie: {_catalogManager?.GetCategoryById(textblock.CategoryId)?.Heading ?? "N/A"}";

         // Copy MS Word range of actual textblock to clipboard and extract rich text format for preview.
         if (_catalogManager?.GetTextblockDocumentRange(textblock) is Microsoft.Office.Interop.Word.Range range) {
            range.Copy();

            // HACK: Avoid empty textblock preview e.g. due to timing issues with clipboard.
            string rtfText;
            int trials = 0;
            do {
               rtfText = Clipboard.GetText(TextDataFormat.Rtf);
               trials++;
            } while (trials <= 3 && !string.IsNullOrEmpty(range.Text) && string.IsNullOrEmpty(rtfText));

            // Replace generic textblock Id with current textblock Id.
            Rtb_Preview.Rtf = rtfText.Replace(@"1.\tab}", $@"{textblock.Id}.\tab}}");

            // Highlight all and/or search patterns in the actual textblock preview text.
            if (!string.IsNullOrEmpty(_filter.ValidatedPattern) && Rtb_Preview.Rtf.Length > 0) {
               HighlightSearchPatternsInTextblockPreview();
            }
         }
      }
   }

   // Highlight all matches of and/or search patterns in actual textblock preview.
   private void HighlightSearchPatternsInTextblockPreview()
   {
      foreach (string highlightPattern in _filter.InclusivePatterns) {
         int nextPos = Rtb_Preview.Text.IndexOf(highlightPattern, 0, StringComparison.OrdinalIgnoreCase);
         while (nextPos > -1) {
            Rtb_Preview.Select(nextPos, highlightPattern.Length);
            Rtb_Preview.SelectionBackColor = Color.Yellow;
            nextPos = Rtb_Preview.Text
               .IndexOf(highlightPattern, nextPos + highlightPattern.Length, StringComparison.OrdinalIgnoreCase);
         }
      }
   }
   #endregion

   #region // Update of form controls to reflect state.
   // Update filter controls to reflect actual state.
   private void UpdateFilterControls(Model.Category category)
   {
      Grp_Textblocks.Text = $"Verfügbare Textblöcke ({_activeTextblocks.Count}/{category.NbrTextblocksInCategory}):";

      // Highlight filter controls blue in case filter settings are active.
      Grp_SearchFilter.Text = string.IsNullOrWhiteSpace(_filter.ValidatedPattern)
         ? "Suchfilter inaktiv:"
         : $"Suchfilter aktiviert (Treffer: {_activeTextblocks.Count}):";
      Grp_SearchFilter.ForeColor = string.IsNullOrWhiteSpace(_filter.ValidatedPattern) ? Color.Black : Color.Blue;
      Tbx_SearchFilter.ForeColor = Grp_SearchFilter.ForeColor;
   }

   // Apply defaults for some main app form control elements like filter tool tips.
   private void ApplyFormControlDefaults()
   {
      // Show App version in main window title.
      Text = $"Textblocks - Version {Properties.Resources.AppVersion} - (c) Christian Sommer";

      // Change defaults of status bar tool strip labels.
      StatusBarText = string.Empty;
      Lbl_StatusBar.Alignment = ToolStripItemAlignment.Left;

      // Configure tool tips for the filter controls.
      ToolTip.AutoPopDelay = 10000;
      ToolTip.ShowAlways = true;

      string searchTip = "Suchbegriffe werden mit Leerzeichen getrennt (z.B.: begriff1 begriff2).\n" +
         $"Jeder Suchbegriff muss mindestens {_minimumRequiredSearchPatternLength} Zeichen enthalten.\n\n" +
         "UND/ODER ändert das Suchverhalten (ALLE bzw. min. EIN Begriff).\n" +
         "Suchbegriffe mit - am Anfang werden ausgeschlossen (z. B.: -begriff1).\n\n" +
         "Die Suche unterscheidet nicht zwischen Groß- und Kleinschreibung.";
      ToolTip.SetToolTip(Tbx_SearchFilter, searchTip);
      ToolTip.SetToolTip(Lbl_SearchFilter, searchTip);

      ToolTip.SetToolTip(Btn_ApplyFilter, "Filtereinstellungen anwenden (oder [ENTER] drücken).");
      ToolTip.SetToolTip(Btn_ResetFilter, "Filtereinstellungen löschen (oder [ESC] drücken).");
      ToolTip.SetToolTip(Rbt_And, "Zeigt Textblöcke die ALLE Suchbegriffe enthalten.");
      ToolTip.SetToolTip(Rbt_Or, "Zeigt Textblöcke die min. EINEN Suchbegriff enthalten.");
   }
   #endregion

   #region // UI Control Event Handler
   // Store last opened catalog path and dispose catalog object when form is closed.
   private void App_FormClosing(object sender, FormClosingEventArgs e)
   {
      // Save default settings so we can autoload the actual loaded WordFile during next startup.
      Properties.Settings.Default.Save();

      // Free managed and unmanaged Model.Catalog resources.
      _catalogManager?.Dispose();
   }

   // Don't use _SelectionChange event to avoid updates triggerd by application code.
   private void Cbx_Categories_SelectionChangeCommitted(object sender, EventArgs e) => RefreshUI(refreshTextblocks: true);

   // Don't use _SelectionChange event to avoid updates triggerd by application code.
   private void Cbx_Textblocks_SelectionChangeCommitted(object sender, EventArgs e) => RefreshUI(refreshTextblocks: false);

   // Re-apply filter, if filter option (AND/OR) changes after filter was enabled.
   private void Rbt_And_CheckedChanged(object sender, EventArgs e)
   {
      if (!string.IsNullOrWhiteSpace(_filter.ValidatedPattern)) {
         Btn_ApplyFilter.PerformClick();
      }
   }

   // Apply filter if ENTER key is pressed inside search filter textbox.
   // Reset filter if filter pattern changes after filter was enabled.
   private void Tbx_SearchFilter_KeyDown(object sender, KeyEventArgs e)
   {
      // Reset filter if filter pattern was changed.
      if (!string.IsNullOrWhiteSpace(_filter.ValidatedPattern)) {
         _filter.ResetPattern();
         RefreshUI(refreshTextblocks: true);
      }

      // Apply filter, if return key is pressed inside search filter textbox.
      if (e.KeyCode == Keys.Enter) {
         Btn_ApplyFilter.PerformClick();

         // Suppress enter for App form to avoid beep sound.
         (e.Handled, e.SuppressKeyPress) = (true, true);
         return;
      }
   }

   // Validate user defined search pattern and refresh form to reflect filter settings.
   private void Btn_ApplyFilter_Click(object sender, EventArgs e)
   {
      // Set new filter and validate search patterns (supporting boolean operators).
      _filter.SetPattern(Tbx_SearchFilter.Text, _minimumRequiredSearchPatternLength);

      // Enable filter mode, if initial and validated search patterns match.
      if (_filter.InputPattern == _filter.ValidatedPattern) {
         RefreshUI(refreshTextblocks: true);
         if (_activeTextblocks.Count > 0) {
            _ = Cbx_Textblocks.Focus();
         }
         return;
      }

      // Disable filter mode and show warning, if no or only invalid search patterns were specified.
      if (string.IsNullOrWhiteSpace(_filter.ValidatedPattern)) {
         _ = MessageBox.Show("Die eingegebenen Suchparameter sind ungültig.\n" +
            $"Jeder Suchparameter muss mindestens {_minimumRequiredSearchPatternLength} Zeichen enthalten."
            , "Filter kann nicht angewandt werden"
            , MessageBoxButtons.OK, MessageBoxIcon.Information);
         return;
      }

      // If at least one valid sub pattern was specified, let user decide how to proceed.
      if (MessageBox.Show("Einige Suchparameter sind ungültig.\n" +
         $"Validierte Parameter: '{_filter.ValidatedPattern}'.\n\n" +
         "Suche mit den validierten Parametern durchführen?", "Filter mit validierten Parametern anwenden",
         MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

         Tbx_SearchFilter.Text = _filter.ValidatedPattern;
         RefreshUI(refreshTextblocks: true);
         if (_activeTextblocks.Count > 0) {
            _ = Cbx_Textblocks.Focus();
         }
         return;
      }

      // User rejected to apply validated sub pattern, so we invalidate it.
      RefreshUI(refreshTextblocks: true);
      return;
   }

   // Reset filter settings and refresh to show all textblocks of selected category.
   private void Btn_ResetFilter_Click(object sender, EventArgs e)
   {
      _filter.ResetPattern();
      Tbx_SearchFilter.Clear();
      Rbt_And.Checked = true;
      RefreshUI(refreshTextblocks: true);
   }
   #endregion

   #region // UI Menu Event Handler
   // Open catalog selected via file open dialog.
   private void Men_OpenCatalog_Click(object sender, EventArgs e)
   {
      if (_catalogManager is not null) {
         string documentPath = Catalog.CatalogManager.SelectCatalog();
         if (!string.IsNullOrEmpty(documentPath)) {
            InfoText = $"Lade Katalog '{Catalog.CatalogManager.GetFilenameOrDefault(documentPath)}' ... bitte warten";
            LoadAndDisplayCatalogData(documentPath);
         }
      }
   }

   private void Men_Quit_Click(object sender, EventArgs e) => Close();

   private void Men_Help_Click(object sender, EventArgs e)
   {
      // Assume help file "Textblocks.pdf" is located in the application startup folder.
      string helpFile = $@"{System.Windows.Forms.Application.StartupPath}\Textblocks.pdf";
      try {
         _ = System.Diagnostics.Process.Start(helpFile);
      }
      catch (Exception) {
         _ = MessageBox.Show($"Hilfedatei konnte nicht geladen werden.\nPfad: {helpFile}",
            "Hilfedatei kann nicht geladen werden",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation);
      }
   }

   private void Men_About_Click(object sender, EventArgs e) => new About().ShowDialog();
   #endregion
}

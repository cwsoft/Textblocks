using System.Windows.Forms;

namespace cwsoft.Textblocks.Gui;

// This class implements the about form of the application.
public partial class About: Form
{
   public About()
   {
      InitializeComponent();
      LblVersion.Text = $"(Version {Properties.Resources.AppVersion})";
      LblReleaseDate.Text = $"{Properties.Resources.AppReleaseDate}";
   }
}

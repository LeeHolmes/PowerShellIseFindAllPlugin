using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.PowerShell.Host.ISE;
using System.Text.RegularExpressions;

namespace FindAll
{
    /*
     Add-Type -Path 'd:\documents\Visual Studio Projects\FindAll\bin\Debug\FindAll.dll'
     $psISE.CurrentPowerShellTab.VerticalAddOnTools.Add(‘Find All’, [FindAll.FindAllWindow], $true)
    */

    /// <summary>
    /// Interaction logic for FindAll.xaml
    /// </summary>
    public partial class FindAllWindow : UserControl, IAddOnToolHostObject
    {
        public FindAllWindow()
        {
            InitializeComponent();
        }

        // Populated by the ISE because we implement the IAddOnToolHostObject interface.
        // Represents the entry-point to the ISE object model.
        public ObjectModelRoot HostObject
        {
            get;
            set;
        }

        // The user had typed new text in the search box
        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Clear the previous results
            this.SearchResults.Items.Clear();

            // Break the file into its lines
            string[] lineBreakers = new string[] { "\r\n" };
            string[] fileText = HostObject.CurrentPowerShellTab.Files.SelectedFile.Editor.Text.Split(
                lineBreakers, StringSplitOptions.None);

            // Try to see if their search text represents a Regular Expression
            Regex searchRegex = null;
            try
            {
                searchRegex = new Regex(this.SearchText.Text, RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                // Ignore the ArgumentException that we get if the regular expression is
                // not valid.
            }

            // Go through all of the lines in the file
            for (int lineNumber = 0; lineNumber < fileText.Length; lineNumber++)
            {
                // See if the line matches the regex or literal text
                if (
                    ((searchRegex != null) && (searchRegex.IsMatch(fileText[lineNumber]))) ||
                    (fileText[lineNumber].IndexOf(this.SearchText.Text, StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    // If so, add it to the search results box.
                    SearchResult result = new SearchResult() {
                        Line = lineNumber + 1, Content = fileText[lineNumber]
                    };
                    this.SearchResults.Items.Add(result);
                }
            }
        }

        // Detect a double-click on an item
        private void SearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectItem();
        }

        // Detect Enter on an item
        private void SearchResults_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SelectItem();
            }
        }

        // Move the editor caret to the position represented by the currently-selected search
        // result
        private void SelectItem()
        {
            SearchResult selectedItem = (SearchResult)this.SearchResults.SelectedItem;
            HostObject.CurrentPowerShellTab.Files.SelectedFile.Editor.SetCaretPosition(selectedItem.Line, 1);
        }
    }

    // A class to hold search results
    public class SearchResult
    {
        public int Line { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return String.Format("{0,5}: {1}", Line, Content);
        }
    }
}
using System;
using System.IO;
using System.Windows.Forms;

/*Custom Notepad App was made inspired off the original Microsoft Notepad App to practice
 using Windows Forms and be familiarized with the c# dotNET Framework
 This code was written and documented by 8BitBabe*/

namespace CustomNotepadApp
{
    public partial class FrmMain : Form
    {

        /*variables used to store the file name and the path of the file just opened, also to compare
         the string contents of the rich textbox; 
         booleans to determine if the app was opened by default or to open a file, to determine if the program is closing and
         if there was any changes to the rich textbox*/
        string fileName;
        string currentPath;

        public string contents = string.Empty;

        bool isArg;
        bool isClosing = false;
        bool isChanged;

        SaveFileDialog sfd = new SaveFileDialog();
        OpenFileDialog ofd = new OpenFileDialog();


        //App boot methods
        public FrmMain(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0) //now accepts command line arguments
            {
                string openFile = args[0];
                NameChange(openFile);
                textBox.LoadFile(openFile, RichTextBoxStreamType.PlainText);
                isArg = true;
                currentPath = openFile;
            }
            else
                isArg = false;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (isArg == false)
                NameChange("");

        }

        private void NameChange(string openingFile)
        {
            if (openingFile == "") {
                this.Text = "CustomNotepadApp - Untitled"; //if no arg, default title
            }
            else
            {
                //look up the location of the file and extract the name without the
                //extension to then alter the title bar of the current window
                string path = new FileInfo(openingFile).FullName;
                string fileTitle = Path.GetFileNameWithoutExtension(path);
                this.Text = "CustomNotepadApp - " + fileTitle;
            }
        }


        //File Menu methods start here
        //Exit methods will remain at the end
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //check if there's any written text so as to ask the user if they wish to save their work
            if (textBox.Text != contents)
            { 
                DialogResult dr = MessageBox.Show("Do You want to save the changes made to " + this.Text, "Save", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    //if yes, run the save method and open empty file
                    sfd.Title = "Save";
                    if (SaveAs() == 0)
                        //if the save method was cancelled, cancel operation
                        return;
                    else
                    {
                        //if Save method executes, open new file
                        textBox.Text = "";
                        NameChange("");
                    }
                    contents = "";
                }
                else if (dr == DialogResult.No)
                {
                    //if no, just open new file
                    textBox.Text = "";
                    NameChange("");
                    contents = "";
                }
                else
                {
                    //keep what's on screen, new doesn't execute
                    textBox.Focus();
                }
            }
            else
            {
                //if the box has nothing, just open the new file 
                NameChange("");
                textBox.Text = "";
                contents = "";
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox.Text != contents)
            {                
                //check if there's any written text so as to ask the user if they wish to save their work
                DialogResult dr = MessageBox.Show("Do You want to save the changes made to " + this.Text, "Save", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    //if yes, save current, open new and refresh the title bar
                    sfd.Title = "Save";
                    Save();
                    Open();
                    NameChange(ofd.FileName);
                }
                else if (dr == DialogResult.No)
                {
                    //open new and refresh title
                    Open();
                    NameChange(ofd.FileName);
                }
                else
                {
                    textBox.Focus();
                }
            }
            else
            {
                Open();
                NameChange(ofd.FileName);
            }
        }

        private void Open()
        {
            ofd.Filter = "Text Documents|*.txt";
            ofd.DefaultExt = ".txt";
            //checks for changes in current window, if true, saves changes and opens other file

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //will only open if user presses okay
                textBox.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);
                currentPath = ofd.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        int SaveAs()
        {
            sfd.Filter = "Text Documents|*.txt";
            sfd.DefaultExt = ".txt";
            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                //operation canceled
                textBox.Focus();
                return 0;
            }
            else
            {
                fileName = sfd.FileName;

                //save and adjust the title bar 
                textBox.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);
                if (isClosing == false) //if saving before closing the app, don't alter title
                    NameChange(fileName);

                //set to false because all changes have been saved
                isChanged = false;
                return 1;
            }
        }    

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (isChanged == true)
            {
                if (Text != "CustomNotepadApp - Untitled") //if title is not default
                {
                    using (StreamWriter writer = new StreamWriter(currentPath))
                    {
                        writer.WriteLine(textBox.Text); //overwrite what the original file had in the textbox
                        isChanged = false;
                    }

                }
                else
                    SaveAs();
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            //checks if there have been any changes in the rich textbox
            isChanged = true;
        }


        //Edit Menu methods start here
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //undoes an action
            textBox.Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //redoes an action
            textBox.Redo();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cuts text
            textBox.Cut();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //copies text
            textBox.Copy();
        }


        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pastes text
            textBox.Paste();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //deletes text
            textBox.Clear();
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //selects everything in the text box
            textBox.SelectAll();
        }


        //Format Menu methods start here
        private void WordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //activates or deactivates word wrap and checks/unchecks the box
            textBox.WordWrap = (textBox.WordWrap == true) ? false : true;
            btnWrap.Checked = (btnWrap.Checked == true) ? false : true;
        }

        private void FontToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //change font and size
            fontDialog1.ShowColor = true;

            fontDialog1.Font = textBox.Font;
            fontDialog1.Color = textBox.ForeColor;

            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                textBox.Font = fontDialog1.Font;
                textBox.ForeColor = fontDialog1.Color;
            }
        }


        //View Menu methods and the Search Bar methods start here 
        private void btnFind_Click(object sender, EventArgs e)
        {
            //finds all words in the richtextBox, selects first occurence
            textBox.Find(findBox.Text);
        }
        private void btnReplace_Click(object sender, EventArgs e)
        {
            //replaces all occurences of text in the findBox and replaces them 
            //with the text in the replaceBox
            textBox.Text = textBox.Text.Replace(findBox.Text, replaceBox.Text);
        }

        private void SearchBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //checks wether the user wants to see the seacrch bar or not; checks box accordingly
            searchBar.Visible = (searchBar.Visible == true) ? false : true;
            btnSearchBar.Checked = (btnSearchBar.Checked == true) ? false : true;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //toggles the visibility for the status bar and un/checks the button
            statusBar.Visible = (statusBar.Visible == true) ? false : true;
            btnStatusBar.Checked = (btnStatusBar.Checked == true) ? false : true;
        }


        //Help menu method starts here 
        private void AboutCustomNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Custom NotepadApp brought to life by 8B2Studios. " +
                "\nLook us up on Facebook, Instagram and Twitter!", "About NotepadApp", 
                MessageBoxButtons.OK);
        }


        //Application Closing methods start here 
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseFile();
            if (isClosing == false)
                e.Cancel = true; //cancel the closing event
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CloseFile()
        {
            //exit
            if (textBox.Text != contents)
            {
                if (isChanged == true)
                {
                    //check if there's any written text so as to ask the user if they wish to save their work
                    DialogResult dr = MessageBox.Show("Do You want to save the changes made to " + this.Text, "Save", MessageBoxButtons.YesNoCancel);
                    if (dr == DialogResult.Yes)
                    {
                        isClosing = true;
                        //if yes, save current and close
                        sfd.Title = "Save";
                        Save();
                    }
                    else if (dr == DialogResult.No)
                    {
                        isClosing = true; //don't save, continue event
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        isClosing = false; //cancel event
                        textBox.Focus();
                    }
                }
            }
            else
            {
                isClosing = true; //nothing to save, just close
            }
        }
    }
}

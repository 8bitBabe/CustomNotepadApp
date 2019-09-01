using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomNotepadApp
{
    public partial class FrmMain : Form
    {
        string fileName;

        SaveFileDialog sfd = new SaveFileDialog();
        OpenFileDialog ofd = new OpenFileDialog();

        public string contents = string.Empty;

        public FrmMain(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0) //now accepts command line arguments
            {
                string openFile = args[0];
                string loadPath = new FileInfo(openFile).FullName;
                string loadTitle = Path.GetFileNameWithoutExtension(loadPath);
                this.Text = "CustomNotepadApp - " + loadTitle;
                textBox.LoadFile(openFile, RichTextBoxStreamType.PlainText);
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //undoes an action
            textBox.Undo();
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //copies text
            textBox.Copy();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //cuts text
            textBox.Cut();
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

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //redoes an action
            textBox.Redo();
        }

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
                    if (saveAs() == 0)
                        //if the save method was cancelled, cancel operation
                        return;
                    else
                    {
                        //if Save method executes, open new file
                        textBox.Text = "";
                        this.Text = "Untitled";
                    }
                    contents = "";
                }
                else if (dr == DialogResult.No)
                {
                    //if no, just open new file
                    textBox.Text = "";
                    this.Text = "Untitled";
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
                this.Text = "Untitled";
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
                    NameChange();
                }
                else if (dr == DialogResult.No)
                {
                    //open new and refresh title
                    Open();
                    NameChange();
                }
                else
                {
                    textBox.Focus();
                }
            }
            else
            {
                Open();
                NameChange();
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
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }

        int saveAs()
        {
            sfd.Filter = "Text Documents|*.txt";
            sfd.DefaultExt = ".txt";
            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                //operation canceled, continue on current work and return 0
                textBox.Focus();
                return 0;
            }
            else
            {
                contents = textBox.Text; //contents of the rich text box
                if (this.Text == "CustomNotepadApp - Untitled")
                {
                    //run Save and adjust the title bar 
                    textBox.SaveFile(this.Text, RichTextBoxStreamType.PlainText);
                    NameChange();
                }
                else
                {
                    //just save the file if there was already a file name 
                    textBox.SaveFile(fileName, RichTextBoxStreamType.PlainText);
                }
                return 1;
            }
        }    

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        void Save()
        {
            if (fileName != null)
            {
                //if there's no file name, write one
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine(textBox.Text);
                }
            }

            else
                saveAs();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //finds all words in the richtextBox, selects first occurance
            textBox.Find(findBox.Text);
        }
        private void btnReplace_Click(object sender, EventArgs e)
        {
            //replaces all occurances of text in the findBox and replaces them 
            //with the text in the replaceBox
            textBox.Text = textBox.Text.Replace(findBox.Text, replaceBox.Text);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //exit
            Close();
        }

        private void SearchBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //checks wether the use wants to see the seacrch bar or not
            bool toShow = (searchBar.Visible == false) ? false : true;
        }

        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.SelectAll();
            //change font and size
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //check if there's a file name and run name change, if yes
            if (fileName != null)
                NameChange();
            else
                this.Text = "CustomNotepadApp - Untitled";
        }

        private void NameChange ()
        {
            //look up the location of the file and extract the name without the
            //extension to then alter the title bar of the current window
            string path = new FileInfo(ofd.FileName).FullName;
            string fileTitle = Path.GetFileNameWithoutExtension(path);
            this.Text = "CustomNotepadApp - " + fileTitle;
        }
    }
}

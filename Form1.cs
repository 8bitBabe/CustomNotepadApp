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

        public FrmMain()
        {
            InitializeComponent();
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
            if (textBox.Text != contents)
            {
                DialogResult dr = MessageBox.Show("Do You want to save the changes made to " + this.Text, "Save", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    sfd.Title = "Save";
                    if (saveAs() == 0)
                        return;
                    else
                    {
                        textBox.Text = "";
                        this.Text = "Untitled";
                    }
                    contents = "";
                }
                else if (dr == DialogResult.No)
                {
                    textBox.Text = "";
                    this.Text = "Untitled";
                    contents = "";
                }
                else
                {
                    textBox.Focus();
                }
            }
            else
            {
                this.Text = "Untitled";
                textBox.Text = "";
                contents = "";
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox.Text != contents)
            {
                DialogResult dr = MessageBox.Show("Do You want to save the changes made to " + this.Text, "Save", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    sfd.Title = "Save";
                    Save();
                    Open();
                    NameChange();
                }
                else if (dr == DialogResult.No)
                {
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
                textBox.Focus();
                return 0;
            }
            else
            {
                contents = textBox.Text;
                if (this.Text == "CustomNotepadApp - Untitled")
                {
                    textBox.SaveFile(this.Text, RichTextBoxStreamType.PlainText);
                    NameChange();
                }
                else
                {
                    //fileName = sfd.FileName;
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
            if (fileName != null)
                NameChange();
            else
                this.Text = "CustomNotepadApp - Untitled";
        }

        private void NameChange ()
        {

            string path = new FileInfo(ofd.FileName).FullName;
            string fileTitle = Path.GetFileNameWithoutExtension(path);
            this.Text = "CustomNotepadApp -" + fileTitle;
        }
    }
}

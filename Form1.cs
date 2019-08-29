using System;
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

        private void ReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //replaces text.
            //define how the person will insert the old and new text
            //TextBox.Text.Replace(oldText, newText);
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //redoes an action
            textBox.Redo();
        }

        bool CheckChanges()
        {
            return true;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //checks for changes in current window, if true, saves changes and opens other file
            if (CheckChanges())
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //will only open if user presses okay
                    textBox.LoadFile(openFileDialog1.FileName);
                }
            }
        }

        void DoSave(string savedFile)
        {
            //save the file
            fileName = savedFile;
            textBox.SaveFile(savedFile);
        }

        void DoSaveAs()
        {
            //prompt for a name
            if (saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                DoSave(saveFileDialog1.FileName);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if file is new (has no name) then the Save As will be 
            //prompted so as to give a name, otherwise it'll just save
            if (string.IsNullOrEmpty(fileName))
            {
                DoSaveAs();
            }
            else
            {
                DoSave(fileName);
            }
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
    }
}

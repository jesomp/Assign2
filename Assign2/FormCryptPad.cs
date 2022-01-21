using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Assign2
{
    public partial class FormCryptPad : Form
    {
        private const int STRINGNOTFOUND = -1;
        private const int STARTPOSITION = 0;

        private uint encryptionKey;
        private string mainTextBoxString;
        private string stringFromFindForm;
        private int foundStringIndex;
        private int foundStringLength;
        private int newPosition;

        EncryptDecrypt ed;
        private readonly FormFind ff;

        public FormCryptPad()
        {
            InitializeComponent();
            ff = new FormFind();
        }

        //open text file to read and display in main textbox
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openTextFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = openTextFile.FileName;
                string textFile = File.ReadAllText(fileName);
                txtMainTextBox.Text = textFile;
                toolStripStatusLabel1.Text = fileName;
            }
        }

        //save text file
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveTextFile.ShowDialog() == DialogResult.OK)
            {
                string path = saveTextFile.FileName;
                string textFile = txtMainTextBox.Text;
                File.WriteAllText(path, textFile);
                toolStripStatusLabel1.Text = path;
            }
        }

        //method to exit the program
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //method to clear all text boxes
        private void ClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtEncryptionKey.Text = "";
            txtMainTextBox.Text = "";
            toolStripStatusLabel1.Text = "";
        }

        //method to select all from main text area
        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtMainTextBox.Focus();
            txtMainTextBox.SelectAll();
        }

        //method to cut text
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtMainTextBox.Cut();
        }

        //method to copy text
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtMainTextBox.Copy();
        }

        //method to paste text
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtMainTextBox.Paste();
        }

        //find text in the main text box
        private void FindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindMethod();
        }

        //find next text
        private void FindNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stringFromFindForm = ff.GetStringToFind;

            //show dialog to enter text if text to search is null
            if (stringFromFindForm == null)
                FindMethod();
            else
                FindString(stringFromFindForm, newPosition);
        }

        //Find Method
        private void FindMethod()
        {
            ff.ShowDialog();
            stringFromFindForm = ff.GetStringToFind;

            //validate if text to search is null
            if (stringFromFindForm != null)
                FindString(stringFromFindForm);
        }

        //method to enable or disable word wrap in main text area
        private void WordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWrapToolStripMenuItem.CheckState == CheckState.Checked)
                txtMainTextBox.WordWrap = true;
            else if (wordWrapToolStripMenuItem.CheckState == CheckState.Unchecked)
                txtMainTextBox.WordWrap = false;
        }

        //method to open up Font Dialog box
        private void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();

            if (fd.ShowDialog() == DialogResult.OK)
                txtMainTextBox.Font = fd.Font;
        }

        //button to call encryption method
        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            string textToEncrypt;
            string encryptedString = "";

            if (!ValidateInput())
                PrintError();
            else
            {
                PreEncryptDecrypt(out textToEncrypt);

                //encrypt each character from the main textbox
                foreach (char c in textToEncrypt)
                {
                    encryptedString += ed.Encrypt(c);
                }

                PostEncryptDecrypt(encryptedString);
            }
        }

        //button to call decryption method
        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            string textToDecrypt;
            string decryptedString = "";

            if (!ValidateInput())
                PrintError();
            else
            {
                PreEncryptDecrypt(out textToDecrypt);

                //decrypt each character from the main textbox
                foreach (char c in textToDecrypt)
                {
                    decryptedString += ed.Decrypt(c);
                }

                PostEncryptDecrypt(decryptedString);
            }
        }

        //tasks to complete before encryption and decryption
        private void PreEncryptDecrypt(out string inputText)
        {
            encryptionKey = uint.Parse(txtEncryptionKey.Text);
            ed = new EncryptDecrypt(encryptionKey);
            inputText = txtMainTextBox.Text;
            txtMainTextBox.Text = "";
        }

        //tasks to complete after encryption and decryption
        private void PostEncryptDecrypt(string encryptedDecryptedString)
        {
            txtMainTextBox.Text = encryptedDecryptedString;
            txtEncryptionKey.Text = "";
            toolStripStatusLabel1.Text = GetStringHash(txtMainTextBox.Text);
        }

        //method to validate input
        private bool ValidateInput()
        {
            if (txtMainTextBox.Text == "" || txtEncryptionKey.Text == "")
                return false;

            uint encryptionKey;
            bool wasParseSuccessful = uint.TryParse(txtEncryptionKey.Text, out encryptionKey);

            return wasParseSuccessful;
        }

        //method to print error messages
        private static void PrintError()
        {
            string inputError;
            string inputError2;
            string inputError3;

            inputError = "Invalid Input!";
            inputError2 = "\nEncryption Key must be Unsigned Integer";
            inputError3 = "\nMain Textbox must have text to encrypt";
            MessageBox.Show(inputError + "\n" + inputError2 + "\n" + inputError3, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //find text in the main text box
        private void FindString(string stringToFind)
        {
            int currentPosition = STARTPOSITION;
            foundStringIndex = STRINGNOTFOUND;


            //start search from beginning of text box
            mainTextBoxString = txtMainTextBox.Text.Substring(currentPosition);
            foundStringIndex = mainTextBoxString.IndexOf(stringToFind);
            foundStringLength = stringToFind.Length;

            //if string found highlight and udpate current position
            if (foundStringIndex != STRINGNOTFOUND)
            {
                txtMainTextBox.Focus();
                currentPosition += foundStringIndex;
                txtMainTextBox.Select(currentPosition, foundStringLength);
                txtMainTextBox.ScrollToCaret();
                currentPosition++;
            }
            else
            {
                MessageBox.Show("No matches found!", "Find Text", MessageBoxButtons.OK, MessageBoxIcon.Information);
                currentPosition = STARTPOSITION;
                stringToFind = null;
            }

            //update new position
            newPosition = currentPosition;
        }

        //find next text in the main text box
        private void FindString(string stringToFind, int newSearchIndex)
        {
            foundStringIndex = STRINGNOTFOUND;

            //start search from current position of cursor
            mainTextBoxString = txtMainTextBox.Text.Substring(newSearchIndex);
            foundStringIndex = mainTextBoxString.IndexOf(stringToFind);
            foundStringLength = stringToFind.Length;

            //if string found highlight and udpate new search index
            if (foundStringIndex != STRINGNOTFOUND)
            {
                txtMainTextBox.Focus();
                newSearchIndex += foundStringIndex;
                txtMainTextBox.Select(newSearchIndex, foundStringLength);
                txtMainTextBox.ScrollToCaret();
                newSearchIndex++;
            }
            else
            {
                MessageBox.Show("No more matches found!", "Find Text", MessageBoxButtons.OK, MessageBoxIcon.Information);
                newSearchIndex = STARTPOSITION;
                stringToFind = null;
            }

            //update new position
            newPosition = newSearchIndex;
        }

        ///
        /// return a hashed string of the text string
        ///
        private string GetStringHash(string text)
        {
            // open up an MD5 hash service
            using (HashAlgorithm hashAlg = new MD5CryptoServiceProvider())
            {
                // get a byte array of the string
                byte[] textBytes = Encoding.ASCII.GetBytes(text);
                // get a byte array containing the hash
                byte[] hash = hashAlg.ComputeHash(textBytes);
                // return a formated string of the hash
                return BitConverter.ToString(hash);
            }
        }
    }
}
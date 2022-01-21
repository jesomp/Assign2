using System;
using System.Windows.Forms;

namespace Assign2
{
    public partial class FormFind : Form
    {
        private string stringToFind;

        public FormFind()
        {
            InitializeComponent();
        }

        //property to pass search text
        public string GetStringToFind
        {
            get { return stringToFind; }
        }

        private void TxtFind_TextChanged(object sender, EventArgs e)
        {
            //enable Ok button once text is entered
            if (txtFind.Text != "")
                btnOk.Enabled = true;
            else if (txtFind.Text == "")
                btnOk.Enabled = false;
        }

        //cancel if Cancel button is clicked
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            stringToFind = null;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            //store text to send for search
            stringToFind = txtFind.Text.ToString();
            this.Hide();

            //reset find form
            txtFind.Text = "";
            btnOk.Enabled = false;
        }
    }
}

using System;
using System.Windows.Forms;

namespace ImapX.Sample
{
    public sealed partial class InputBox : Form
    {
        public InputBox(string title, string text, string value ="")
        {
            InitializeComponent();
            Text = title;
            lblTitle.Text = title;
            lblText.Text = text;
            txtValue.Text = value;
        }

        public string Value
        {
            get { return txtValue.Text; }
        }

        public static string Show(string title, string text, string value = "", IWin32Window owner = null)
        {
            using (var dlg = new InputBox(title, text, value))
                if (dlg.ShowDialog(owner) == DialogResult.OK)
                    return dlg.Value;
            return null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            Activate();
            txtValue.Focus();
        }
    }
}
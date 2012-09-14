using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImapX.Sample
{
    public partial class FrmError : Form
    {
        public FrmError()
        {
           
        }

        public FrmError(Exception ex)
        {
            InitializeComponent();
            lblMessage.Text = ex.Message;
            txtStacktrace.Text = ex.ToString();
        }
    }
}

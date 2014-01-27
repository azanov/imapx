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
    public partial class FrmBodyStructure : Form
    {
        public FrmBodyStructure()
        {
            InitializeComponent();
        }

        public FrmBodyStructure(string value) : this()
        {
            txtStructure.Text = value;
        }
    }
}

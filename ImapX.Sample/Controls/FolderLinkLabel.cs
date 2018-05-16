using System.Drawing;

namespace ImapX.Sample.Controls
{
    public class FolderLinkLabel : System.Windows.Forms.LinkLabel
    {
        public FolderLinkLabel()
        {
            
        }

        private Folder _folder;

        public Folder Folder
        {
            get
            {
                return _folder;
            }
            set
            {
                _folder = value;
                Refresh();
            }
        }

        public new void Refresh()
        {
            Visible = _folder != null;

            if (_folder != null)
            {
                base.Font = new Font(base.Font, Folder.Selected ? FontStyle.Bold : FontStyle.Regular);
                LinkColor = Folder.Selected ? Color.FromArgb(255, 60, 173, 231) : Color.White;

                if (_folder.Exists == 0)
                    base.Text = _folder.Name;
                else if (_folder.Unseen == 0)
                    base.Text = $"{_folder.Name} ({_folder.Exists})";
                else
                    base.Text = $"{_folder.Name} ({_folder.Unseen} / {_folder.Exists})";

                base.Refresh();
            }
        }

    }
}

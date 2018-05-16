using ImapX.Sample.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ImapX.Sample.Extenders
{
    [ProvideProperty("CueBannerText", typeof(Control))]
    public class CueBannerExtender : Component, IExtenderProvider
    {
        private readonly Dictionary<Control, string> _banners;
        private bool _showOnFocus;

        public CueBannerExtender()
        {
            _banners = new Dictionary<Control, string>();
        }
        
        public bool ShowOnFocus
        {
            get { return _showOnFocus; }
            set
            {
                _showOnFocus = value;
                
                foreach (var item in _banners)
                    SetCueBannerText(item.Key, item.Value, true);
            }
        }

        public bool CanExtend(object extendee)
        {
            return extendee is TextBox || extendee is ComboBox;
        }

        public void SetCueBannerText(Control control, string text)
        {
            SetCueBannerText(control, text, false);
        }

        private void SetCueBannerText(Control control, string text, bool update)
        {
            if (!update)
            {
                if (_banners.ContainsKey(control))
                    _banners[control] = text;
                else _banners.Add(control, text);
            }
            control.SetCueText(text, ShowOnFocus);
        }

        public string GetCueBannerText(Control control)
        {
            return _banners.ContainsKey(control) ? _banners[control] : string.Empty;
        }

    }
}
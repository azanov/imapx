using ImapX.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ImapX
{
    public class ImapIdentity : Dictionary<string, string>
    {
        public string Name
        {
            get
            {
                return GetValueOrDefault("name");
            }
            set
            {
                this["name"] = value;
            }
        }

        public string Version
        {
            get
            {
                return GetValueOrDefault("version");
            }
            set
            {
                this["version"] = value;
            }
        }

        public string OS
        {
            get
            {
                return GetValueOrDefault("os");
            }
            set
            {
                this["os"] = value;
            }
        }

        public string OSVersion
        {
            get
            {
                return GetValueOrDefault("os-version");
            }
            set
            {
                this["os-version"] = value;
            }
        }

        public string Vendor
        {
            get
            {
                return GetValueOrDefault("vendor");
            }
            set
            {
                this["vendor"] = value;
            }
        }

        public string SupportUrl
        {
            get
            {
                return GetValueOrDefault("support-url");
            }
            set
            {
                this["support-url"] = value;
            }
        }

        public string Address
        {
            get
            {
                return GetValueOrDefault("address");
            }
            set
            {
                this["address"] = value;
            }
        }

        public DateTime? ReleaseDate
        {
            get
            {
                var value = GetValueOrDefault("date");
                if (!string.IsNullOrWhiteSpace(value))
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(value, "", DateTimeExtensions.EnUSCulture, DateTimeStyles.None, out dt))
                        return dt;
                }
                return null;
            }
            set
            {
                this["date"] = value == null ? null : value.Value.ToImapDate();
            }
        }

        public string StartupCommand
        {
            get
            {
                return GetValueOrDefault("command");
            }
            set
            {
                this["command"] = value;
            }
        }

        public string Arguments
        {
            get
            {
                return GetValueOrDefault("arguments");
            }
            set
            {
                this["arguments"] = value;
            }
        }

        public string Environment
        {
            get
            {
                return GetValueOrDefault("environment");
            }
            set
            {
                this["environment"] = value;
            }
        }

        protected string GetValueOrDefault(string key, string defaultValue = null)
        {
            string value;
            if (TryGetValue(key, out value))
                return value;
            return defaultValue;
        }
    }
}

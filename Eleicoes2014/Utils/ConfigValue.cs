namespace Eleicoes2014.Utils
{
    using System;
    using System.Reflection;
    using System.Windows.Resources;
    using System.Xml;

    public class ConfigValue
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string _appVersion, _appCopyright, _appDescription, _appDeveloper, _developerEmailTo, _applicationIdForTransparenciaBrasil, _errorMessage;

        #region Properties

        public string AppVersion
        {
            get
            {
                return GetAppVersion();
            }
        }

        public string AppCopyright
        {
            get
            {
                return GetAppCopyright();
            }
        }

        public string AppDescription
        {
            get
            {
                return GetAppDescription();
            }
        }

        public string AppDeveloper
        {
            get
            {
                return GetAppDeveloper();
            }
        }

        public string DeveloperEmailTo
        {
            get
            {
                return GetDeveloperEmailTo();
            }
        }

        public string ApplicationIdForTransparenciaBrasil
        {
            get
            {
                return GetApplicationIdForTransparenciaBrasil();
            }
        }

        public string ErrorMessage
        {
            get
            {
                return GetErrorMessage();
            }
        }

        #endregion

        #region Methods

        private string GetAppVersion()
        {
            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    _appVersion = ((AssemblyFileVersionAttribute)customAttributes[0]).Version;
                }
            }
            return _appVersion;
        }

        private string GetAppCopyright()
        {
            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    _appCopyright = ((AssemblyCopyrightAttribute)customAttributes[0]).Copyright;
                }
            }
            return _appCopyright;
        }

        private string GetAppDescription()
        {
            if (assembly != null)
            {
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    _appDescription = ((AssemblyDescriptionAttribute)customAttributes[0]).Description;
                }
            }
            return _appDescription;
        }

        private string GetAppDeveloper()
        {
            StreamResourceInfo xml = App.GetResourceStream(new Uri("WMAppManifest.xml", UriKind.Relative));
            XmlReader reader = XmlReader.Create(xml.Stream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "App")
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "Author")
                            {
                                _appDeveloper = reader.Value;
                            }
                        }
                    }
                }
            }
            return _appDeveloper;
        }

        private string GetDeveloperEmailTo()
        {
            StreamResourceInfo xml = App.GetResourceStream(new Uri("Config.xml", UriKind.Relative));
            XmlReader reader = XmlReader.Create(xml.Stream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Emails")
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "DeveloperEmailTo")
                            {
                                _developerEmailTo = reader.Value;
                            }
                        }
                    }
                }
            }
            return _developerEmailTo;
        }

        private string GetApplicationIdForTransparenciaBrasil()
        {
            StreamResourceInfo xml = App.GetResourceStream(new Uri("Config.xml", UriKind.Relative));
            XmlReader reader = XmlReader.Create(xml.Stream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "App")
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.MoveToAttribute("ApplicationId"))
                            {
                                _applicationIdForTransparenciaBrasil = reader.Value;
                            }
                        }
                    }
                }
            }
            return _applicationIdForTransparenciaBrasil;
        }

        private string GetErrorMessage()
        {
            StreamResourceInfo xml = App.GetResourceStream(new Uri("Config.xml", UriKind.Relative));
            XmlReader reader = XmlReader.Create(xml.Stream);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "ErrorMessages")
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "ErrorMessage")
                            {
                                _errorMessage = reader.Value;
                            }
                        }
                    }
                }
            }
            return _errorMessage;
        }

        #endregion
    }
}

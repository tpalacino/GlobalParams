using GlobalParams.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GlobalParams.Config
{
    /// <summary>Represents settings for the multi-project template wizard.</summary>
    internal class SettingsMPT
    {
        #region Member Variables

        /// <summary>The list of project mappings.</summary>
        private List<MappedTemplate> m_ProjectMappings = new List<MappedTemplate>();

        #endregion Member Variables

        #region Constructors

        /// <summary>Creates a new instance of <see cref="SettingsMPT"/> from the specified <paramref name="xml"/>.</summary>
        /// <param name="xml">The xml representation of the settings.</param>
        internal SettingsMPT(XElement xml)
        {
            if (xml != null && xml.Name != null && xml.Name.LocalName.Equals(Constants.XML_ELEM_SETTINGS))
            {
                XAttribute nameAttr = null, pathAttr = null, templateAttr = null;
                string name = string.Empty, path = string.Empty, template = string.Empty;
                foreach (XElement mapElem in xml.Elements().Where(x => x.Name.LocalName.Equals(Constants.XML_ELEM_MAPPED_PROJECT_TEMPLATE)))
                {
                    if (mapElem != null)
                    {
                        nameAttr = mapElem.Attribute(Constants.XML_ATTR_NAME);
                        pathAttr = mapElem.Attribute(Constants.XML_ATTR_PATH);
                        templateAttr = mapElem.Attribute(Constants.XML_ATTR_TEMPLATE);
                        if (nameAttr != null) { name = Parameters.Resolve(nameAttr.Value); }
                        if (pathAttr != null) { path = Parameters.Resolve(pathAttr.Value); }
                        if (templateAttr != null) { template = Parameters.Resolve(templateAttr.Value); }

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(template) && path != null)
                        {
                            m_ProjectMappings.Add(new MappedTemplate() { Name = name, Path = path, Template = template, });
                        }
                    }
                }
            }
        }
        #endregion Constructors

        #region Properties

        #region ProjectMappings
        /// <summary>The list of project mappings.</summary>
        public List<MappedTemplate> ProjectMappings { get { return m_ProjectMappings; } set { m_ProjectMappings = value; } }
        #endregion ProjectMappings

        #endregion Properties
    }
}

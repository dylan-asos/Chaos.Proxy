//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asos.Chaos.WebApi.Configuration
{
    
    
    /// <summary>
    /// The ChaosConfigurationSection Configuration Section.
    /// </summary>
    public partial class ChaosConfigurationSection : global::System.Configuration.ConfigurationSection
    {
        
        #region Singleton Instance
        /// <summary>
        /// The XML name of the ChaosConfigurationSection Configuration Section.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string ChaosConfigurationSectionSectionName = "chaosConfigurationSection";
        
        /// <summary>
        /// The XML path of the ChaosConfigurationSection Configuration Section.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string ChaosConfigurationSectionSectionPath = "chaosConfigurationSection";
        
        /// <summary>
        /// Gets the ChaosConfigurationSection instance.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public static global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection Instance
        {
            get
            {
                return ((global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection)(global::System.Configuration.ConfigurationManager.GetSection(global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.ChaosConfigurationSectionSectionPath)));
            }
        }
        #endregion
        
        #region Xmlns Property
        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string XmlnsPropertyName = "xmlns";
        
        /// <summary>
        /// Gets the XML namespace of this Configuration Section.
        /// </summary>
        /// <remarks>
        /// This property makes sure that if the configuration file contains the XML namespace,
        /// the parser doesn't throw an exception because it encounters the unknown "xmlns" attribute.
        /// </remarks>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.XmlnsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public string Xmlns
        {
            get
            {
                return ((string)(base[global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.XmlnsPropertyName]));
            }
        }
        #endregion
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region Errors Property
        /// <summary>
        /// The XML name of the <see cref="Errors"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string ErrorsPropertyName = "errors";
        
        /// <summary>
        /// Gets or sets the Errors.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        [global::System.ComponentModel.DescriptionAttribute("The Errors.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.ErrorsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual global::Asos.Chaos.WebApi.Configuration.PossibleErrors Errors
        {
            get
            {
                return ((global::Asos.Chaos.WebApi.Configuration.PossibleErrors)(base[global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.ErrorsPropertyName]));
            }
            set
            {
                base[global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.ErrorsPropertyName] = value;
            }
        }
        #endregion
        
        #region Settings Property
        /// <summary>
        /// The XML name of the <see cref="Settings"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string SettingsPropertyName = "settings";
        
        /// <summary>
        /// Gets or sets the Settings.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        [global::System.ComponentModel.DescriptionAttribute("The Settings.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.SettingsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual global::Asos.Chaos.WebApi.Configuration.ChaosSettings Settings
        {
            get
            {
                return ((global::Asos.Chaos.WebApi.Configuration.ChaosSettings)(base[global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.SettingsPropertyName]));
            }
            set
            {
                base[global::Asos.Chaos.WebApi.Configuration.ChaosConfigurationSection.SettingsPropertyName] = value;
            }
        }
        #endregion
    }
}
namespace Asos.Chaos.WebApi.Configuration
{
    
    
    /// <summary>
    /// The HttpError Configuration Element.
    /// </summary>
    public partial class HttpError : global::System.Configuration.ConfigurationElement
    {
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region HttpErrorCode Property
        /// <summary>
        /// The XML name of the <see cref="HttpErrorCode"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string HttpErrorCodePropertyName = "httpErrorCode";
        
        /// <summary>
        /// Gets or sets the HttpErrorCode.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        [global::System.ComponentModel.DescriptionAttribute("The HttpErrorCode.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Asos.Chaos.WebApi.Configuration.HttpError.HttpErrorCodePropertyName, IsRequired=true, IsKey=true, IsDefaultCollection=false)]
        public virtual int HttpErrorCode
        {
            get
            {
                return ((int)(base[global::Asos.Chaos.WebApi.Configuration.HttpError.HttpErrorCodePropertyName]));
            }
            set
            {
                base[global::Asos.Chaos.WebApi.Configuration.HttpError.HttpErrorCodePropertyName] = value;
            }
        }
        #endregion
    }
}
namespace Asos.Chaos.WebApi.Configuration
{
    
    
    /// <summary>
    /// A collection of HttpError instances.
    /// </summary>
    [global::System.Configuration.ConfigurationCollectionAttribute(typeof(global::Asos.Chaos.WebApi.Configuration.HttpError), CollectionType=global::System.Configuration.ConfigurationElementCollectionType.BasicMapAlternate, AddItemName=global::Asos.Chaos.WebApi.Configuration.PossibleErrors.HttpErrorPropertyName)]
    public partial class PossibleErrors : global::System.Configuration.ConfigurationElementCollection
    {
        
        #region Constants
        /// <summary>
        /// The XML name of the individual <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> instances in this collection.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string HttpErrorPropertyName = "httpError";
        #endregion
        
        #region Overrides
        /// <summary>
        /// Gets the type of the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>The <see cref="global::System.Configuration.ConfigurationElementCollectionType"/> of this collection.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public override global::System.Configuration.ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return global::System.Configuration.ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        
        /// <summary>
        /// Gets the name used to identify this collection of elements
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        protected override string ElementName
        {
            get
            {
                return global::Asos.Chaos.WebApi.Configuration.PossibleErrors.HttpErrorPropertyName;
            }
        }
        
        /// <summary>
        /// Indicates whether the specified <see cref="global::System.Configuration.ConfigurationElement"/> exists in the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="elementName">The name of the element to verify.</param>
        /// <returns>
        /// <see langword="true"/> if the element exists in the collection; otherwise, <see langword="false"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        protected override bool IsElementName(string elementName)
        {
            return (elementName == global::Asos.Chaos.WebApi.Configuration.PossibleErrors.HttpErrorPropertyName);
        }
        
        /// <summary>
        /// Gets the element key for the specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="global::System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="object"/> that acts as the key for the specified <see cref="global::System.Configuration.ConfigurationElement"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        protected override object GetElementKey(global::System.Configuration.ConfigurationElement element)
        {
            return ((global::Asos.Chaos.WebApi.Configuration.HttpError)(element)).HttpErrorCode;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        protected override global::System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new global::Asos.Chaos.WebApi.Configuration.HttpError();
        }
        #endregion
        
        #region Indexer
        /// <summary>
        /// Gets the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public global::Asos.Chaos.WebApi.Configuration.HttpError this[int index]
        {
            get
            {
                return ((global::Asos.Chaos.WebApi.Configuration.HttpError)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> with the specified key.
        /// </summary>
        /// <param name="httpErrorCode">The key of the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public global::Asos.Chaos.WebApi.Configuration.HttpError this[object httpErrorCode]
        {
            get
            {
                return ((global::Asos.Chaos.WebApi.Configuration.HttpError)(base.BaseGet(httpErrorCode)));
            }
        }
        #endregion
        
        #region Add
        /// <summary>
        /// Adds the specified <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="httpError">The <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> to add.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public void Add(global::Asos.Chaos.WebApi.Configuration.HttpError httpError)
        {
            base.BaseAdd(httpError);
        }
        #endregion
        
        #region Remove
        /// <summary>
        /// Removes the specified <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="httpError">The <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> to remove.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public void Remove(global::Asos.Chaos.WebApi.Configuration.HttpError httpError)
        {
            base.BaseRemove(this.GetElementKey(httpError));
        }
        #endregion
        
        #region GetItem
        /// <summary>
        /// Gets the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public global::Asos.Chaos.WebApi.Configuration.HttpError GetItemAt(int index)
        {
            return ((global::Asos.Chaos.WebApi.Configuration.HttpError)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> with the specified key.
        /// </summary>
        /// <param name="httpErrorCode">The key of the <see cref="global::Asos.Chaos.WebApi.Configuration.HttpError"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public global::Asos.Chaos.WebApi.Configuration.HttpError GetItemByKey(int httpErrorCode)
        {
            return ((global::Asos.Chaos.WebApi.Configuration.HttpError)(base.BaseGet(((object)(httpErrorCode)))));
        }
        #endregion
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
    }
}
namespace Asos.Chaos.WebApi.Configuration
{
    
    
    /// <summary>
    /// The ChaosSettings Configuration Element.
    /// </summary>
    public partial class ChaosSettings : global::System.Configuration.ConfigurationElement
    {
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region ChaosLevel Property
        /// <summary>
        /// The XML name of the <see cref="ChaosLevel"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string ChaosLevelPropertyName = "chaosLevel";
        
        /// <summary>
        /// Gets or sets the ChaosLevel.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        [global::System.ComponentModel.DescriptionAttribute("The ChaosLevel.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Asos.Chaos.WebApi.Configuration.ChaosSettings.ChaosLevelPropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual int ChaosLevel
        {
            get
            {
                return ((int)(base[global::Asos.Chaos.WebApi.Configuration.ChaosSettings.ChaosLevelPropertyName]));
            }
            set
            {
                base[global::Asos.Chaos.WebApi.Configuration.ChaosSettings.ChaosLevelPropertyName] = value;
            }
        }
        #endregion
        
        #region Enabled Property
        /// <summary>
        /// The XML name of the <see cref="Enabled"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        internal const string EnabledPropertyName = "enabled";
        
        /// <summary>
        /// Gets or sets the Enabled.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.1.7")]
        [global::System.ComponentModel.DescriptionAttribute("The Enabled.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::Asos.Chaos.WebApi.Configuration.ChaosSettings.EnabledPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual bool Enabled
        {
            get
            {
                return ((bool)(base[global::Asos.Chaos.WebApi.Configuration.ChaosSettings.EnabledPropertyName]));
            }
            set
            {
                base[global::Asos.Chaos.WebApi.Configuration.ChaosSettings.EnabledPropertyName] = value;
            }
        }
        #endregion
    }
}

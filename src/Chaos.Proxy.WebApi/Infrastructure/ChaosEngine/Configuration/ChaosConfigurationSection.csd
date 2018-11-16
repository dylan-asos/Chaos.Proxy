<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="28f7f076-4a5b-48a9-b45e-1f5549409eaa" namespace="Asos.Chaos.WebApi.Configuration" xmlSchemaNamespace="urn:Asos.Chaos.WebApi.Configuration" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="ChaosConfigurationSection" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="chaosConfigurationSection">
      <elementProperties>
        <elementProperty name="Errors" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="errors" isReadOnly="false">
          <type>
            <configurationElementCollectionMoniker name="/28f7f076-4a5b-48a9-b45e-1f5549409eaa/PossibleErrors" />
          </type>
        </elementProperty>
        <elementProperty name="Settings" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="settings" isReadOnly="false">
          <type>
            <configurationElementMoniker name="/28f7f076-4a5b-48a9-b45e-1f5549409eaa/ChaosSettings" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="HttpError">
      <attributeProperties>
        <attributeProperty name="HttpErrorCode" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="httpErrorCode" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/28f7f076-4a5b-48a9-b45e-1f5549409eaa/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="PossibleErrors" xmlItemName="httpError" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/28f7f076-4a5b-48a9-b45e-1f5549409eaa/HttpError" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="ChaosSettings">
      <attributeProperties>
        <attributeProperty name="ChaosLevel" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="chaosLevel" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/28f7f076-4a5b-48a9-b45e-1f5549409eaa/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="Enabled" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="enabled" isReadOnly="false">
          <type>
            <externalTypeMoniker name="/28f7f076-4a5b-48a9-b45e-1f5549409eaa/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>
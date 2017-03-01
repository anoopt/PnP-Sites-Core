﻿using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml.Serializers
{
    /// <summary>
    /// Class to serialize/deserialize the parameters of the template
    /// </summary>
    [TemplateSchemaSerializer(
        SchemaTemplates = new Type[] { typeof(Xml.V201605.ProvisioningTemplate) },
        Default = false)]
    internal class TemplateParametersSerializer : PnPBaseSchemaSerializer
    {
        public override void Deserialize(object persistence, ProvisioningTemplate template)
        {
            var preferences = persistence.GetPublicInstancePropertyValue("Preferences");
            
            if (preferences != null)
            {
                var parameters = preferences.GetPublicInstancePropertyValue("Parameters");

                if (parameters != null)
                {
                    template.GetPublicInstanceProperty("Parameters")
                        .SetValue(template, PnPObjectsMapper.MapObjects(parameters,
                                new TemplateParameterFromSchemaToModelTypeResolver())
                                as Dictionary<String, String>);
                }
            }
        }

        public override void Serialize(ProvisioningTemplate template, object persistence)
        {
            var preferences = persistence.GetPublicInstancePropertyValue("Preferences");

            if (preferences != null)
            {
                var parametersTypeName = $"{PnPSerializationScope.Current?.BaseSchemaNamespace}.PreferencesParameter, {PnPSerializationScope.Current?.BaseSchemaAssemblyName}";
                var parametersType = Type.GetType(parametersTypeName, true);

                preferences.GetPublicInstanceProperty("Parameters")
                    .SetValue(
                        preferences,
                        PnPObjectsMapper.MapObjects(template.Parameters,
                            new TemplateParameterFromModelToSchemaTypeResolver(parametersType)));
            }
        }
    }
}

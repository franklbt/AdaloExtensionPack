using CaseExtensions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AdaloExtensionPack.Core.Adalo
{
    public class AdaloControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsGenericType) return;

            switch (controller.ControllerType.GenericTypeArguments.Length)
            {
                case 1:
                {
                    var customNameAttribute = controller.ControllerType.GenericTypeArguments[0]; 

                    var tableName = customNameAttribute.Name.Pluralize().ToKebabCase();
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel =
                            new AttributeRouteModel(new RouteAttribute($"tables/{tableName}")),
                    });
                    controller.ControllerName = customNameAttribute.Name;
                    break;
                }
                case 3:
                {
                    var customNameAttribute = controller.ControllerType.GenericTypeArguments[2]; 

                    var tableName = customNameAttribute.Name.Pluralize().ToKebabCase();
                    controller.Selectors.Add(new SelectorModel
                    {
                        AttributeRouteModel =
                            new AttributeRouteModel(new RouteAttribute($"views/{tableName}")),
                    });
                    controller.ControllerName = customNameAttribute.Name;
                    break;
                }
            }
        }
    }
}

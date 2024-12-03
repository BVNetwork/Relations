using System;
using EPiServer.Shell;
using Microsoft.AspNetCore.Mvc.Routing;

namespace EPiCode.Relations.Admin.Controllers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ModuleRoute : Attribute, IRouteTemplateProvider
{
    private readonly string _controllerName;
    private readonly string _actionName;

    public string Template => Paths.ToResource(typeof(ModuleRoute), $"{_controllerName}/{_actionName}");
    public int? Order { get; set; } = 0;
    public string Name { get; set; }

    public ModuleRoute(string controllerName, string actionName)
    {
        _controllerName = controllerName;
        _actionName = actionName;
    }
}
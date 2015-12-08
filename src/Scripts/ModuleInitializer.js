define([
    // Dojo    
    "dojo",
    "dojo/_base/declare",

    //CMS    
    "epi/_Module",
    "epi/dependency",
    "epi/routes"
], function (
    // Dojo    
    dojo,
    declare,

    //CMS    
    _Module,
    dependency,
    routes

) {

    return declare("relations.ModuleInitializer", [_Module], {

        initialize: function () {
            this.inherited(arguments);
            var registry = this.resolveDependency("epi.storeregistry");
            registry.create("relations.relations", this._getRestPath("relations"));
            registry.create("relations.rulequery", this._getRestPath("rules"));
            registry.create("relations.relationremove", this._getRestPath("relationremove"));
            registry.create("relations.relationadd", this._getRestPath("relationadd"));
            registry.create("relations.overviewquery", this._getRestPath("overview"));

        },
        _getRestPath: function (name) {
            return routes.getRestPath({ moduleArea: "epicode.relations", storeName: name });
        }
    });
});
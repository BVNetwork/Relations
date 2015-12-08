define([
// Dojo
    "dojo/_base/declare",
    "dojo/html",
    "dojo/dom-geometry",
// Dijit

    "dijit/_TemplatedMixin",
    "dijit/_WidgetBase",
    "dijit/_Container",
    "dijit/layout/_LayoutWidget",
    "dijit/_WidgetsInTemplateMixin",

//CMS
    "epi/cms/_ContentContextMixin",
    "epi/shell/dnd/Source",
    "epi/shell/dnd/Target",
    "epi/cms/component/ContentQueryGrid",
    "epi/dependency"

], function (
// Dojo
    declare,
    html,
    domGeometry,
// Dijit
    _TemplatedMixin,
    _WidgetBase,
    _Container,
    _LayoutWidget,
    _WidgetsInTemplateMixin,
//CMS
    _ContentContextMixin,
    Source, 
    Target,
    ContentQueryGrid,
    dependency
) {
    //We declare the namespace of our widget and add the mixins we want to use.
    //Note: Declaring the name of the widget is removed in Dojo 1.8 
    //but the release version of EPiServer 7 uses Dojo 1.7 and still needs this.
    return declare("relations.components.RelationOverview",
        [_Container, _LayoutWidget, _TemplatedMixin, _WidgetsInTemplateMixin, _WidgetBase, _ContentContextMixin], {
            // summary: A simple widget that listens to changes to the 
            // current content item and puts the name in a div.

            currentContext: "",

            templateString: '<div>\
                            <div class="epi-gadgetInnerToolbar" data-dojo-attach-point="toolbar"><div data-dojo-attach-point="overviewText">Text</div></div>\
                        </div>',


            postCreate: function () {
                this.inherited(arguments);
                this._setupTargets();
            },


            _setupTargets: function () {
                console.log("Setting up overview");
                var contextService = epi.dependency.resolve("epi.shell.ContextService");
                currentContext = contextService.currentContext;
                this.contextChanged(currentContext, null);

            },
            
            store: null,

            contextChanged: function (context, callerData) {
                this.inherited(arguments);

                this.currentContext = context.id;

                if (!this.store) {
                    var registry = dependency.resolve("epi.storeregistry");
                    this.store = registry.get("relations.overviewquery");
                }

                var mycache = {};

                mycache["this"] = this;

                dojo.when(mycache["stuff"] || this.store.get(context.id.split("_", 1000000)[0]), function (returnValue) {
                    mycache["stuff"] = returnValue;
                    html.set(mycache["this"].overviewText, returnValue);
                });

                // the context changed, probably because we navigated or published something
                //html.set(this.contentName, "Relations for "+context.name);
            }
        });
});
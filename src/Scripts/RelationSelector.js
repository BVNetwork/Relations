define([
    "dojo/_base/connect",
    "dojo/_base/declare",
    "dijit/_CssStateMixin",
    "dijit/_Widget",
    "dijit/_TemplatedMixin",
    "dijit/_WidgetsInTemplateMixin",
    "dijit/form/FilteringSelect",
    "epi/dependency",
    "epi/epi",
    "epi/shell/widget/_ValueRequiredMixin",
    "epi-cms/dgrid/DnD",
    "epi-cms/component/ContentQueryGrid",


    //We are calling the require module class to ensure that the App module has been set up  
    "alloy/requiremodule!App"
],

function (
    connect,
    declare,
    _CssStateMixin,
    _Widget,
    _TemplatedMixin,
    _WidgetsInTemplateMixin,
    FilteringSelect,
    dependency,
    epi,
    _ValueRequiredMixin,
    DnD,
    ContentQueryGrid,
    appModule
    ) {

    return declare("relations.RelationSelector", [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin, _CssStateMixin, _ValueRequiredMixin],
        {
            templateString: "<div class=\"dijitInline\">\
                <div data-dojo-attach-point=\"stateNode, tooltipNode\">\
                <div data-dojo-attach-point=\"contentQuery\" data-dojo-type=\"epi-cms/component/ContentQueryGrid\" ></div>\
                </div>\
            </div>",

            intermediateChanges: false,
            value: null,
            store: null,
            onChange: function (value) {
                
                // Event that tells EPiServer when the widget's value has changed. 
            },
            postCreate: function () {

                // call base implementation    
                this.inherited(arguments);
                // Init textarea and bind event     
                var registry = dependency.resolve("epi.storeregistry");
                this.store = this.store || registry.get("alloy.customquery");
            },
            isValid: function () {
                // summary:       
                //    Check if widget's value is valid.   
                // tags:      
                //    protected, override   
                return this.inputWidget.isValid();
            },
            // Setter for value property    
            _setValueAttr: function (value) {
                this.inputWidget.set("value", value);
                this._set("value", value);
            },
            _setReadOnlyAttr: function (value) {
                this._set("readOnly", value);
                this.inputWidget.set("readOnly", value);
            },

            // Event handler for the changed event of the input widget   
            _onInputWidgetChanged: function (value) {
                this._updateValue(value);
            },

            _updateValue: function (value) {
                if (this._started && epi.areEqual(this.value, value)) {
                    return;
                }

                this._set("id", value);
                this.onChange(value);
            }
        });
});
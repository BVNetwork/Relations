define([
// Dojo
    "dojo/_base/declare",
    "dojo/html",
    "dojo/dom-geometry",
    "dojo/_base/fx",
    "dojo/topic",
    "dojo/on",
// Dijit

    "dijit/_TemplatedMixin",
    "dijit/_WidgetBase",
    "dijit/_Container",
    "dijit/layout/_LayoutWidget",
    "dijit/_WidgetsInTemplateMixin",

//CMS
    "epi-cms/_ContentContextMixin",
    "epi/shell/dnd/Source",
    "epi/shell/dnd/Target",
    "epi-cms/component/ContentQueryGrid",
    "epi/dependency"

], function (
// Dojo
declare,
html,
domGeometry,
fx,
topic,
on,
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
    return declare("relations.components.RelationEditor",
        [_Container, _LayoutWidget, _TemplatedMixin, _WidgetsInTemplateMixin, _WidgetBase, _ContentContextMixin], {

            currentContext: "",

            currentRule: "",

            currentDirection: "",

            ignoreContextChange: false,

            templateString: '<div >\
                            <div class="epi-gadgetInnerToolbar" data-dojo-attach-point="toolbar"><div data-dojo-attach-point="contentName"></div></div>\
                            <div style="background-color: #ddd;" >\
                            <div data-dojo-attach-point="relationsArea"  >\
                                <div class="relationsArea" id="relationsArea" style="background-color: #fff;" >\
                                    <div style="padding:5px;font-weight: bold;" data-dojo-attach-point="ruleDescription"></div>\
                                    <div style="padding:5px;margin-top: 5px;"><strong>Existing relations</strong></div>\
                                    <div data-dojo-attach-point="addRelationsArea" class="addRelationsArea">\
                                     <div data-dojo-attach-point="relationsQuery" data-dojo-type="epi-cms/component/ContentQueryGrid"></div>\
                                    </div>\
                                        <div style="padding:7px;color:#fff;font-weight:bold;opacity:0; background-color: #428bca;" data-dojo-attach-point="statusText" ></div>\
                                    <div style="clear:both;"></div><div style="padding:5px;margin-top:5px;"><strong>New relations</strong></div>\
                                    <div class="epi-gadgetInnerToolbar" data-dojo-attach-point="toolbar2">Search relations: <div data-dojo-type="dijit.form.TextBox" data-dojo-attach-point="queryText" data-dojo-props="intermediateChanges:true" data-dojo-attach-event="onChange: _reloadNotRelatedQuery"></div></div>\
                                    <div data-dojo-attach-point="removeRelationsArea">\
                                    <div data-dojo-attach-point="notRelatedQuery" data-dojo-type="epi-cms/component/ContentQueryGrid"></div></div>\
                                </div>\
                            </div>\
                        </div>\
                        </div>',

            postCreate: function () {
                this.inherited(arguments);
                this._setupTargets();
            },

            setSize:function(){
                var contentSize = domGeometry.getContentBox("relationsArea");
                this.resizeDynamic({w: contentSize.w, h: contentSize.h + 59 })
            },

            resize:function(newSize){
                if(this.useDynamicHeight){
                    this.resizeDynamic(newSize);
                }else{
                    this.resizeStatic(newSize);
                }
            },

            resizeStatic: function (newSize) {
                this.inherited(arguments);
                var otherContentSize = domGeometry.getMarginBox(this.contentName);
                var gridSize = { w: newSize.w - 5, h: (newSize.h - otherContentSize.h - 100) / 2 };
                this.relationsQuery.resize(gridSize);
                this.notRelatedQuery.resize(gridSize);
            },

            resizeDynamic: function (newSize) {
                this.inherited(arguments);
                var otherContentSize = domGeometry.getMarginBox(this.contentName);
                var items = $('.addRelationsArea table').length - 1;
                items = items < 1 ? 1 : items;
                var gridHeight = items * 33;
                var newHeight = (newSize.h - otherContentSize.h - 100) / 2;
                var gridSize = { w: newSize.w - 5, h: newHeight  };
                var rGridSize = { w: gridSize.w, h: newHeight  };
                var notGridSize =  { w: gridSize.w, h: newHeight  };

                if(gridHeight < newHeight)
                {
                    rGridSize.h = gridHeight;
                    notGridSize.h = newHeight + (newHeight - gridHeight);
                }

                this.relationsQuery.resize(rGridSize);
                this.notRelatedQuery.resize(notGridSize);
            },

            _setupTargets: function () {
                this.ignoreContextChange = false;

                var targetAdd = new Target(this.addRelationsArea, {
                    accept: ["link"],
                    createItemOnDrop: false
                });

                var targetRemove = new Target(this.removeRelationsArea, {
                    accept: ["link"],
                    createItemOnDrop: false
                });

                this.connect(targetRemove, "onDropData", "_onDropDataRemove");
                this.connect(targetAdd, "onDropData", "_onDropDataAdd");

                var contextService = epi.dependency.resolve("epi.shell.ContextService");
                this.currentContext = contextService.currentContext;

                if(this.useDynamicHeight)
                {
                    this.own(
                        on(this.relationsQuery.grid.domNode, "dgrid-refresh-complete", function(){
                            setTimeout(() => this.setSize(), 250);
                        }.bind(this))
                    );
                }

                this.contextChanged(this.currentContext, null);
            },

            rulestore: null,
            relationcreatestore: null,
            relationremovestore: null,

            createButtonRow: function () {
                if (!this.rulestore) {
                    var registry = dependency.resolve("epi.storeregistry");
                    this.rulestore = registry.get("relations.rulequery");
                }

                var contextService = epi.dependency.resolve("epi.shell.ContextService");
                this.currentContext = contextService.currentContext;

                var mycache = {};
                mycache["this"] = this;
                html.set(this.statusText, "Drag and drop content to create relations!");
                dojo.query('.rulebutton', this.contentName).style({ display: "none", fontWeight: "normal" });
                dojo.when(mycache["stuff"] || this.rulestore.get(this.currentContext.id.split("_", 1000000)[0]), function (returnValue) {
                    mycache["stuff"] = returnValue;
                    var found = false;
                    returnValue.forEach(function (rule) {
                        mycache["this"].createButton(rule.id, rule.name, rule.description, rule.direction, rule.sortOrder, mycache["this"], rule.guid);
                        if (mycache["this"].currentRule == rule.id && mycache["this"].currentDirection == rule.direction) {
                            found = true;
                        }

                    });
                    if (found) {
                        fx.fadeIn({ node: mycache["this"].relationsArea, duration: 500 }).play();
                        mycache["this"]._reloadAllRelatedQueries();
                    }
                });

            },

            createButton: function (buttonid, buttonName, ruleDisplayName, ruleDirection, sortOrder, from, guid) {
                var btnid = ("btn" + guid + ruleDirection);
                if (!dojo.query('.' + btnid, this.contentName).length > 0) {
                    var button = new dijit.form.ToggleButton({ id: btnid, label: buttonName, rule: buttonid, ruledirection: ruleDirection, sortorder: sortOrder, ruledescription: ruleDisplayName });
                    button.set("class", "rulebutton " + btnid);
                    button.set("style", "display:inline;");
                    dojo.connect(button, "onClick", dojo.partial(this.enableButtons, button, from));
                    button.placeAt(this.contentName);
                }
                else {
                    dojo.query('.' + btnid, this.contentName).style({ display: "inline" });
                }
            },

            enableButtons: function (evt, from, sender) {
                fx.fadeIn({ node: from.relationsArea, duration: 500 }).play();
                dojo.query('.dijitButtonContents', this.contentName).style({ opacity: 0.7, color: "#000" });
                dojo.query('#' + evt.id, this.contentName).style({ opacity: 1, color: "#428bca" });
                from.switchRule(evt.rule, evt.ruledescription, evt.ruledirection, evt.sortorder);
            },

            switchRule: function (rulename, ruledescr, ruledirection, sortorder) {


                if (sortorder) {
                    if (ruledescr) {
                        ruledescr = ruledescr + "<br /><span style=\"font-weight: normal;\">Sort order: " + sortorder + "</span>";
                    } else {
                        ruledescr = "<span style=\"font-weight: normal;\">Sort order: " + sortorder + "</span>";
                    }
                }

                html.set(this.ruleDescription , ruledescr);

                this.currentRule = rulename;
                this.currentDirection = ruledirection;
                var contextService = epi.dependency.resolve("epi.shell.ContextService");
                this.currentContext = contextService.currentContext;
                this._reloadAllRelatedQueries();

            },

            _onDropDataAdd: function (dndData, source, nodes, copy) {
                var dropItem = dndData ? (dndData.length ? dndData[0] : dndData) : null;
                if (dropItem) {
                    dojo.when(dropItem.data, dojo.hitch(this, function (value) {

                        var mycache = {};

                        mycache["this"] = this;
                        var registry = dependency.resolve("epi.storeregistry");

                        this.relationcreatestore = registry.get("relations.relationadd");
                        dojo.when(this.relationcreatestore.put({ ruleName: this.currentRule, relationPageLeftString: this.currentContext.id, relationPageRightString: value.permanentUrl, ruleDirection: this.currentDirection }), function (returnValue) {
                            mycache["stuff"] = returnValue;
                            html.set(mycache["this"].statusText, returnValue);
                            dojo.setStyle(mycache["this"].statusText, "opacity", 1); // == 0.5
                            fx.fadeOut({ node: mycache["this"].statusText, duration: 7000 }).play();
                            mycache["this"]._refreshContext();
                        });
                    }));
                }
            },

            _onDropDataRemove: function (dndData, source, nodes, copy) {
                var dropItem = dndData ? (dndData.length ? dndData[0] : dndData) : null;

                if (dropItem) {
                    dojo.when(dropItem.data, dojo.hitch(this, function (value) {

                        var mycache = {};

                        mycache["this"] = this;
                        var registry = dependency.resolve("epi.storeregistry");

                        this.relationremovestore = registry.get("relations.relationremove");
                        dojo.when(this.relationremovestore.put({ ruleName: this.currentRule, relationPageLeftString: this.currentContext.id, relationPageRightString: value.permanentUrl, ruleDirection: this.currentDirection }), function (returnValue) {
                            mycache["stuff"] = returnValue;
                            html.set(mycache["this"].statusText, returnValue);
                            dojo.setStyle(mycache["this"].statusText, "opacity", 1); // == 0.5
                            fx.fadeOut({ node: mycache["this"].statusText, duration: 7000 }).play();
                            mycache["this"]._refreshContext();
                        });
                    }));
                }
            },

            _refreshContext: function () {
                this.ignoreContextChange = true;
                var contextParameters = { uri : 'epi.cms.contentdata:///'+ this.currentContext.id, hasSiteChanged: false, previewUrl: this.currentContext.previewUrl+'&'+Date.now() };
                dojo.publish("/epi/shell/context/updateRequest", [this.currentContext]);
                this._reloadAllRelatedQueries();
            },


            _reloadAllRelatedQueries: function () {
                this._reloadRelatedQuery();
                this._reloadNotRelatedQuery();
            },

            _reloadRelatedQuery: function () {

                this.relationsQuery.set("queryParameters", { queryText: "", relationPageLeft: this.currentContext.id, relationPageRight: null, relationRule: this.currentRule, action: "none", direction: this.currentDirection });
                this.relationsQuery.set("queryName", "RelationsQuery");

                if (this.dblclick) {
                    this.relationsQuery.set("contextChangeEvent", "dblclick");
                }

                if (!this.rulestore) {
                    var registry = dependency.resolve("epi.storeregistry");
                    this.rulestore = registry.get("relations.statusquery");
                }
            },

            _reloadNotRelatedQuery: function () {
                this.notRelatedQuery.set("queryParameters", { queryText: this.queryText.value, relationPageLeft: this.currentContext.id, relationPageRight: null, relationRule: this.currentRule, direction: this.currentDirection });
                this.notRelatedQuery.set("queryName", "NotRelationsQuery");

                if (this.dblclick) {
                    this.notRelatedQuery.set("contextChangeEvent", "dblclick");
                }
            },

            contextChanged: function (context, callerData) {
                this.currentContext = context;
                fx.fadeOut({ node: this.relationsArea, duration: 5 }).play();
                this.createButtonRow();
                this._reloadAllRelatedQueries();
                this.inherited(arguments);
            }
        });
});
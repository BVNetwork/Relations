using System;
using EPiCode.Relations.Core;
using EPiCode.Relations.Helpers;using EPiServer.Shell.Services.Rest;
using Microsoft.AspNetCore.Mvc;

namespace EPiCode.Relations.Queries
{
    [RestStore("relationremove")]
    public class RemoveRelationRest : RestControllerBase
    {


        public RestResult Get(int? id, ItemRange range)
        {
            // When requesting a specific item we return that item; otherwise return all items.

            return Rest("Hello world!");
        }

        public RestResult Post([FromBody] RelationRestStoreModel model)
        {
            int relationPageLeft;
            int relationPageRight;
            bool isLeftRule;
            string relationRule;

            try
            {
                relationRule = model.RuleName;
                relationPageLeft = int.Parse(model.RelationPageLeftString.Split('_')[0]);
                relationPageRight = EPiServer.Web.PermanentLinkUtility.FindContentReference(
                    EPiServer.Web.PermanentLinkUtility.GetGuid(model.RelationPageRightString)).ID;
                isLeftRule = model.RuleDirection == "left";
            }
            catch (Exception e)
            {
                return Rest(string.Format(TranslationHelper.Translate("/relations/edit/couldnotparse"), e.Message));
            }


            try
            {
                if (isLeftRule)
                {
                    Relation rel = RelationEngine.Instance.GetRelation(relationRule, relationPageLeft, relationPageRight);
                    RelationEngine.Instance.DeleteRelation(rel);
                }
                else
                {
                    Relation rel = RelationEngine.Instance.GetRelation(relationRule, relationPageRight, relationPageLeft);
                    RelationEngine.Instance.DeleteRelation(rel);

                }
            }
            catch { }


            return Rest(TranslationHelper.Translate("/relations/edit/removed"));
        }
    }
}
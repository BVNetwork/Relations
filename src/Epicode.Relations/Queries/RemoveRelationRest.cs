using System;
using EPiCode.Relations.Core;
using EPiCode.Relations.Helpers;using EPiServer.Shell.Services.Rest;

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

        public RestResult Post(string ruleName, string relationPageLeftString, string relationPageRightString, string ruleDirection)
        {
            int relationPageLeft;
            int relationPageRight;
            bool isLeftRule;
            string relationRule;

            try
            {
                relationRule = ruleName;
                relationPageLeft = int.Parse(relationPageLeftString.Split('_')[0]);
                relationPageRight = EPiServer.Web.PermanentLinkUtility.FindContentReference(
                    EPiServer.Web.PermanentLinkUtility.GetGuid(relationPageRightString)).ID;
                isLeftRule = ruleDirection == "left";
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
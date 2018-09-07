using System;
using EPiCode.Relations.Core;
using EPiCode.Relations.Helpers;
using EPiServer.Core;
using EPiServer.Shell.Services.Rest;

namespace EPiCode.Relations.Queries
{
    [RestStore("relationadd")]
    public class AddRelationRest : RestControllerBase
    {


        public RestResult Get(int? id, ItemRange range)
        {
            // When requesting a specific item we return that item; otherwise return all items.

            return Rest("Hello world!");
        }

        public RestResult Post(string ruleName, string relationPageLeftString, string relationPageRightString, string ruleDirection)
        {
            int relationPageLeft = 0;
            int relationPageRight = 0;
            bool isLeftRule = true;
            string relationRule = "";

            //string[] newRelationValues = (newRelation.ToString()).Split(';');
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
                return Rest("Could not parse relation: " + e.Message);
            }

            PageReference contextPage = new PageReference(relationPageLeft);
            if (contextPage != null)

                try
                {
                    if (isLeftRule)
                    {
                        Validator.ValidationResult validation = Validator.Validate(relationRule, contextPage.ID,
                            relationPageRight);
                        if (validation == Validator.ValidationResult.Ok)
                            RelationEngine.Instance.AddRelation(relationRule, contextPage.ID,
                                relationPageRight);
                        else
                        {
                            string errorMessage = TranslationHelper.GetValidationText(validation);

                            return Rest(TranslationHelper.Translate("/relations/edit/error") + errorMessage);
                        }
                    }
                    else
                    {
                        Validator.ValidationResult validation = Validator.Validate(relationRule, relationPageRight,
                            contextPage.ID);
                        if (validation == Validator.ValidationResult.Ok)
                            RelationEngine.Instance.AddRelation(relationRule, relationPageRight,
                                contextPage.ID);
                        else
                        {
                            string errorMessage = TranslationHelper.GetValidationText(validation);

                            return Rest(TranslationHelper.Translate("/relations/edit/error") + errorMessage);
                        }
                    }
                }
                catch (Exception e)
                {
                    return Rest(TranslationHelper.Translate("/relations/edit/error") + e.Message);
                        
                }


            return Rest(TranslationHelper.Translate("/relations/edit/added"));
        }


    }
}
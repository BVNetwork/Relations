using System;
using EPiCode.Relations.Core;
using EPiCode.Relations.Helpers;
using EPiServer.Core;
using EPiServer.Shell.Services.Rest;
using Microsoft.AspNetCore.Mvc;

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

        public RestResult Post([FromBody] RelationRestStoreModel model)
        {
            int relationPageLeft = 0;
            int relationPageRight = 0;
            bool isLeftRule = true;
            string relationRule = "";

            //string[] newRelationValues = (newRelation.ToString()).Split(';');
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
    
    public class RelationRestStoreModel
    {
        public string RuleName { get; set; }
        public string RelationPageLeftString { get; set; }
        public string RelationPageRightString { get; set; }
        public string RuleDirection { get; set; }
    }
}
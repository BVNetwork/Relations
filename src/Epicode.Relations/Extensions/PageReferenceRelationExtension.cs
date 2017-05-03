using System;
using EPiServer.Core;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Extensions
{
    public static partial class PageReferenceRelationExtension
    {
        public static PageDataCollection GetRelatedPages(this PageReference pageReference, string relationRuleName) {
            return Helpers.PageHelper.GetPagesRelated(pageReference, relationRuleName);
        }

        public static PageDataCollection GetRelatedPages(this PageReference pageReference, string firstRuleName, string secondRuleName)
        {
            return Helpers.PageHelper.GetPagesRelated(pageReference, firstRuleName, secondRuleName);
        }

        public static PageDataCollection GetRelatedPages(this PageReference pageReference, string firstRuleName, string firstRuleDirection, string secondRuleName, string secondRuleDirection)
        {
            try
            {
                Rule.Direction firstDir = (Rule.Direction)Enum.Parse(typeof(Rule.Direction), firstRuleDirection);
                Rule.Direction secondDir = (Rule.Direction)Enum.Parse(typeof(Rule.Direction), secondRuleDirection);

                return Helpers.PageHelper.GetPagesRelated(pageReference, firstRuleName, firstDir, secondRuleName, secondDir);
            }
            catch (Exception e) {
                throw new Exception("Could not parse rule direction. Should be Left, Right or Both - "+e.Message);
            }
        }

    }
}
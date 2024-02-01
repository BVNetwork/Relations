using System.Collections.Generic;
using System.Linq;
using EPiCode.Relations.Core;
using EPiCode.Relations.Diagnostics;

namespace EPiCode.Relations.Db
{
    public class RelationProviderOptimized : RelationDbProvider
    {
        public override List<Relation> GetRelationsForPage(int pageId, Rule rule)
        {
            var timer = new Timer(string.Format("GetRelationsForPage (*) pageId={0}, rule name={1}", pageId, rule.RuleName));

            var relations =  GetRelationsForPage(pageId).Where(rel => rel.RuleName == rule.RuleName && (rel.PageIDLeft == pageId || rel.PageIDRight == pageId))                                       
                                                        .ToList();
            timer.Stop();

            return relations;
        }

        public override List<Relation> GetRelationsForPage(int pageId, Rule rule, Rule.Direction direction)
        {
            var relations = new List<Relation>();

            var timer = new Timer(string.Format("GetRelationsForPage (*) pageId={0}, rule name={1}, direction={2}", pageId, rule.RuleName, direction));
            switch (direction)
            {
                case Rule.Direction.Both:
                    relations = GetRelationsForPage(pageId, rule);
                    break;
                case Rule.Direction.Left:
                    relations = GetRelationsForPage(pageId).Where(rel => rel.RuleName == rule.RuleName && rel.PageIDLeft == pageId)
                                                           .ToList();
                    break;
                case Rule.Direction.Right:
                    relations = GetRelationsForPage(pageId).Where(rel => rel.RuleName == rule.RuleName && rel.PageIDRight == pageId)
                                                           .ToList();
                    break;
            }
            timer.Stop();

            return relations;
        }
    }
}

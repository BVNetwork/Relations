using System;
using System.ComponentModel.DataAnnotations;

namespace EPiCode.Relations.Db.Data
{
    public class Relation
    {
        [Key]
        public Guid RelationId { get; set; }
        public string RuleName { get; set; }
        public int PageIdLeft { get; set; }
        public int PageIdRight { get; set; }
        public string LanguageBranch { get; set; }
    }
}

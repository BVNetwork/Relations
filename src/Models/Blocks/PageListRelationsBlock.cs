using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace EPiCode.Relations.Models.Blocks
{
    [ContentType(DisplayName = "PageListRelationsBlock", GUID = "269d0032-d3c7-429c-9d4f-88c0f3098c9c", Description = "")]
    public class PageListRelationsBlock : BlockData
    {
        public virtual string Heading { get; set; }

        [UIHint("Rules")]
        public virtual string Rule { get; set; }

        public virtual bool UsePartialTemplates { get; set; }
        /*
                [CultureSpecific]
                [Display(
                    Name = "Name",
                    Description = "Name field's description",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
                public virtual String Name { get; set; }
         */
    }
}
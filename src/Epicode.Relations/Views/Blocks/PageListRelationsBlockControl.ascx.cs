using System;
using EPiCode.Relations.Core;
using EPiCode.Relations.Models.Blocks;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace EPiCode.Relations.Views.Blocks
{
    public partial class PageListRelationsBlockControl : BlockControlBase<PageListRelationsBlock>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageDataCollection relatedPages =
                   EPiCode.Relations.Helpers.PageHelper.GetPagesRelated(CurrentPage.PageLink,
                       CurrentBlock.Rule, Rule.Direction.Both);
            if (relatedPages.Count == 0)
                this.Visible = false;
            if (CurrentBlock.UsePartialTemplates)
            {

                var contentArea = new ContentArea();
                
                foreach (var content in relatedPages)
                {
                    contentArea.Items.Add(new ContentAreaItem { ContentLink = (content as IContent).ContentLink });
                }

                var previewProperty = new PropertyContentArea
                {
                    Value = contentArea,
                    Name = "PreviewPropertyData",
                    IsLanguageSpecific = true
                };

                RelatedContentPlaceHolder.InnerProperty = previewProperty;
                RelatedContentPlaceHolder.DataBind();
                RelationsRepeater.Visible = false;
            }
            else
            {
                RelatedContentPlaceHolder.Visible = false;
                RelationsRepeater.DataSource = relatedPages;
                RelationsRepeater.DataBind();

            }
        }
    }
}
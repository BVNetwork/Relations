using System;
using System.Web.UI;
using EPiServer.Core;
using EPiCode.Relations.Helpers;
using System.Collections;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.WebControls
{
    public partial class RelationPageList : EPiServer.UserControlBase
    {
        private ITemplate _ItemTemplate;
        private ITemplate _AlternateItemTemplate;
        private ITemplate _HeaderTemplate;
        private ITemplate _FooterTemplate;
        private PageDataCollection _DataSource;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string RuleStep1 { get; set; }
        public string RuleStep2 { get; set; }
        public string Direction1 { get; set; }
        public string Direction2 { get; set; }

        private PageDataCollection DataSource
        {
            get
            {
                Rule.Direction dir1 = Rule.Direction.Both;
                Rule.Direction dir2 = Rule.Direction.Both;
                if (Direction1 != null && Direction1.ToLower() == Rule.Direction.Left.ToString().ToLower())
                    dir1 = Rule.Direction.Left;
                if (Direction1 != null && Direction1.ToLower() == Rule.Direction.Right.ToString().ToLower())
                {
                    dir1 = Rule.Direction.Right;
                }
                if (Direction2 != null && Direction2.ToLower() == Rule.Direction.Left.ToString().ToLower())
                    dir2 = Rule.Direction.Left;
                if (Direction2 != null && Direction2.ToLower() == Rule.Direction.Right.ToString().ToLower())
                {
                    dir2 = Rule.Direction.Right;
                }


                if (_DataSource == null)
                {
                    if (RuleStep2 != null && RuleStep1 != null)
                        _DataSource = PageHelper.GetPagesRelated(CurrentPage.PageLink, RuleStep1, dir1, RuleStep2, dir2);
                    else if (RuleStep1 != null)
                    {
                        _DataSource = PageHelper.GetPagesRelated(CurrentPage.PageLink, RuleStep1, dir1);
                    }
                    
                }
                EPiServer.Filters.FilterForVisitor.Filter(_DataSource);
                return _DataSource;
            }
            set
            {
                _DataSource = value;
            }
        }

        [TemplateContainer(typeof(RelationTemplateItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate ItemTemplate
        {
            get { return _ItemTemplate; }
            set { _ItemTemplate = value; }
        }

        [TemplateContainer(typeof(RelationTemplateItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate AlternateItemTemplate
        {
            get { return _AlternateItemTemplate; }
            set { _AlternateItemTemplate = value; }
        }

        [TemplateContainer(typeof(RelationTemplateItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate HeaderTemplate
        {
            get { return _HeaderTemplate; }
            set { _HeaderTemplate = value; }
        }

        [TemplateContainer(typeof(RelationTemplateItem))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate FooterTemplate
        {
            get { return _FooterTemplate; }
            set { _FooterTemplate = value; }
        }

        public override void DataBind()
        {
            if (DataSource == null || DataSource.Count == 0)
                return;
            if(HeaderTemplate != null)
            AddTemplateAsControl(HeaderTemplate, null);

            IEnumerator ie = DataSource.GetEnumerator();
            bool renderAlternateTemplate = false;

            while (ie.MoveNext())
            {


                if (renderAlternateTemplate && AlternateItemTemplate != null)
                {
                    AddTemplateAsControl(AlternateItemTemplate, ie.Current);

                }
                else
                {
                    AddTemplateAsControl(ItemTemplate, ie.Current);
                }

                renderAlternateTemplate = !renderAlternateTemplate;
            }

            if(FooterTemplate != null)
            AddTemplateAsControl(FooterTemplate, null);

            base.DataBind();
        }

        private void AddTemplateAsControl(ITemplate anyTemplate, object currentItem)
        {
            RelationTemplateItem templateContentHolder = new RelationTemplateItem(currentItem);
            anyTemplate.InstantiateIn(templateContentHolder);
            this.Controls.Add(templateContentHolder);
        }


    }
}

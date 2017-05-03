using System.Collections.Generic;
using System.Configuration.Provider;
using EPiServer.Data;

namespace EPiCode.Relations.Core.RelationProviders
{
    public abstract class RelationProviderBase : ProviderBase
    {
        protected string _name;

        public override string Name
        {
            get { return ProviderName; }
        }

        public string ProviderName { get; set; }

        protected string _description;

        public override string Description
        {
            get { return _description; }
        }

        public delegate void RelationEventHandler(object sender, RelationEventArgs e);

        public static event RelationEventHandler OnDeletedRelation;
        public static event RelationEventHandler OnDeletingRelation;
        public static event RelationEventHandler OnAddingRelation;
        public static event RelationEventHandler OnAddedRelation;

        public void RaiseOnDeletedRelation(RelationEventArgs e)
        {
            if (OnDeletedRelation != null)
                OnDeletedRelation(null, e);
        }

        public void RaiseOnDeletingRelation(RelationEventArgs e)
        {
            if (OnDeletingRelation != null)
                OnDeletingRelation(null, e);
        }

        public void RaiseOnAddingRelation(RelationEventArgs e)
        {
            if (OnAddingRelation != null)
                OnAddingRelation(null, e);
        }

        public void RaiseOnAddedRelation(RelationEventArgs e)
        {
            if (OnAddedRelation != null)
                OnAddedRelation(null, e);
        }

        /// <summary>
        /// Create a new relation between two pages in EPi
        /// </summary>
        /// <param name="rule">Rule to use for new relation</param>
        /// <param name="pageLeft">Left side page</param>
        /// <param name="pageRight">Right side page</param>
        public abstract void AddRelation(string rule, int pageLeft, int pageRight);


        /// <summary>
        /// Save relation
        /// </summary>
        /// <param name="relation">Relation to be saved</param>
        public abstract void Save(Relation relation);

        /// <summary>
        /// Get relations for given page
        /// </summary>
        /// <param name="pageID">Page ID to fetch relations from</param>
        /// <returns></returns>
        public abstract List<Relation> GetRelationsForPage(int pageID);

        /// <summary>
        /// Get relations for given page
        /// </summary>
        /// <param name="pageID">Page ID to fetch relations from</param>
        /// <param name="rule">Relation rule to use</param>
        /// <returns></returns>
        public abstract List<Relation> GetRelationsForPage(int pageID, Rule rule);

        /// <summary>
        /// Get all page relations for the rule through a specific direction
        /// </summary>
        /// <param name="pageID">Page ID to get relations from</param>
        /// <param name="rule">Rule to get relations through</param>
        /// <returns></returns>
        public abstract List<Relation> GetRelationsForPage(int pageID, Rule rule, Rule.Direction dir);

        /// <summary>
        /// Get related page IDs for page
        /// </summary>
        /// <param name="pageID">Page ID to get relations from</param>
        /// <param name="rule">Rule to get relations through</param>
        /// <returns></returns>
        public abstract List<int> GetRelationPagesForPage(int pageID, Rule rule);


        /// <summary>
        /// Get a list of all related pages through one direction of a given rule
        /// </summary>
        /// <param name="pageID">Page ID to get relations from</param>
        /// <param name="rule">Rule to get relations through</param>
        /// <param name="direction">Direction to search through</param>
        /// <returns></returns>
        public abstract List<int> GetRelationPagesForPage(int pageID, Rule rule, Rule.Direction direction);

        /// <summary>
        /// Get a list of all related pages through one direction of two given rules
        /// </summary>
        /// <param name="pageID"></param>
        /// <param name="firstRule"></param>
        /// <param name="firstDirection"></param>
        /// <param name="secondRule"></param>
        /// <param name="secondDirection"></param>
        /// <returns></returns>
        public abstract List<int> GetRelationsForPageTwoHop(int pageID, Rule firstRule, Rule.Direction firstDirection, Rule secondRule, Rule.Direction secondDirection);
        
        /// <summary>
        /// Get single relation
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <param name="pageLeft">Left page</param>
        /// <param name="pageRight">Right page</param>
        /// <returns></returns>
        public abstract Relation GetRelation(string rule, int pageLeft, int pageRight);

        /// <summary>
        /// Get single relation
        /// </summary>
        /// <param name="id">Relation ID</param>
        /// <returns></returns>
        public abstract Relation GetRelation(Identity id);

        /// <summary>
        /// Delete single relation
        /// </summary>
        /// <param name="relationToDelete">Relation to be deleted</param>
        public abstract void DeleteRelation(Relation relationToDelete);

        /// <summary>
        /// Check if a relation already exists
        /// </summary>
        /// <param name="rule">Rule to check</param>
        /// <param name="pageLeft">Left page to check</param>
        /// <param name="pageRight">Right page to check</param>
        /// <returns></returns>
        public abstract bool RelationExists(string rule, int pageLeft, int pageRight);

        /// <summary>
        /// Get number of relations for a rule
        /// </summary>
        /// <param name="rule">Rule to get number of relations of</param>
        /// <returns></returns>
        public abstract int GetRelationsCount(string rule);

        /// <summary>
        /// Get all relations of a given rule
        /// </summary>
        /// <param name="rule">Name of rule</param>
        /// <returns></returns>
        public abstract List<Relation> GetAllRelations(string rule);
        
        /// <summary>
        /// Delete all rules
        /// </summary>
        public abstract void DeleteAll();

        /// <summary>
        /// Delete all relations of a given rule
        /// </summary>
        /// <param name="rule"></param>
        public abstract void DeleteAll(string rule);


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EPiCode.Relations.Admin.ViewModels;
using EPiCode.Relations.Core;
using EPiCode.Relations.Core.RelationProviders;
using EPiServer.Data;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Validator = EPiCode.Relations.Core.Validator;

namespace EPiCode.Relations.Admin.Controllers;

[Authorize(Policy = Constants.PolicyName)]
public class AdminController : Controller
{
    private readonly IContentTypeRepository _contentTypeRepository;

    public AdminController(IContentTypeRepository contentTypeRepository)
    {
        _contentTypeRepository = contentTypeRepository;
    }
    
    [HttpGet]
    [ModuleRoute("Admin", "")]
    public IActionResult Index()
    {
        var model = new AdminViewModel
        {
            Rules = RuleEngine.Instance.GetAllRulesList()
        };

        return View("~/EpiCode.Relations.Views/Views/Admin/Index.cshtml", model);
    }
    
    [HttpGet]
    [ModuleRoute("Admin", "Edit")]
    public IActionResult Edit(string ruleId)
    {

        var rule = string.IsNullOrWhiteSpace(ruleId) ? new Rule() : RuleEngine.Instance.GetRule(Identity.Parse(ruleId));
        
        var ruleModel = new RuleViewModel
        {
            RuleId = rule.Id.ToString(),
            RuleName = rule.RuleName,
            PageTypeLeft = HttpUtility.UrlDecode(rule.PageTypeLeft) ?? "",
            RuleTextLeft = rule.RuleTextLeft,
            PageTypeRight = HttpUtility.UrlDecode(rule.PageTypeRight) ?? "",
            RuleTextRight = rule.RuleTextRight,
            RelationHierarchyStartLeft = rule.RelationHierarchyStartLeft.ToString(),
            RelationHierarchyStartRight = rule.RelationHierarchyStartRight.ToString(),
            VisibleLeft = rule.RuleVisibleLeft,
            VisibleRight = rule.RuleVisibleRight,
            RuleDescriptionLeft = rule.RuleDescriptionLeft,
            RuleDescriptionRight = rule.RuleDescriptionRight,
            SortOrderLeft = rule.SortOrderLeft.ToString(),
            SortOrderRight = rule.SortOrderRight.ToString(),
            UniquePrLanguage = rule.UniquePrLanguage.ToString(),
            EditModeAccessLevel = rule.EditModeAccessLevel
        };
        
        var model = new AdminEditViewModel
        {
            RuleModel = ruleModel,
            NumberOfRelations = !string.IsNullOrEmpty(rule.RuleName) ? RelationEngine.Instance.GetRelationsCount(rule.RuleName) : 0,
            Rules = RuleEngine.Instance.GetAllRulesList(),
            PageTypes = _contentTypeRepository.List().Where(t => t is PageType).Select(t => new SelectListItem(t.Name, t.Name)),
            SortOrderLeft = Enum.GetNames(typeof(FilterSortOrder)).Select(t => new SelectListItem(t, ((int)Enum.Parse(typeof (FilterSortOrder),t)).ToString(), ((int)Enum.Parse(typeof (FilterSortOrder),t)).ToString() == ruleModel.SortOrderLeft)).ToList(),
            SortOrderRight = Enum.GetNames(typeof(FilterSortOrder)).Select(t => new SelectListItem(t, ((int)Enum.Parse(typeof (FilterSortOrder),t)).ToString(), ((int)Enum.Parse(typeof (FilterSortOrder),t)).ToString() == ruleModel.SortOrderRight)).ToList(),
            AccessOptions = Enum.GetNames<AccessLevel>().Select(t => new SelectListItem(t, ((int)Enum.Parse(typeof (AccessLevel),t)).ToString(), ((int)Enum.Parse(typeof (AccessLevel),t)).ToString() == ruleModel.EditModeAccessLevel)).ToList()
        };
        
        return View("~/EpiCode.Relations.Views/Views/Admin/Edit.cshtml", model);
    }
    

    [HttpPost]
    [ModuleRoute("Admin", "SaveRule")]
    public IActionResult SaveRule(AdminEditViewModel editModel, string command)
    {
        var model = editModel.RuleModel;
        var rule = RuleEngine.Instance.GetRule(Identity.Parse(model.RuleId));
        
        if (command == "Delete rule")
        {
            RuleEngine.Instance.DeleteRule(rule.RuleName);
        }
        else
        {
            // set all values
            rule.RuleName = model.RuleName;
            rule.PageTypeLeft = HttpUtility.UrlEncode(model.PageTypeLeft);
            rule.RuleTextLeft = model.RuleTextLeft;
            rule.PageTypeRight = HttpUtility.UrlEncode(model.PageTypeRight);
            rule.RuleTextRight = model.RuleTextRight;
            rule.RelationHierarchyStartLeft = int.Parse(model.RelationHierarchyStartLeft);
            rule.RelationHierarchyStartRight = int.Parse(model.RelationHierarchyStartRight);
            rule.RuleVisibleLeft = model.VisibleLeft;
            rule.RuleVisibleRight = model.VisibleRight;
            rule.RuleDescriptionLeft = model.RuleDescriptionLeft;
            rule.RuleDescriptionRight = model.RuleDescriptionRight;
            rule.SortOrderLeft = (int) Enum.Parse(typeof (FilterSortOrder), model.SortOrderLeft);
            rule.SortOrderRight = (int) Enum.Parse(typeof (FilterSortOrder), model.SortOrderRight);
            rule.EditModeAccessLevel = model.EditModeAccessLevel;
        
            RuleEngine.Instance.Save(rule);
        }
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    [ModuleRoute("Admin", "ImportExport")]
    public IActionResult ImportExport()
    {
        return View("~/EpiCode.Relations.Views/Views/Admin/ImportExport.cshtml", new AdminImportExportModel{IncludeRules = true, IncludeRelations = true});
    }
    
    [HttpPost]
    [ModuleRoute("Admin", "ImportOrExport")]
    public IActionResult ImportOrExport(string command, bool includeRules, bool includeRelations, string textArea)
    {
        var model = new AdminImportExportModel()
        {
            IncludeRules = includeRules,
            IncludeRelations = includeRelations  
        };
        
        if (command == "Export")
        {
            
            var rules = RuleEngine.Instance.GetAllRulesList();
            var sb = new StringBuilder();
            foreach (var rule in rules)
            {
                if (model.IncludeRules)
                {
                    sb.Append("Rule;");
                    sb.Append(rule.RuleName + ";");
                    sb.Append(rule.Id + ";");
                    sb.Append(rule.PageTypeLeft + ";");
                    sb.Append(rule.PageTypeRight + ";");
                    sb.Append(rule.RelationHierarchyStartLeft + ";");
                    sb.Append(rule.RelationHierarchyStartRight + ";");
                    sb.Append(rule.RuleTextLeft + ";");
                    sb.Append(rule.RuleTextRight+";");
                    sb.Append(rule.RuleVisibleLeft + ";");
                    sb.Append(rule.RuleVisibleRight + ";");
                    sb.Append(rule.EditModeAccessLevel + ";");
                    sb.Append(rule.SortOrderLeft + ";");
                    sb.Append(rule.SortOrderRight + ";");
                    sb.Append("\n");
                }
                
                if (model.IncludeRelations)
                {
                    var relations = RelationEngine.Instance.GetAllRelations(rule.RuleName);
                    
                    foreach (var relation in relations)
                    {
                        sb.Append("Relation;");
                        sb.Append(relation.RuleName + ";");
                        sb.Append(relation.PageIDLeft + ";");
                        sb.Append(relation.PageIDRight + ";");
                        sb.Append(relation.LanguageBranch+";");
                        sb.Append("\n");
                    }
                }
            }

            model.Export = sb.ToString();
        }
        else
        {
            // import
            var status = "";
            var rulesImported = 0;
            var rulesNotImported = 0;
            var relationsImported = 0;
            var relationsNotImported = 0;
            
            if (!string.IsNullOrWhiteSpace(textArea))
            {
                string[] rows = textArea.Split('\n');
                if (rows.Length > 0) {
                    
                    foreach (string row in rows) 
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string[] columns = row.Split(';');
                            {
                                if (columns.Length == 15 && columns[0] == "Rule" && model.IncludeRules) {
                                    status +=  (ImportRule(columns));
                                    if (!string.IsNullOrEmpty(status))
                                    {
                                       // Status(status);
                                        rulesNotImported++;
                                    }
                                    else
                                        rulesImported++;
                                }

                                if (columns.Length == 6 && columns[0] == "Relation" && model.IncludeRelations) {
                                    status = (ImportRelation(columns));
                                    if (!string.IsNullOrEmpty(status))
                                    {
                                       // Status(status);
                                        relationsNotImported++;
                                    }
                                    else
                                        relationsImported++;
                                }
                            }
                        }
                    }
                    
                    model.Status = $"Rules imported: {rulesImported} / {(rulesImported + rulesNotImported)}<br />";
                    model.Status += $"Relations imported: {relationsImported} / {(relationsImported + relationsNotImported)}<br />";
                }
            }
            else
            {
                model.Status = "No data to import. Aborting.";
            }
        }
        
        return View("~/EpiCode.Relations.Views/Views/Admin/ImportExport.cshtml", model);
    }

    private string ImportRule(string[] row)
    {
        try
        {
            string name = row[1];

            //int left = int.Parse(row[1]);
            //int right = int.Parse(row[2]);
            string pageTypeLeft = row[3];
            string pageTypeRight = row[4];
            int relationHierarchyStartLeft = int.Parse(row[5]);
            int relationHierarchyStartRight = int.Parse(row[6]);
            string ruleTextLeft = row[7];
            string ruleTextRight = row[8];
            bool ruleVisibleLeft = bool.Parse(row[9]);
            bool ruleVisibleRight = bool.Parse(row[10]);
            string ruleEditModeAccessLevel = row[11];
            int ruleSortOrderLeft = int.Parse(row[12]);
            int ruleSortOrderRight = int.Parse(row[13]);

            Rule newRule;
            if (RuleEngine.Instance.RuleExists(name))
            {
                newRule = RuleEngine.Instance.GetRule(name);
            }
            else
            {
                newRule = RuleEngine.Instance.AddNewRule(name, pageTypeLeft, pageTypeRight, ruleTextLeft, ruleTextRight);
            }

            newRule.PageTypeLeft = pageTypeLeft;
            newRule.PageTypeRight = pageTypeRight;
            newRule.RelationHierarchyStartLeft = relationHierarchyStartLeft;
            newRule.RelationHierarchyStartRight = relationHierarchyStartRight;
            newRule.RuleTextLeft = ruleTextLeft;
            newRule.RuleTextRight = ruleTextRight;
            newRule.RuleVisibleLeft = ruleVisibleLeft;
            newRule.RuleVisibleRight = ruleVisibleRight;
            newRule.EditModeAccessLevel = ruleEditModeAccessLevel;
            newRule.SortOrderLeft = ruleSortOrderLeft;
            newRule.SortOrderRight = ruleSortOrderRight;

            RuleEngine.Instance.Save(newRule);
            
            return string.Empty;
        }
        catch (Exception e)
        {
            return "Something went wrong: " + e.Message;
        }
    }


    private static string ImportRelation(string[] row)
    {
        string name = row[1];
        int left = int.Parse(row[2]);
        int right = int.Parse(row[3]);

        Validator.ValidationResult validation = Validator.Validate(name, left, right);
        if (validation == Validator.ValidationResult.Ok)
        {
            RelationEngine.Instance.AddRelation(name, left, right);
            return string.Empty;
        }

        return "Could not import relation: " + validation;
    }

    [HttpGet]
    [ModuleRoute("Admin", "Settings")]
    public IActionResult Settings()
    {
        var relationProviders = RelationProviderManager.GetRelationProviders();
        var ruleProviders = RuleProviderManager.GetRuleProviders();
        
        var model = new AdminSettingsViewModel
        {
            CurrentRelationProvider = Core.Settings.GetSettingValue("DefaultRelationProviderString"),
            CurrentRuleProvider = Core.Settings.GetSettingValue("DefaultRuleProviderString"),
            RelationProviders = CreateSelectList(relationProviders),
            RuleProviders = CreateSelectList(ruleProviders)
        };
        
        return View("~/EpiCode.Relations.Views/Views/Admin/Settings.cshtml", model);
    }
    
    private IEnumerable<SelectListItem> CreateSelectList(IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            if (type.Assembly.FullName != null)
            {
                yield return new SelectListItem(type.Name, $"{type.FullName}, {type.Assembly.FullName.Substring(0, type.Assembly.FullName.IndexOf(','))}");
            }
        }
    }
    
    [HttpPost]
    [ModuleRoute("Admin", "SaveSettings")]
    public IActionResult SaveSettings(AdminSettingsViewModel settingsViewModel)
    {
        Core.Settings.SaveSetting("DefaultRelationProviderString", settingsViewModel.CurrentRelationProvider);
        RelationProviderManager.Initialize();
        Core.Settings.SaveSetting("DefaultRuleProviderString", settingsViewModel.CurrentRuleProvider);
        RuleProviderManager.Initialize();
        
        return RedirectToAction("Settings");
    }
    
    
    [HttpGet]
    public IActionResult RelationValidator()
    {
        return View("~/EpiCode.Relations.Views/Views/Admin/Validator.cshtml");
    }

    [HttpPost]
    [ModuleRoute("Admin", "Validate")]
    public IActionResult Validate(string command, string id)
    {
        if (command == "Delete" && !string.IsNullOrEmpty(id))
        {
            var relation = RelationEngine.Instance.GetRelation(Identity.Parse(id));
            if (relation != null)
            {
                RelationEngine.Instance.DeleteRelation(relation);
            }
        }
        
        var invalidRelations = new List<ValidationResultViewModel>();
        var rules = RuleEngine.Instance.GetAllRules().ToList();
        foreach (Rule rule in rules)
        {
            List<Relation> relations = RelationEngine.Instance.GetAllRelations(rule.RuleName);
            foreach (Relation relation in relations)
            {
                var result = Validator.Validate(relation.RuleName, relation.PageIDLeft, relation.PageIDRight, true);
                if (result != Validator.ValidationResult.Ok)
                    invalidRelations.Add(new ValidationResultViewModel()
                    {
                        InvalidRelation = relation,
                        Status = result.ToString(),
                        ContentNameLeft = PageEngine.GetPage(relation.PageIDLeft)?.Name,
                        ContentNameRight = PageEngine.GetPage(relation.PageIDRight)?.Name
                    });
            }
        }

        var model = new ValidatorViewModel()
        {
            InvalidRelations = invalidRelations
        };

        if (command == "Remove all invalid")
        {
            model.Status = RemoveInvalidRelations(invalidRelations.Select(x => x.InvalidRelation).ToList());
        }
        
        return View("~/EpiCode.Relations.Views/Views/Admin/Validator.cshtml", model);
    }
    
    private string RemoveInvalidRelations(List<Relation> invalidRelations)
    {
        int cnt = 0;
        foreach (var invalidRelation in invalidRelations)
        {
            RelationEngine.Instance.DeleteRelation(invalidRelation);
            cnt++;
        }
        
        return $"Removed {cnt} invalid relations";
    }
}
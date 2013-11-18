using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GlobalParams
{
    /// <summary>The wizard used to expose replacement parameters across multiple projects.</summary>
    public class WizardMPT : IWizard
    {
        #region Methods

        #region BeforeOpeningFile
        /// <summary>This method is called before opening any item that has the OpenInEditor attribute.</summary>
        /// <param name="projectItem">The item to be opened.</param>
        public void BeforeOpeningFile(ProjectItem projectItem) { OnBeforeOpeningFile(projectItem); }
        #endregion BeforeOpeningFile

        #region OnBeforeOpeningFile
        /// <summary>This method is called before opening any item that has the OpenInEditor attribute.</summary>
        /// <param name="projectItem">The item to be opened.</param>
        protected virtual void OnBeforeOpeningFile(ProjectItem projectItem) { }
        #endregion OnBeforeOpeningFile

        #region OnProjectFinishedGenerating
        /// <summary>Called after a project has finished generating.</summary>
        /// <param name="project">The project that was generated.</param>
        protected virtual void OnProjectFinishedGenerating(Project project) { }
        #endregion OnProjectFinishedGenerating

        #region OnProjectItemFinishedGenerating
        /// <summary>This method is only called for item templates, not for project templates.</summary>
        /// <param name="projectItem">The item that was generated.</param>
        protected virtual void OnProjectItemFinishedGenerating(ProjectItem projectItem) { }
        #endregion OnProjectItemFinishedGenerating

        #region OnRunFinished
        /// <summary>This method is called when the wizard is complete.</summary>
        protected virtual void OnRunFinished() { }
        #endregion OnRunFinished

        #region OnBeforeRunStarted
        /// <summary>Called when the wizard first begins and before the global parameters are create.</summary>
        /// <remarks>Override and return false to stop default global parameters.</remarks>
        protected virtual bool OnBeforeRunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            return true;
        }
        #endregion OnBeforeRunStarted

        #region OnAfterRunStarted
        /// <summary>Called when the wizard first begins and before the global parameters are create.</summary>
        protected virtual void OnAfterRunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams) { }
        #endregion OnAfterRunStarted

        #region OnShouldAddProjectItem
        /// <summary>This method is only called for item templates, not for project templates.</summary>
        /// <param name="filePath">The full path to the item.</param>
        /// <returns>True if the item should be added, otherwise false.</returns>
        protected virtual bool OnShouldAddProjectItem(string filePath) { return true; }
        #endregion OnShouldAddProjectItem

        #region ProjectFinishedGenerating
        /// <summary>Called after a project has finished generating.</summary>
        /// <param name="project">The project that was generated.</param>
        public void ProjectFinishedGenerating(Project project) { OnProjectFinishedGenerating(project); }
        #endregion ProjectFinishedGenerating

        #region ProjectItemFinishedGenerating
        /// <summary>This method is only called for item templates, not for project templates.</summary>
        /// <param name="projectItem">The item that was generated.</param>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { OnProjectItemFinishedGenerating(projectItem); }
        #endregion ProjectItemFinishedGenerating

        #region RunFinished
        /// <summary>This method is called when the wizard is complete.</summary>
        public void RunFinished() { OnRunFinished(); }
        #endregion RunFinished

        #region RunStarted
        /// <summary>This method is called when the wizard first begins.</summary>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            if (OnBeforeRunStarted(automationObject, replacementsDictionary, runKind, customParams))
            {
                // Check if we are running as the top level template
                if (WizardRunKind.AsMultiProject.Equals(runKind))
                {
                    // Clear any leftover items from the static instance.
                    Parameters.Clear();

                    // We are running as the top level template so record the replacements dictionary
                    foreach (string key in replacementsDictionary.Keys)
                    {
                        Parameters.Set(key, replacementsDictionary[key]);
                    }

                    // Extend the number of guids from 10 to 100
                    for (int i = 1; i <= 100; i++)
                    {
                        Parameters.Set(string.Format("guid{0}", i), Guid.NewGuid().ToString("D"));
                    }
                }

                // Make sure each template that runs us has access to the global parameters
                foreach (var parameter in Parameters.All)
                {
                    if (replacementsDictionary.ContainsKey(parameter.Key))
                    {
                        replacementsDictionary[parameter.Key] = Parameters.Get(parameter.Key).ToString();
                    }
                    else
                    {
                        replacementsDictionary.Add(parameter.Key, parameter.Value.ToString());
                    }
                }
            }
            OnAfterRunStarted(automationObject, replacementsDictionary, runKind, customParams);
        }
        #endregion RunStarted

        #region ShouldAddProjectItem
        /// <summary>This method is only called for item templates, not for project templates.</summary>
        /// <param name="filePath">The full path to the item.</param>
        /// <returns>True if the item should be added, otherwise false.</returns>
        public bool ShouldAddProjectItem(string filePath) { return OnShouldAddProjectItem(filePath); }
        #endregion ShouldAddProjectItem

        #endregion Methods
    }
}
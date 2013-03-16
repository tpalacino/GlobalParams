using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;

namespace GlobalParams
{
	/// <summary>The wizard used to expose variables across multiple projects.</summary>
	public class Wizard : IWizard
	{
		#region Member Variables

		/// <summary>Called before opening a project item that has the OpenInEditor attribute.</summary>
		protected Action<ProjectItem> BeforeOpeningFileEvent = null;

		/// <summary>Called after a project has finished generating.</summary>
		protected Action<Project> ProjectFinishedGeneratingEvent = null;

		/// <summary>Called after a project item has finished generating.</summary>
		protected Action<ProjectItem> ProjectItemFinishedGeneratingEvent = null;

		/// <summary>Called when the wizard is complete.</summary>
		protected Action RunFinishedEvent = null;

		/// <summary>Called when the wizard first begins.</summary>
		protected Func<object, Dictionary<string, string>, WizardRunKind, object[], bool> RunStartedEvent = null;

		/// <summary>Called for item templates only to determine if the item should be added.</summary>
		protected Func<string, bool> ShouldAddProjectItemEvent = null;

		#endregion Member Variables

		#region Methods

		#region BeforeOpeningFile
		/// <summary>This method is called before opening any item that has the OpenInEditor attribute.</summary>
		/// <param name="projectItem">The item to be opened.</param>
		public void BeforeOpeningFile(ProjectItem projectItem)
		{
			if (BeforeOpeningFileEvent != null)
			{
				BeforeOpeningFileEvent(projectItem);
			}
		}
		#endregion BeforeOpeningFile

		#region ProjectFinishedGenerating
		/// <summary>Called after a project has finished generating.</summary>
		/// <param name="project">The project that was generated.</param>
		public void ProjectFinishedGenerating(Project project)
		{
			if (ProjectFinishedGeneratingEvent != null)
			{
				ProjectFinishedGeneratingEvent(project);
			}
		}
		#endregion ProjectFinishedGenerating

		#region ProjectItemFinishedGenerating
		/// <summary>This method is only called for item templates, not for project templates.</summary>
		/// <param name="projectItem">The item that was generated.</param>
		public void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
			if (ProjectItemFinishedGeneratingEvent != null)
			{
				ProjectItemFinishedGeneratingEvent(projectItem);
			}
		}
		#endregion ProjectItemFinishedGenerating

		#region RunFinished
		/// <summary>This method is called when the wizard is complete.</summary>
		public void RunFinished()
		{
			if (RunFinishedEvent != null)
			{
				RunFinishedEvent();
			}
		}
		#endregion RunFinished

		#region RunStarted
		/// <summary>This method is called when the wizard first begins.</summary>
		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			if (RunStartedEvent == null || RunStartedEvent(automationObject, replacementsDictionary, runKind, customParams))
			{
				// Check if we are running as the parent template
				if (IsMainTemplate(replacementsDictionary))
				{
					// We are running as the parent template so record the solution name and the replacements dictionary
					foreach (string key in replacementsDictionary.Keys)
					{
						Parameters.Set(key, replacementsDictionary[key]);
					}

					// Extend the number of global guids from 10 to 100
					for (int i = 11; i <= 100; i++)
					{
						Parameters.Set(string.Format("guid{0}", i), Guid.NewGuid().ToString());
					}
				}
				else
				{
					// We are running as a child template so bring in the replacements dictionary entries from the parent
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
			}
		}
		#endregion RunStarted

		#region ShouldAddProjectItem
		/// <summary>This method is only called for item templates, not for project templates.</summary>
		/// <param name="filePath">The full path to the item.</param>
		/// <returns>True if the item should be added, otherwise false.</returns>
		public bool ShouldAddProjectItem(string filePath)
		{
			return (ShouldAddProjectItemEvent == null || ShouldAddProjectItemEvent(filePath));
		}
		#endregion ShouldAddProjectItem

		#region IsMainTemplate
		/// <summary>Determines if the project being generated is the top level template.</summary>
		/// <param name="parms">The replacements dictionary that will include the solution name if it is the top level template.</param>
		/// <returns>True if the replacements dictionary includes the solution name, otherwise false.</returns>
		protected bool IsMainTemplate(Dictionary<string, string> parms)
		{
			return (parms != null && parms.ContainsKey(Constants.SOLUTION_KEY) && !string.IsNullOrEmpty(parms[Constants.SOLUTION_KEY]));
		}
		#endregion IsMainTemplate

		#endregion Methods
	}
}

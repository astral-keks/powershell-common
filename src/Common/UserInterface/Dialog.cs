using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation.Host;

namespace AstralKeks.PowerShell.Common.UserInterface
{
    public static class Dialog
    {
        public static DialogResult<TOption> PromptForAnswer<TOption>(this PSHostUserInterface ui, string message,
            TOption[] optionValues, string[] helpMessages = null)
        {
            return ui.PromptForAnswer(null, message, optionValues, helpMessages);
        }

        public static DialogResult<TOption> PromptForAnswer<TOption>(this PSHostUserInterface ui, string caption, string message,
            TOption[] optionValues, string[] helpMessages = null)
        {
            var options = optionValues.Select((v, i) => new PromptOption<TOption>(v.ToString(), helpMessages?[i], v)).ToArray();
            return ui.PromptForAnswer(caption, message, options);
        }

        public static DialogResult<TOption> PromptForAnswer<TOption>(this PSHostUserInterface ui, string caption, string message,
            PromptOption<TOption>[] options)
        {
            var question = new PromptQuestion<TOption>(caption, message, 0, options);
            return ui.PromptForAnswer(question);
        }

        public static DialogResult<TOption> PromptForAnswer<TOption>(this PSHostUserInterface ui, PromptQuestion<TOption> question)
        {
            var references = new HashSet<char>();
            var choises = new Collection<ChoiceDescription>(question.Options
                .Select(o => new ChoiceDescription(SetLabelReference(o.Label, references), o.Help ?? string.Empty))
                .ToList());
            var index = ui.PromptForChoice(question.Caption, question.Message, choises, question.Index);
            var option = question.Options[index];

            return new DialogResult<TOption>(index, option.Value);
        }

        private static string SetLabelReference(string label, HashSet<char> existingReferences)
        {
            for (int i = 0; i < label.Length; i++)
            {
                var ch = label[i];
                if (existingReferences.Add(ch))
                {
                    label = label.Insert(i, "&");
                    return label;
                }
            }

            return label;
        }

    }
}

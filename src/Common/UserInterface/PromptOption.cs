using System;

namespace AstralKeks.PowerShell.Common.UserInterface
{
    public struct PromptOption<TOption>
    {
        public PromptOption(string label, string help, TOption value)
        {
            Label = label;
            Help = help;
            Value = value;
        }

        public string Label { get; }

        public string Help { get; }

        public TOption Value { get; }
    }
}

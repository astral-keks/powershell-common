using System;

namespace AstralKeks.PowerShell.Common.UserInterface
{
    public partial struct PromptQuestion<TOption>
    {
        public PromptQuestion(string caption, params PromptOption<TOption>[] options)
            : this(caption, string.Empty, 0, options)
        {
        }

        public PromptQuestion(string caption, string message, int index, params PromptOption<TOption>[] options)
        {
            Caption = caption;
            Message = message;
            Index = index;
            Options = options;
        }

        public string Caption { get; }

        public string Message { get; }

        public int Index { get; }

        public PromptOption<TOption>[] Options { get; }
    }
}

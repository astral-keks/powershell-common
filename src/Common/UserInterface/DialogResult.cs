using System;

namespace AstralKeks.PowerShell.Common.UserInterface
{
    public struct DialogResult<TOption>
    {
        public DialogResult(int selectedIndex, TOption selectedValue)
        {
            SelectedIndex = selectedIndex >= 0 ? selectedIndex : throw new IndexOutOfRangeException();
            SelectedValue = selectedValue;
        }

        public int SelectedIndex { get; }
        public TOption SelectedValue { get; }
    }
}

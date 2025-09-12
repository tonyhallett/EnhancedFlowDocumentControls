using System;
using System.Reflection;
using System.Windows.Controls;

namespace EnhancedFlowDocumentControls.Management
{
    internal class FindToolBarWrapper : IFinder
    {
        private sealed class ReflectionMembers
        {
            public FieldInfo OptionsWholeWordMenuItem { get; }

            public FieldInfo OptionsCaseMenuItem { get; }

            public FieldInfo OptionsDiacriticMenuItem { get; }

            public FieldInfo OptionsKashidaMenuItem { get; }

            public FieldInfo OptionsAlefHamzaMenuItem { get; }

            public PropertyInfo SearchUp { get; }

            public FieldInfo FindTextBox { get; }

            public MethodInfo OnFindClick { get; }

            public ReflectionMembers(ToolBar findToolBar)
            {
                Type findToolBarType = findToolBar.GetType();
                OptionsWholeWordMenuItem = findToolBarType.GetField("OptionsWholeWordMenuItem", BindingFlags.Instance | BindingFlags.NonPublic);
                OptionsCaseMenuItem = findToolBarType.GetField("OptionsCaseMenuItem", BindingFlags.Instance | BindingFlags.NonPublic);
                OptionsDiacriticMenuItem = findToolBarType.GetField("OptionsDiacriticMenuItem", BindingFlags.Instance | BindingFlags.NonPublic);
                OptionsKashidaMenuItem = findToolBarType.GetField("OptionsKashidaMenuItem", BindingFlags.Instance | BindingFlags.NonPublic);
                OptionsAlefHamzaMenuItem = findToolBarType.GetField("OptionsAlefHamzaMenuItem", BindingFlags.Instance | BindingFlags.NonPublic);

                SearchUp = findToolBarType.GetProperty("SearchUp", BindingFlags.Instance | BindingFlags.Public);
                FindTextBox = findToolBarType.GetField("FindTextBox", BindingFlags.Instance | BindingFlags.NonPublic);
                OnFindClick = findToolBarType.GetMethod("OnFindClick", BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

        private static ReflectionMembers s_reflectionMembers;
        private readonly ToolBar _findToolbar;

        public FindToolBarWrapper(ToolBar findToolbar)
        {
            if (s_reflectionMembers == null)
            {
                s_reflectionMembers = new ReflectionMembers(findToolbar);
            }

            _findToolbar = findToolbar;
        }

        private void SelectMatchWholeWord(bool isChecked) => SelectOption(s_reflectionMembers.OptionsWholeWordMenuItem, isChecked);

        private void SelectMatchCase(bool isChecked) => SelectOption(s_reflectionMembers.OptionsCaseMenuItem, isChecked);

        private void SelectMatchDiacritic(bool isChecked) => SelectOption(s_reflectionMembers.OptionsDiacriticMenuItem, isChecked);

        private void SelectMatchKashida(bool isChecked) => SelectOption(s_reflectionMembers.OptionsKashidaMenuItem, isChecked);

        private void SelectMatchAlefHamza(bool isChecked) => SelectOption(s_reflectionMembers.OptionsAlefHamzaMenuItem, isChecked);

        private void SetSearchUp(bool isSearchUp) => s_reflectionMembers.SearchUp.SetValue(_findToolbar, isSearchUp);

        private void SetFindText(string findText) => (s_reflectionMembers.FindTextBox.GetValue(_findToolbar) as TextBox).Text = findText;

        public void Find(IFindParameters findParameters)
        {
            SetFindText(findParameters.FindText);
            SetSearchUp(findParameters.IsSearchUp);
            SelectMatchWholeWord(findParameters.MatchWholeWord);
            SelectMatchCase(findParameters.MatchCase);
            SelectMatchDiacritic(findParameters.MatchDiacritic);
            SelectMatchKashida(findParameters.MatchKashida);
            SelectMatchAlefHamza(findParameters.MatchAlefHamza);
            _ = s_reflectionMembers.OnFindClick.Invoke(_findToolbar, null);
        }

        private bool SelectOption(FieldInfo menuField, bool isChecked)
        {
            var menuItem = menuField.GetValue(_findToolbar) as MenuItem;
            menuItem.IsChecked = isChecked;
            return true;
        }
    }
}

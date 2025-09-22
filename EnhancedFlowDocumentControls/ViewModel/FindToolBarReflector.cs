using System;
using System.Windows.Controls;
using Fasterflect;

namespace EnhancedFlowDocumentControls.ViewModel
{
    internal sealed class FindToolBarReflector : IFindToolBarReflector
    {
        private readonly ToolBar _findToolBar;
        private static bool s_isInitialized;
        private static MemberGetter s_selectOptionsWholeWordMenuItemGetter;
        private static MemberGetter s_selectOptionsCaseMenuItemGetter;
        private static MemberGetter s_selectOptionsDiacriticMenuItemGetter;
        private static MemberGetter s_selectOptionsKashidaMenuItemGetter;
        private static MemberGetter s_selectOptionsOptionsAlefHamzaMenuItemGetter;
        private static MemberSetter s_searchUpSetter;
        private static MemberGetter s_findTextBoxGetter;
        private static MethodInvoker s_onFindClickInvoker;

        public FindToolBarReflector(ToolBar findToolBar)
        {
            _findToolBar = findToolBar;
            if (s_isInitialized)
            {
                return;
            }

            Type findToolBarType = findToolBar.GetType();
            s_selectOptionsWholeWordMenuItemGetter = findToolBarType.DelegateForGetFieldValue("OptionsWholeWordMenuItem", Flags.InstancePrivate);
            s_selectOptionsCaseMenuItemGetter = findToolBarType.DelegateForGetFieldValue("OptionsCaseMenuItem", Flags.InstancePrivate);
            s_selectOptionsDiacriticMenuItemGetter = findToolBarType.DelegateForGetFieldValue("OptionsDiacriticMenuItem", Flags.InstancePrivate);
            s_selectOptionsKashidaMenuItemGetter = findToolBarType.DelegateForGetFieldValue("OptionsKashidaMenuItem", Flags.InstancePrivate);
            s_selectOptionsOptionsAlefHamzaMenuItemGetter = findToolBarType.DelegateForGetFieldValue("OptionsAlefHamzaMenuItem", Flags.InstancePrivate);
            s_searchUpSetter = findToolBarType.DelegateForSetPropertyValue("SearchUp", Flags.InstancePublic);
            s_findTextBoxGetter = findToolBarType.DelegateForGetFieldValue("FindTextBox", Flags.InstancePrivate);
            s_onFindClickInvoker = findToolBarType.DelegateForCallMethod("OnFindClick", Flags.InstancePrivate);
            s_isInitialized = true;
        }

        public void InvokeFind() => s_onFindClickInvoker(_findToolBar);

        public void SetSearchUp(bool isSearchUp) => s_searchUpSetter(_findToolBar, isSearchUp);

        public TextBox GetFindTextBox() => s_findTextBoxGetter(_findToolBar) as TextBox;

        public MenuItem GetMatchWholeWordMenuItem() => s_selectOptionsWholeWordMenuItemGetter(_findToolBar) as MenuItem;

        public MenuItem GetMatchCaseMenuItem() => s_selectOptionsCaseMenuItemGetter(_findToolBar) as MenuItem;

        public MenuItem GetMatchDiacriticMenuItem() => s_selectOptionsDiacriticMenuItemGetter(_findToolBar) as MenuItem;

        public MenuItem GetMatchKashidaMenuItem() => s_selectOptionsKashidaMenuItemGetter(_findToolBar) as MenuItem;

        public MenuItem GetMatchAlefHamzaMenuItem() => s_selectOptionsOptionsAlefHamzaMenuItemGetter(_findToolBar) as MenuItem;
    }
}

using FlaUI.Core.AutomationElements;
using UIAutomationHelpers;

namespace VideoRecorder
{
    internal sealed class RecordSteps
    {
        private const int NavigationDelay = 500;

        public static List<IStep> GetSteps()
        {
            List<IStep> steps = [
             ClickFindButtonStep(),
              ..InputTextSteps("FlowDocumentReader"),
              ..NavigateAndToggleMatchCaseShowMenuAndSearch(3),
              ..SwitchPaletteShowMenuSteps(),

              KeepAlive(2000)];
            return steps;
        }

        private static SimpleStep KeepAlive(int duration) => new(_ => { }, duration);

        private static SimpleStep ClickFindButtonStep()
            => new(
                window =>
                {
                    Button findButton = ControlFinder.FindFindButton(window!)!;
                    findButton.Click();
                },
                1000);

        private static List<SimpleStep> InputTextSteps(string text)
        {
            List<SimpleStep> steps = [
                new SimpleStep(
                    window =>
                    {
                        TextBox? findTextBox = ControlFinder.FindFindTextBox(window!);
                        findTextBox!.Focus();
                    },
                    1000),
                ..TypeWordSteps(text, 100)
            ];
            return steps;
        }

        private static IEnumerable<SimpleStep> TypeWordSteps(string word, int keyDelay)
            => Typer.TypeWord(word).Select(action => new SimpleStep(_ => action(), keyDelay));

        private static readonly IStep s_openMenuWithClickStep = new MouseMoveClickStep(window => ControlFinder.FindFindMenu(window)!, 1000);

        private static List<IStep> NavigateAndToggleMatchCaseShowMenuAndSearch(int numTabs)
        {
            List<IStep> steps = [
              ..NavigateToMenuSelectMatchCaseSteps(numTabs),
              s_openMenuWithClickStep,
              ..NavigateToSearchForwardAndPressSteps(),
              ];
            return steps;
        }

        private static List<SimpleStep> NavigateToSearchForwardAndPressSteps() => [
            new(_ => Typer.TypeTab(), NavigationDelay),
            new(_ => Typer.TypeTab(), NavigationDelay),
            new(_ => Typer.TypeTab(), 1500),
            new(_ => Typer.TypeEnter(), NavigationDelay)
        ];

        private static List<SimpleStep> NavigateToMenuSelectMatchCaseSteps(int numTabs)
        {
            List<SimpleStep> steps =
            [
                ..Enumerable.Range(0, numTabs).Select(_ => new SimpleStep(_ => Typer.TypeTab(), NavigationDelay)),
                new SimpleStep(_ => Typer.TypeEnter(), NavigationDelay),
                new SimpleStep(_ => Typer.TypeDown(), NavigationDelay),
                new SimpleStep(_ => Typer.TypeEnter(), 2000)
            ];

            return steps;
        }

        private static List<IStep> SwitchPaletteShowMenuSteps()
        {
            List<IStep> steps = [
                new MouseMoveClickStep(window => ControlFinder.FindPinkPaletteRadioButton(window)!, 0),
                s_openMenuWithClickStep,];
            return steps;
        }
    }
}

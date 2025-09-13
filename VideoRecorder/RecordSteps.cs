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
             ClickFindButton(),
              ..InputText("FlowDocumentReader"),
              ..TabToMenuSelectMatchCase(3),

              // show what just selected
                s_moveClickMenuDropDown,
                s_moveClickMenuDropDown,

              ..NavigateToSearchForwardAndPress(),
              ..SwitchPaletteAndShowMenuWithMouseMoveAndClicks(),
              KeepAlive(2000)];
            return steps;
        }

        private static SimpleStep KeepAlive(int duration) => new(_ => { }, duration);

        private static SimpleStep ClickFindButton()
            => new(
                window =>
                {
                    Button findButton = ControlFinder.FindFindButton(window!)!;
                    findButton.Click();
                },
                1000);

        private static List<SimpleStep> InputText(string text)
        {
            List<SimpleStep> steps = [
                new SimpleStep(
                    window =>
                    {
                        TextBox? findTextBox = ControlFinder.FindFindTextBox(window!);
                        findTextBox!.Focus();
                    },
                    1000),
                ..TypeWord(text, 100)
            ];
            return steps;
        }

        private static IEnumerable<SimpleStep> TypeWord(string word, int keyDelay)
            => Typer.TypeWord(word).Select(action => new SimpleStep(_ => action(), keyDelay));

        private static readonly IStep s_moveClickMenuDropDown = new MouseMoveClickStep(window => ControlFinder.FindFindMenu(window)!, 1000);

        private static List<SimpleStep> NavigateToSearchForwardAndPress() => [
            new(_ => Typer.TypeTab(), NavigationDelay),
            new(_ => Typer.TypeTab(), NavigationDelay),
            new(_ => Typer.TypeTab(), 1500),
            new(_ => Typer.TypeEnter(), NavigationDelay)
        ];

        private static List<SimpleStep> TabToMenuSelectMatchCase(int numTabs)
        {
            List<SimpleStep> steps =
            [
                ..MultipleTabs(numTabs),

                // open menu
                new SimpleStep(_ => Typer.TypeEnter(), NavigationDelay),

                // navigate to Match Case and select
                new SimpleStep(_ => Typer.TypeDown(), NavigationDelay),
                new SimpleStep(_ => Typer.TypeEnter(), 2000)
            ];

            return steps;
        }

        private static IEnumerable<SimpleStep> MultipleTabs(int numTabs)
            => Enumerable.Range(0, numTabs).Select(_ => new SimpleStep(_ => Typer.TypeTab(), NavigationDelay));

        private static List<IStep> SwitchPaletteAndShowMenuWithMouseMoveAndClicks()
        {
            List<IStep> steps = [
                new MouseMoveClickStep(window => ControlFinder.FindPinkPaletteRadioButton(window)!, 0),
                s_moveClickMenuDropDown,];
            return steps;
        }
    }
}

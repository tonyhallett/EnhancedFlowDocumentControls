namespace UITests.TestHelpers
{
    internal static class SolutionConfiguration
    {
#if UITestsSolutionConfig
        public const bool IsUITests = true;
#else
        public const bool IsUITests = false;
#endif
    }
}

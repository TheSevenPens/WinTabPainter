namespace WinTabPressureTester
{
    /// <summary>
    /// Result of parsing a scale reading line.
    /// </summary>
    public sealed record ScaleParsedLine(string Input, bool Parsed, ScaleRecord? ScaleRecord, string Error);
}

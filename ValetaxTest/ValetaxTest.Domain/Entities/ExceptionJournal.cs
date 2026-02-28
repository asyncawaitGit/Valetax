namespace ValetaxTest.Domain.Entities;

public class ExceptionJournal
{
    public long Id { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string Parameters { get; private set; } = null!;
    public string StackTrace { get; private set; } = null!;

    private ExceptionJournal() { }

    public ExceptionJournal(DateTime timestamp, string parameters, string stackTrace)
    {
        Timestamp = timestamp;
        Parameters = parameters;
        StackTrace = stackTrace;
    }
}

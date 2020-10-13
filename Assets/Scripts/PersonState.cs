public class PersonState
{
    public PersonState()
    {
        IsBeingReaped = false;
        IsReaped = false;
        LastWordsShared = false;
        IsCondemned = false;
        IsCommended = false;
    }

    public bool IsBeingReaped { get; set; } = false;
    public bool IsReaped { get; set; } = false;
    public bool LastWordsShared { get; set; } = false;
    public bool IsCondemned { get; set; } = false;
    public bool IsCommended { get; set; } = false;
}

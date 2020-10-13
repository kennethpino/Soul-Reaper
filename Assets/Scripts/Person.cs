using UnityEngine;

[CreateAssetMenu(menuName = "Person")]

public class Person : ScriptableObject
{
    [SerializeField] private bool isGuilty = default;
    [SerializeField] private string personName = default;
    [TextArea(2, 5)] [SerializeField] private string condemnReaction = default;
    [TextArea(2, 5)] [SerializeField] private string commendReaction = default;
    [TextArea(2, 5)] [SerializeField] private string defaultText = default;
    [TextArea(2, 5)] [SerializeField] private string lastWords = default;
    [TextArea(2, 5)] [SerializeField] private string condemnedMessage = default;
    [TextArea(2, 5)] [SerializeField] private string commendedMessage = default;

    public PersonState PersonState { get; set; }

    public Person()
    {
        PersonState = new PersonState();
    }

    public string PersonName { get => personName; set => personName = value; }
    public bool IsGuilty { get => isGuilty; set => isGuilty = value; }
    public string CondemnReaction { get => condemnReaction; set => condemnReaction = value; }
    public string CommendReaction { get => commendReaction; set => commendReaction = value; }
    public string DefaultText { get => defaultText; set => defaultText = value; }
    public string LastWords { get => lastWords; set => lastWords = value; }
    public string CondemnedMessage { get => condemnedMessage; set => condemnedMessage = value; }
    public string CommendedMessage { get => commendedMessage; set => commendedMessage = value; }
}

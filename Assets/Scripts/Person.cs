using UnityEngine;

[CreateAssetMenu(menuName = "Person")]

public class Person : ScriptableObject
{
    [SerializeField] private bool isGuilty = default;
    [SerializeField] private string personName = default;
    [TextArea(2, 5)] [SerializeField] private string condemnReaction = default;
    [TextArea(2, 5)] [SerializeField] private string rewardReaction = default;
    [TextArea(2, 5)] [SerializeField] private string defaultText = default;
    [TextArea(2, 5)] [SerializeField] private string personDefaultText = default;
    [TextArea(2, 5)] [SerializeField] private string lastWords = default;
    [TextArea(2, 5)] [SerializeField] private string condemnedMessage = default;
    [TextArea(2, 5)] [SerializeField] private string rewardMessage = default;

    public PersonState PersonState { get; set; }

    public string PersonName { get => personName; set => personName = value; }
    public bool IsGuilty { get => isGuilty; set => isGuilty = value; }
    public string CondemnReaction { get => condemnReaction; set => condemnReaction = value; }
    public string RewardRaction { get => rewardReaction; set => rewardReaction = value; }
    public string DefaultText { get => defaultText; set => defaultText = value; }
    public string LastWords { get => lastWords; set => lastWords = value; }
    public string CondemnedMessage { get => condemnedMessage; set => condemnedMessage = value; }
    public string RewardMessage { get => rewardMessage; set => rewardMessage = value; }
    public string PersonDefaultText { get => personDefaultText; set => personDefaultText = value; }
    public bool IsGuiltyPlayer { get; set; } = false;

    public Person()
    {
        PersonState = new PersonState();
    }
}

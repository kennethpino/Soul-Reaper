using System.Collections.Generic;
using UnityEngine;

class DialogEngine
{
    public bool DialogOn { get; set; } = false;
    public Dictionary<int, NPCDialogChoicesEnum> DialogChoices { get; set; }

    private const string dashSeparation = " - ";

    //Creates an options menu for the dialog scenes
    public string GetDialogOptions(Person person)
    {
        Debug.Log("***UpdateDialogOptions***");

        //[POSSIBLE CHOICES] go back, ask for last words, reap soul, commend, condemn
        //[CONDITIONS] IsBeingReaped, IsReaped, AskedForLastWords

        Debug.Log($"The person is: {person.PersonName}\n");
        Debug.Log($"being reaped: {person.PersonState.IsBeingReaped}\n");
        Debug.Log($"is reaped: {person.PersonState.IsReaped}\n");
        Debug.Log($"asked for words: {person.PersonState.LastWordsShared}\n");

        List<NPCDialogChoicesEnum> optList = new List<NPCDialogChoicesEnum>();
        optList.Clear();

        bool lastWordsShared = person.PersonState.LastWordsShared;
        bool isBeingReaped = person.PersonState.IsBeingReaped;
        bool isReaped = person.PersonState.IsReaped;

        optList.Add(NPCDialogChoicesEnum.GoBack);

        if (!lastWordsShared && !isBeingReaped && !isReaped)
        {
            optList.Add(NPCDialogChoicesEnum.LastWords);
        }

        if (lastWordsShared && !isBeingReaped && !isReaped)
        {
            optList.Add(NPCDialogChoicesEnum.ReapSoul);
        }

        if (lastWordsShared && isBeingReaped && !isReaped)
        {
            optList.Add(NPCDialogChoicesEnum.Commend);
            optList.Add(NPCDialogChoicesEnum.Condemn);
        }
        return FormatIndexedMenu(GenerateIndexedMenu(optList));
    }

    //Creates a list of items using a list of available dialog choices 
    private Dictionary<int, NPCDialogChoicesEnum> GenerateIndexedMenu(List<NPCDialogChoicesEnum> optList)
    {
        Debug.Log("***GenerateIndexedMenu***");

        DialogChoices = new Dictionary<int, NPCDialogChoicesEnum>();
        optList.ForEach(x => { DialogChoices.Add(optList.IndexOf(x) + 1, x); });
        return DialogChoices;
    }

    private string FormatIndexedMenu(Dictionary<int, NPCDialogChoicesEnum> dialogChoices)
    {
        Debug.Log("***FormatIndexedMenu***");

        string items = null;

        foreach (KeyValuePair<int, NPCDialogChoicesEnum> keyValuePair in dialogChoices)
        {
            int key = keyValuePair.Key;
            NPCDialogChoicesEnum value = keyValuePair.Value;

            switch (value)
            {
                case NPCDialogChoicesEnum.GoBack:
                case NPCDialogChoicesEnum.LastWords:
                    items = FormatItemString(items, key, "white", value);
                    break;
                case NPCDialogChoicesEnum.ReapSoul:
                case NPCDialogChoicesEnum.Condemn:
                    items = FormatItemString(items, key, "red", value);
                    break;
                case NPCDialogChoicesEnum.Commend:
                    items = FormatItemString(items, key, "green", value);
                    break;
            }
            items += "\n";
        }
        return items;
    }

    private static string FormatItemString(string items, int key, string color, NPCDialogChoicesEnum value)
    {
        Debug.Log("***FormatItemString***");

        items = string.Concat(
            items,
            PrettyString.FormatString(key.ToString(), true, color),
            dashSeparation,
            value.GetText());

        return items;
    }
}

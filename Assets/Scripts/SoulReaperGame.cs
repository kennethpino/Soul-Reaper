using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SoulReaperGame : MonoBehaviour
{
    private const string gameTextPath = @"\gameText\gameText.json";

    [SerializeField] Text screenTitle = default;
    [SerializeField] Text gameStory = default;
    [SerializeField] State gameState = default;
    [SerializeField] Image splashScreen = default;
    [SerializeField] Text optionsMenu = default;
    [SerializeField] Image storyPhoto = default;
    [SerializeField] Text myIntro = default;
    [SerializeField] Image map = default;

    State state;
    Player player;
    DialogEngine dialogEngine;
    int key;
    Person person;
    private bool gameStart = true;

    void Start()
    {
        Debug.Log("***************************Start***************************");

        state = gameState;
        person = ScriptableObject.CreateInstance<Person>();
        //File.WriteAllText(Application.dataPath + "/person.json", JsonUtility.ToJson(person, true));
        player = ScriptableObject.CreateInstance<Player>();
        dialogEngine = new DialogEngine();
        key = 1;
        map.enabled = false;
        UpdateState();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("***************************Runing Update***************************");
            try
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    Debug.Log($"[Debug] Input.inputString is: {Input.inputString}");
                    ToggleMap();
                }
                else
                {
                    GetUserKey();
                    Debug.Log($"[Debug] User key is: {key}");
                    DisableIntroUI();

                    //Nagigate
                    if (key <= state.GetStates().Length && !dialogEngine.DialogOn)
                    {
                        UpdateState();
                        UpdateOptionsMenuUI(GenerateNagivationOptionsMenu());
                    }
                    //Go back to navigating
                    else if (dialogEngine.DialogOn && key == 1)
                    {
                        dialogEngine.DialogOn = false;
                        UpdateState();
                        UpdateOptionsMenuUI(GenerateNagivationOptionsMenu());
                    }
                    //Converse
                    else
                    {
                        //Actions when dialog is enabled for the first time
                        if (!dialogEngine.DialogOn)
                        {
                            person = state.GetPeople()[key - state.GetStates().Length - 1];
                            UpdateScreenTitle($"talking to {person.PersonName}");

                            //Person is dead
                            if (person.PersonState.IsReaped)
                            {
                                UpdateGameStoryUI(person.PersonState.IsCondemned ? person.CondemnedMessage : person.CommendedMessage);
                            }
                            //Person is alive
                            else
                            {
                                UpdateGameStoryUI(person.DefaultText);
                            }
                            dialogEngine.DialogOn = true;
                        }
                        //Actions for an ongoing dialog
                        else
                        {
                            UpdateGameStoryUI(ProcessNPCDialogChoice());
                        }
                        UpdateOptionsMenuUI(dialogEngine.GetDialogOptions(person));
                    }
                }
            }
            catch (Exception)
            {
                Debug.Log("Something went wrong. Could be an invalid key or who knows what else." +
                    "\n Try with another key or restart the game.");
            }
        }
    }

    //Displays a map of the precinct
    private void ToggleMap()
    {
        map.enabled = !map.enabled;
    }

    private void DisableIntroUI()
    {
        if (gameStart)
        {
            splashScreen.enabled = false;
            myIntro.enabled = false;
            gameStart = false;
        }
    }

    //This will exectute a choice from within the dialog option menu
    //via the user imput key
    public string ProcessNPCDialogChoice()
    {
        Debug.Log("***ProcessNPCDialogChoice***");

        switch (dialogEngine.DialogChoices[key])
        {
            case NPCDialogChoicesEnum.LastWords:
                return player.GetLastWords(person);
            case NPCDialogChoicesEnum.ReapSoul:
                return player.ReapSoul(person);
            case NPCDialogChoicesEnum.Commend:
                return player.Commend(person);
            case NPCDialogChoicesEnum.Condemn:
                return player.Condemn(person);
            default:
                return null;
        }
    }

    //Obtain a numeric value for the user input key
    private void GetUserKey()
    {
        Debug.Log("***GetUserKey***");
        key = Int32.Parse(Input.inputString);
    }

    //Updates the game optionsMenu text field with new navigation values
    private void UpdateOptionsMenuUI(string items)
    {
        Debug.Log("***UpdateOptionsMenuUI***");

        optionsMenu.text = items;
    }

    //Shows an options menu for the navigation states
    private string GenerateNagivationOptionsMenu()
    {
        Debug.Log("***GenerateNagivationOptionsMenu***");

        int optionsLength = state.GetStates().Length + state.GetPeople().Length;
        string items = null;

        for (int i = 0; i < optionsLength;)
        {
            PopulateOptions(state.GetStates(), ref items, ref i);
            PopulateOptions(state.GetPeople(), ref items, ref i);
        }
        return items;
    }

    //Generates the player options for the menu
    private void PopulateOptions<T>(T[] options, ref string items, ref int i)
    {
        Debug.Log("***PopulateOptions***");

        for (int j = 0; j < options.Length; j++, i++)
        {
            string option, action, color;

            if (options.GetType().GetElementType() == typeof(State))
            {
                //Debug.Log("***PopulateOptions State***");
                action = " - Go to ";
                option = ((State)(object)options[j]).GetLocationName();
                color = "white";
            }
            else
            {
                //Debug.Log("***PopulateOptions Person***");
                action = " - Talk to ";
                option = ((Person)(object)options[j]).PersonName;
                color = "orange";
            }
            items = string.Concat(items, "<color=" + color + "><b>" + (i + 1) + "</b>" + "</color>", action, option + "\n");
        }
    }

    //Displays a photo of the location where the user is
    private void UpdateStoryPhoto()
    {
        Debug.Log("***UpdateStoryPhoto***");

        storyPhoto.sprite = Resources.Load<Sprite>("Images/" + state.GetLocationName());
    }

    //Displays the game text of each state
    private void UpdateStateText(StateDataCollection sdc)
    {
        Debug.Log("***UpdateStateText***");

        bool nameFound = false;
        for (int i = 0; i < sdc.stateDatas.Count | !nameFound; i++)
        {
            if (state.name == sdc.stateDatas[i].stateName)
            {
                UpdateGameStoryUI(sdc.stateDatas[i].stateText);
                nameFound = true;
            }
        }
    }

    //Displays the game text of each state
    private void UpdateGameStoryUI(string text)
    {
        Debug.Log("***UpdateGameStoryUI***");

        gameStory.text = text;
    }

    //Updates the current state
    private void UpdateState()
    {
        Debug.Log("***UpdateState***");

        state = state.GetStates()[key - 1];
        UpdateScreenTitle(state.GetLocationName());
        UpdateStateText(LoadStateDataCollection(Application.dataPath + gameTextPath));
        UpdateStoryPhoto();
    }

    //Updates the screen title/location
    private void UpdateScreenTitle(string screenTitle)
    {
        Debug.Log("***UpdateScreenTitle***");

        this.screenTitle.text = screenTitle;
    }

    //Loads the gameText data from a JSON resource
    private StateDataCollection LoadStateDataCollection(string path)
    {
        Debug.Log("***LoadStateDataCollection***");

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<StateDataCollection>(json);
    }
}

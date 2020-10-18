using System;
using UnityEngine;
using UnityEngine.UI;

public class SoulReaperGame : MonoBehaviour
{
    private const string gameTextPath = @"\text\gameText";
    private const float xPosition = 1f;
    private const float yPosition = 4.25f;
    [SerializeField] Text screenTitle = default;
    [SerializeField] Text gameStory = default;
    [SerializeField] State gameState = default;
    [SerializeField] Image splashScreen = default;
    [SerializeField] Text optionsMenu = default;
    [SerializeField] Image storyPhoto = default;
    [SerializeField] Text myIntro = default;
    [SerializeField] Image map = default;
    [SerializeField] GameObject soulPrefab = default;
    [SerializeField] GameObject soulPrefabGrayed = default;
    [SerializeField] GameObject progressContainer = default;
    [SerializeField] Person[] people = default;
    [SerializeField] Camera gameOverCamera = default;
    [SerializeField] GameObject gameOverGO = default;

    State state;
    State lastState;
    Player player;
    DialogEngine dialogEngine;
    int key;
    Person person;
    GameOver gOver;
    private bool isNewGame = true;
    private bool isGameOver = false;

    public Player GetPlayer()
    {
        return player;
    }

    public Person[] People { get => people; set => people = value; }

    void Start()
    {
        Debug.Log("***************************Start***************************");

        state = gameState;
        person = ScriptableObject.CreateInstance<Person>();
        player = ScriptableObject.CreateInstance<Player>();
        dialogEngine = new DialogEngine();
        PlaceProcessBGUI();
        progressContainer.SetActive(false);
        key = 1;
        map.enabled = false;
        gameOverCamera.enabled = false;
        gOver = gameOverGO.GetComponent<GameOver>();
        TextAsset textAsset = Resources.Load<TextAsset>("text/gameText");
        UpdateState();
    }

    private void PlaceProcessBGUI()
    {
        float xPosition = SoulReaperGame.xPosition;
        var transform = progressContainer.transform;
        var progressContainerZPos = transform.transform.position.z;

        for (int i = 0; i < 10; i++, xPosition += 0.75f)
        {
            GameObject go = Instantiate(soulPrefabGrayed, new Vector3(xPosition, yPosition, progressContainerZPos - 0.05f), Quaternion.identity, transform);
            go.transform.localScale = new Vector3(75f, 75f);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("***************************Runing Update***************************");
                try
                {
                    //Toggle map
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
                            state = lastState;
                            UpdateUI();
                            UpdateOptionsMenuUI(GenerateNagivationOptionsMenu());
                        }
                        //Converse
                        else
                        {
                            //Actions when dialog is enabled for the first time
                            if (!dialogEngine.DialogOn)
                            {
                                person = state.GetPeople()[key - state.GetStates().Length - 1];
                                UpdateScreenTitle($"Talking to {person.PersonName}");
                                lastState = state;

                                //Person is dead
                                if (person.PersonState.IsReaped)
                                {
                                    UpdateGameStoryUI(person.PersonState.IsCondemned ? person.CondemnedMessage : person.RewardMessage);
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
                                if (player.SoulsReaped > 0)
                                {
                                    UpdateProgressUI(player.SoulsReaped);
                                }
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

        if (player.SoulsReaped == 10)
        {
            isGameOver = true;
            gOver.Run();
            gOver.DisplayResults();
            gameOverCamera.enabled = true;
            GetComponent<SoulReaperGame>().enabled = false;
        }
    }

    private void UpdateProgressUI(int soulsReaped)
    {
        float xPosition = SoulReaperGame.xPosition;
        var transform = progressContainer.transform;
        var progressContainerZPos = transform.transform.position.z;

        for (int i = 0; i < soulsReaped; i++, xPosition += 0.75f)
        {
            GameObject go = Instantiate(soulPrefab, new Vector3(xPosition, yPosition, progressContainerZPos - 0.06f), Quaternion.identity, transform);
            go.transform.localScale = new Vector3(75f, 75f);
        }
    }



    //Displays a map of the precinct
    private void ToggleMap()
    {
        map.enabled = !map.enabled;
    }

    private void DisableIntroUI()
    {
        if (isNewGame)
        {
            splashScreen.enabled = false;
            myIntro.enabled = false;
            isNewGame = false;
            progressContainer.SetActive(true);
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
            case NPCDialogChoicesEnum.Reward:
                return player.Reward(person);
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
                if (((Person)(object)options[j]).PersonState.IsReaped)
                {
                    action = " - How is ";
                }
                else
                {
                    action = " - Talk to ";
                }
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
    private string FindStateText(StateDataCollection sdc)
    {
        Debug.Log("***UpdateStateText***");

        string text = null;

        bool nameFound = false;
        for (int i = 0; i < sdc.stateDatas.Count | !nameFound; i++)
        {
            if (state.name == sdc.stateDatas[i].stateName)
            {
                text = sdc.stateDatas[i].stateText;
                nameFound = true;
            }
        }

        return text;
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
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateScreenTitle(state.GetLocationName());
        string stateText = FindStateText(LoadStateDataCollection(gameTextPath));
        string personText = null;
        UpdateStoryPhoto();

        try
        {
            personText = GetPersonDefaultText();
        }
        catch (Exception e)
        {
            Debug.Log($"No people were found. Error: {e.Message}");
        }

        UpdateGameStoryUI($"{stateText} {personText}");
    }

    private string GetPersonDefaultText()
    {
        Person[] people = state.GetPeople();
        string personText = null;
        string personTextDead = null;
        if (people.Length > 0)
        {
            foreach (Person person in people)
            {
                if (!person.PersonState.IsReaped)
                {
                    personText = string.Concat(personText, " ", person.PersonDefaultText);
                }
                else
                {
                    personTextDead = string.Concat(personTextDead, " ", person.PersonName, " has moved on.");
                }
            }
        }
        return string.Concat(personText, personTextDead);
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

        TextAsset textAsset = Resources.Load<TextAsset>("text/gameText");
        string json = textAsset.text;
        return JsonUtility.FromJson<StateDataCollection>(json);
    }
}

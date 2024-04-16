using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject computerPanel;

    [SerializeField]
    private TMP_Text computerNameTMP;

    [SerializeField]
    private TMP_Text computerOwnerTMP;

    private TextMeshProUGUI panelText;

    private TextMeshProUGUI panelButtonText;

    public GameObject numPad;
    public TMP_InputField inputField;

    public bool isPlaying;

    public GameObject buttonPrefab;
    public RectTransform buttonContainer;

    private List<GameObject> instantiatedButtons = new List<GameObject>();
    
    [SerializeField]
    // spacing in between each button
    private float verticalSpacing = 10f;

    [SerializeField]
    private float horizontalSpacing = 30f;

    public float buttonsPerColumn;

    public int buttonsPerRow;
    

    public PlayerController playerController;
    public AudioManager audioManager;

    public static PanelManager Instance;

    void Awake(){
        Instance = this;
    }

    void Start(){
        isPlaying = false;
        ComputerPanelSetup();
        numPad.SetActive(false);

        audioManager = AudioManager.Instance;
    }

    void ComputerPanelSetup(){
        computerPanel.SetActive(false);
    }

    void Update(){
        if (!isPlaying){
            return;
        }
    }

    public void EnterNumPad(){
        isPlaying = true;
        numPad.SetActive(true);
        playerController.FreezePlayer();
        playerController.UnFreezeCursor();
    }

    public bool CheckPassword(int password){
        string enteredText = inputField.text;

        bool passwordMatch = enteredText == password.ToString();

        return passwordMatch;
    }   

    public void EnterComputerPanel(string computerName, string ownerName, List<ButtonInfo> buttonInfoList){
        
        isPlaying = true;
        computerPanel.SetActive(true);
        playerController.FreezePlayer();
        playerController.UnFreezeCursor();

        computerNameTMP.text = computerName;
        computerOwnerTMP.text = ownerName;

        int column = 0;
        int row = 0;
        
        for (int i = 0; i < buttonInfoList.Count; i++){
            // for every name, instantiate a new button
            GameObject buttonGameObject = Instantiate(buttonPrefab, buttonContainer);
            Button button = buttonGameObject.GetComponent<Button>();

            instantiatedButtons.Add(buttonGameObject);

            // get the text of the button
            TMP_Text buttonText = buttonGameObject.GetComponentInChildren<TMP_Text>();
            buttonText.text = buttonInfoList[i].buttonText;

            float xPosition = column * (buttonContainer.rect.width / buttonsPerRow) + column * horizontalSpacing;
            float yPosition = -row * (verticalSpacing + buttonContainer.rect.height / buttonsPerColumn);

            buttonGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, yPosition);

            // handle button clicks
            int buttonIndex = i;
            button.onClick.AddListener(() => OnButtonClicked(buttonInfoList[buttonIndex].audioSourceName));

            row++;
            if (row >= buttonsPerColumn){
                row = 0;
                column++;
            }
        }
    }

    void OnButtonClicked(string audioSourceName){
        AudioClip audioClip = Resources.Load<AudioClip>(audioSourceName);

        audioManager.Play(audioClip);
    }

    public void ExitPanel(){
        foreach (GameObject button in instantiatedButtons){
            Destroy(button);
        }

        instantiatedButtons.Clear();

        isPlaying = false;
        playerController.UnFreezePlayer();
        playerController.FreezeCursor();
        computerPanel.SetActive(false);
    }

    public void ExitNumPad(){
        isPlaying = false;
        playerController.UnFreezePlayer();
        playerController.FreezeCursor();
        numPad.SetActive(false);
    }
}
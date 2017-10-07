using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MUDClienteUI : MonoBehaviour {
    [Header("Connection:")]
    [SerializeField] private InputField addressInput;
    [SerializeField] private InputField portInput;
    [SerializeField] private InputField namePlayerInput;
    [SerializeField] private Text       connectMessage;
    [SerializeField] private Text       buttonConnectName;
    [Header("Command line:")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Text       logText;
    [SerializeField] private InputField command;

    public string Address { get { return addressInput.text; } set { addressInput.text = value; } }
    public int Port { get { return int.Parse(portInput.text); } set { portInput.text = value.ToString(); } }
    public string NamePlayer { get { return namePlayerInput.text; } }
    public string ConnectMessage { get { return connectMessage.text; } set { connectMessage.text = value; } }
    public string ButtonConnectName { get { return buttonConnectName.text; } set { buttonConnectName.text = value; } }
    public string LogText { get { return logText.text; } set { logText.text = value; } }
    public InputField Command { get { return command; } }


    public void ReadOnly(bool value)
    {
        addressInput.readOnly = value;
        portInput.readOnly = value;
        namePlayerInput.readOnly = value;
        command.readOnly = !value;
    }

    public void AddMessage(string msg)
    {
        logText.text += msg + "\n";
    }

    public void Roll()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    // Use this for initialization
    void Start()
    {
        //logText = GetComponentInChildren<Text>();
        //command = GetComponentInChildren<InputField>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}

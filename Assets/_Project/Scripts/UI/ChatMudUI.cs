using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class ChatMudUI : MonoBehaviour {

    [SerializeField] private Text logText;
    [SerializeField] private InputField command;

    public string LogText { get { return logText.text; } set { logText.text = value; } }
    public InputField Command { get { return command; } }

    // Use this for initialization
    void Start () {
        logText = GetComponentInChildren<Text>();
        command = GetComponentInChildren<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}

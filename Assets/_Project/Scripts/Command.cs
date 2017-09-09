using UnityEngine;
using System.Collections;

public class Command : MonoBehaviour {

    [SerializeField] private ChatMudUI mudUI;
    [SerializeField] private string namePlayer = "Player";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GetCommand()
    {
        mudUI.LogText += "["+namePlayer+"]: " + mudUI.Command.text + "\n";
        mudUI.Command.text = "";
    }
}

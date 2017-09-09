using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectionMudUI : MonoBehaviour {

    [SerializeField] private InputField addressInput;
    [SerializeField] private InputField portInput;
    [SerializeField] private Text message;


    public string Address { get { return addressInput.text; } set { addressInput.text = value; } }
    public int Port { get { return int.Parse(portInput.text); } set { portInput.text = value.ToString(); } }
    public string Message { get { return message.text; } set { message.text = value; } }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

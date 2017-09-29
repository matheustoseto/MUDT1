using UnityEngine;
using UnityEngine.UI;
using System;

public class MUDServidorUI : MonoBehaviour {

    [SerializeField] private InputField addressInput;
    [SerializeField] private InputField portInput;
    [SerializeField] private Text connectMessage;
    [SerializeField] private Text messageArea;
    [SerializeField] private Dropdown changeRoom;
    [SerializeField] private Dropdown itens;
    [SerializeField] private Text buttonConnectName;
    [SerializeField] private Text qtdConnections;


    public string Address { get { return addressInput.text; } set { addressInput.text = value; } }
    public int Port { get { return int.Parse(portInput.text); } set { portInput.text = value.ToString(); } }
    public string ConnectMessage { get { return connectMessage.text; } set { connectMessage.text = value; } }
    public string MessageArea { get { return messageArea.text; } set { messageArea.text = value; } }
    public int ChangeRoom { get { return changeRoom.value; } }
    public int Item { get { return itens.value; } }
    public int QtdConnections { get { return int.Parse(qtdConnections.text); } set { qtdConnections.text = "Conexões: " + value.ToString(); } }

    public string ButtonConnectName { get { return buttonConnectName.text; } set { buttonConnectName.text = value; } }

    // Use this for initialization
    void Start () {
        StartDropdownItens();
    }
	
    void StartDropdownItens()
    {
        itens.ClearOptions();
        Array tipoObjeto = Enum.GetValues(typeof(TipoObjeto));
        for (int i = 0; i < tipoObjeto.Length; i++)
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = tipoObjeto.GetValue(i).ToString();
            itens.options.Add(op);
        }
        itens.value = 1;
        itens.value = 0;
    }

	// Update is called once per frame
	void Update () {
	
	}
}

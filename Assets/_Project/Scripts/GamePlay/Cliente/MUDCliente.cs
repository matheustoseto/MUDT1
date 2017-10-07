using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MUDCliente : MonoBehaviour
{

    public string connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    public string playerName;
    private string idPlayer;
    private string idSala;
    private NetworkView netWorkView;
    public string textChat;
    private bool setPlayerPrefs = false;
    //private List<string> chatEntries = new List<string>();


    // UI Componentes
    public MUDClienteUI clienteUI;
    private bool isConectado = false;

    // Use this for initialization
    void Start()
    {
        netWorkView = GetComponent<NetworkView>();
        clienteUI.Address = connectToIP;
        clienteUI.Port = connectPort;
        clienteUI.LogText = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!isConectado)
        {

            if (Network.peerType == NetworkPeerType.Connecting)
            {
                clienteUI.ConnectMessage = "Cliente: Conectando...";

            }
            else if (Network.peerType == NetworkPeerType.Client)
            {
                isConectado = true;
                clienteUI.ConnectMessage = "Cliente: Conectado.";
            }
        }
        else
        {
            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                DesconectarClienteMUD(true);
                clienteUI.AddMessage("O servidor desconectou!");
            }
        }


        //textChat = clienteUI.LogText;
        //textChat = GUILayout.TextField(textChat, GUILayout.MinWidth(100));

        //foreach (string tx in chatEntries)
        //    clienteUI.LogText = tx;

        //if (Input.GetKeyDown(KeyCode.Return) && textChat.Length > 0)
        //    DigitarTexto(textChat);


    }



    public void ConectarClienteMUD()
    {
        if (!isConectado)
        {
            if (Network.peerType == NetworkPeerType.Disconnected)
            {

                //We are currently disconnected: Not a client or host
                //clienteUI.AddMessage("Cliente desconectado.");
                if (clienteUI.NamePlayer != "")
                {
                    connectToIP = clienteUI.Address;
                    connectPort = clienteUI.Port;
                    playerName = clienteUI.NamePlayer;


                    clienteUI.AddMessage("Conectando ao servidor...");
                    //Connect to the "connectToIP" and "connectPort" as entered via the GUI
                    //Ignore the NAT for now
                    Network.useNat = false;
                    Network.Connect(connectToIP, connectPort);
                    clienteUI.ButtonConnectName = "DESCONECTAR";
                    clienteUI.ReadOnly(true);

                }
                else
                {
                    clienteUI.AddMessage("Você não preencheu o nome!");
                }
            }
        }
        else
        {
            DesconectarClienteMUD(false);
            clienteUI.AddMessage("Desconectado!");
        }
    }

    public void DesconectarClienteMUD(bool servidorDesconectou)
    {
        clienteUI.ConnectMessage = "Cliente: Desconectado";
        clienteUI.ButtonConnectName = "Conectar ao\n"
                                     + "SERVIDOR";
        if (!servidorDesconectou)
            netWorkView.RPC("ShowText", RPCMode.All, "", "O jogador " + playerName + " desconectou!", idSala);

        Network.Disconnect(200);
        isConectado = false;
        clienteUI.LogText = "";
        clienteUI.ReadOnly(false);
    }

    /*
    public void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            //GUILayout.Label("Cliente desconectado.");


            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
            connectPort = int.Parse(GUILayout.TextField(connectPort.ToString()));
            playerName = GUILayout.TextField(playerName, GUILayout.MinWidth(100));

            GUILayout.BeginVertical();
            if (GUILayout.Button("Connectar no servidor"))
            {
                //Connect to the "connectToIP" and "connectPort" as entered via the GUI
                //Ignore the NAT for now
                Network.useNat = false;
                Network.Connect(connectToIP, connectPort);
            }

            GUILayout.EndVertical();
        }
        else
        {
            if (Network.peerType == NetworkPeerType.Connecting)
            {

                GUILayout.Label("Conectando...");

            } 
            else if (Network.peerType == NetworkPeerType.Client)
            {
                GUILayout.Label("Cliente Connectado com sucesso!");
                //GUILayout.Label("idPlayer: " + idPlayer);
				GUILayout.Label("NomeSala: " + idSala);
                GUILayout.Label("NomeJogador: " + playerName);
                //GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));

                if (GUILayout.Button("Desconectar")) {
                    netWorkView.RPC("ShowText", RPCMode.All, "", "O jogador " + playerName + " desconectou!");
                    Network.Disconnect(200);       
                }

                if (GUILayout.Button("Clear"))
                    chatEntries.Clear();

                textChat = GUILayout.TextField(textChat, GUILayout.MinWidth(100));

                foreach (string tx in chatEntries)
                    GUILayout.Label(tx);

            }


        }
    }*/

    public void ReceberComando()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && clienteUI.Command.text != "")
        {
            DigitarTexto(clienteUI.Command.text);
            clienteUI.Command.text = "";
            clienteUI.Command.Select();
            clienteUI.Command.ActivateInputField();
        }
    }

    void DigitarTexto(string texto)
    {
        ShowText("", texto, idSala);
        netWorkView.RPC("Comando", RPCMode.Server, idPlayer, texto);
        textChat = "";
    }

    void OnConnectedToServer()
    {
        textChat = "";
        //chatEntries.Clear();
        clienteUI.LogText = "";
        netWorkView.RPC("ConectarCliente", RPCMode.Server, playerName);
    }

    [RPC]
    void SetPlayerPref(string playerName, string idPlayer, string idSala)
    {
        if(this.playerName == playerName){
			this.idPlayer = idPlayer;
			this.idSala = idSala;

            if (!setPlayerPrefs)
            {
                clienteUI.AddMessage("Cliente Connectado com sucesso!");
                clienteUI.AddMessage("NomeSala: " + idSala);
                clienteUI.AddMessage("NomeJogador: " + playerName);
                clienteUI.AddMessage("Ping: " + Network.GetAveragePing(Network.connections[0]));
                setPlayerPrefs = true;
            }
        }        
    }

    [RPC]
    void ShowText(string idPlayer, string texto, string idSala)
    {
        if (this.idSala == idSala)
        {
            if (idPlayer != "" && idPlayer != null)
            {
                //chatEntries.Add("[" + playerName + "] " + texto);
                clienteUI.AddMessage("[" + playerName + "] " + texto);
            }
            else
            {
                //chatEntries.Add(texto);
                clienteUI.AddMessage(texto);
            }
        }
    }

    [RPC]
    void Sendmsg(string idPlayer, string texto, string idSala)
    {
        if (this.idPlayer == idPlayer)
            clienteUI.AddMessage(texto);
            //ShowText("", texto, idSala);
    }
	
	[RPC]
    void NotificaOutros(string idPlayer, string idSala, string texto)
    {
        if (this.idPlayer != idPlayer)
            ShowText("", texto, idSala);
    }

    //Server function
    //As funções do servidor devem ser declaradas no cliente porém vazias
    [RPC]
    void ConectarCliente(string name, NetworkMessageInfo info){}
    [RPC]
    void Comando(string idPlayer, string texto){}

}

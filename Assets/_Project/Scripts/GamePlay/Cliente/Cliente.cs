using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cliente : MonoBehaviour
{

    public string connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    public string playerName;
    private string idPlayer;
    private NetworkView netWorkView;
    public string textChat;
    private List<string> chatEntries = new List<string>();

    // Use this for initialization
    void Start()
    {
        netWorkView = GetComponent<NetworkView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && textChat.Length > 0)
            DigitarTexto(textChat);
    }

    public void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Cliente desconectado.");

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
                GUILayout.Label("idPlayer: " + idPlayer);
                GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));

                if (GUILayout.Button("Desconectar"))
                    Network.Disconnect(200);

                textChat = GUILayout.TextField(textChat, GUILayout.MinWidth(100));

                foreach (string tx in chatEntries)
                    GUILayout.Label(tx);

            }


        }
    }

    void DigitarTexto(string texto)
    {
        ShowText(playerName , texto);
        netWorkView.RPC("Comando", RPCMode.Server, idPlayer, texto);
    }

    void OnConnectedToServer()
    {
        textChat = "";
        chatEntries.Clear();
        netWorkView.RPC("ConectarCliente", RPCMode.Server, playerName);
    }

    [RPC]
    void SetIdPlayer(string idPlayer)
    {
        this.idPlayer = idPlayer;
    }

    [RPC]
    void ShowText(string nomePlayer, string texto)
    {
        if(nomePlayer != "" && nomePlayer != null)
            chatEntries.Add("[" + nomePlayer + "] " + texto);
        else
            chatEntries.Add(texto);

        textChat = "";
    }

    //Server function
    //As funções do servidor devem ser declaradas no cliente porém vazias
    [RPC]
    void ConectarCliente(string name, NetworkMessageInfo info){}
    [RPC]
    void Comando(string idPlayer, string texto){}

}

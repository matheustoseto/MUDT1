using UnityEngine;
using System.Collections;

public class ConnectMUD : MonoBehaviour
{

    [SerializeField]
    private ConnectionMudUI connectMudUI;

    [SerializeField] private MUDClienteUI chatMudUI;

    [SerializeField]
    private string connectToIP = "127.0.0.1";
    [SerializeField]
    private int connectPort = 25001;

    // Use this for initialization
    void Start()
    {
        connectMudUI.Address = connectToIP;
        connectMudUI.Port = connectPort;
        connectMudUI.Message = "Connection status: Disconnected";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartServer()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            connectToIP = connectMudUI.Address;
            connectPort = connectMudUI.Port;

            //Start a server for 32 clients using the "connectPort" given via the GUI
            //Ignore the nat for now	
            Network.useNat = false;
            Network.InitializeServer(32, connectPort);
        }
        else
        {
            if (Network.peerType == NetworkPeerType.Connecting)
            {
                connectMudUI.Message = "Connection status: Connecting";
                chatMudUI.LogText += "Connection status: Connecting\n";
            }

            else if (Network.peerType == NetworkPeerType.Server)
            {

                //We've got a connection(s)!
                connectMudUI.Message = "Connection status: Server!";
                connectMudUI.Message = "Connections: " + Network.connections.Length;

                chatMudUI.LogText += "Connection status: Server!\n";
                chatMudUI.LogText += "Connections: " + Network.connections.Length+"\n";

                if (Network.connections.Length >= 1)
                {
                    connectMudUI.Message = "Ping to first player: " + Network.GetAveragePing(Network.connections[0]) + "\n";
                    chatMudUI.LogText += "Ping to first player: " + Network.GetAveragePing(Network.connections[0]) + "\n";
                }

            }

        }
    }

    public void ConnectClient()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            connectToIP = connectMudUI.Address;
            connectPort = connectMudUI.Port;

            //Connect to the "connectToIP" and "connectPort" as entered via the GUI
            //Ignore the NAT for now
            Network.useNat = false;
            Network.Connect(connectToIP, connectPort);
        }
        else
        {
            //We've got a connection(s)!
            if (Network.peerType == NetworkPeerType.Connecting)
                GUILayout.Label("Connection status: Connecting");
            else if (Network.peerType == NetworkPeerType.Client)
            {
                GUILayout.Label("Connection status: Client!");
                GUILayout.Label("Ping to server: " + Network.GetAveragePing(Network.connections[0]));
            }
        }
    }

    public void Disconnect()
    {
        if (Network.peerType != NetworkPeerType.Disconnected)
            Network.Disconnect(200);
    }

}

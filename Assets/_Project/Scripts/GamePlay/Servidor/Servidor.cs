using UnityEngine;
using System.Collections;

public class Servidor : MonoBehaviour
{

    public string connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    private NetworkView netWorkView;
    private Repositorio repositorio = new Repositorio();
    private Command comandos = new Command();

    // Use this for initialization
    void Start()
    {
        netWorkView = GetComponent<NetworkView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            GUILayout.Label("Servidor Desativado.");

            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
            connectPort = int.Parse(GUILayout.TextField(connectPort.ToString()));

            GUILayout.BeginVertical();

            if (GUILayout.Button("Inicializar Servidor"))
            {
                Network.InitializeServer(32, connectPort, false);
            }
            GUILayout.EndVertical();
        }
        else
        {
            if (Network.peerType == NetworkPeerType.Connecting)
            {

                GUILayout.Label("Conectando...");

            }
            else if (Network.peerType == NetworkPeerType.Server)
            {

                GUILayout.Label("Servidor inicializado com sucesso!");
                GUILayout.Label("Conexões: " + Network.connections.Length);
            }

            if (GUILayout.Button("Desconectar"))
            {
                Network.Disconnect(200);
            }
        }
    }

    private void OnConnectedToServer()
    {
        Debug.Log("This CLIENT has connected to a server");
    }

    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        Debug.Log("This SERVER OR CLIENT has disconnected from a server");
    }

    private void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Could not connect to server: " + error);
    }

    private void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from: " + player.ipAddress + ":" + player.port);
    }

    private void OnServerInitialized()
    {
        Debug.Log("Server initialized and ready");
    }

    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Player disconnected from: " + player.ipAddress + ":" + player.port);
    }

    private void OnFailedToConnectToMasterServer(NetworkConnectionError info)
    {
        Debug.Log("Could not connect to master server: " + info);
    }

    private void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        Debug.Log("New object instantiated by " + info.sender);
    }

    private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Debug.Log("stream: " + stream + " info : " + info.sender);
    }

    [RPC]
    void ConectarCliente(string name, NetworkMessageInfo info)
    {
        Debug.Log("name: " + name);
        Debug.Log("networkView: " + info.networkView);

        Player player = new Player();
        player.idPlayer = "id_" + name;
        player.nome = name;
        player.networkPlayer = info.sender;

        repositorio.players.Add(player);

        NotificaTodosPlayers("", "Jogador " + name + " se connectou!");
        info.networkView.RPC("SetIdPlayer", RPCMode.All, name, player.idPlayer);
    }

    [RPC]
    void Comando(string idPlayer, string texto)
    {
        string retorno = comandos.falarChat(texto);
        notificaPlayer(idPlayer, retorno);
    }

    void NotificaTodosPlayers(string nomePlayer, string texto)
    {
        netWorkView.RPC("ShowText", RPCMode.All, nomePlayer, texto);
    }

    void notificaPlayer(string nomePlayer, string texto)
    {
        netWorkView.RPC("Sendmsg", RPCMode.All, nomePlayer, texto);
    }

    //Client function
    //As funções do cliente devem ser declaradas no servidor porém vazias
    [RPC]
    void SetIdPlayer(string playerName, string idPlayer) {}
    [RPC]
    void ShowText(string nomePlayer, string texto) {}
    [RPC]
    void Sendmsg(string idPlayer, string texto) {}
}

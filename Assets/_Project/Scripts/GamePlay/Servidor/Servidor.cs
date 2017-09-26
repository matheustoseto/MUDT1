using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public ChatEntries {
	public IdSalas idSala;
	public List<string> chat = new List<string>();
}

public class Servidor : MonoBehaviour
{

    public string connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    private NetworkView netWorkView;
    private Repositorio repositorio = new Repositorio();
    private Command comandos = new Command();
	public string objetoTextChat = "Digite o tipo do objeto.";
	public string idSalaTextChat = "Digite o id da sala";
    private List<ChatEntries> chatEntries = new List<ChatEntries>();
	private IdSalas idSalaAtual = IdSalas.Sala1;

    // Use this for initialization
    void Start()
    {
        netWorkView = GetComponent<NetworkView>();
		
		foreach(Sala sala in repositorio.salas){
			ChatEntries chat = new ChatEntries();
			chat.idSala = sala.idSala;
			
			chatEntries.Add(chat);
		}
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
				Network.Disconnect(200);
			
			
			objetoTextChat = GUILayout.TextField(objetoTextChat, GUILayout.MinWidth(100));
			idSalaTextChat = GUILayout.TextField(idSalaTextChat, GUILayout.MinWidth(100));
			if(GUI.Button(new Rect(200, 50, 120, 50), "Adicionar Item")){
				comandos.AdicionaObjetoSala(idSalaTextChat, objetoTextChat);
				objetoTextChat = "";
				idSalaTextChat = "";
			}
			
			if (GUILayout.Button(new Rect(0, 0, 50, 50),"Sala1"))
				idSalaAtual = IdSalas.Sala1;
			
			if (GUILayout.Button(new Rect(60, 0, 50, 50),"Sala2"))
				idSalaAtual = IdSalas.Sala2;
			
			if (GUILayout.Button(new Rect(120, 0, 50, 50),"Sala3"))
				idSalaAtual = IdSalas.Sala3;
			
			if (GUILayout.Button(new Rect(180, 0, 50, 50),"Sala4"))
				idSalaAtual = IdSalas.Sala4;
			
			if (GUILayout.Button(new Rect(240, 0, 50, 50),"Sala5"))
				idSalaAtual = IdSalas.Sala5;
			
            foreach (ChatEntries chat in chatEntries){
				GUILayout.Label(chat.idSala);
				if(chat.idSala = idSalaAtual)
					foreach(string txt in chat.chat)
						GUILayout.Label(tx);
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
		player.idSala = IdSalas.Sala1;

        repositorio.players.Add(player);

        string txt = "Jogador " + name + " se connectou!";

        NotificaTodosPlayers("", txt);
        AdicionaTextoByIdSala(IdSalas.Sala1, txt);
        
        info.networkView.RPC("SetIdPlayer", RPCMode.All, name, player.idPlayer);
    }

    [RPC]
    void Comando(string idPlayer, string texto)
    {
        Player player = comandos.buscarPlayerById(idPlayer);
        AdicionaTexto(player, texto);
        comandos.falarChat(player, texto);
    }

    void NotificaTodosPlayers(string nomePlayer, string texto)
    {
        netWorkView.RPC("ShowText", RPCMode.All, nomePlayer, texto);
    }

    void notificaPlayer(string idPlayer, string texto)
    {
        netWorkView.RPC("Sendmsg", RPCMode.All, idPlayer, texto);
    }

    void AdicionaTexto(Player player, string texto)
    {
		foreach(ChatEntries ce in chatEntries){
			if(ce.idSala == player.idSala)
				ce.chat.Add("[" + player.nome + "]" + texto);
			break;
		} 	  
    }
	
	void AdicionaTextoByIdSala(IdSalas idSala, string texto)
    {
		foreach(ChatEntries ce in chatEntries){
			if(ce.idSala == idSala)
				ce.chat.Add(texto);
			break;
		} 		  
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

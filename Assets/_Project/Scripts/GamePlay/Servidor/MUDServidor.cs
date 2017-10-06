using UnityEngine;
using System.Collections.Generic;

public class MUDServidor : MonoBehaviour {
    public MUDServidorUI servidorUI;
    public string connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    public NetworkView netWorkView;
    //public string objetoTextChat = "Digite o tipo do objeto.";
    //public string idSalaTextChat = "Digite o id da sala";
    private List<ChatEntries> chatEntries = new List<ChatEntries>();
    private IdSalas idSalaAtual = IdSalas.Sala1;

    private Repositorio repositorio;
    private MUDCommand comandos;

    private bool isConectado = false;


    // Use this for initialization
    void Start()
    {
        netWorkView = GetComponent<NetworkView>();
        servidorUI.Address = connectToIP;
        servidorUI.Port = connectPort;
        servidorUI.ConnectMessage = "Servidor: Desativado";
        servidorUI.ButtonConnectName = "INICIAR\n"
                                     + "SERVIDOR";
        servidorUI.MessageArea = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Conectando ao servidor
        if (!isConectado)
        {
            if (Network.peerType == NetworkPeerType.Connecting)
            {
                servidorUI.MessageArea += "Conectando...\n";
            }
            else if (Network.peerType == NetworkPeerType.Server)
            {
                servidorUI.MessageArea += "Servidor inicializado com sucesso!\n";
                servidorUI.MessageArea += "Conexões: " + Network.connections.Length +"\n";
                servidorUI.ConnectMessage = "Servidor: Ativado";
                servidorUI.ButtonConnectName = "DESCONECTAR";
                isConectado = true;
            }
        }

        if (Network.peerType == NetworkPeerType.Server)
        {
            servidorUI.QtdConnections = Network.connections.Length;

            // Atualiza MessageArea
            foreach (ChatEntries chat in chatEntries)
            {
                if (chat.idSala == idSalaAtual)
                {
                    servidorUI.MessageArea = chat.idSala.ToString() + "\n";
                    foreach (string txt in chat.chat)
                        //GUILayout.Label(txt);
                        servidorUI.MessageArea += txt + "\n";
                }
            }
        }    
    }

    void Awake()
    {
        repositorio = GameObject.FindGameObjectWithTag("Repositorio").GetComponent<Repositorio>();
        comandos = GameObject.FindGameObjectWithTag("Repositorio").GetComponent<MUDCommand>();

        foreach (Sala sala in repositorio.salas)
        {
            ChatEntries chat = new ChatEntries();
            chat.idSala = sala.idSala;

            chatEntries.Add(chat);
        }
    }

    public void ConectarServidor()
    {
        if (!isConectado)
        {
            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                connectToIP = servidorUI.Address;
                connectPort = servidorUI.Port;

                servidorUI.MessageArea += "Inicializando o servidor...\n";
                Network.InitializeServer(32, connectPort, false);
            }
        }
        else
        {
            Network.Disconnect(200);
            servidorUI.ConnectMessage = "Servidor: Desativado";
            servidorUI.ButtonConnectName = "INICIAR\n"
                                         + "SERVIDOR";
            isConectado = false;
        }
    }


    public void TrocarSala()
    {
        switch ((IdSalas)servidorUI.ChangeRoom + 1)
        {
            case IdSalas.Sala1:
                idSalaAtual = IdSalas.Sala1;
                servidorUI.MessageArea = "";
                break;
            case IdSalas.Sala2:
                idSalaAtual = IdSalas.Sala2;
                servidorUI.MessageArea = "";
                break;
            case IdSalas.Sala3:
                idSalaAtual = IdSalas.Sala3;
                servidorUI.MessageArea = "";
                break;
            case IdSalas.Sala4:
                idSalaAtual = IdSalas.Sala4;
                servidorUI.MessageArea = "";
                break;
            case IdSalas.Sala5:
                idSalaAtual = IdSalas.Sala5;
                servidorUI.MessageArea = "";
                break;
            default:
                idSalaAtual = IdSalas.Sala1;
                break;
        }
    }

    public void AdicionarItem()
    {
        if(servidorUI.Item > 0)
        {
            TipoObjeto objeto = (TipoObjeto)servidorUI.Item;
            comandos.AdicionaObjetoSala(idSalaAtual.ToString(), objeto.ToString());
        }
    }


    /*
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
            if (GUI.Button(new Rect(200, 50, 120, 50), "Adicionar Item"))
            {
                comandos.AdicionaObjetoSala(idSalaTextChat, objetoTextChat);
                objetoTextChat = "";
                idSalaTextChat = "";
            }

            if (GUILayout.Button("Sala1"))
                idSalaAtual = IdSalas.Sala1;

            if (GUILayout.Button("Sala2"))
                idSalaAtual = IdSalas.Sala2;

            if (GUILayout.Button("Sala3"))
                idSalaAtual = IdSalas.Sala3;

            if (GUILayout.Button("Sala4"))
                idSalaAtual = IdSalas.Sala4;

            if (GUILayout.Button("Sala5"))
                idSalaAtual = IdSalas.Sala5;

            foreach (ChatEntries chat in chatEntries)
            {
                if (chat.idSala == idSalaAtual)
                {
                    GUILayout.Label(chat.idSala.ToString());
                    foreach (string txt in chat.chat)
                        GUILayout.Label(txt);
                }
            }
        }
    }
    */

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

        repositorio.players.Remove(comandos.BuscarPlayerByNetwork(player));
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

        Inventario inventario = new Inventario();
        inventario.idPlayer = player.idPlayer;

        repositorio.inventarios.Add(inventario);
        repositorio.players.Add(player);

        string txt = "Jogador " + name + " se connectou!";

        NotificaTodosPlayers("", txt, comandos.BuscarSalaByIdSala(player.idSala).nome);
        AdicionaTextoByIdSala(IdSalas.Sala1, txt);

        string nomeSala = comandos.BuscarSalaByIdSala(player.idSala).nome;

        info.networkView.RPC("SetPlayerPref", RPCMode.All, name, player.idPlayer, nomeSala);
        comandos.falarChat(player, "Examinar " + comandos.BuscarSalaByIdSala(player.idSala).nome);
    }

    [RPC]
    void Comando(string idPlayer, string texto)
    {
        Debug.Log(idPlayer);
        Player player = comandos.buscarPlayerById(idPlayer);
        AdicionaTexto(player, texto);
        comandos.falarChat(player, texto);
    }

    public void NotificaTodosPlayers(string idPlayer, string texto, string idSala)
    {
        netWorkView.RPC("ShowText", RPCMode.All, idPlayer, texto, idSala);
    }

    public void NotificaOutrosPlayersBySala(string idPlayer, string idSala, string texto)
    {
        netWorkView.RPC("NotificaOutros", RPCMode.Others, idPlayer, idSala, texto);
    }

    public void notificaPlayer(string idPlayer, string texto)
    {
        string nomeSala = comandos.BuscarSalaByIdSala(comandos.buscarPlayerById(idPlayer).idSala).nome;
        netWorkView.RPC("Sendmsg", RPCMode.All, idPlayer, texto, nomeSala);
    }

    void AdicionaTexto(Player player, string texto)
    {
        foreach (ChatEntries ce in chatEntries)
        {
            if (ce.idSala == player.idSala)
            {
                ce.chat.Add("[" + player.nome + "]" + texto);
                break;
            }
        }
    }

    public void AdicionaTextoByIdSala(IdSalas idSala, string texto)
    {
        foreach (ChatEntries ce in chatEntries)
        {
            if (ce.idSala == idSala)
            {
                ce.chat.Add(texto);
                break;
            }
        }
    }

    public void SetPref(Player player)
    {
        netWorkView.RPC("SetPlayerPref", RPCMode.All, player.nome, player.idPlayer, comandos.BuscarSalaByIdSala(player.idSala).nome);
    }

    //Client function
    //As funções do cliente devem ser declaradas no servidor porém vazias
    [RPC]
    void SetPlayerPref(string playerName, string idPlayer, string idSala) { }
    [RPC]
    void ShowText(string idPlayer, string texto, string idSala) { }
    [RPC]
    void Sendmsg(string idPlayer, string texto, string idSala) { }
    [RPC]
    void NotificaOutros(string idPlayer, string idSala, string texto) { }
}

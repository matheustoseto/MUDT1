using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum TipoObjeto
{
    Machado,
    Carta,
    Moeda,
    Vara_De_Pescar,
    Tocha,
    Bota,
    Espada,
    Mapa,
    Lenha,
    Peixe,
    Peixe_Assado,
    Arvores,
    Rocha,
    Lago,
    Ponte_Para_Pesca,
    Fogueira
}

//------ OBJETOS DO JOGO --------
public class Objeto
{
    public string nome;
    public TipoObjeto tipo;
    public bool pegar;
    public string descricao;
}
//------------------------------

//------ PLAYER ----------------
public class Player
{
    public string idPlayer;
    public string nome;
    public NetworkPlayer networkPlayer;
}
//------------------------------

//------ SALA ------------------
public class Sala
{
    public string nome;
    public string descricao;
    public List<TipoObjeto> objetos;
}
//------------------------------

public class Repositorio : MonoBehaviour
{
    public List<Objeto> objetos = new List<Objeto>();
    public List<Player> players = new List<Player>();
    public List<Sala> salas = new List<Sala>();

    public Dictionary<Sala, List<Player>> salaComPlayers;

    // Use this for initialization
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {

    }

    public Player BuscarPlayer(string idPlayer)
    {
        Player player = (from item in players
                         where item.idPlayer == idPlayer
                         select item).First();

        return player;
    }
}

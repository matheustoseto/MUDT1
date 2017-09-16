using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

public enum IdSalas
{
    Sala1,
    Sala2,
    Sala3,
    Sala4,
    Sala5
}

public enum Coordenadas
{
    Norte,
    Sul,
    Leste,
    Oeste
}

[System.Serializable]
public class SalasLigadas
{
    public IdSalas sala;
    public Coordenadas coordenada;
}

//------ OBJETOS DO JOGO --------
[System.Serializable]
public class Objeto
{
    public string nome;
    public TipoObjeto tipo;
    public bool pegar;
    public bool usar;
    public string descricao;
}
//------------------------------

//------ PLAYER ----------------
[System.Serializable]
public class Player
{
    public string idPlayer;
    public string nome;
}
//------------------------------

//------ SALA ------------------
[System.Serializable]
public class Sala
{
    public string nome;
    public string descricao;
    public IdSalas idSala;
    public List<TipoObjeto> objetos;
    public List<SalasLigadas> salasLigadas;
}
//------------------------------

public class Repositorio : MonoBehaviour
{
    public List<Objeto> objetos;
    public List<Player> players;
    public List<Sala> salas;

    public Dictionary<Sala, List<Player>> salaComPlayers;

    // Use this for initialization
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {

    }
}

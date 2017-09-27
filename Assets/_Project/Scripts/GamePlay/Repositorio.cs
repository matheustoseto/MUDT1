using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum TipoObjeto
{
    Default,
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
    Default,
    Sala1,
    Sala2,
    Sala3,
    Sala4,
    Sala5
}

public enum Coordenadas
{
    Default,
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
	public string descricaoUsarS;
	public string descricaoUsarN;
}
//------------------------------

//------ PLAYER ----------------
[System.Serializable]
public class Player
{
    public string idPlayer;
    public string nome;
    public IdSalas idSala;
    public NetworkPlayer networkPlayer;
}
//------------------------------

//------ SALA ------------------
[System.Serializable]
public class Sala
{
    public string nome;
    public string descricao;
    public IdSalas idSala;
    public List<TipoObjeto> objetos = new List<TipoObjeto>();
    public List<SalasLigadas> salasLigadas = new List<SalasLigadas>();
}
//------------------------------

 public class Inventario
{
    public string idPlayer;
    public List<Objeto> objetos = new List<Objeto>();
}

public class Repositorio : MonoBehaviour
{
    public List<Objeto> objetos = new List<Objeto>();
    public List<Player> players = new List<Player>();
    public List<Sala> salas = new List<Sala>();
    public List<Inventario> inventarios = new List<Inventario>();
}

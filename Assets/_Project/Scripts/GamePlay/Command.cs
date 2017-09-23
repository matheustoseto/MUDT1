
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Command : MonoBehaviour {

    [SerializeField] private ChatMudUI mudUI;
    [SerializeField] private string namePlayer = "Player";

    private Repositorio repositorio;

	// Use this for initialization
	void Start () {
        repositorio = new Repositorio();
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void GetCommand()
    {
        mudUI.LogText += "["+namePlayer+"]: " + falarChat(buscarPlayerById("idPlayer"), mudUI.Command.text) + "\n";
        mudUI.Command.text = "";
    }

    public string falarChat(Player player, string texto)
    {
        if ("Examinar".Contains(texto))
        { // Examinar [sala/objeto]
            string splitTexto = texto.Split(null)[1];

            Sala sala = (from item in repositorio.salas
                        where item.idSala == player.idSala && item.nome == splitTexto
                        select item).First();

            if (sala != null)
                return sala.descricao;

            Objeto objeto = (from item in repositorio.inventarios
                             where item.idPlayer == player.idPlayer
                             select (from obj in item.objetos
                                     where obj.nome == splitTexto
                                     select obj).First()).First();

            if (objeto != null)
                return objeto.descricao;

            return "Sala ou Objeto não encontrado.";
        }
        else
        if ("Mover".Contains(texto))
        { // Mover [N/S/L/O]

        }
        else
        if ("Pegar".Contains(texto))
        { // Pegar [objeto]
            string splitTexto = texto.Split(null)[1];

            Objeto objeto = (from item in repositorio.salas
                             where item.idSala == player.idSala
                             select (from tipo in item.objetos
                                     select (from obj in repositorio.objetos
                                             where obj.tipo == tipo && obj.nome == splitTexto
                                             select obj).First()).First()).First();
            if (objeto != null)
            {
                foreach (Inventario inventario in repositorio.inventarios)
                    if (inventario.idPlayer == player.idPlayer)
                        inventario.objetos.Add(objeto);

                foreach (Sala sala in repositorio.salas)
                    if (sala.idSala == player.idSala)
                        sala.objetos.Remove(objeto.tipo);

                return "Objeto adicionado no seu inventario.";
            }

            return "Objeto não encontrado na sala.";
        }
        else
        if ("Largar".Contains(texto))
        { // Largar [objeto]
            string splitTexto = texto.Split(null)[1];

            Objeto objeto = (from item in repositorio.inventarios
                             where item.idPlayer == player.idPlayer
                             select (from obj in item.objetos
                                     where obj.nome == splitTexto
                                     select obj).First()).First();
            if(objeto != null)
            {
                foreach(Inventario inv in repositorio.inventarios)
                    if (inv.idPlayer == player.idPlayer)
                        foreach (Objeto obj in inv.objetos)
                            if (obj.nome == splitTexto)
                                inv.objetos.Remove(obj);

                foreach(Sala sala in repositorio.salas)
                    if (sala.idSala == player.idSala)
                        sala.objetos.Add(objeto.tipo);

                return "Objeto removido do inventario.";
            }

            return "Objeto não foi encontrado no inventario.";
        }
        else
        if ("Inventario".Contains(texto))
        { // Inventário
            Inventario inventario = (from item in repositorio.inventarios
                                     where item.idPlayer == player.idPlayer
                                     select item).First();
            string objetos = "Você possui os seguintes objetos: ";

            if (inventario != null) { 
                foreach (Objeto obj in inventario.objetos)
                    objetos += obj.nome + " - ";
                return objetos;
            }
            return "Inventario não encontrado.";
        }
        else
        if ("Usar".Contains(texto))
        { // Usar [objeto] {alvo}
            string[] splitTexto = texto.Split(null);
            Objeto objeto   = buscarObjeto(splitTexto[1]);
            Objeto alvo     = buscarObjeto(splitTexto[2]);

            if (!verificaInventario(objeto))
                return "Você não possui esse objeto.";

            if (objeto != null && alvo != null)
                return usarObjetoAlvo(objeto.tipo, alvo.tipo);
            else if (objeto != null)
                return usarObjeto(objeto.tipo);
        }
        else
        if ("Falar".Contains(texto))
        { // Falar [texto]
            return texto.Split(null)[1];
        }
        else
        if ("Cochichar".Contains(texto))
        { // Cochichar [texto] [jogador]

        }
        else
        if ("Ajuda".Contains(texto))
        { // Ajuda
            return "Lista de Comandos: \n"
                    + "Examinar [sala/objeto] \n"
                    + "Mover [N/S/L/O] \n"
                    + "Pegar [objeto] \n"
                    + "Largar [objeto] \n"
                    + "Inventário [sala/objeto] \n"
                    + "Usar [objeto] {alvo} \n"
                    + "Falar [texto] \n"
                    + "Cochichar [texto] [jogador] \n";
        }
        return "Não Foi Possível Realizar Essa Ação.";
    }

    public void acessouJogo(string nome)
    {
        Player player = new Player();
        player.idPlayer = Guid.NewGuid().ToString("N");
        player.nome = nome;

        repositorio.players.Add(player);
    }

    public bool verificaInventario(Objeto objeto)
    {
        return false;
    }

    public string usarObjeto(TipoObjeto objeto)
    {
        switch (objeto)
        {
            case TipoObjeto.Machado:
                {

                    break;
                }
        }         
        return null;
    }

    public string usarObjetoAlvo(TipoObjeto objeto, TipoObjeto alvo)
    {
        switch (objeto)
        {
            case TipoObjeto.Machado:
                {

                    break;
                }
        }
        return null;
    }

    public Objeto buscarObjeto(string objeto)
    {
        foreach (Objeto obj in repositorio.objetos)
        {
            if (obj.nome.Contains(objeto))
                return obj;
        }
        return null;
    }

    public Player buscarPlayerById(string idPlayer)
    {
        Player player = (from item in repositorio.players
                         where item.idPlayer == idPlayer
                         select item).First();
        return player;
    }
}

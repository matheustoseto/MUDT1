
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Command : MonoBehaviour {

    private Repositorio repositorio;
	private Servidor servidor;

    public void falarChat(Player player, string texto)
    {
        if ("Examinar".Contains(texto))
        { // Examinar [sala/objeto]
            string splitTexto = texto.Split(null)[1];

            Sala sala = (from item in repositorio.salas
                        where item.idSala == player.idSala && item.nome == splitTexto
                        select item).First();

            if (sala != null){
                servidor.notificaPlayer(player.idPlayer, sala.descricao);
				return;
			}

            Objeto objeto = (from item in repositorio.inventarios
                             where item.idPlayer == player.idPlayer
                             select (from obj in item.objetos
                                     where obj.nome == splitTexto
                                     select obj).First()).First();

            if (objeto != null){
                servidor.notificaPlayer(player.idPlayer, objeto.descricao);
				return;
			}
            servidor.notificaPlayer(player.idPlayer, "Sala ou Objeto não encontrado.");
        }
        else
        if ("Mover".Contains(texto))
        { // Mover [N/S/L/O]
			splitTexto = texto.Split(null)[1];
			
			Sala salaAtual = (from item in repositorio.salas
							  where item.idSala == player.idSala
							  select item).First();
							  
			Coordenadas coordenada = BuscarCoordenada(splitTexto);
			
			IdSalas idSala = (from item in salaAtual.salasLigadas
							  where item.coordenada == coordenada
							  select item).First();
			
			if(idSala != null){
				Sala salaMover = (from item in repositorio.salas
								  where item.idSala == idSala
								  select item).First();
				foreach(Player pl in repositorio.players){
					if(pl.idPlayer == player.idPlayer)
						pl.idSala = salaMover.idSala;
				}
				servidor.notificaPlayer(player.idPlayer, "Você se moveu para a sala " + salaMover.nome);
				return;
			}				
			servidor.notificaPlayer(player.idPlayer, "Não foi possivel mover para a sala descrita.");
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

                servidor.notificaPlayer(player.idPlayer, "Objeto adicionado no seu inventario.");
				return;
            }
            servidor.notificaPlayer(player.idPlayer, "Objeto não encontrado na sala.");
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

                servidor.notificaPlayer(player.idPlayer, "Objeto removido do inventario.");
				return;
            }
            servidor.notificaPlayer(player.idPlayer, "Objeto não foi encontrado no inventario.");
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
                servidor.notificaPlayer(player.idPlayer, objetos);
				return;
            }
            servidor.notificaPlayer(player.idPlayer, "Inventario não encontrado." );
        }
        else
        if ("Usar".Contains(texto))
        { // Usar [objeto] {alvo}
            string[] splitTexto = texto.Split(null);
            Objeto objeto   = buscarObjeto(splitTexto[1]);
            Objeto alvo     = buscarObjeto(splitTexto[2]);

            if (!verificaInventario(player, objeto))
                servidor.notificaPlayer(player.idPlayer, "Você não possui esse objeto.");
			
			if (alvo != null && !verificaInventario(player, alvo))
                servidor.notificaPlayer(player.idPlayer, "Você não possui esse objeto.");

            if (objeto != null && alvo != null)
                servidor.notificaPlayer(player.idPlayer, usarObjetoAlvo(objeto.tipo, alvo.tipo));
            else if (objeto != null)
                servidor.notificaPlayer(player.idPlayer, usarObjeto(objeto.tipo));
        }
        else
        if ("Falar".Contains(texto))
        { // Falar [texto]
            servidor.NotificaTodosPlayers(player.nome, texto.Split(null)[1]);
        }
        else
        if ("Cochichar".Contains(texto))
        { // Cochichar [texto] [jogador]
			string splitTexto = texto.Split(null)[1];
			string jogador = texto.Split(null)[2];
			Player plyer = buscarPlayerByName(jogador);
			
			servidor.notificaPlayer(plyer.idPlayer, "O jogador " + player.nome + " enviou uma mensagem: " + splitTexto);
        }
        else
        if ("Ajuda".Contains(texto))
        { // Ajuda
			
            string ajuda =  "Lista de Comandos: \n"
                    + "Examinar [sala/objeto] \n"
                    + "Mover [N/S/L/O] \n"
                    + "Pegar [objeto] \n"
                    + "Largar [objeto] \n"
                    + "Inventário [sala/objeto] \n"
                    + "Usar [objeto] {alvo} \n"
                    + "Falar [texto] \n"
                    + "Cochichar [texto] [jogador] \n";
					
			servidor.notificaPlayer(player.idPlayer, ajuda);
        }else{
			servidor.notificaPlayer(player.idPlayer, "Não Foi Possível Realizar Essa Ação.");
		}
    }

    public bool verificaInventario(Player player, Objeto objeto)
    {
		Objeto obj = (from item in repositorio.inventario
					  where item.idPlayer == player.idPlayer
					  select (from objet in item.objetos
							 where objet.tipo == objeto.tipo
							 select objet).First()).First();
							 
        return (obj != null);
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

    public Objeto buscarObjeto(string nomeObjeto)
    {
        return (from item in repositorio.objetos
				where item.nome == nomeObjeto
				select item).First();
    }

    public Player buscarPlayerById(string idPlayer)
    {
        Player player = (from item in repositorio.players
                         where item.idPlayer == idPlayer
                         select item).First();
        return player;
    }
	
    public Player buscarPlayerByName(string nome)
    {
        Player player = (from item in repositorio.players
                         where item.nome == nome
                         select item).First();
        return player;
    }
	
	public Coordenadas BuscarCoordenada(string cd){
		if("N".contais(cd))
			return Coordenadas.Norte;
		if("S".contais(cd))
			return Coordenadas.Sul;
		if("L".contais(cd))
			return Coordenadas.Leste;
		if("O".contais(cd))
			return Coordenadas.Oeste;
		
		return Coordenadas.Default;
	}
	
	public IdSalas BuscarIdSalas(string cd){
		if("Sala1".contais(cd))
			return IdSalas.Sala1;
		if("Sala2".contais(cd))
			return IdSalas.Sala2;
		if("Sala3".contais(cd))
			return IdSalas.Sala3;
		if("Sala4".contais(cd))
			return IdSalas.Sala4;
		if("Sala5".contais(cd))
			return IdSalas.Sala5;
		
		return IdSalas.Default;
	}
	
	public void AdicionaObjetoSala(string cdSala, string texto){
		
		IdSalas idSala = BuscarIdSalas(cdSala);
		
		if(idSala == IdSalas.Default){
			servidor.AdicionaTextoByIdSala(idSala, "Sala não encontrado!");
			return;
		}

		Objeto objeto = (from item in repositorio.salas
				 where item.idSala == idSala
				 select (from tipo in item.objetos
						 select (from obj in repositorio.objetos
								 where obj.tipo == tipo && obj.nome == texto
								 select obj).First()).First()).First();
								 
		if (objeto != null){
			foreach (Sala sala in repositorio.salas)
					if (sala.idSala == idSala)
						sala.objetos.Add(objeto.tipo);
					
			servidor.AdicionaTextoByIdSala(idSala, "Objeto " + objeto.tipo + "adicionado!");
			return;
		}	
		servidor.AdicionaTextoByIdSala(idSala, "Objeto não encontrado!");				
	}
}

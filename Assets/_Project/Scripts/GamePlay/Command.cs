
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Command : MonoBehaviour {

    private Servidor servidor;
    private Repositorio repositorio;

    void Awake()
    {
        servidor = GameObject.FindGameObjectWithTag("Repositorio").GetComponent<Servidor>();
        repositorio = GameObject.FindGameObjectWithTag("Repositorio").GetComponent<Repositorio>();
    }

    public void falarChat(Player player, string texto)
    {
        Debug.Log("falarChat: "+texto);

        if (texto.Contains("Examinar"))
        { // Examinar [sala/objeto]
            string splitTexto = texto.Split(null)[1];

            Sala sala = (from item in repositorio.salas
                         where item.idSala == player.idSala && item.nome == splitTexto
                         select item).FirstOrDefault();

            if (sala != null) {
                servidor.notificaPlayer(player.idPlayer, GerarDescricaoSala(sala.descricao, player));
                return;
            }

            //Busca no inventario
            Objeto objeto = (from item in repositorio.inventarios
                             where item.idPlayer == player.idPlayer
                             select (from obj in item.objetos
                                     where obj.nome == splitTexto
                                     select obj).FirstOrDefault()).FirstOrDefault();

            if (objeto != null) {
                servidor.notificaPlayer(player.idPlayer, objeto.descricao);
                return;
            } else
            {
                //Busca na sala
                Objeto objTexto = buscarObjeto(splitTexto);

                if (objTexto != null)
                {
                    TipoObjeto tipoObjeto = (from item in repositorio.salas
                                             where item.idSala == player.idSala
                                             select (from obj in item.objetos
                                                     where obj == objTexto.tipo
                                                     select obj).FirstOrDefault()).FirstOrDefault();

                    if (tipoObjeto != TipoObjeto.Default)
                    {
                        Objeto obj = (from item in repositorio.objetos
                                      where item.tipo == tipoObjeto
                                      select item).First();
                        servidor.notificaPlayer(player.idPlayer, obj.descricao);
                        return;
                    }
                }      
            }
            servidor.notificaPlayer(player.idPlayer, "Sala ou Objeto não encontrado.");
        }
        else
        if (texto.Contains("Mover"))
        { // Mover [N/S/L/O]
            string splitTexto = texto.Split(null)[1];

            Sala salaAtual = (from item in repositorio.salas
                              where item.idSala == player.idSala
                              select item).FirstOrDefault();

            Coordenadas coordenada = BuscarCoordenada(splitTexto);

            IdSalas idSala = (from item in salaAtual.salasLigadas
                              where item.coordenada == coordenada
                              select item.sala).FirstOrDefault();

            if (idSala != IdSalas.Default) {
                Sala salaMover = (from item in repositorio.salas
                                  where item.idSala == idSala
                                  select item).FirstOrDefault();
                foreach (Player pl in repositorio.players) {
                    if (pl.idPlayer == player.idPlayer)
                        pl.idSala = salaMover.idSala;
                }

                servidor.NotificaOutrosPlayersBySala(player.idPlayer, salaAtual.nome, "Jogador " + player.nome + " moveu-se para a sala " + salaMover.nome);
                servidor.SetPref(player);
                servidor.notificaPlayer(player.idPlayer, "Você se moveu para a sala " + salaMover.nome);
                servidor.NotificaOutrosPlayersBySala(player.idPlayer, salaMover.nome, "Jogador " + player.nome + " moveu-se para a sala " + salaMover.nome);

                falarChat(player, "Examinar " + salaMover.nome);
                return;
            }
            servidor.notificaPlayer(player.idPlayer, "Não foi possivel mover para a sala descrita.");
        }
        else
        if (texto.Contains("Pegar"))
        { // Pegar [objeto]
            string splitTexto = texto.Split(null)[1];

            List<TipoObjeto> objetos = (from item in repositorio.salas
                                        where item.idSala == player.idSala
                                        select item.objetos).FirstOrDefault();

            Objeto objeto = (from obj in repositorio.objetos
                            where objetos.Contains(obj.tipo) && obj.nome == splitTexto
                            select obj).FirstOrDefault();

            Debug.Log("objeto: "+ objeto);

            if (objeto != null)
            {
				
				if(!objeto.pegar){
					servidor.notificaPlayer(player.idPlayer, "Esse objeto não é permitido coletar.");
					return;
				}					
				
                foreach (Inventario inventario in repositorio.inventarios)
                    if (inventario.idPlayer == player.idPlayer)
                        inventario.objetos.Add(objeto);

                foreach (Sala sala in repositorio.salas)
                    if (sala.idSala == player.idSala) { 
                        sala.objetos.Remove(objeto.tipo);
                        break;
                    }

                servidor.notificaPlayer(player.idPlayer, "Objeto adicionado no seu inventario.");
				servidor.NotificaOutrosPlayersBySala(player.idPlayer, BuscarSalaByIdSala(player.idSala).nome, "Jogador " + player.nome + " pegou o objeto " + objeto.nome);
                return;
            }
            servidor.notificaPlayer(player.idPlayer, "Objeto não encontrado na sala.");
        }
        else
        if (texto.Contains("Largar"))
        { // Largar [objeto]
            string splitTexto = texto.Split(null)[1];

            Objeto objeto = (from obj in repositorio.objetos
                             where obj.nome == splitTexto
                             select obj).FirstOrDefault();

            Inventario inv = (from item in repositorio.inventarios
                              where item.idPlayer == player.idPlayer && item.objetos.Contains(objeto)
                              select item).FirstOrDefault();

            if (objeto != null && inv != null)
            {
                foreach (Objeto obj in inv.objetos)
                    if (obj.nome == splitTexto) { 
                        inv.objetos.Remove(obj);
                        break;
                    }

                Debug.Log("Removeu do inventario!!!");

                foreach (Sala sala in repositorio.salas)
                    if (sala.idSala == player.idSala)
                        sala.objetos.Add(objeto.tipo);

                servidor.notificaPlayer(player.idPlayer, "Objeto removido do inventario.");
				servidor.NotificaOutrosPlayersBySala(player.idPlayer, BuscarSalaByIdSala(player.idSala).nome, "Jogador " + player.nome + " largou o objeto " + objeto.nome);
                return;
            }
            servidor.notificaPlayer(player.idPlayer, "Objeto não foi encontrado no inventario.");
        }
        else
        if (texto.Contains("Inventario"))
        { // Inventário
            Inventario inventario = (from item in repositorio.inventarios
                                     where item.idPlayer == player.idPlayer
                                     select item).FirstOrDefault();
            string objetos = "Você possui os seguintes objetos: ";

            if (inventario != null && inventario.objetos.Count > 0) {
                foreach (Objeto obj in inventario.objetos)
                    objetos += obj.nome + " - ";
                servidor.notificaPlayer(player.idPlayer, objetos);
                return;
            }
            servidor.notificaPlayer(player.idPlayer, "Você não possui nenhum item no inventário.");
        }
        else
        if (texto.Contains("Usar"))
        { // Usar [objeto] {alvo}
            string[] splitTexto = texto.Replace("Usar ", "").Split(null);

            Objeto objeto = buscarObjeto(splitTexto[0]);
            Objeto alvo = null;

            if (splitTexto.Length > 1)
                alvo = buscarObjeto(splitTexto[1]);

			if (objeto != null && alvo != null){
	
				//Implementar objeto porta
				//Só pode usar objeto => alvo em porta, o resto retorna que não é possivel
				//Verificar se objeto porta esta na mesma sala
	
				servidor.notificaPlayer(player.idPlayer, usarObjetoAlvo(objeto.tipo, alvo.tipo));
			} else if (objeto != null){
				if (!verificaInventario(player, objeto)){
					servidor.notificaPlayer(player.idPlayer, "Você não possui esse objeto.");
					return;
				}
				servidor.notificaPlayer(player.idPlayer, usarObjeto(player, objeto.tipo));
			}    
        }
        else
        if (texto.Contains("Falar"))
        { // Falar [texto]
            servidor.NotificaTodosPlayers(player.idPlayer, texto.Replace("Falar ",""), BuscarSalaByIdSala(player.idSala).nome);
        }
        else
        if (texto.Contains("Cochichar"))
        { // Cochichar [texto] [jogador]
            string[] fala = texto.Replace("Cochichar ", "").Split(null);
            string jogador = fala[fala.Length - 1].Replace(" ","");

            string splitTexto = "";
            foreach (string f in fala)
                splitTexto = splitTexto + f + " ";

            splitTexto = splitTexto.Replace(jogador, "");

            Debug.Log(splitTexto);
            Debug.Log(jogador);

            Player plyer = buscarPlayerByName(jogador);
			
			servidor.notificaPlayer(plyer.idPlayer, "O jogador " + player.nome + " enviou uma mensagem: " + splitTexto);
        }
        else
        if (texto.Contains("Ajuda"))
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
		Objeto obj = (from item in repositorio.inventarios
					  where item.idPlayer == player.idPlayer
					  select (from objet in item.objetos
							 where objet.tipo == objeto.tipo
							 select objet).FirstOrDefault()).FirstOrDefault();
							 
        return (obj != null);
    }

    public string usarObjeto(Player player, TipoObjeto objeto)
    {
        foreach (Inventario inventario in repositorio.inventarios)
			if (inventario.idPlayer == player.idPlayer){
				foreach(Objeto obj in inventario.objetos)
					if(obj.tipo == objeto && obj.usar){
						if(obj.usou){
							obj.usou = false;
							return obj.descricaoUsarN;
						} else {
							obj.usou = true;
							return obj.descricaoUsarS;
						}
					}					
			}
        return "Você não pode usar esse objeto.";
    }

    public string usarObjetoAlvo(TipoObjeto objeto, TipoObjeto alvo)
    {
        return null;
    }

    public Objeto buscarObjeto(string nomeObjeto)
    {
        return (from item in repositorio.objetos
				where item.nome == nomeObjeto
				select item).FirstOrDefault();
    }

    public Player buscarPlayerById(string idPlayer)
    {
        Player player = (from item in repositorio.players
                         where item.idPlayer == idPlayer
                         select item).FirstOrDefault();
        return player;
    }
	
    public Player buscarPlayerByName(string nome)
    {
        Player player = (from item in repositorio.players
                         where item.nome == nome
                         select item).FirstOrDefault();
        return player;
    }
	
	public Sala BuscarSalaByIdSala(IdSalas idSala){
		Sala sala = (from item in repositorio.salas
					 where item.idSala == idSala
					 select item).FirstOrDefault();
		
		return sala;
	}

    public Player BuscarPlayerByNetwork(NetworkPlayer network)
    {
        return (from item in repositorio.players
                where item.networkPlayer == network
                select item).FirstOrDefault();
    }
	
	public Coordenadas BuscarCoordenada(string cd){
		if("N".Contains(cd))
			return Coordenadas.Norte;
		if("S".Contains(cd))
			return Coordenadas.Sul;
		if("L".Contains(cd))
			return Coordenadas.Leste;
		if("O".Contains(cd))
			return Coordenadas.Oeste;
		
		return Coordenadas.Default;
	}
	
	public IdSalas BuscarIdSalas(string cd){
		if("Sala1".Contains(cd))
			return IdSalas.Sala1;
		if("Sala2".Contains(cd))
			return IdSalas.Sala2;
		if("Sala3".Contains(cd))
			return IdSalas.Sala3;
		if("Sala4".Contains(cd))
			return IdSalas.Sala4;
		if("Sala5".Contains(cd))
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
								 select obj).FirstOrDefault()).FirstOrDefault()).FirstOrDefault();
								 
		if (objeto != null){
			foreach (Sala sala in repositorio.salas)
					if (sala.idSala == idSala)
						sala.objetos.Add(objeto.tipo);
					
			servidor.AdicionaTextoByIdSala(idSala, "Objeto " + objeto.tipo + " adicionado!");
			return;
		}	
		servidor.AdicionaTextoByIdSala(idSala, "Objeto não encontrado!");				
	}
	
	public string GerarDescricaoSala(string texto, Player player){
		
		string descricao = texto;

        List<Player> jogadores = (from item in repositorio.players
                                  where item.idSala == player.idSala
                                  select item).ToList();

        string qntJogadores = jogadores.Count.ToString();

        List<TipoObjeto> tipoObjeto = (from item in repositorio.salas
                                        where item.idSala == player.idSala
                                        select item.objetos).FirstOrDefault();
									
		List<Objeto> objs = (from item in repositorio.objetos
							 where tipoObjeto.Contains(item.tipo)
							 select item).ToList();
							 
		string objtsTexto = "";
		foreach(Objeto obj in objs)
			objtsTexto = objtsTexto + obj.nome + " - ";
			
		string salasAdj = "";
		List<SalasLigadas> salasLigadas = (from item in repositorio.salas
											where item.idSala == player.idSala
											select item.salasLigadas).FirstOrDefault();
		
		foreach(SalasLigadas sl in salasLigadas)
            salasAdj= salasAdj + sl.coordenada + " possui uma porta para a sala " + BuscarSalaByIdSala(sl.sala).nome + ". ";									
		
		descricao = descricao.Replace("XJ",qntJogadores);
		descricao = descricao.Replace("XOBJ",objtsTexto);
		descricao = descricao.Replace("XCS",salasAdj);
		
		return descricao;
	}
}

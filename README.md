# MUDT1

Especialização em Desenvolvimento de Jogos Digitais

Redes Aplicadas a Jogos - 2017/2

Trabalho 1 - MUD

O trabalho consiste no desenvolvimento de um jogo multiplayer estilo MUD (“Multi-user Dungeon”) com as seguintes características:

	(X) Modelo cliente-servidor implementado na Unity;
	(X) Implementação de um servidor que contenha toda a lógica de gameplay;
	(X) Implementação de um cliente que permite especificar o endereço para a conexão e a partir daí conectar para enviar e receber mensagens textuais;
	(X) No mínimo, o servidor deve suportar de 1 até 4 jogadores simultâneos;
	(X) Ao conectar-se, o jogador deve informar ao servidor qual será seu nome no jogo(equivalente a um login, que deve ser único para cada jogador conectado);
	(X) O MUD deve ter no mínimo 5 salas
	
O MUD deve suportar a lista de comandos e objetos citados abaixo.

	- Examinar [sala/objeto]
		(X) Retorna a descrição da sala atual (sala) ou objeto mencionado;
		(X) A descrição da sala também deve listar as salas adjacentes e suas respectivas direções, objetos e demais jogadores presentes no local.
	- Mover [N/S/L/O]
 		(X) O jogador deve mover-se para a direção indicada (norte, sul, leste ou oeste);
		(X) Ao entrar numa nova sala, o jogo deve executar automaticamente o comando “examinar sala”, que descreve o novo ambiente ao jogador.
	- Pegar [objeto]
 		(X) O jogador coleta um objeto que está na sala atual;
 		(X) Alguns objetos não podem ser coletados, como no caso de “porta”.
	- Largar [objeto]
 		(X) O jogador larga um objeto que está no seu inventório, na sala atual.
	- Inventório
 		(X) O jogo lista todos os objetos carregados atualmente pelo jogador.
	- Usar [objeto] {alvo}
 		(X) O jogador usa o objeto mencionado;
 		( ) Em alguns casos específicos, o objeto indicado necessitará de outro (alvo) para ser ativado (ex: usar chave porta).
	- Falar [texto]
		(X) O jogador envia um texto que será retransmitido para todos os jogadores presentes na sala atual.
	- Cochichar [texto] [jogador]
		(X) O jogador envia uma mensagem de texto apenas para o jogador especificado.
	- Ajuda
		(X) Lista todos os comandos possíveis do MUD.
		
Feedback dos Comandos

Todos os comandos devem retornar “mensagens de sucesso ou de erro” quando forem executados:

	(X) As mensagens de sucesso devem ser transmitidas para todos os jogadores do recinto, além do próprio que fez a ação. Por exemplo, quando um jogador larga um objeto, o jogo deve responder com “objeto X foi largado pelo jogador Y”, ou quando alguém se move, “jogador X moveu-se para o sul”;
	(X) Nos casos de erro, apenas o jogador que executou a ordem deve receber uma mensagem, como por exemplo, “não é possível mover-se para o norte” ou “objeto X não existe”.
	
Objetos

	O jogo deve ter no mínimo os seguintes objetos:
	
	- Porta
		(X) Item não coletável que necessita de uma chave para ser aberta;
		(X) “Examinar porta” retorna se a mesma encontra-se aberta ou fechada;
		(X) Ao tentar mover-se na direção onde a porta está posicionada, o jogador poderá prosseguir apenas se ela estiver aberta.
	- Chave
		(X) Item coletável que pode ser utilizado em uma ou mais portas.
	- Mapa
		( ) Ao utilizá-lo, o jogo desenha em “ASCII art” o mapa do MUD, indicando em qual sala o jogador se encontra.

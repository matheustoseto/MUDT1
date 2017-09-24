using UnityEngine;
using System.Collections;
using System;

public class Command : MonoBehaviour {

    private Repositorio repositorio;

	// Use this for initialization
	void Start () {
        repositorio = new Repositorio();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public string falarChat(string texto)
    {
        if ("Examinar".Contains(texto))
        { // Examinar [sala/objeto]

        }
        else
        if ("Mover".Contains(texto))
        { // Mover [N/S/L/O]

        }
        else
        if ("Pegar".Contains(texto))
        { // Pegar [objeto]

        }
        else
        if ("Largar".Contains(texto))
        { // Largar [objeto]

        }
        else
        if ("Inventario".Contains(texto))
        { // Inventário

        }
        else
        if ("Usar".Contains(texto))
        { // Usar [objeto] {alvo}
            string[] splitTexto = texto.Split(null);
            Objeto objeto   = buscarObjeto(splitTexto[1]);
            Objeto alvo     = buscarObjeto(splitTexto[2]);
            if (objeto != null && alvo != null)
            {

            }
        }
        else
        if ("Falar".Contains(texto))
        { // Falar [texto]

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

    public string usarObjeto(TipoObjeto objeto, TipoObjeto alvo)
    {


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
}

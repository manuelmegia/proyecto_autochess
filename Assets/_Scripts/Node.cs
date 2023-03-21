// Importamos los paquetes necesarios
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definimos la clase Node
public class Node
{
    // Definimos las variables p�blicas que utilizar� la clase
    public Vector2 Position; // Posici�n en el mapa
    public Node Parent; // Nodo padre
    public int G; // Costo acumulado desde el nodo inicial hasta este nodo
    public float F; // Costo total estimado desde el nodo inicial hasta el nodo final

    // Constructor de la clase
    public Node(Vector2 position, Node parent = null)
    {
        // Asignamos las variables
        Position = position;
        Parent = parent;
        G = parent == null ? 0 : parent.G + 1; // Si no hay padre, el costo es 0, de lo contrario, se suma el costo del padre m�s 1
        F = 0;
    }

    // M�todo para calcular el costo total estimado desde el nodo inicial hasta el nodo final
    public float CalculateF(Vector2 startPos, Vector2 targetPos)
    {
        var h = Mathf.Abs(Position.x - targetPos.x) + Mathf.Abs(Position.y - targetPos.y);// Calculamos la distancia estimada (heur�stica) desde este nodo hasta el nodo final
        F = G + h;// Calculamos el costo total estimado sumando el costo acumulado desde el nodo inicial hasta este nodo y la distancia estimada hasta el nodo final
        return F;// Devolvemos el costo total estimado
    }
}


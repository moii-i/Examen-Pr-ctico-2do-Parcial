using System;
using System.Collections.Generic;

class NodoFamilia
{
    public string Nombre { get; set; }
    public List<NodoFamilia> Padres { get; set; } // Los padres son los "hijos" del nodo

    public NodoFamilia(string nombre)
    {
        Nombre = nombre;
        Padres = new List<NodoFamilia>();
    }

    public void AgregarPadre(NodoFamilia padre)
    {
        Padres.Add(padre);
    }
}

class ArbolGenealogico
{
    public NodoFamilia Raiz { get; set; }
    public Dictionary<string, NodoFamilia> Miembros { get; set; }

    public ArbolGenealogico()
    {
        Miembros = new Dictionary<string, NodoFamilia>();
    }

    public void InsertarMiembro(string nombre, List<string> nombresPadres = null)
    {
        NodoFamilia nuevoNodo = new NodoFamilia(nombre);
        Miembros[nombre] = nuevoNodo;

        if (nombresPadres != null)
        {
            foreach (string nombrePadre in nombresPadres)
            {
                if (Miembros.ContainsKey(nombrePadre))
                {
                    nuevoNodo.AgregarPadre(Miembros[nombrePadre]);
                }
                else
                {
                    // Si el padre no existe, lo creamos
                    NodoFamilia padreNodo = new NodoFamilia(nombrePadre);
                    Miembros[nombrePadre] = padreNodo;
                    nuevoNodo.AgregarPadre(padreNodo);
                }
            }
        }

        if (Raiz == null)
        {
            Raiz = nuevoNodo;
        }
    }

    public void Preorden(NodoFamilia nodo = null, int nivel = 0)
    {
        if (nodo == null)
        {
            if (Raiz != null)
            {
                nodo = Raiz;
            }
            else
            {
                Console.WriteLine("El árbol está vacío");
                return;
            }
        }

        Console.WriteLine(new string(' ', nivel * 2) + nodo.Nombre);
        foreach (NodoFamilia padre in nodo.Padres)
        {
            Preorden(padre, nivel + 1);
        }
    }

    public List<string> EncontrarPadres(string nombre)
    {
        if (!Miembros.ContainsKey(nombre))
        {
            return null;
        }

        NodoFamilia nodo = Miembros[nombre];
        if (nodo.Padres.Count == 0)
        {
            return new List<string>();
        }

        List<string> padres = new List<string>();
        foreach (NodoFamilia padre in nodo.Padres)
        {
            padres.Add(padre.Nombre);
        }
        return padres;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ArbolGenealogico arbol = new ArbolGenealogico();

        while (true)
        {
            Console.WriteLine("\n--- Menú Árbol Genealógico ---");
            Console.WriteLine("1. Agregar nuevo miembro");
            Console.WriteLine("2. Mostrar árbol (preorden)");
            Console.WriteLine("3. Buscar padres de un miembro");
            Console.WriteLine("4. Salir");

            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            if (opcion == "1")
            {
                Console.Write("Nombre del nuevo miembro: ");
                string nombre = Console.ReadLine();
                Console.Write("Nombres de los padres (separados por coma, dejar vacío si no tiene): ");
                string padresInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(padresInput))
                {
                    List<string> listaPadres = new List<string>(padresInput.Split(','));
                    for (int i = 0; i < listaPadres.Count; i++)
                    {
                        listaPadres[i] = listaPadres[i].Trim();
                    }
                    arbol.InsertarMiembro(nombre, listaPadres);
                }
                else
                {
                    arbol.InsertarMiembro(nombre);
                }
                Console.WriteLine($"{nombre} ha sido agregado al árbol genealógico.");
            }
            else if (opcion == "2")
            {
                Console.WriteLine("\nÁrbol Genealógico (Preorden):");
                arbol.Preorden();
            }
            else if (opcion == "3")
            {
                Console.Write("Nombre del miembro a buscar padres: ");
                string nombre = Console.ReadLine();
                List<string> padres = arbol.EncontrarPadres(nombre);
                if (padres == null)
                {
                    Console.WriteLine($"{nombre} no existe en el árbol.");
                }
                else if (padres.Count == 0)
                {
                    Console.WriteLine($"{nombre} no tiene padres registrados.");
                }
                else
                {
                    Console.WriteLine($"Padres de {nombre}: {string.Join(", ", padres)}");
                }
            }
            else if (opcion == "4")
            {
                Console.WriteLine("Saliendo del programa...");
                break;
            }
            else
            {
                Console.WriteLine("Opción no válida. Intente de nuevo.");
            }
        }
    }
}
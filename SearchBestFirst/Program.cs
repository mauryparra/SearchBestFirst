using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SearchBestFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            ProblemaCubo nuevoProblema = new ProblemaCubo();
            Console.WriteLine("Estado Inicial del Cubo:");
            nuevoProblema.MostrarCubo();
            Console.ReadLine();
        }
    }

    class ProblemaCubo
    {
        private byte[,] Cubo;
        private char[] Operadores = new char[] { '<', '^', '>', 'v' };
        public struct nodo{
            public byte[,] estado;
            public int heuristicaVal;
            public Queue<char> operadores;
        };

        public ProblemaCubo()
        {
            Cubo = new byte[,] { {2, 8, 3}, {1, 6, 4}, {7, 0, 5} };
        }

        public void MostrarCubo()
        {
            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 3; j++)
                {
                    if (Cubo[i, j] == 0)
                        Console.Write(" ");
                    else
                        Console.Write(Cubo[i, j]);

                    Console.Write((j == 2) ? "" : "    ");
                }
                Console.WriteLine();
            }
        }

        public void MostrarCubo(byte [,] Cubo)
        {
            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 3; j++)
                {
                    if (Cubo[i, j] == 0)
                        Console.Write(" ");
                    else
                        Console.Write(Cubo[i, j]);

                    Console.Write((j == 2) ? "" : "    ");
                }
                Console.WriteLine();
            }
        }

        public byte Heuristica()
        {
            byte resultado = 0;
            byte[,] CuboResuelto = new byte[,] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };

            for (byte i = 0; i < 3; i++)
                for (byte j = 0; j < 3; j++)
                    if (!(Cubo[i, j] == CuboResuelto[i, j]))
                        resultado++;
            return resultado;
        }

        public byte Heuristica(byte[,] Cubo)
        {
            byte resultado = 0;
            byte[,] CuboResuelto = new byte[,] { { 1, 2, 3 }, { 8, 0, 4 }, { 7, 6, 5 } };

            for (byte i = 0; i < 3; i++)
                for (byte j = 0; j < 3; j++)
                    if (!(Cubo[i, j] == CuboResuelto[i, j]))
                        resultado++;
            return resultado;
        }

        /* FUNCION SUCESOR(NODO,OPERADOR)
            1. Hacer ESTADO-SUCESOR
                igual a APLICA(OPERADOR,ESTADO(NODO))
            2. Si ESTADO-SUCESOR=NO-APLICABLE
                devolver NO-APLICABLE
            en caso contrario,
                devolver un nodo cuyo estado es ESTADO-SUCESOR, cuyo camino
                es el resultado de añadir OPERADOR a CAMINO(NODO) y cuya
                heurística es la de ESTADO-SUCESOR
         */
        private nodo Sucesor(nodo nodoActual, char operador)
        {
            nodo nodoSucesor = new nodo();
            nodoSucesor = nodoActual;   //Copia el nodo actual a nodo sucesor

            // Busca el espacio vacio (0)
            byte indexI = 0;
            byte indexJ = 0;
            for (byte i = 0; i < 3; i++)
                for (byte j = 0; j < 3; j++)
                {
                    if (nodoActual.estado[i, j] == 0)
                    {
                        //Guarda los indices del espacio vacio
                        indexI = i;
                        indexJ = j;
                    }
                }

            switch (operador)
            {
                case '<':
                    if (indexJ < 2)
                    {
                        nodoSucesor.estado[indexI, indexJ] = nodoActual.estado[indexI, indexJ++];
                        nodoSucesor.estado[indexI, indexJ++] = 0;
                        nodoSucesor.heuristicaVal = Heuristica(nodoSucesor.estado);
                        nodoSucesor.operadores.Enqueue(operador);
                        return nodoSucesor;
                    }
                    else return default(nodo);

                case '^':
                    if (indexI < 2)
                    {
                        nodoSucesor.estado[indexI, indexJ] = nodoActual.estado[indexI++, indexJ];
                        nodoSucesor.estado[indexI++, indexJ] = 0;
                        nodoSucesor.heuristicaVal = Heuristica(nodoSucesor.estado);
                        nodoSucesor.operadores.Enqueue(operador);
                        return nodoSucesor;
                    }
                    else return default(nodo);

                case '>':
                    if (indexJ > 0)
                    {
                        nodoSucesor.estado[indexI, indexJ] = nodoActual.estado[indexI, indexJ--];
                        nodoSucesor.estado[indexI, indexJ--] = 0;
                        nodoSucesor.heuristicaVal = Heuristica(nodoSucesor.estado);
                        nodoSucesor.operadores.Enqueue(operador);
                        return nodoSucesor;
                    }
                    else return default(nodo);
                    
                case 'v':
                    if (indexI > 0)
                    {
                        nodoSucesor.estado[indexI, indexJ] = nodoActual.estado[indexI--, indexJ];
                        nodoSucesor.estado[indexI--, indexJ] = 0;
                        nodoSucesor.heuristicaVal = Heuristica(nodoSucesor.estado);
                        nodoSucesor.operadores.Enqueue(operador);
                        return nodoSucesor;
                    }
                    else return default(nodo);

                default:
                    return default(nodo);
            }
        }
            
         /* FUNCION SUCESORES(NODO)
            1. Hacer SUCESORES vacío
            2. Para cada OPERADOR en *OPERADORES*,
                si SUCESOR(NODO,OPERADOR) =/= NO-APLICABLE,
                incluir SUCESOR(NODO,OPERADOR) en SUCESORES
            3. Devolver SUCESORES
         */
        private Queue<nodo> Sucesores(nodo nodoActual)
        {
            Queue<nodo> listaSucesores = new Queue<nodo>();
            foreach (char operador in Operadores)
            {
                nodo aux = new nodo();
                aux = Sucesor(nodoActual, operador);

                if (aux.estado != null)
                    listaSucesores.Enqueue(aux);
            }

            return listaSucesores;
        }

        /* FUNCION BUSQUEDA-POR-PRIMERO-EL-MEJOR()
            1. Hacer ABIERTOS la cola formada por el nodo inicial (el nodo
                cuyo estado es *ESTADO-INICIAL*, cuyo camino es vacío
                y cuya heurística es la de *ESTADO-INICIAL*);
                Hacer CERRADOS vacío
         *  2. Mientras que ABIERTOS no esté vacía,
                2.1 Hacer ACTUAL el primer nodo de ABIERTOS
                2.2 Hacer ABIERTOS el resto de ABIERTOS
                2.3 Poner el nodo ACTUAL en CERRADOS.
                2.4 Si ES-ESTADO-FINAL(ESTADO(ACTUAL)),
         *          2.4.1 devolver el nodo ACTUAL y terminar.
                    2.4.2 en caso contrario,
         *              2.4.2.1 Hacer NUEVOS-SUCESORES la lista de nodos
                            de SUCESORES(ACTUAL) cuyo estado no está
                            ni en ABIERTOS ni en CERRADOS
                        2.4.2.2 Hacer ABIERTOS el resultado de incluir
                            NUEVOS-SUCESORES en ABIERTOS y ordenar
                            en orden creciente de sus heurísticas
         *      3. Devolver FALLO.
         */
        public nodo DoBestFirst(nodo estadoInicial)
        {
            return default(nodo);
        }
    }
}

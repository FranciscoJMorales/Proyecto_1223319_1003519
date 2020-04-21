using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClasesGenericas.Interfaces;

namespace ClasesGenericas.Estructuras
{
    public class ColaPrioridad<IComparable>
    {
        private Nodo<IComparable> Raiz { get; set; }
        public int Count { get; set; } = 0;

        public void Add(IComparable nuevo, Comparison<IComparable> comparer)
        {
            if (Count == 0)
                Raiz = new Nodo<IComparable> { Valor = nuevo };
            else
            {
                Nodo<IComparable> posicion = Raiz;
                int n = Count + 1;
                Pila<int> direcciones = new Pila<int>();
                while (n > 1)
                {
                    direcciones.Push(n % 2);
                    n /= 2;
                }
                while (direcciones.Count > 1)
                {
                    if (direcciones.Pop() == 0)
                        posicion = posicion.Izquierda;
                    else
                        posicion = posicion.Derecha;
                }
                if (direcciones.Pop() == 0)
                {
                    posicion.Izquierda = new Nodo<IComparable> { Valor = nuevo };
                    posicion.Izquierda.Padre = posicion;
                    posicion = posicion.Izquierda;
                }
                else
                {
                    posicion.Derecha = new Nodo<IComparable> { Valor = nuevo };
                    posicion.Derecha.Padre = posicion;
                    posicion = posicion.Derecha;
                }
                while (posicion.Padre != null)
                {
                    if (comparer.Invoke(posicion.Valor, posicion.Padre.Valor) < 0)
                    {
                        IComparable aux = posicion.Valor;
                        posicion.Valor = posicion.Padre.Valor;
                        posicion.Padre.Valor = aux;
                    }
                    posicion = posicion.Padre;
                }
            }
            Count++;
        }

        public IComparable Remove(Comparison<IComparable> comparer)
        {
            if (Count > 0)
            {
                IComparable valorPorDevolver = Raiz.Valor;
                if (Count == 1)
                    Raiz = null;
                else
                {
                    Nodo<IComparable> posicion = Raiz;
                    int n = Count;
                    Pila<int> direcciones = new Pila<int>();
                    while (n > 1)
                    {
                        direcciones.Push(n % 2);
                        n /= 2;
                    }
                    while (direcciones.Count > 1)
                    {
                        if (direcciones.Pop() == 0)
                            posicion = posicion.Izquierda;
                        else
                            posicion = posicion.Derecha;
                    }
                    if (direcciones.Pop() == 0)
                    {
                        Raiz.Valor = posicion.Izquierda.Valor;
                        posicion.Izquierda = null;
                    }
                    else
                    {
                        Raiz.Valor = posicion.Derecha.Valor;
                        posicion.Derecha = null;
                    }
                    posicion = Raiz;
                    while (posicion.Izquierda != null)
                    {
                        if (posicion.Derecha != null)
                        {
                            if (comparer.Invoke(posicion.Valor, posicion.Izquierda.Valor) <= 0 && comparer.Invoke(posicion.Valor, posicion.Derecha.Valor) <= 0)
                                posicion = new Nodo<IComparable>();
                            else
                            {
                                if (comparer.Invoke(posicion.Derecha.Valor, posicion.Izquierda.Valor) < 0)
                                {
                                    IComparable aux = posicion.Valor;
                                    posicion.Valor = posicion.Derecha.Valor;
                                    posicion.Derecha.Valor = aux;
                                }
                                else
                                {
                                    IComparable aux = posicion.Valor;
                                    posicion.Valor = posicion.Izquierda.Valor;
                                    posicion.Izquierda.Valor = aux;
                                }
                            }
                        }
                        else
                        {
                            if (comparer.Invoke(posicion.Valor, posicion.Izquierda.Valor) <= 0)
                                posicion = new Nodo<IComparable>();
                            else
                            {
                                IComparable aux = posicion.Valor;
                                posicion.Valor = posicion.Izquierda.Valor;
                                posicion.Izquierda.Valor = aux;
                            }
                        }
                    }
                }
                Count--;
                return valorPorDevolver;
            }
            else
                return default(IComparable);
        }

        public void Delete(Comparison<IComparable> comparer)
        {
            Remove(comparer);
        }

        public IComparable Get()
        {
            if (Raiz != null)
                return Raiz.Valor;
            else
                return default(IComparable);
        }

        public void Clear()
        {
            Raiz = null;
            Count = 0;
        }
    }
}

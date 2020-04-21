﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClasesGenericas.Interfaces;

namespace ClasesGenericas.Estructuras
{
    public class AVL<IComparable> : EstructuraNoLineal<IComparable>, IEnumerable<IComparable>
    {
        private Nodo<IComparable> Raiz { get; set; }
        public int Count { get; set; } = 0;

        public override void Add(IComparable value, Comparison<IComparable> comparer)
        {
            if (Raiz == null)
            {
                Raiz = new Nodo<IComparable>
                {
                    Valor = value,
                    Padre = null,
                    Izquierda = null,
                    Derecha = null
                };
                Count++;
            }
            else
                Insert(value, Raiz, comparer);
        }

        private void Insert(IComparable value, Nodo<IComparable> position, Comparison<IComparable> comparer)
        {
            if (comparer.Invoke(value, position.Valor) > 0)
            {
                if (position.Derecha == null)
                {
                    position.Derecha = new Nodo<IComparable>
                    {
                        Valor = value,
                        Padre = position,
                        Izquierda = null,
                        Derecha = null
                    };
                    Count++;
                    Verificar(position);
                }
                else
                    Insert(value, position.Derecha, comparer);
            }
            else if (comparer.Invoke(value, position.Valor) < 0)
            {
                if (position.Izquierda == null)
                {
                    position.Izquierda = new Nodo<IComparable>
                    {
                        Valor = value,
                        Padre = position,
                        Izquierda = null,
                        Derecha = null
                    };
                    Count++;
                    Verificar(position);
                }
                else
                    Insert(value, position.Izquierda, comparer);
            }
        }

        public override IComparable Remove(IComparable value, Comparison<IComparable> comparer)
        {
            try
            {
                Nodo<IComparable> aux = Search(value, Raiz, comparer);
                if (aux.Derecha == null && aux.Izquierda == null)
                {
                    if (aux.Padre != null)
                    {
                        if (aux.Padre.Izquierda == aux)
                            aux.Padre.Izquierda = null;
                        else
                            aux.Padre.Derecha = null;
                    }
                    if (aux == Raiz)
                        Raiz = null;
                    if (aux.Padre != null)
                        Verificar(aux.Padre);
                }
                else if (aux.Derecha != null && aux.Izquierda != null)
                {
                    Nodo<IComparable> reemplazo = aux.Izquierda;
                    while (reemplazo.Derecha != null)
                    {
                        reemplazo = reemplazo.Derecha;
                    }
                    Delete(reemplazo.Valor, comparer);
                    aux.Valor = reemplazo.Valor;
                    /*if (aux.Padre != null)
                    {
                        if (aux.Padre.Izquierda == aux)
                        {
                            aux.Padre.Izquierda = reemplazo;
                        }
                        else
                        {
                            aux.Padre.Derecha = reemplazo;
                        }
                    }
                    reemplazo.Padre = aux.Padre;
                    aux.Izquierda.Padre = reemplazo;
                    aux.Derecha.Padre = reemplazo;
                    reemplazo.Izquierda = aux.Izquierda;
                    reemplazo.Derecha = reemplazo.Derecha;
                    if (aux == Raiz)
                        Raiz = reemplazo;*/
                }
                else
                {
                    if (aux.Padre != null)
                    {
                        if (aux.Padre.Izquierda == aux)
                        {
                            if (aux.Izquierda != null)
                            {
                                aux.Padre.Izquierda = aux.Izquierda;
                                aux.Izquierda.Padre = aux.Padre;
                            }
                            else
                            {
                                aux.Padre.Izquierda = aux.Derecha;
                                aux.Derecha.Padre = aux.Padre;
                            }
                        }
                        else
                        {
                            if (aux.Izquierda != null)
                            {
                                aux.Padre.Derecha = aux.Izquierda;
                                aux.Izquierda.Padre = aux.Padre;
                            }
                            else
                            {
                                aux.Padre.Derecha = aux.Derecha;
                                aux.Derecha.Padre = aux.Padre;
                            }
                        }

                    }
                    else
                    {
                        if (aux.Izquierda != null)
                        {
                            aux.Izquierda.Padre = aux.Padre;
                        }
                        else
                        {
                            aux.Derecha.Padre = aux.Padre;
                        }
                    }
                    if (aux == Raiz)
                    {
                        if (aux.Izquierda != null)
                        {
                            Raiz = aux.Izquierda;
                        }
                        else
                        {
                            Raiz = aux.Derecha;
                        }
                    }
                    if (aux.Padre != null)
                        Verificar(aux.Padre);
                }
                Count--;
                return aux.Valor;
            }
            catch
            {
                return default(IComparable);
            }
        }

        public override void Delete(IComparable value, Comparison<IComparable> comparer)
        {
            Remove(value, comparer);
        }

        public IComparable Search(IComparable value, Comparison<IComparable> comparer)
        {
            Nodo<IComparable> result = Search(value, Raiz, comparer);
            if (result != null)
                return result.Valor;
            else
                return default(IComparable);
        }

        private Nodo<IComparable> Search(IComparable value, Nodo<IComparable> position, Comparison<IComparable> comparer)
        {
            if (position != null)
            {
                if (comparer.Invoke(value, position.Valor) == 0)
                    return position;
                else
                {
                    if (comparer.Invoke(value, position.Valor) < 0)
                        return Search(value, position.Izquierda, comparer);
                    else
                        return Search(value, position.Derecha, comparer);
                }
            }
            else
                return new Nodo<IComparable>();
        }

        public override void Clear()
        {
            Raiz = null;
            Count = 0;
        }

        private void Inorden(Nodo<IComparable> position, List<IComparable> recorrido)
        {
            if (position.Izquierda != null)
                Inorden(position.Izquierda, recorrido);
            recorrido.Add(position.Valor);
            if (position.Derecha != null)
                Inorden(position.Derecha, recorrido);
        }

        private void Postorden(Nodo<IComparable> position, List<int> recorrido)
        {
            if (position.Izquierda != null)
                Postorden(position.Izquierda, recorrido);
            if (position.Derecha != null)
                Postorden(position.Derecha, recorrido);
            recorrido.Add(Altura(position));
        }

        private void Verificar(Nodo<IComparable> position)
        {
            if (FactorEquilibrio(position) > 1)
            {
                if (FactorEquilibrio(position.Derecha) == -1)
                {
                    //Rotacion doble a la izquierda
                    RotarDerecha(position.Derecha);
                }
                RotarIzquierda(position);
            }
            else if (FactorEquilibrio(position) < -1)
            {
                if (FactorEquilibrio(position.Izquierda) == 1)
                {
                    //Rotacion doble a la derecha
                    RotarIzquierda(position.Izquierda);
                }
                RotarDerecha(position);
            }
            if (position.Padre != null)
            {
                Verificar(position.Padre);
            }
        }

        private void RotarDerecha(Nodo<IComparable> position)
        {
            if (position == Raiz)
                Raiz = position.Izquierda;
            if (position.Padre != null)
            {
                if (position.Padre.Izquierda == position)
                    position.Padre.Izquierda = position.Izquierda;
                else
                    position.Padre.Derecha = position.Izquierda;
            }
            position.Izquierda.Padre = position.Padre;
            position.Padre = position.Izquierda;
            position.Izquierda = position.Padre.Derecha;
            if (position.Izquierda != null)
                position.Izquierda.Padre = position;
            position.Padre.Derecha = position;
        }

        private void RotarIzquierda(Nodo<IComparable> position)
        {
            if (position == Raiz)
                Raiz = position.Derecha;
            if (position.Padre != null)
            {
                if (position.Padre.Izquierda == position)
                    position.Padre.Izquierda = position.Derecha;
                else
                    position.Padre.Derecha = position.Derecha;
            }
            position.Derecha.Padre = position.Padre;
            position.Padre = position.Derecha;
            position.Derecha = position.Padre.Izquierda;
            if (position.Derecha != null)
                position.Derecha.Padre = position;
            position.Padre.Izquierda = position;
        }

        private int FactorEquilibrio(Nodo<IComparable> position)
        {
            int alturaDerecha = Altura(position);
            int alturaIzquierda = alturaDerecha;
            List<int> izquierda = new List<int>();
            List<int> derecha = new List<int>();
            if (position.Derecha != null)
                Postorden(position.Derecha, derecha);
            if (position.Izquierda != null)
                Postorden(position.Izquierda, izquierda);
            for (int i = 0; i < izquierda.Count; i++)
            {
                if (izquierda[i] > alturaIzquierda)
                    alturaIzquierda = izquierda[i];
            }
            for (int i = 0; i < derecha.Count; i++)
            {
                if (derecha[i] > alturaDerecha)
                    alturaDerecha = derecha[i];
            }
            return alturaDerecha - alturaIzquierda;
        }

        private int Altura(Nodo<IComparable> position)
        {
            if (position.Padre != null)
                return 1 + Altura(position.Padre);
            else
                return 0;
        }

        public IEnumerator<IComparable> GetEnumerator()
        {
            List<IComparable> recorrido = new List<IComparable>();
            if (Raiz != null)
            {
                Inorden(Raiz, recorrido);
                recorrido.Sort();
            }
            while (recorrido.Count > 0)
            {
                yield return recorrido[0];
                recorrido.Remove(recorrido[0]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

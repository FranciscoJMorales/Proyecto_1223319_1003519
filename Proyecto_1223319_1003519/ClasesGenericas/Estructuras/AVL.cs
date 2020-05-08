using System;
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

        //Agrega un nuevo valor al árbol
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

        //Método privado recursivo que busca la posición donde debe ir el nuevo valor
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
                    //Verifica si se necesita hacer rotaciones
                    Verificar(position);
                }
                else
                    Insert(value, position.Derecha, comparer);
            }
            else
            {
                //Los elementos repetidos se insertan a la izquierda
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
                    //Verifica si se necesita hacer rotaciones
                    Verificar(position);
                }
                else
                    Insert(value, position.Izquierda, comparer);
            }
        }

        //Elimina el valor especificado del árbol
        public override IComparable Remove(IComparable value, Comparison<IComparable> comparer)
        {
            try
            {
                List<Nodo<IComparable>> resultados = new List<Nodo<IComparable>>();
                Search(value, Raiz, comparer, resultados);
                if (resultados.Count > 0)
                {
                    Nodo<IComparable> aux = resultados[0];
                    //Caso donde el nodo no tiene hijos
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
                    //Caso donde el nodo tiene 2 hijos
                    else if (aux.Derecha != null && aux.Izquierda != null)
                    {
                        //Se busca y se elimina el reemplazo
                        Nodo<IComparable> reemplazo = aux.Izquierda;
                        while (reemplazo.Derecha != null)
                        {
                            reemplazo = reemplazo.Derecha;
                        }
                        Delete(reemplazo.Valor, comparer);
                        //Se cambia el valor del nodo
                        aux.Valor = reemplazo.Valor;
                    }
                    //Caso donde el nodo tiene un hijo
                    else
                    {
                        if (aux.Padre != null)
                        {
                            //Se revisa si es hijo izquierdo o derecho de su padre para entrelazar los nodos
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
                            //Si no tiene padre solamente se enlazan del lado de los hijos
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
                else
                    return default(IComparable);
            }
            catch
            {
                return default(IComparable);
            }
        }

        //Elimina un valor sin devolverlo
        public override void Delete(IComparable value, Comparison<IComparable> comparer)
        {
            Remove(value, comparer);
        }

        //Llama al método recursivo de búsquedas
        public List<IComparable> Search(IComparable value, Comparison<IComparable> comparer)
        {
            List<Nodo<IComparable>> result = new List<Nodo<IComparable>>();
            Search(value, Raiz, comparer, result);
            List<IComparable> searchResults = new List<IComparable>();
            //No devuelve los nodos, sino sus valores
            foreach (Nodo<IComparable> item in result)
                searchResults.Add(item.Valor);
            return searchResults;
        }

        //Método recursivo de búsqueda. Devuelve una lista con los nodos cuyos valores son iguales al valor buscado
        private void Search(IComparable value, Nodo<IComparable> position, Comparison<IComparable> comparer, List<Nodo<IComparable>> resultados)
        {
            if (position != null)
            {
                if (comparer.Invoke(value, position.Valor) == 0)
                {
                    //Si se encuentra el valor, se agrega a la lista de resultados, pero se sigue verificando en ambos hijos por los elementos repetidos
                    resultados.Add(position);
                    Search(value, position.Izquierda, comparer, resultados);
                    Search(value, position.Derecha, comparer, resultados);
                }
                else
                {
                    if (comparer.Invoke(value, position.Valor) > 0)
                        Search(value, position.Derecha, comparer, resultados);
                    else
                        Search(value, position.Izquierda, comparer, resultados);
                }
            }
        }

        //Vacía el árbol
        public override void Clear()
        {
            Raiz = null;
            Count = 0;
        }

        //Devuelve una lista con todos los elementos del árbol
        private void Inorden(Nodo<IComparable> position, List<IComparable> recorrido)
        {
            if (position.Izquierda != null)
                Inorden(position.Izquierda, recorrido);
            recorrido.Add(position.Valor);
            if (position.Derecha != null)
                Inorden(position.Derecha, recorrido);
        }

        //Devuelve una lista con las alturas de los nodos hijos de un subárbol
        private void Postorden(Nodo<IComparable> position, List<int> recorrido)
        {
            if (position.Izquierda != null)
                Postorden(position.Izquierda, recorrido);
            if (position.Derecha != null)
                Postorden(position.Derecha, recorrido);
            recorrido.Add(Altura(position));
        }

        //Verifica si es necesario hacer una rotación para mantener equilibrado el árbol
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

        //Rota a la derecha a partir de la raíz recibida
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

        //Rota a la izquierda a partir de la raíz recibida
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

        //Devuelve el factor de equilibrio del nodo recibido
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
            //Revisa la altura de todos los nodos en ambos lados para restar las alturas máximas de cada lado
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

        //Devuelve la altura del nodo recibido
        private int Altura(Nodo<IComparable> position)
        {
            if (position.Padre != null)
                return 1 + Altura(position.Padre);
            else
                return 0;
        }

        //Método heredado de IEnumerable
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

        //Método heredado de IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

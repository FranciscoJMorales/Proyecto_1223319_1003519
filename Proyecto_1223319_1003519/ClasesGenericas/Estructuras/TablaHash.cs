using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesGenericas.Estructuras
{
    //Tabla Hash utilizada para simular las camas de los hospitales
    //Maneja colisiones colocando el elemento en el próximo espacio vacío.
    //No permite más de 10 elementos.
    public class TablaHash<T>
    {
        public T[] Arreglo = new T[10];
        public bool isFull = false;

        //Si la tabla no está llena, agrega un nuevo elemento a la tabla
        public void Add(T value, Func<T, string> llave)
        {
            if (!isFull)
            {
                if (Arreglo[FuncionHash(llave(value))] == null)
                {
                    Arreglo[FuncionHash(llave(value))] = value;
                    //Revisa si se llenó la tabla al asignar el nuevo elemento
                    CheckIfFull();
                }
                else
                {
                    int posicionInicial = FuncionHash(llave(value));
                    int pos = posicionInicial + 1;
                    bool continuar = true;
                    if (pos == 10)
                        pos = 0;
                    //Revisa la próxima posición hasta encontrar una posición vacía
                    while (pos != posicionInicial && continuar)
                    {
                        if (Arreglo[pos] == null)
                        {
                            Arreglo[pos] = value;
                            CheckIfFull();
                            continuar = false;
                        }
                        else
                        {
                            pos++;
                            if (pos == 10)
                                pos = 0;
                        }
                    }
                }
            }
        }

        //Revisa si la tabla está llena y cambia el valor de la variable isFull
        private void CheckIfFull()
        {
            isFull = true;
            for (int i = 0; i < Arreglo.Length; i++)
            {
                if (Arreglo[i] == null)
                    isFull = false;
            }
        }

        //Elimina y devuelve un valor de la tabla hash
        public T Remove(T value, Func<T, string> llave)
        {
            T resultado = default(T);
            if (Arreglo[FuncionHash(llave(value))] != null)
            {
                if (llave(Arreglo[FuncionHash(llave(value))]) == llave(value))
                {
                    //Si está en la posición esperada, lo elimina y lo devuelve inmediatamente
                    resultado = Arreglo[FuncionHash(llave(value))];
                    Arreglo[FuncionHash(llave(value))] = default(T);
                }
                else
                {
                    int posicionInicial = FuncionHash(llave(value));
                    int pos = posicionInicial + 1;
                    bool continuar = true;
                    if (pos == 10)
                        pos = 0;
                    //Si está ocupada por otro valor, revisa el siguiente valor hasta encontrarlo, encontrar un vacío o dar una vuelta entera
                    while (pos != posicionInicial && continuar)
                    {
                        if (Arreglo[pos] != null)
                        {
                            if (llave(Arreglo[pos]).Equals(llave(value)))
                            {
                                resultado = Arreglo[pos];
                                Arreglo[pos] = default(T);
                                isFull = false;
                                continuar = false;
                            }
                            else
                            {
                                pos++;
                                if (pos == 10)
                                    pos = 0;
                            }
                        }
                        else
                            continuar = false;
                    }
                }
            }
            return resultado;
        }

        //Elimina un valor sin devolverlo
        public void Delete(T value, Func<T, string> llave)
        {
            Remove(value, llave);
        }

        //Busca el valor indicado
        public T Search(T value, Func<T, string> llave)
        {
            T resultado = default(T);
            if (Arreglo[FuncionHash(llave(value))] != null)
            {
                //Si está en el lugar esperado lo devuelve inmediatamente
                if (llave(Arreglo[FuncionHash(llave(value))]).Equals(llave(value)))
                    resultado = Arreglo[FuncionHash(llave(value))];
                else
                {
                    int posicionInicial = FuncionHash(llave(value));
                    int pos = posicionInicial + 1;
                    bool continuar = true;
                    if (pos == 10)
                        pos = 0;
                    //Si está ocupado por otro elemento, sigue revisando hasta encontrarlo, encontrar un valor vacío o dar la vuelta entera
                    while (pos != posicionInicial && continuar)
                    {
                        if (Arreglo[pos] != null)
                        {
                            if (llave(Arreglo[pos]).Equals(llave(value)))
                            {
                                resultado = Arreglo[pos];
                                continuar = false;
                            }
                            else
                            {
                                pos++;
                                if (pos == 10)
                                    pos = 0;
                            }
                        }
                        else
                            continuar = false;
                    }
                }
            }
            return resultado;
        }

        //Función que devuelve la posición donde se guardará el valor
        private int FuncionHash(string llave)
        {
            try
            {
                int pos = 0;
                for (int i = 0; i < llave.Length; i++)
                {
                    pos += int.Parse(llave.Substring(i, 1)) * 7;
                }
                return pos % Arreglo.Length;
            }
            catch
            {
                return (llave.Length * 7) % Arreglo.Length;
            }
        }
    }
}

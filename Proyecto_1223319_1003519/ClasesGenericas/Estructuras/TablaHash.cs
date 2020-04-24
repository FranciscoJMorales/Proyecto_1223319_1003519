using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesGenericas.Estructuras
{
    public class TablaHash<T>
    {
        private T[] Arreglo = new T[10];
        public bool isFull = false;

        public void Add(T value, Func<T, string> llave)
        {
            if (!isFull)
            {
                if (Arreglo[FuncionHash(llave(value))].Equals(default(T)))
                {
                    Arreglo[FuncionHash(llave(value))] = value;
                    CheckIfFull();
                }
                else
                {
                    int posicionInicial = FuncionHash(llave(value));
                    int pos = posicionInicial + 1;
                    bool continuar = true;
                    if (pos == 10)
                        pos = 0;
                    while (pos != posicionInicial && continuar)
                    {
                        if (Arreglo[pos].Equals(default(T)))
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

        private void CheckIfFull()
        {
            isFull = true;
            for (int i = 0; i < Arreglo.Length; i++)
            {
                if (Arreglo[i].Equals(default(T)))
                    isFull = false;
            }
        }

        public T Remove(T value, Func<T, string> llave)
        {
            T resultado = default(T);
            if (llave(Arreglo[FuncionHash(llave(value))]).Equals(llave(value)))
            {
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
                while (pos != posicionInicial && continuar)
                {
                    if (!Arreglo[pos].Equals(default(T)))
                    {
                        if (llave(Arreglo[pos]).Equals(llave(value)))
                        {
                            resultado = Arreglo[pos];
                            Arreglo[pos] = default(T);
                            isFull = false;
                            continuar = false;
                        }
                    }
                    else
                    {
                        pos++;
                        if (pos == 10)
                            pos = 0;
                    }
                }
            }
            return resultado;
        }

        public void Delete(T value, Func<T, string> llave)
        {
            Remove(value, llave);
        }

        public T Search(T value, Func<T, string> llave)
        {
            T resultado = default(T);
            if (llave(Arreglo[FuncionHash(llave(value))]).Equals(llave(value)))
                resultado = Arreglo[FuncionHash(llave(value))];
            else
            {
                int posicionInicial = FuncionHash(llave(value));
                int pos = posicionInicial + 1;
                bool continuar = true;
                if (pos == 10)
                    pos = 0;
                while (pos != posicionInicial && continuar)
                {
                    if (!Arreglo[pos].Equals(default(T)))
                    {
                        if (llave(Arreglo[pos]).Equals(llave(value)))
                        {
                            resultado = Arreglo[pos];
                            continuar = false;
                        }
                    }
                    else
                    {
                        pos++;
                        if (pos == 10)
                            pos = 0;
                    }
                }
            }
            return resultado;
        }

        public List<T> Items()
        {
            List<T> valores = new List<T>();
            foreach (T item in Arreglo)
            {
                if (!item.Equals(default(T)))
                {
                    valores.Add(item);
                }
            }
            return valores;
        }

        private int FuncionHash(string llave)
        {
            try
            {
                return (int.Parse(llave) * 7) % 10;
            }
            catch
            {
                return (llave.Length * 7) % 10;
            }
        }
    }
}

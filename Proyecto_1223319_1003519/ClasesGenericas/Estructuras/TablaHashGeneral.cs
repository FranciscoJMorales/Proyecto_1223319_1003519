﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesGenericas.Estructuras
{
    //Tabla Hash utilizada para almacenar todos los datos de todos los pacientes.
    //Maneja colisiones por medio de listas en cada espacio del arreglo.
    public class TablaHashGeneral<T>
    {
        private List<T>[] Arreglo = new List<T>[50];

        public TablaHashGeneral()
        {
            for (int i = 0; i < Arreglo.Length; i++)
                Arreglo[i] = new List<T>();
        }

        public void Add(T value, Func<T, string> llave)
        {
            if (!Arreglo[FuncionHash(llave(value))].Contains(value))
                Arreglo[FuncionHash(llave(value))].Add(value);
        }

        public T Remove(T value, Func<T, string> llave)
        {
            T resultado = default(T);
            for (int i = 0; i < Arreglo[FuncionHash(llave(value))].Count; i++)
            {
                if (llave(Arreglo[FuncionHash(llave(value))][i]).Equals(llave(value)))
                {
                    resultado = Arreglo[FuncionHash(llave(value))][i];
                    Arreglo[FuncionHash(llave(value))].Remove(Arreglo[FuncionHash(llave(value))][i]);
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
            foreach (T item in Arreglo[FuncionHash(llave(value))])
            {
                if (llave(item).Equals(llave(value)))
                    resultado = item;
            }
            return resultado;
        }

        public List<T> Items()
        {
            List<T> valores = new List<T>();
            foreach (List<T> list in Arreglo)
            {
                foreach (T item in list)
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
                return (int.Parse(llave) * 7) % Arreglo.Length;
            }
            catch
            {
                return (llave.Length * 7) % Arreglo.Length;
            }
        }
    }
}

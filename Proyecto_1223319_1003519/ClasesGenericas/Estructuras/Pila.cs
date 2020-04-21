﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClasesGenericas.Interfaces;

namespace ClasesGenericas.Estructuras
{
    public class Pila<T> : EstructuraLineal<T>
    {
        private NodoLineal<T> Head;
        public int Count { get; set; } = 0;

        public void Push(T value)
        {
            Add(value);
        }

        protected override void Add(T value)
        {
            if (Head == null)
                Head = new NodoLineal<T> { Valor = value, Anterior = null, Siguiente = null };
            else
            {
                Head.Siguiente = new NodoLineal<T> { Valor = value, Anterior = Head, Siguiente = null };
                Head = Head.Siguiente;
            }
            Count++;
        }

        public override void Delete()
        {
            Pop();
        }

        public T Get()
        {
            if (Head != null)
                return Head.Valor;
            else
                return default(T);
        }

        public T Pop()
        {
            return Remove();
        }

        protected override T Remove()
        {
            if (Head != null)
            {
                T valor = Head.Valor;
                Head = Head.Anterior;
                if (Head != null)
                    Head.Siguiente = null;
                Count--;
                return valor;
            }
            else
                return default(T);
        }

        public override void Clear()
        {
            Head = null;
            Count = 0;
        }
    }
}

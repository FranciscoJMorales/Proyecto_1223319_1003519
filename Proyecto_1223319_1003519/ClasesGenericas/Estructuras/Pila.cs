using System;
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

        //Llama al método Add
        public void Push(T value)
        {
            Add(value);
        }

        //Ingresa un valor a la pila
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

        //Elimina un valor sin devolverlo
        public override void Delete()
        {
            Pop();
        }

        //Devuelve el valor del primer elemento
        public T Get()
        {
            if (Head != null)
                return Head.Valor;
            else
                return default(T);
        }

        //Llama al método heredado remove
        public T Pop()
        {
            return Remove();
        }

        //Elimina el primer elemento y lo devuelve
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

        //Vacía la pila
        public override void Clear()
        {
            Head = null;
            Count = 0;
        }
    }
}

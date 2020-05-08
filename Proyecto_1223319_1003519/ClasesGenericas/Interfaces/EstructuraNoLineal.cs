using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClasesGenericas.Estructuras;

namespace ClasesGenericas.Interfaces
{
    //Interfaz para las estructuras de datos no lineales
    public abstract class EstructuraNoLineal<T>
    {
        public abstract void Add(T value, Comparison<T> comparer);
        public abstract void Delete(T value, Comparison<T> comparer);
        public abstract T Remove(T value, Comparison<T> comparer);
        public abstract void Clear();
    }
}

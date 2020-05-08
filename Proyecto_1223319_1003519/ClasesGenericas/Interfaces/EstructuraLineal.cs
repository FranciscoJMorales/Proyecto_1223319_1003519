using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesGenericas.Interfaces
{
    //Interfaz para las estructuras de datos lineales
    public abstract class EstructuraLineal<T>
    {
        protected abstract void Add(T value);
        public abstract void Delete();
        protected abstract T Remove();
        public abstract void Clear();
    }
}

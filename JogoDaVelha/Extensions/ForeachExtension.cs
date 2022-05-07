using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoDaVelha.Extensions
{
    static class ForeachExtension
    {
        public static IEnumerable<(T item, int index)> LoopIndex<T>(this IEnumerable<T> self) 
            => self.Select((item, index) => (item,index));
    }
}

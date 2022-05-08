using System.Collections.Generic;
using System.Linq;

namespace JogoDaVelhaConsole
{
    static class ForeachExtension
    {
        public static IEnumerable<(T item, int index)> LoopIndex<T>(this IEnumerable<T> self) 
            => self.Select((item, index) => (item,index));
    }
}

using ME.BECS.Internal;

namespace ME.BECS.Addons.WorldData.Runtime
{
    public class ConcreteWorldData<T> where T : unmanaged
    {
        private static readonly Unity.Burst.SharedStatic<Array<T>> worldsArrBurst = Unity.Burst.SharedStatic<Array<T>>.GetOrCreatePartiallyUnsafeWithHashCode<WorldsStorage>(TAlign<Array<T>>.align, 10033);
        internal static ref Array<T> worlds => ref worldsArrBurst.Data;

        static ConcreteWorldData() {
            for (ushort i = 0; i < worlds.Length; ++i) {
                if (Worlds.IsAlive(i))
                {
                    worlds.Get(i) = default;
                }
            }
            worlds.Dispose();
        }
    }
}
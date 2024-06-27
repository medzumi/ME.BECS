namespace ME.BECS.Addons.WorldData.Runtime
{
    public unsafe static class WorldData
    {
        public static ref T Get<T>(in this World world) where T : unmanaged
        {
            ConcreteWorldData<T>.worlds.Resize(world.id + 1u);
            ref var ptr = ref ConcreteWorldData<T>.worlds.Get(world.id);
            return ref ptr;
        }
    }
}
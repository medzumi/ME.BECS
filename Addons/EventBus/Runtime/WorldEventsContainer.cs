using System.Runtime.InteropServices;
using ME.BECS.Addons.WorldData.Runtime;

namespace ME.BECS.Addons.EventBus.Runtime
{
    public unsafe struct EntEventsContainer<T>
    {
        public List<ClassPtr<EntEventsContainer<T>.Event>> Actions;
        public List<ClassPtr<Condition>> Conditions;

        public delegate void Event(in Ent ent, ref T data);
        
        public delegate bool Condition(in Ent ent, ref T data);

        public void Dispose(in World world)
        {
            if (world.isCreated)
            {
                for (uint i = 0; i < Actions.Count; i++)
                {
                    var element = Actions[world.state, i];
                    Actions[world.state, i].Dispose();
                }

                if (Actions.isCreated)
                {
                    Actions.Dispose(ref world.state->allocator);
                }
            }
        }
    }
    
    public unsafe struct WorldEventsContainer<T>
    {
        public List<ClassPtr<WorldEventsContainer<T>.Event>> Actions;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Event(in World world, ref T data);

        public void Dispose(in World world)
        {
            if (world.isCreated)
            {
                for (uint i = 0; i < Actions.Count; i++)
                {
                    var element = Actions[world.state, i];
                    Actions[world.state, i].Dispose();
                }

                if (Actions.isCreated)
                {
                    Actions.Dispose(ref world.state->allocator);
                }
            }
        }
    }
}
using System;
using ME.BECS.Addons.WorldData.Runtime;

namespace ME.BECS.Addons.EventBus.Runtime
{
    using static Cuts;

    public unsafe static class EventBusModule
    {
        public static void Subscribe<T>(in this World world, WorldEventsContainer<T>.Event action)
        {
            ref var actions = ref world.Get<WorldEventsContainer<T>>().Actions;
            if (!actions.isCreated)
            {
                actions = new List<ClassPtr<WorldEventsContainer<T>.Event>>(ref world.state->allocator, 1);
            }
            
            var ptr = _classPtr(action);
            if (!actions.Contains(world.state->allocator, ptr))
            {
                actions.Add(ref world.state->allocator, _classPtr(action));
            }
            else
            {
                ptr.Dispose();
            }
        }

        public static void Subscribe<T>(in this World world, EntEventsContainer<T>.Event action)
        {
            ref var actions = ref world.Get<EntEventsContainer<T>>().Actions;
            if (!actions.isCreated)
            {
                actions = new List<ClassPtr<EntEventsContainer<T>.Event>>(ref world.state->allocator, 1);
            }

            var ptr = _classPtr(action);
            if (!actions.Contains(world.state->allocator, ptr))
            {
                actions.Add(ref world.state->allocator, _classPtr(action));
            }
            else
            {
                ptr.Dispose();
            }
        }

        public static void AddCondition<T>(in this World world, EntEventsContainer<T>.Condition condition)
        {
            ref var conditions = ref world.Get<EntEventsContainer<T>>().Conditions;
            if (!conditions.isCreated)
            {
                conditions = new List<ClassPtr<EntEventsContainer<T>.Condition>>(ref world.state->allocator, 1);
            }

            var ptr = _classPtr(condition);
            if (!conditions.Contains(world.state->allocator, ptr))
            {
                conditions.Add(ref world.state->allocator, ptr);
            }
            else
            {
                ptr.Dispose();
            }
        }
        
        public static void Unsubscribe<T>(in this World world, EntEventsContainer<T>.Event action)
        {
            ref var actions = ref world.Get<EntEventsContainer<T>>().Actions;
            if (actions.isCreated)
            {
                world.Get<EntEventsContainer<T>>().Actions.Remove(ref world.state->allocator, _classPtr(action));
            }
        }

        public static void Unsubscribe<T>(in this World world, WorldEventsContainer<T>.Event action) 
        {
            ref var actions = ref world.Get<WorldEventsContainer<T>>().Actions;
            if (actions.isCreated)
            {
                world.Get<WorldEventsContainer<T>>().Actions.Remove(ref world.state->allocator, _classPtr(action));
            }
        }

        public static void Push<T>(in this World world, ref T data) 
        {
            var actions = world.Get<WorldEventsContainer<T>>().Actions;
            for (uint i = 0; i < actions.Count; i++)
            {
                actions[world.state, i].Value.Invoke(world, ref data);
            }
        }

        public static void RegisterCondition<T>(in this World world, EntEventsContainer<T>.Condition condition)
        {
            ref var conditions = ref world.Get<EntEventsContainer<T>>().Conditions;
            if (!conditions.isCreated)
            {
                conditions = new List<ClassPtr<EntEventsContainer<T>.Condition>>(ref world.state->allocator, 1);
            }

            var ptr = _classPtr(condition);
            if (!conditions.Contains(world.state->allocator, ptr))
            {
                conditions.Add(ref world.state->allocator, ptr);
            }
        }
        
        public static void Push<T>(in this Ent ent, ref T data)
        {
            var world = ent.World;
            ref var state = ref world.state;
            var eventsContainer = world.Get<EntEventsContainer<T>>();
            var actions = eventsContainer.Actions;
            var conditions = eventsContainer.Conditions;
            var isValid = false;
            for (uint i = 0; i < conditions.Count; i++)
            {
                isValid |= conditions[state, i].Value.Invoke(ent, ref data);
            }

            if (isValid)
            {
                for (uint i = 0; i < actions.Count; i++)
                {
                    actions[world.state, i].Value.Invoke(in ent, ref data);
                }
            }
        } 
        
        public static void Push<T>(in this Ent ent, T data)
        {
            var world = ent.World;
            var actions = world.Get<EntEventsContainer<T>>().Actions;
            for (uint i = 0; i < actions.Count; i++)
            {
                actions[world.state, i].Value.Invoke(in ent, ref data);
            }
        } 
    }
}
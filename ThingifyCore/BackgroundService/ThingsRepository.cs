using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using ThingifyCore.Models;

namespace ThingifyCore.BackgroundService;

public class ThingsRepository : IThingsRepository
{
    private ConcurrentBag<ThingConcurrent> _things = new ConcurrentBag<ThingConcurrent>();

    private ConcurrentDictionary<string, ThingConcurrent> _thingsLookup = new ConcurrentDictionary<string, ThingConcurrent>();

    public IEnumerable<Thing> Get()
    {
        return _things.Select(t => t.ToThing());
    }

    public void Upsert(IEnumerable<Thing> things)
    {
        UpsertThings(things, _things);
    }

    private void UpsertThings(IEnumerable<Thing> things, ConcurrentBag<ThingConcurrent> thingsToUpdate)
    {
        foreach(var thing in things)
        {
            if (string.IsNullOrWhiteSpace(thing.Id))
            {
                throw new ArgumentException("Id of thing empty");
            }
            
            if (_thingsLookup.TryGetValue(thing.Id, out ThingConcurrent? existingThing))
            {
                // already exists > update
                UpsertThing(thing, existingThing);

                // upsert children
                if (thing.Things != null)
                {
                    if (thing.Things.Any())
                    {
                        UpsertThings(thing.Things, existingThing.Things);
                    }
                }
            }
            else
            {
                // does not exist yet > add 
                var thingConcurrent = new ThingConcurrent(thing);

                if (_thingsLookup.TryAdd(thing.Id, thingConcurrent))
                {
                    thingsToUpdate.Add(thingConcurrent);
                }
            }
        }

        // TODO remove things
    }

    private void UpsertThing(Thing thing, ThingConcurrent existingThing)
    {
        if (thing.Measurements != null)
        {
            foreach(var measurement in thing.Measurements)
            {
                existingThing.Measurements.Add(measurement);
            }
        }
    }
}
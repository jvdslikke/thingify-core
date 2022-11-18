using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using ThingifyCore.Models;
using System;

namespace ThingifyCore.BackgroundService;

public class ThingConcurrent
{
    public string Id { get; set; }

    public string? Description { get; set; }

    public ConcurrentBag<ThingConcurrent> Things { get; set; }

    public string? MeasurementUnit { get; set; }

    public IEnumerable<object> MeasurementPossibleValues { get; set; }

    public ConcurrentBag<Measurement> Measurements { get; set; }

    public ThingConcurrent(Thing thing)
    {
        Id = thing.Id ?? throw new ArgumentNullException(nameof(thing.Id));
        Description = thing.Description;
        Things = new ConcurrentBag<ThingConcurrent>(thing.Things?.Select(t => new ThingConcurrent(t)) ?? Enumerable.Empty<ThingConcurrent>());
        MeasurementUnit = thing.MeasurementUnit;
        MeasurementPossibleValues = thing.MeasurementPossibleValues ?? Enumerable.Empty<object>();
        Measurements = new ConcurrentBag<Measurement>(thing.Measurements ?? Enumerable.Empty<Measurement>());
    }

    public Thing ToThing()
    {
        return new Thing(Id)
        {
            Description = Description,
            Things = Things.Select(t => t.ToThing()).ToList(),
            MeasurementUnit = MeasurementUnit,
            MeasurementPossibleValues = MeasurementPossibleValues.ToList(),
            Measurements = Measurements.ToList()
        };
    }
}
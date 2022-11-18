using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ThingifyCore.Models;

public class Thing
{
    [Required]
    public string Id { get; set; }

    public string? Description { get; set; }

    public List<Thing> Things { get; set; }

    public string? MeasurementUnit { get; set; }

    public List<object> MeasurementPossibleValues { get; set; }

    public List<Measurement> Measurements { get; set; }

    [JsonConstructorAttribute]
    public Thing(string id)
        : this(
            id: id,
            things: new List<Thing>())
    {
    }

    public Thing(
        string id, 
        List<Thing> things)
        : this(
            id: id,
            things: things,
            measurementPossibleValues: new List<object>(),
            measurements: new List<Measurement>())
    {   
    }

    public Thing(
        string id,
        List<Thing> things,
        List<object> measurementPossibleValues,
        List<Measurement> measurements)
    {
        Id = id;
        Things = things;
        MeasurementPossibleValues = measurementPossibleValues;
        Measurements = measurements;
    }
}
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;
using ThingifyCore.Models;
using ThingifyCore.BackgroundService;

namespace ThingifyCore.Tests.BackgroundServiceTests;

public class ThingsRepositoryTests
{
    [Fact]
    public void TestUpsertMultipleNew()
    {
        var thingsRepository = new ThingsRepository();

        var thing = new Thing("test");
        
        var thingUpdatedWithSub = new Thing(
            id: "test",
            things: new List<Thing>
            {
                new Thing("subtest")
                {
                }
            });

        var thingUpdatedWithSubSub = new Thing(
            id: "test",
            things: new List<Thing>
            {
                new Thing(
                    id: "subtest",
                    things: new List<Thing>
                    {
                        new Thing("subsubtest")
                    })
            });

        var subThingUpdated = new Thing("subtest")
        {
            Measurements = new List<Measurement>
            {
                new Measurement(
                    dateTime: DateTime.Now,
                    value: 0
                )
            }
        };

        thingsRepository.Upsert(new [] { thing });
        thingsRepository.Upsert(new [] { thingUpdatedWithSub });
        thingsRepository.Upsert(new [] { thingUpdatedWithSubSub });
        thingsRepository.Upsert(new [] { subThingUpdated });

        Assert.True(thingsRepository.Get().Count() == 1);
        Assert.True(thingsRepository.Get().First().Id == "test");

        var thingThings = thingsRepository.Get().First().Things;
        Assert.True(thingThings != null);
        if (thingThings != null)
        {
            Assert.True(thingThings.Count() == 1);
            Assert.True(thingThings.First().Id == "subtest");

            var thingThingThings = thingThings.First().Things;
            Assert.True(thingThingThings != null);

            if (thingThingThings != null)
            {
                Assert.True(thingThingThings.Count() == 1);
                Assert.True(thingThingThings.First().Id == "subsubtest");
            }

            var thingThingMeasurements = thingThings.First().Measurements;
            Assert.True(thingThingMeasurements != null);
            if (thingThingMeasurements != null)
            {
                Assert.True(thingThingMeasurements.Count() == 1);                
            }
        }
    }
}
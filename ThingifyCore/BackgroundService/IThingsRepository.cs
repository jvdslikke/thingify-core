using System.Collections.Generic;
using ThingifyCore.Models;

namespace ThingifyCore.BackgroundService;

public interface IThingsRepository
{
    IEnumerable<Thing> Get();

    void Upsert(IEnumerable<Thing> things);
}
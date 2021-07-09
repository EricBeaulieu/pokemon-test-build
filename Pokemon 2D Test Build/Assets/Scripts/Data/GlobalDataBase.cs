using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public static class GlobalDataBase
{
    public static void InitializeAllDatabases()
    {
        ConditionsDB.Initialization(GetAllConditions().ToList());
        EntryHazardsDB.Initialization(GetAllEntryHazards().ToList());
        WeatherEffectDB.Initialization(GetAllWeatherEffects().ToList());
        AbilityDB.Initialization(GetAllAbilities().ToList());
        HoldItemDB.Initialization(GetAllHoldItems().ToList());
    }

    static IEnumerable<ConditionBase> GetAllConditions()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ConditionBase)))
            .Select(type => Activator.CreateInstance(type) as ConditionBase);
    }

    static IEnumerable<EntryHazardBase> GetAllEntryHazards()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(EntryHazardBase)))
            .Select(type => Activator.CreateInstance(type) as EntryHazardBase);
    }

    static IEnumerable<WeatherEffectBase> GetAllWeatherEffects()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(WeatherEffectBase)))
            .Select(type => Activator.CreateInstance(type) as WeatherEffectBase);
    }

    static IEnumerable<AbilityBase> GetAllAbilities()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(AbilityBase)))
            .Select(type => Activator.CreateInstance(type) as AbilityBase);
    }

    static IEnumerable<HoldItemBase> GetAllHoldItems()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(HoldItemBase)))
            .Select(type => Activator.CreateInstance(type) as HoldItemBase);
    }
}

using System;
using System.Threading.Tasks;
using CombatSystemDemo.Devices;

namespace CombatSystemDemo;

internal class Program
{
    static async Task Main(string[] args)
    {
        var c4I = new C4I();
        //await c4I.Import();
        await c4I.ExportMission();
        Console.ReadLine();

    }
}
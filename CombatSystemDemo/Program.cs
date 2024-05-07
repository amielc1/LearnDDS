using System;
using System.Threading.Tasks;
using CombatSystemDemo.Devices;

namespace CombatSystemDemo;

internal class Program
{
    static async Task Main(string[] args)
    {
        var c4I = new C4I();
        await c4I.Import();
        for (int i = 0; i < 1000; i++)
        { 
            await c4I.ExportMission();
            await c4I.ExportLocation();
            await Task.Delay(100);
        }
        Console.ReadLine();
    }
}
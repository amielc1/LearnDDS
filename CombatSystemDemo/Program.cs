using System;
using System.Threading.Tasks;
using CombatSystemDemo.Devices;

namespace CombatSystemDemo;

internal class Program
{
    static async Task Main(string[] args)
    {
        var radar = new Radar();
        var launcher = new Launcher();
        var c4I = new C4I(); 
        Task.Run(async () => { await radar.Export(); });

        await c4I.Import();
        await launcher.Import();
    
       

        Console.ReadLine();
    }
}
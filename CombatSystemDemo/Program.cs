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

        await launcher.Import(); 
        await radar.Export();  

        Console.ReadLine();
    }
}
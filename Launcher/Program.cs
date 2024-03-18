// See https://aka.ms/new-console-template for more information

using CombatSystemDemo.Devices;

Console.WriteLine("Hello, World!");


var launcher = new Launcher();
await launcher.Import();


Console.ReadLine();
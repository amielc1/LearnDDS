using CombatSystemDemo.Devices;

var c4I = new C4I();
await c4I.Import();
await c4I.ImportMis();
Console.ReadLine();
using System;
using CombatSystem.Devices;
using CombatSystemDemo.Devices;

namespace CombatSystemDemo;

internal class Program
{
    static void Main(string[] args)
    {
      Launcher launcher = new Launcher();
      launcher.Import();


      Console.ReadLine();
    }
}
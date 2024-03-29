﻿using System.Text;
using FrooxEngine.FinalIK;

namespace ResoniteWikiMine.Commands;

public sealed class CreateEnumValues : ICommand
{
    public Task<int> Run(WorkContext context, string[] args)
    {
        // This is so types are available.
        FrooxLoader.InitializeFrooxWorker();

        foreach (var arg in args)
        {
            var type = FrooxLoader.FindFrooxType(arg);
            if (type == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unable to find type: {arg}");
                Console.ResetColor();
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(arg);
            Console.ResetColor();

            var sb = new StringBuilder();
            sb.AppendLine($$$"""
                {{Infobox Enum
                |name={{{type.Name}}}
                |type={{{type.FullName}}}
                """);

            if (type.IsNested)
                sb.AppendLine("|nested=true");

            sb.AppendLine("""
                }}

                {{stub}}

                {{Table EnumValues
                """);

            foreach (var name in Enum.GetNames(type).OrderBy(name => Enum.Parse(type, name)))
            {
                var value = (int) Enum.Parse(type, name);
                sb.AppendLine($"|{name}|{value}|");
            }

            sb.AppendLine("}}");

            Console.WriteLine(sb.ToString());
        }

        return Task.FromResult(0);
    }
}

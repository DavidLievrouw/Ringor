using System;
using System.Linq;

namespace Dalion.Ringor.Build.Startup {
    public class InteractiveOptions {
        public static Options Prompt(Options options) {
            var possibleActions = new (string Label, Action Invoke)[] {
                ("Quit", o => null),
                ("Restore packages", RestorePackages),
                ("Publish for Azure files", PublishAzureFiles),
                ("Publish for Web Deploy", PublishWebDeploy),
                ("Run unit tests", RunTests)
            };

            using (new TemporaryConsoleColor(ConsoleColor.Cyan)) {
                Console.WriteLine("What do you want to do? Possible actions:");
                var possibleActionsOverview = string.Join(Environment.NewLine,
                    possibleActions.Select((action, i) => $"{i}. {action.Label}"));
                Console.WriteLine(possibleActionsOverview);
            }
            Console.Write($"Action [0-{possibleActions.Length - 1}]: ");
            var chosenAction = ReadChosenAction(possibleActions);
            return chosenAction.Invoke(options.Clone());
        }

        private static (string Label, Action Invoke) ReadChosenAction((string Label, Action Invoke)[] possibleActions) {
            var response = Console.ReadLine();
            if (response == null || !int.TryParse(response, out var index) || index < 0 || index >= possibleActions.Length) {
                Console.Error.Write("Invalid response, type a valid action number: ");
                return ReadChosenAction(possibleActions);
            }
            return possibleActions[index];
        }

        private static Options RestorePackages(Options options) {
            options.Target = nameof(Tasks.Restore.RestorePackages);
            return options;
        }

        private static Options PublishAzureFiles(Options options) {
            options.Target = nameof(Tasks.Publish.PublishAzureFiles);
            return options;
        }

        private static Options PublishWebDeploy(Options options) {
            options.Target = nameof(Tasks.Publish.PublishWebDeploy);
            return options;
        }

        private static Options RunTests(Options options) {
            options.Target = nameof(Tasks.Test.UnitTest);
            options.Configuration = ConfigurationOptions.Debug;
            return options;
        }
        
        private delegate Options Action(Options options);
    }
}
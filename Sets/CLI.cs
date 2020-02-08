using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sets
{
    // ReSharper disable once InconsistentNaming
    public class CLI
    {
        private readonly Dictionary<string, Operation> _commands;
        private Context _currentContext = Context.NoTest;
        private State _currentState = State.Normal;

        private Set _set1;
        private Set _set2;

        public CLI() => _commands = new Dictionary<string, Operation>
        {
            {"exit", new Operation("stop execution and exit", Context.All, OnExit)},
            {"single", new Operation("start test with single set", Context.NoTest, OnSingleSetTest)},
            {"two", new Operation("start test with two sets", Context.NoTest, OnTwoSetsTest)},
            {"union", new Operation("print union of two sets", Context.TwoSetsTest, OnUnion)},
            {"intersect", new Operation("print intersection of two sets", Context.TwoSetsTest, OnIntersect)},
            {"add", new Operation("add <val>: add value to set", Context.SingleSetTest, OnAdd)},
            {"remove", new Operation("remove <val>: remove value from set", Context.SingleSetTest, OnRemove)},
            {
                "contains", new Operation("contains <val>: check whether value is present in set",
                    Context.SingleSetTest,
                    OnContains)
            },
            {"show", new Operation("print set contents", Context.SingleSetTest, OnShow)},
            {"help", new Operation("print info about all available commands", Context.All, OnHelp)},
            {"finish", new Operation("finish current test", Context.AnyTest, OnFinish)},
            {"logical", new Operation("use logical set representation", Context.SetTypeSelection)},
            {"bit", new Operation("use bit set representation", Context.SetTypeSelection)},
        };

        private void ReadSetContent(Set set)
        {
            Console.Write("Enter set contents (either a sequence of numbers separated by whitespace," +
            " or a valid path to file in quotes): ");

            var input = Console.ReadLine().Trim();
            if (Regex.IsMatch(input, "^['\"].+['\"]$"))
            {
                using var reader = new StreamReader(input);
                var nums = reader.ReadToEnd().Split('\n').Select(int.Parse);
                set.Append(nums.ToArray());
            }
            else
            {
                set.Append(input);
            }
        }

        private void OnFinish(string args) => _currentState = State.Finishing;

        public void ProcessInput()
        {
            Welcome();
            OnHelp(null);
            ExecuteEventLoop(Context.NoTest, State.Exiting);
        }

        private void OnError(Exception e, bool detailed = false) =>
            Console.WriteLine(detailed ? e.ToString() : e.Message);

        private void OnExit(string args)
        {
            _currentState = State.Exiting;
            Console.WriteLine("Thank you and good luck!");
        }

        private void OnSingleSetTest(string args)
        {
            _currentContext = Context.SingleSetTest;

            var length = ReadSetLength();
            _set1 = SelectRepresentation(length);
            ReadSetContent(_set1);

            ExecuteEventLoop(Context.SingleSetTest, State.Finishing);

            _currentContext = Context.NoTest;
        }

        private void ExecuteEventLoop(Context allowed, State exit)
        {
            while (!_currentState.HasFlag(exit))
            {
                var command = WaitForCommand(allowed);
                command.Operation.Execute(command.Args);
            }
        }

        private Set SelectRepresentation(int length)
        {
            Console.Write("Enter representation of the set: ");

            var command = WaitForCommand(Context.SetTypeSelection);
            // TODO: Handle help/finish/exit commands
            return command.Name switch
            {
                "logical" => new SimpleSet(length),
                "bit" => new BitSet(length),
                _ => throw new ArgumentException("Can't create set representation.")
            };
        }

        private int ReadSetLength()
        {
            Console.Write("Enter length of the set: ");

            var length = 0;
            var success = false;
            while (!success && !_currentState.HasFlag(State.Finishing))
            {
                success = int.TryParse(Console.ReadLine(), out length);
            }

            return length;
        }

        private void OnTwoSetsTest(string args)
        {
            _currentContext = Context.TwoSetsTest;

            Console.WriteLine("Set #1:");
            var len1 = ReadSetLength();
            _set1 = SelectRepresentation(len1);
            ReadSetContent(_set1);

            Console.WriteLine("\nSet #2:");
            var len2 = ReadSetLength();
            _set2 = SelectRepresentation(len2);
            ReadSetContent(_set2);

            ExecuteEventLoop(Context.TwoSetsTest, State.Finishing);

            _currentContext = Context.NoTest;
        }

        private void OnUnion(string args) => Console.WriteLine(_set1 + _set2);

        private void OnIntersect(string args) => Console.WriteLine(_set1 * _set2);

        private void OnAdd(string args)
        {
            var num = int.Parse(args);
            try
            {
                _set1.Add(num);
            }
            catch (OutOfSetRangeException e)
            {
                OnError(e);
            }
        }

        private void OnRemove(string args)
        {
            var num = int.Parse(args);
            _set1.Remove(num);
        }

        private void OnContains(string args)
        {
            var num = int.Parse(args);
            Console.WriteLine(_set1.Contains(num));
        }

        private void OnShow(string args)
        {
            switch (_currentContext)
            {
                case Context.SingleSetTest:
                    Console.WriteLine(_set1);
                    break;
                case Context.TwoSetsTest:
                    Console.WriteLine($"{_set1}; {_set2}");
                    break;
                default:
                    throw new InvalidOperationException("Can't print content of set/sets if no test was started.");
            }
        }

        private void Welcome()
        {
            Console.WriteLine("\n==========================================================\n");
            Console.WriteLine("Welcome to Sets!");
            Console.WriteLine("Select test that you want to perform and execute commands.");
        }

        private void OnHelp(string args)
        {
            Console.WriteLine("\n==========================================================");
            Console.WriteLine("List of available commands:");
            foreach (var (name, command) in AvailableCommands(_currentContext))
            {
                Console.WriteLine($"\t{name} - {command.Description}");
            }
        }

        private IEnumerable<KeyValuePair<string, Operation>> AvailableCommands(Context allowed) =>
            _commands.Where(command =>
                command.Value.Supports(allowed));

        private UserCommand WaitForCommand(Context allowed)
        {
            var lines = Console.ReadLine()
                .RemoveInsignificantWhitespaces(2)
                .ToArray();

            // TODO: Throws InvalidOperationException with an unreadable message?
            var (name, command) = _commands.First(com =>
                string.Equals(com.Key, lines[0], StringComparison.InvariantCultureIgnoreCase));
            if (command.Supports(allowed))
            {
                return new UserCommand(name, command, lines.Length > 1 ? lines[1] : null);
            }

            throw new InvalidOperationException("You can't use this operation right now.");
        }

        private struct Operation
        {
            public readonly string Description;
            public readonly Action<string> Execute;
            private readonly Context _supportedContexts;

            public Operation(string description, Context supportedContexts, Action<string> execute = null)
            {
                Description = description;
                Execute = execute;
                _supportedContexts = supportedContexts;
            }

            public bool Supports(Context ctx) => _supportedContexts.HasFlag(ctx);
        }

        private struct UserCommand
        {
            public readonly string Name;
            public readonly Operation Operation;
            public readonly string Args;

            public UserCommand(string name, Operation operation, string args)
            {
                Name = name;
                Operation = operation;
                Args = args;
            }
        }

        [Flags]
        private enum State
        {
            Normal = 1,
            Finishing = 2,
            Exiting = 4 | Finishing
        }

        [Flags]
        private enum Context
        {
            NoTest = 1,
            SingleSetTest = 2,
            TwoSetsTest = 4,
            SetTypeSelection = 8,
            AnyTest = SingleSetTest | TwoSetsTest,
            All = ~0,
        }
    }
}

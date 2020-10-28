using System.Collections.Generic;

namespace Generics.Robots
{
    public interface IRobotAI<out TCommand>
        where TCommand : IMoveCommand
    {
        TCommand GetCommand();
    }

    public class ShooterAI : IRobotAI<IMoveCommand>
    {
        int counter = 1;

        public IMoveCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    public class BuilderAI : IRobotAI<IMoveCommand>
    {
        int counter = 1;
        public IMoveCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    public interface IDevice<in TCommand>
        where TCommand : IMoveCommand
    {
        string ExecuteCommand(TCommand command);
    }

    public class Mover : IDevice<IMoveCommand>
    {
        public string ExecuteCommand(IMoveCommand command)
        {
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    public class Robot<TCommand>
        where TCommand : IMoveCommand
    {
        IRobotAI<TCommand> ai;
        IDevice<TCommand> device;

        public Robot(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
        {
            this.ai = ai;
            device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
             for (int i = 0; i < steps; i++)
             {
                 var command = ai.GetCommand();
                 if (command == null)
                     break;
                 yield return device.ExecuteCommand(command);
             }
        }
    }
    
    public static class Robot
    {
        public static Robot<TCommand> Create<TCommand>(IRobotAI<TCommand> ai, IDevice<TCommand> executor)
            where TCommand : IMoveCommand
        {
            return new Robot<TCommand>(ai, executor);
        }
    }
}
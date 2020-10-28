using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public interface ICanBeAssigned
    {
        int Owner { get; set; }

        void AssignTo(Player player);
    }

    public interface ICanBeConsumed
    {
        Treasure Treasure { get; set; }

        Treasure GetTreasure();
    }

    public interface IToBattle
    {
        Army Army { get; }
    }

    public class Dwelling : ICanBeAssigned
    {
        public int Owner { get; set; }

        public void AssignTo(Player player)
        {
            Owner = player.Id;
        }
    }

    public class Mine : ICanBeAssigned, ICanBeConsumed, IToBattle
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }

        public void AssignTo(Player player)
        {
            Owner = player.Id;
        }

        public Treasure GetTreasure()
        {
            return Treasure;
        }
    }

    public class Creeps : IToBattle, ICanBeConsumed
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }

        public Treasure GetTreasure()
        {
            return Treasure;
        }
    }

    public class Wolves : IToBattle
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ICanBeConsumed
    {
        public Treasure Treasure { get; set; }

        public Treasure GetTreasure()
        {
            return Treasure;
        }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IToBattle battleObject)
            {
                if (!player.CanBeat(battleObject.Army))
                    player.Die();
            }

            if (!player.Dead)
            {
                if (mapObject is ICanBeConsumed collectObject)
                {
                    player.Consume(collectObject.GetTreasure());
                }
                if (!player.Dead && mapObject is ICanBeAssigned assignObject)
                {
                    assignObject.AssignTo(player);
                }
            }
        }
    }
}

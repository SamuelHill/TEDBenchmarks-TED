using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scripts.Utilities;
using UnityEngine;
using Random = System.Random;

namespace Scripts.ValueTypes
{
    public static class Interactions
    {
        public struct Outcome
        {
            public readonly float ActorDelta;
            public readonly float OtherDelta;

            public Outcome(float actorDelta, float otherDelta)
            {
                ActorDelta = actorDelta;
                OtherDelta = otherDelta;
            }

            public override string ToString() => $"{ActorDelta}, {OtherDelta}";
        }
        public delegate Outcome Interaction(Person actor, Person other, float actorOther, float otherActor, Random rng);

        static Outcome Chat(Person actor, Person other, float actorOther, float otherActor, Random rng) {
            return new Outcome(1, 1);
        }
        static Outcome Fight(Person actor, Person other, float actorOther, float otherActor, Random rng) {
            return new Outcome(-1, -1);
        }
        static Outcome Argue(Person actor, Person other, float actorOther, float otherActor, Random rng) {
            return Randomize.Probability(rng) < (actorOther + otherActor < -100 ? 0.5f : 0.05f) ?
                       new Outcome(-1, -1) : new Outcome(1, 1);
        }
        static Outcome Flirt(Person actor, Person other, float actorOther, float otherActor, Random rng) {
            return new Outcome(0, Mathf.Sign(otherActor));
        }
        static Outcome Compliment(Person actor, Person other, float actorOther, float otherActor, Random rng) {
            return new Outcome(0, 5);
        }

        public static readonly Interaction[] PossibleInteractions = { Chat, Chat, Chat, Chat, Fight, Argue, Flirt, Compliment };

        public static Outcome Interact(Person actor, Person other, float actorOther, float otherActor, Random rng) =>
            PossibleInteractions[rng.Next() % PossibleInteractions.Length](actor, other, actorOther, otherActor, rng);
    }
}

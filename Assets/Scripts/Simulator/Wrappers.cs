using Scripts.Utilities;
using Scripts.ValueTypes;
using TED;

namespace Scripts.Simulator {
    using static TED.Language;
    
    public static class Wrappers {
        public static Function<Person, Fingerprint, Location, float> PersonLocationAffinity =
            new(nameof(PersonLocationAffinity), (person, mood, location) => person.Personality.Affinity(location.Personality, mood));
        public static Function<Person, Person, float> PersonPersonAffinity =
            new(nameof(PersonPersonAffinity), (person, other) => person.Personality.Affinity(other.Personality));
        public static Function<Person, Person, float, float, Interactions.Outcome> Interact =
            new(nameof(Interact), (person, other, personOther, otherPerson) => Interactions.Interact(person, other, personOther, otherPerson, Randomize.RngForInitialization));
        public static Function<Interactions.Outcome, float> ActorOutcome = new(nameof(ActorOutcome), outcome => outcome.ActorDelta);
        public static Function<Interactions.Outcome, float> OtherOutcome = new(nameof(OtherOutcome), outcome => outcome.OtherDelta);
        public static Function<Fingerprint> RandomMood = new(nameof(RandomMood), Fingerprint.Mood, false);
    }
}

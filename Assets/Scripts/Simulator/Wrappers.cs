using System;
using Scripts.Time;
using Scripts.Utilities;
using Scripts.ValueTypes;
using TED;

namespace Scripts.Simulator {
    using static TED.Language;
    
    public static class Wrappers {
        // Needs to be public for compiled code
        public static float PersonLocationAffinityImplementation(Person person, Fingerprint mood, Location location) => 
            person.Personality.Affinity(location.Personality, mood);
        public static Function<Person, Fingerprint, Location, float> PersonLocationAffinity =
            new(nameof(PersonLocationAffinity), PersonLocationAffinityImplementation) {NameForCompilation = nameof(PersonLocationAffinityImplementation)};
        
        public static float PersonPersonAffinityImplementation(Person person, Person other) => 
            person.Personality.Affinity(other.Personality);
        public static Function<Person, Person, float> PersonPersonAffinity =
            new(nameof(PersonPersonAffinity), PersonPersonAffinityImplementation) {NameForCompilation = nameof(PersonPersonAffinityImplementation)};

        public static Interactions.Outcome InteractImplementation(Person person, Person other, float personOther, float otherPerson) =>
            Interactions.Interact(person, other, personOther, otherPerson, Randomize.RngForInitialization);
        public static Function<Person, Person, float, float, Interactions.Outcome> Interact =
            new(nameof(Interact), InteractImplementation) {NameForCompilation = nameof(InteractImplementation)};
        
        public static float ActorOutcomeImplementation(Interactions.Outcome outcome) => outcome.ActorDelta;
        public static Function<Interactions.Outcome, float> ActorOutcome = new(nameof(ActorOutcome), ActorOutcomeImplementation) 
            {NameForCompilation = nameof(ActorOutcomeImplementation)};
        public static float OtherOutcomeImplementation(Interactions.Outcome outcome) => outcome.OtherDelta;
        public static Function<Interactions.Outcome, float> OtherOutcome = new(nameof(OtherOutcome), OtherOutcomeImplementation) 
            {NameForCompilation = nameof(OtherOutcomeImplementation)};
        
        public static Function<Fingerprint> RandomMood = new(nameof(RandomMood), Fingerprint.Mood, false) {NameForCompilation = "Fingerprint.Mood"};
        
        public static bool IsAMImplementation() => Clock.ClockTick % 2 == 1;
        public static Function<bool> IsAM = new(nameof(IsAM), IsAMImplementation) {NameForCompilation = nameof(IsAMImplementation)};
    }
}

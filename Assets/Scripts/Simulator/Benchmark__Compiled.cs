// ReSharper disable InconsistentNaming
// ReSharper disable JoinDeclarationAndInitializer
// ReSharper disable RedundantUsingDirective
using System;
using TED;
using TED.Interpreter;
using TED.Compiler;
using TED.Tables;
using Scripts.ValueTypes;
using static Scripts.ValueTypes.Interactions;
using static Scripts.Simulator.Wrappers;

#pragma warning disable 0164,8618,8600,8620

// ReSharper disable once CheckNamespace
namespace Scripts.Simulator

{
    [CompiledHelpersFor("Benchmark")]
    public class Benchmark__Compiled : TED.Compiler.CompiledTEDProgram
    {

        public static void WhereTheyAre__CompiledUpdate()
        {
            // WhereTheyAre[in person,in location].If(False == IsAM, Person[out person], mood == RandomMood, Maximal[in location,in affinity,And[TED.Interpreter.Goal[]]])
            {
                Person person;
                Fingerprint mood;
                Location location;
                float affinity;

                // False == IsAM
                if (false != IsAMImplementation()) goto rule2;

                // Person[out person]
                var row__1 = unchecked((uint)-1);
                restart__1:
                if (++row__1 == Person.Length) goto rule2;
                ref var data__1 = ref Person.Data[row__1];
                person = data__1;

                // mood == RandomMood
                mood = Fingerprint.Mood();

                // Maximal[in location,in affinity,And[TED.Interpreter.Goal[]]]
                var gotOne__3 = false;
                var bestlocation__3 = default(Location);
                var bestaffinity__3 = default(float);
                // And[TED.Interpreter.Goal[]]

                // Location[out location]
                var row__3_maxLoop_0 = unchecked((uint)-1);
                restart__3_maxLoop_0:
                if (++row__3_maxLoop_0 == Location.Length) goto maxDone__3;
                ref var data__3_maxLoop_0 = ref Location.Data[row__3_maxLoop_0];
                location = data__3_maxLoop_0;

                // affinity == PersonLocationAffinity(person, mood, location)
                affinity = PersonLocationAffinityImplementation(person, mood, location);

                if (!gotOne__3 || affinity > bestaffinity__3)
                {
                    gotOne__3 = true;
                    bestlocation__3 = location;
                    bestaffinity__3 = affinity;
                }
                goto restart__3_maxLoop_0;

                maxDone__3:
                if (!gotOne__3) goto restart__1;
                location = bestlocation__3;
                affinity = bestaffinity__3;

                // Write [in person,in location]
                WhereTheyAre.Add((person,location));
                goto restart__1;
            }

            rule2:;

            // WhereTheyAre[in person,in location].If(True == IsAM, WorkLocation[out person,out location])
            {
                Person person;
                Location location;

                // True == IsAM
                if (true != IsAMImplementation()) goto end;

                // WorkLocation[out person,out location]
                var row__1 = unchecked((uint)-1);
                restart__1:
                if (++row__1 == WorkLocation.Length) goto end;
                ref var data__1 = ref WorkLocation.Data[row__1];
                person = data__1.Item1;
                location = data__1.Item2;

                // Write [in person,in location]
                WhereTheyAre.Add((person,location));
                goto restart__1;
            }

            end:;
        }
        public static void InteractedWith__CompiledUpdate()
        {
            // InteractedWith[in person,in other,in affinity,in otherAffinity,in outcome].If(Person[out person], WhereTheyAre[in person,out location], Maximal[in other,in affinity,And[TED.Interpreter.Goal[]]], FirstOf[TED.Interpreter.Goal[]], outcome == Interact(person, other, affinity, otherAffinity))
            {
                Person person;
                Location location;
                Person other;
                float affinity;
                float otherAffinity;
                Outcome outcome;

                // Person[out person]
                var row__0 = unchecked((uint)-1);
                restart__0:
                if (++row__0 == Person.Length) goto end;
                ref var data__0 = ref Person.Data[row__0];
                person = data__0;

                // WhereTheyAre[in person,out location]
                var row__1 = Table.NoRow;
                for (var bucket__1=person.GetHashCode()&WhereTheyAre__0_key.Mask; WhereTheyAre__0_key.Buckets[bucket__1].row != Table.NoRow; bucket__1 = (bucket__1+1)&WhereTheyAre__0_key.Mask)
                    if (WhereTheyAre__0_key.Buckets[bucket__1].key == person)
                    {
                        row__1 = WhereTheyAre__0_key.Buckets[bucket__1].row;
                        break;
                    }
                if (row__1 == Table.NoRow) goto restart__0;
                ref var data__1 = ref WhereTheyAre.Data[row__1];
                location = data__1.Item2;

                // Maximal[in other,in affinity,And[TED.Interpreter.Goal[]]]
                var gotOne__2 = false;
                var bestother__2 = default(Person);
                var bestaffinity__2 = default(float);
                // And[TED.Interpreter.Goal[]]

                // WhereTheyAre[out other,in location]
                var row__2_maxLoop_0 = WhereTheyAre__1.FirstRowWithValue(in location);
                if (row__2_maxLoop_0 != Table.NoRow) goto match__2_maxLoop_0;
                goto maxDone__2;
                restart__2_maxLoop_0:
                row__2_maxLoop_0 = WhereTheyAre__1.NextRow[row__2_maxLoop_0];
                if (row__2_maxLoop_0 == Table.NoRow) goto maxDone__2;
                match__2_maxLoop_0:
                ref var data__2_maxLoop_0 = ref WhereTheyAre.Data[row__2_maxLoop_0];
                other = data__2_maxLoop_0.Item1;

                // FirstOf[TED.Interpreter.Goal[]]
                // Affinity[in person,in other,out affinity]
                var key__2_maxLoop_1_0 = (person, other);
                var row__2_maxLoop_1_0 = Table.NoRow;
                for (var bucket__2_maxLoop_1_0=key__2_maxLoop_1_0.GetHashCode()&Affinity__0_1_key.Mask; Affinity__0_1_key.Buckets[bucket__2_maxLoop_1_0].row != Table.NoRow; bucket__2_maxLoop_1_0 = (bucket__2_maxLoop_1_0+1)&Affinity__0_1_key.Mask)
                    if (Affinity__0_1_key.Buckets[bucket__2_maxLoop_1_0].key == key__2_maxLoop_1_0)
                    {
                        row__2_maxLoop_1_0 = Affinity__0_1_key.Buckets[bucket__2_maxLoop_1_0].row;
                        break;
                    }
                if (row__2_maxLoop_1_0 == Table.NoRow) goto firstOFBranch1__2_maxLoop_1;
                ref var data__2_maxLoop_1_0 = ref Affinity.Data[row__2_maxLoop_1_0];
                affinity = data__2_maxLoop_1_0.Item3;
                goto firstOfSuccess__2_maxLoop_1;

                firstOFBranch1__2_maxLoop_1:
                // affinity == PersonPersonAffinity(person, other)
                affinity = PersonPersonAffinityImplementation(person, other);
                goto firstOfSuccess__2_maxLoop_1;

                firstOfSuccess__2_maxLoop_1: ;

                if (!gotOne__2 || affinity > bestaffinity__2)
                {
                    gotOne__2 = true;
                    bestother__2 = other;
                    bestaffinity__2 = affinity;
                }
                goto restart__2_maxLoop_0;

                maxDone__2:
                if (!gotOne__2) goto restart__0;
                other = bestother__2;
                affinity = bestaffinity__2;

                // FirstOf[TED.Interpreter.Goal[]]
                // Affinity[in other,in person,out otherAffinity]
                var key__3_0 = (other, person);
                var row__3_0 = Table.NoRow;
                for (var bucket__3_0=key__3_0.GetHashCode()&Affinity__0_1_key.Mask; Affinity__0_1_key.Buckets[bucket__3_0].row != Table.NoRow; bucket__3_0 = (bucket__3_0+1)&Affinity__0_1_key.Mask)
                    if (Affinity__0_1_key.Buckets[bucket__3_0].key == key__3_0)
                    {
                        row__3_0 = Affinity__0_1_key.Buckets[bucket__3_0].row;
                        break;
                    }
                if (row__3_0 == Table.NoRow) goto firstOFBranch1__3;
                ref var data__3_0 = ref Affinity.Data[row__3_0];
                otherAffinity = data__3_0.Item3;
                goto firstOfSuccess__3;

                firstOFBranch1__3:
                // otherAffinity == PersonPersonAffinity(other, person)
                otherAffinity = PersonPersonAffinityImplementation(other, person);
                goto firstOfSuccess__3;

                firstOfSuccess__3: ;

                // outcome == Interact(person, other, affinity, otherAffinity)
                outcome = InteractImplementation(person, other, affinity, otherAffinity);

                // Write [in person,in other,in affinity,in otherAffinity,in outcome]
                InteractedWith.Add((person,other,affinity,otherAffinity,outcome));
                goto restart__0;
            }

            end:;
        }
        public static void Affinity__add__CompiledUpdate()
        {
            // Affinity__add[in person,in other,in temp0].If(InteractedWith[out person,out other,out affinity,out otherAffinity,out outcome], temp0 == affinity+ActorOutcome(outcome))
            {
                Person person;
                Person other;
                float affinity;
                float otherAffinity;
                Outcome outcome;
                float temp0;

                // InteractedWith[out person,out other,out affinity,out otherAffinity,out outcome]
                var row__0 = unchecked((uint)-1);
                restart__0:
                if (++row__0 == InteractedWith.Length) goto rule2;
                ref var data__0 = ref InteractedWith.Data[row__0];
                person = data__0.Item1;
                other = data__0.Item2;
                affinity = data__0.Item3;
                otherAffinity = data__0.Item4;
                outcome = data__0.Item5;

                // temp0 == affinity+ActorOutcome(outcome)
                temp0 = affinity+(ActorOutcomeImplementation(outcome));

                // Write [in person,in other,in temp0]
                Affinity__add.Add((person,other,temp0));
                goto restart__0;
            }

            rule2:;

            // Affinity__add[in person,in other,in temp1].If(InteractedWith[out other,out person,out _Single0,out affinity,out outcome], temp1 == affinity+OtherOutcome(outcome))
            {
                Person other;
                Person person;
                float _Single0;
                float affinity;
                Outcome outcome;
                float temp1;

                // InteractedWith[out other,out person,out _Single0,out affinity,out outcome]
                var row__0 = unchecked((uint)-1);
                restart__0:
                if (++row__0 == InteractedWith.Length) goto end;
                ref var data__0 = ref InteractedWith.Data[row__0];
                other = data__0.Item1;
                person = data__0.Item2;
                _Single0 = data__0.Item3;
                affinity = data__0.Item4;
                outcome = data__0.Item5;

                // temp1 == affinity+OtherOutcome(outcome)
                temp1 = affinity+(OtherOutcomeImplementation(outcome));

                // Write [in person,in other,in temp1]
                Affinity__add.Add((person,other,temp1));
                goto restart__0;
            }

            end:;
        }

        public override void Link(TED.Program program)
        {
            program["WhereTheyAre"].CompiledRules = (Action)WhereTheyAre__CompiledUpdate;
            program["InteractedWith"].CompiledRules = (Action)InteractedWith__CompiledUpdate;
            program["Affinity__add"].CompiledRules = (Action)Affinity__add__CompiledUpdate;
            Person = (Table<Person>)program["Person"].TableUntyped;
            Location = (Table<Location>)program["Location"].TableUntyped;
            WorkLocation = (Table<ValueTuple<Person,Location>>)program["WorkLocation"].TableUntyped;
            Affinity = (Table<ValueTuple<Person,Person,float>>)program["Affinity"].TableUntyped;
            Affinity__0_1_key = (KeyIndex<ValueTuple<Person,Person,float>,ValueTuple<Person,Person>>)Affinity.IndexFor(0, 1);
            WhereTheyAre = (Table<ValueTuple<Person,Location>>)program["WhereTheyAre"].TableUntyped;
            WhereTheyAre__0_key = (KeyIndex<ValueTuple<Person,Location>,Person>)WhereTheyAre.IndexFor(0);
            WhereTheyAre__1 = (GeneralIndex<ValueTuple<Person,Location>,Location>)WhereTheyAre.IndexFor(1);
            InteractedWith = (Table<ValueTuple<Person,Person,float,float,Outcome>>)program["InteractedWith"].TableUntyped;
            Affinity__add = (Table<ValueTuple<Person,Person,float>>)program["Affinity__add"].TableUntyped;
        }

        public static Table<Person> Person;
        public static Table<Location> Location;
        public static Table<ValueTuple<Person,Location>> WorkLocation;
        public static Table<ValueTuple<Person,Person,float>> Affinity;
        public static KeyIndex<ValueTuple<Person,Person,float>,ValueTuple<Person,Person>> Affinity__0_1_key;
        public static Table<ValueTuple<Person,Location>> WhereTheyAre;
        public static KeyIndex<ValueTuple<Person,Location>,Person> WhereTheyAre__0_key;
        public static GeneralIndex<ValueTuple<Person,Location>,Location> WhereTheyAre__1;
        public static Table<ValueTuple<Person,Person,float,float,Outcome>> InteractedWith;
        public static Table<ValueTuple<Person,Person,float>> Affinity__add;
    }

}
#pragma warning restore 0164,8618,8600,8620

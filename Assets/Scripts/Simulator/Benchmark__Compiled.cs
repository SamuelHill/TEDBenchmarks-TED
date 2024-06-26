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
            // InteractedWith[in person,in other,in affinity,in otherAffinity,in outcome].If(Person[out person], WhereTheyAre[in person,out location], Maximal[in other,in affinity,And[TED.Interpreter.Goal[]]], Or[TED.Interpreter.Goal[]], outcome == Interact(person, other, affinity, otherAffinity))
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
                var row__1 = WhereTheyAre__0_key.RowWithKey(in person);
                if (row__1 == Table.NoRow) goto restart__0;
                ref var data__1 = ref WhereTheyAre.Data[row__1];
                if (data__1.Item1 != person) goto restart__0;
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
                if (data__2_maxLoop_0.Item2 != location) goto restart__2_maxLoop_0;

                // Or[TED.Interpreter.Goal[]]

                int branch__2_maxLoop_1;

                start0__2_maxLoop_1:
                branch__2_maxLoop_1 = 0;
                // Affinity[in person,in other,out affinity]
                var row__2_maxLoop_1_0 = Affinity__0_1_key.RowWithKey((person, other));
                if (row__2_maxLoop_1_0 == Table.NoRow) goto start1__2_maxLoop_1;
                ref var data__2_maxLoop_1_0 = ref Affinity.Data[row__2_maxLoop_1_0];
                if (data__2_maxLoop_1_0.Item1 != person) goto start1__2_maxLoop_1;
                if (data__2_maxLoop_1_0.Item2 != other) goto start1__2_maxLoop_1;
                affinity = data__2_maxLoop_1_0.Item3;
                goto orSuccess__2_maxLoop_1;
                start1__2_maxLoop_1:
                branch__2_maxLoop_1 = 1;
                // affinity == PersonPersonAffinity(person, other)
                affinity = PersonPersonAffinityImplementation(person, other);
                goto orSuccess__2_maxLoop_1;


                restartDispatch__2_maxLoop_1:
                switch (branch__2_maxLoop_1)
                {
                    case 0: goto start1__2_maxLoop_1;
                    case 1: goto restart__2_maxLoop_0;
                }

                orSuccess__2_maxLoop_1: ;

                if (!gotOne__2 || affinity > bestaffinity__2)
                {
                    gotOne__2 = true;
                    bestother__2 = other;
                    bestaffinity__2 = affinity;
                }
                goto restartDispatch__2_maxLoop_1;

                maxDone__2:
                if (!gotOne__2) goto restart__0;
                other = bestother__2;
                affinity = bestaffinity__2;

                // Or[TED.Interpreter.Goal[]]

                int branch__3;

                start0__3:
                branch__3 = 0;
                // Affinity[in other,in person,out otherAffinity]
                var row__3_0 = Affinity__0_1_key.RowWithKey((other, person));
                if (row__3_0 == Table.NoRow) goto start1__3;
                ref var data__3_0 = ref Affinity.Data[row__3_0];
                if (data__3_0.Item1 != other) goto start1__3;
                if (data__3_0.Item2 != person) goto start1__3;
                otherAffinity = data__3_0.Item3;
                goto orSuccess__3;
                start1__3:
                branch__3 = 1;
                // otherAffinity == PersonPersonAffinity(other, person)
                otherAffinity = PersonPersonAffinityImplementation(other, person);
                goto orSuccess__3;


                restartDispatch__3:
                switch (branch__3)
                {
                    case 0: goto start1__3;
                    case 1: goto restart__0;
                }

                orSuccess__3: ;

                // outcome == Interact(person, other, affinity, otherAffinity)
                outcome = InteractImplementation(person, other, affinity, otherAffinity);

                // Write [in person,in other,in affinity,in otherAffinity,in outcome]
                InteractedWith.Add((person,other,affinity,otherAffinity,outcome));
                goto restartDispatch__3;
            }

            end:;
        }
        public static void Affinity__add__CompiledUpdate()
        {
            // Affinity__add[in person,in other,in total].If(InteractedWith[out person,out other,out affinity,out otherAffinity,out outcome], total == affinity+ActorOutcome(outcome))
            {
                Person person;
                Person other;
                float affinity;
                float otherAffinity;
                Outcome outcome;
                float total;

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

                // total == affinity+ActorOutcome(outcome)
                total = affinity+(ActorOutcomeImplementation(outcome));

                // Write [in person,in other,in total]
                Affinity__add.Add((person,other,total));
                goto restart__0;
            }

            rule2:;

            // Affinity__add[in person,in other,in total].If(InteractedWith[out other,out person,out _Single0,out affinity,out outcome], total == affinity+OtherOutcome(outcome))
            {
                Person other;
                Person person;
                float _Single0;
                float affinity;
                Outcome outcome;
                float total;

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

                // total == affinity+OtherOutcome(outcome)
                total = affinity+(OtherOutcomeImplementation(outcome));

                // Write [in person,in other,in total]
                Affinity__add.Add((person,other,total));
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

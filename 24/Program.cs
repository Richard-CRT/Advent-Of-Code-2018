using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace _24
{
    public enum DamageType { Bludgeoning, Slashing, Fire, Cold, Radiation }
    public enum GroupType { ImmuneSystem, Infection }

    class Program
    {
        static DamageType GetTypeFromString(string typeString)
        {
            switch (typeString)
            {
                case "bludgeoning":
                    return DamageType.Bludgeoning;
                case "slashing":
                    return DamageType.Slashing;
                case "fire":
                    return DamageType.Fire;
                case "cold":
                    return DamageType.Cold;
                case "radiation":
                    return DamageType.Radiation;
                default:
                    throw new NotImplementedException();
            }
        }

        static Group GetGroupFromInputLine(string inputLine)
        {
            string pat = @"([^ ]+) units each with ([^ ]+) hit points (?:\(([^)]+)\) )?with an attack that does ([^ ]+) ([^ ]+) damage at initiative (.+)";

            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(inputLine);

            int unitCount = Int32.Parse(m.Groups[1].Value);
            int hitpointsPerUnit = Int32.Parse(m.Groups[2].Value);
            string weaknessesStrengths = m.Groups[3].Value;
            int attackDamagePerUnit = Int32.Parse(m.Groups[4].Value);
            string attackTypeString = m.Groups[5].Value;
            int initiative = Int32.Parse(m.Groups[6].Value);

            string[] weaknessesStrengthsParts = weaknessesStrengths.Split(';');

            List<DamageType> immuneTo = new List<DamageType>();
            List<DamageType> weakTo = new List<DamageType>();
            if (weaknessesStrengths != "")
            {
                foreach (string part in weaknessesStrengthsParts)
                {
                    string trimmedPart = part.Trim();
                    if (trimmedPart.Substring(0, 6) == "immune")
                    {
                        string[] partParts = trimmedPart.Substring(10).Split(',');
                        foreach (string partPart in partParts)
                        {
                            immuneTo.Add(GetTypeFromString(partPart.Trim()));
                        }
                    }
                    else
                    {
                        string[] partParts = trimmedPart.Substring(8).Split(',');
                        foreach (string partPart in partParts)
                        {
                            weakTo.Add(GetTypeFromString(partPart.Trim()));
                        }
                    }
                }
            }

            DamageType attackType = GetTypeFromString(attackTypeString);

            return new Group(unitCount, hitpointsPerUnit, immuneTo, weakTo, attackDamagePerUnit, attackType, initiative);
        }

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            bool ImmuneSystemWon = false;
            int ImmuneBoost = 0;
            int winningTeamRemainingUnits = 0;
            while (!ImmuneSystemWon)
            {
                //Console.WriteLine(ImmuneBoost);

                List<Group> AllGroups = new List<Group>();
                List<Group> ImmuneSystem = new List<Group>();
                List<Group> Infection = new List<Group>();

                int groupCounter = 1;
                int n = 1;
                while (inputList[n] != "")
                {
                    Group newGroup = GetGroupFromInputLine(inputList[n]);
                    newGroup.Type = GroupType.ImmuneSystem;
                    newGroup.Number = groupCounter;
                    newGroup.AttackDamagePerUnit += ImmuneBoost;
                    ImmuneSystem.Add(newGroup);
                    AllGroups.Add(newGroup);
                    groupCounter++;
                    n++;
                }
                n += 2;
                groupCounter = 1;
                while (n < inputList.Count && inputList[n] != "")
                {
                    Group newGroup = GetGroupFromInputLine(inputList[n]);
                    newGroup.Type = GroupType.Infection;
                    newGroup.Number = groupCounter;
                    Infection.Add(newGroup);
                    AllGroups.Add(newGroup);
                    groupCounter++;
                    n++;
                }

                int tick = 0;
                while (ImmuneSystem.Count > 0 && Infection.Count > 0)
                {
                    // START COMBAT

#if DEBUG
                    AoCUtilities.DebugWriteLine("Immune System:");
                    foreach (Group immuneGroup in ImmuneSystem)
                    {
                        AoCUtilities.DebugWriteLine("Group {0} contains {1} units", immuneGroup.Number, immuneGroup.UnitCount);
                    }
                    AoCUtilities.DebugWriteLine("Infection System:");
                    foreach (Group infectionGroup in Infection)
                    {
                        AoCUtilities.DebugWriteLine("Group {0} contains {1} units", infectionGroup.Number, infectionGroup.UnitCount);
                    }
                    AoCUtilities.DebugWriteLine();
#endif

                    // Target Selection Phase
                    AllGroups = AllGroups.OrderByDescending(group => group.EffectivePower).ThenByDescending(group => group.Initiative).ToList();
                    foreach (Group group in AllGroups)
                    {
                        List<Group> potentialTargets;
                        if (group.Type == GroupType.ImmuneSystem)
                        {
                            potentialTargets = Infection;
                        }
                        else
                        {
                            potentialTargets = ImmuneSystem;
                        }
                        // Select the group in potentialTargets this group would do the most damage to
                        Group bestTarget = null;
                        int bestDamage = -1;
                        foreach (Group potentialTarget in potentialTargets)
                        {
                            if (potentialTarget.TargetOfThisGroupThisTurn == null)
                            {
                                int potentialDamage;
                                if (potentialTarget.ImmuneTo.Contains(group.AttackType))
                                {
                                    potentialDamage = 0;
                                }
                                else if (potentialTarget.WeakTo.Contains(group.AttackType))
                                {
                                    potentialDamage = group.EffectivePower * 2;
                                }
                                else
                                {
                                    potentialDamage = group.EffectivePower;
                                }
                                if (potentialDamage > bestDamage)
                                {
                                    bestTarget = potentialTarget;
                                    bestDamage = potentialDamage;
                                }
                                else if (bestDamage == potentialDamage)
                                {
                                    if (potentialTarget.EffectivePower > bestTarget.EffectivePower)
                                    {
                                        bestTarget = potentialTarget;
                                    }
                                    else if (potentialTarget.EffectivePower == bestTarget.EffectivePower)
                                    {
                                        if (potentialTarget.Initiative > bestTarget.Initiative)
                                        {
                                            bestTarget = potentialTarget;
                                        }
                                    }
                                }
#if DEBUG
                                if (group.Type == GroupType.ImmuneSystem)
                                {
                                    AoCUtilities.DebugWrite("Immune System ");
                                }
                                else
                                {
                                    AoCUtilities.DebugWrite("Infection ");
                                }
                                AoCUtilities.DebugWriteLine("Group {0} would deal defending group {1} {2} damage", group.Number, potentialTarget.Number, potentialDamage);
#endif
                            }
                        }

                        if (bestDamage <= 0)
                        {
                            // Choose no victim
                            group.AttackingThisGroupThisTurn = null;
                        }
                        else
                        {
                            // Choose victim
                            group.AttackingThisGroupThisTurn = bestTarget;
                            bestTarget.TargetOfThisGroupThisTurn = group;
                        }
                    }

                    AoCUtilities.DebugWriteLine();

                    int totalUnitsKilled = 0;
                    // Attack Phase
                    AllGroups = AllGroups.OrderByDescending(group => group.Initiative).ToList();
                    foreach (Group group in AllGroups)
                    {
                        Group victim = group.AttackingThisGroupThisTurn;
                        if (group.UnitCount > 0 && victim != null)
                        {
                            int damage = 0;
                            // Should never try to attack an immune enemy so don't need to check that possibility
                            if (victim.WeakTo.Contains(group.AttackType))
                            {
                                damage = group.EffectivePower * 2;
                            }
                            else
                            {
                                damage = group.EffectivePower;
                            }

                            int unitsKilled = damage / victim.HitpointsPerUnit; // integer division
                            totalUnitsKilled += unitsKilled;
                            victim.UnitCount -= unitsKilled;

#if DEBUG
                            if (group.Type == GroupType.ImmuneSystem)
                            {
                                AoCUtilities.DebugWrite("Immune System ");
                            }
                            else
                            {
                                AoCUtilities.DebugWrite("Infection ");
                            }
                            AoCUtilities.DebugWriteLine("Group {0} attacks defending group {1}, killing {2} units", group.Number, victim.Number, victim.UnitCount < 0 ? unitsKilled + victim.UnitCount : unitsKilled);
#endif
                        }

                        group.AttackingThisGroupThisTurn = null;
                        group.TargetOfThisGroupThisTurn = null;
                    }

                    AoCUtilities.DebugWriteLine();

                    for (int i = 0; i < AllGroups.Count;)
                    {
                        Group group = AllGroups[i];
                        if (group.UnitCount <= 0)
                        {
                            AllGroups.Remove(group);
                            if (group.Type == GroupType.ImmuneSystem)
                            {
                                ImmuneSystem.Remove(group);
                            }
                            else
                            {
                                Infection.Remove(group);
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }

                    if (totalUnitsKilled == 0)
                    {
                        AoCUtilities.DebugWriteLine("System was in equillibrium");
                        break;
                    }

                    tick++;

                    //AoCUtilities.DebugReadLine();
                }

#if DEBUG
                AoCUtilities.DebugWriteLine("Immune System:");
                foreach (Group immuneGroup in ImmuneSystem)
                {
                    AoCUtilities.DebugWriteLine("Group {0} contains {1} units", immuneGroup.Number, immuneGroup.UnitCount);
                }
                AoCUtilities.DebugWriteLine("Infection System:");
                foreach (Group infectionGroup in Infection)
                {
                    AoCUtilities.DebugWriteLine("Group {0} contains {1} units", infectionGroup.Number, infectionGroup.UnitCount);
                }
                AoCUtilities.DebugWriteLine();
#endif

                AoCUtilities.DebugWriteLine("Immune System had a boost of {0} attack damage", ImmuneBoost);
                List<Group> winningTeam;
                if (Infection.Count == 0)
                {
                    winningTeam = ImmuneSystem;
                    ImmuneSystemWon = true;
                    AoCUtilities.DebugWrite("Immune System wins ");
                }
                else
                {
                    winningTeam = Infection;
                    AoCUtilities.DebugWrite("Infection wins ");
                }

                winningTeamRemainingUnits = 0;
                foreach (Group group in winningTeam)
                {
                    winningTeamRemainingUnits += group.UnitCount;
                }
                AoCUtilities.DebugWriteLine("with {0} units remaining", winningTeamRemainingUnits);

                AoCUtilities.DebugReadLine();

                ImmuneBoost++;
            }

            Console.WriteLine("Immune System had a boost of {0} attack damage", ImmuneBoost - 1);
            Console.WriteLine("Immune System won with {0} units remaining", winningTeamRemainingUnits);
            Console.ReadLine();
        }
    }

    class Group
    {
        public int Number;

        public int HitpointsPerUnit;
        public List<DamageType> ImmuneTo;
        public List<DamageType> WeakTo;
        public DamageType AttackType;
        public int Initiative;

        public GroupType Type;

        private int attackDamagePerUnit;
        public int AttackDamagePerUnit
        {
            get
            {
                return attackDamagePerUnit;
            }
            set
            {
                attackDamagePerUnit = value;
                EffectivePower = UnitCount * AttackDamagePerUnit;
            }
        }

        private int unitCount;
        public int UnitCount
        {
            get
            {
                return unitCount;
            }
            set
            {
                unitCount = value;
                EffectivePower = UnitCount * AttackDamagePerUnit;
            }
        }

        public int EffectivePower { get; private set; }

        public Group TargetOfThisGroupThisTurn = null;
        public Group AttackingThisGroupThisTurn = null;

        public Group(int unitCount, int hitpointsPerUnit, List<DamageType> immuneTo, List<DamageType> weakTo, int attackDamagePerUnit, DamageType attackType, int initiative)
        {
            HitpointsPerUnit = hitpointsPerUnit;
            ImmuneTo = immuneTo;
            WeakTo = weakTo;
            AttackDamagePerUnit = attackDamagePerUnit;
            AttackType = attackType;
            Initiative = initiative;

            UnitCount = unitCount;
        }
    }
}

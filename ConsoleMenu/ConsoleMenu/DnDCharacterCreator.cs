using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using MySql.Data.MySqlClient;

namespace ConsoleMenu
{
    public class DnDCharacterCreator
    {
        public void Menu()
        {
            //Variable Declaration
            bool isRunning = true;
            List<Character> CharacterList = new List<Character>();

            //Loop for the menu to run
            while (isRunning)
            {
                //Object generation, generating a new one each loop in order to hopefully generate more randomness in the die
                Character character = new Character();
                Dice dice = new Dice();

                //Menu 
                Console.WriteLine("What would you like to do? \n" +
                    "1. See characters from the database \n" +
                    "2. Create a new character \n" +
                    "3. Roll a die \n" +
                    "4. Back to main menu");

                //Input to check on
                string input = Console.ReadLine();
                //Switch case to figure out which functions to use
                switch (input)
                {
                    //Showing the database 
                    case "1":
                        Console.Clear();
                        character.ShowNamesFromDataBase();
                        Console.Clear();
                        break;

                    case "2":

                        Console.Clear();

                        character.RaceSelector(character);

                        character.CharacterNaming(character);

                        Console.WriteLine("All right {0} now its time to roll your stats", character.name);
                        Thread.Sleep(1000);

                        character.RollingMenu(character);
                        Console.Clear();

                        character.ClassSelector(character);

                        character.AlignmentSelector(character);
                        Console.Clear();

                        Console.WriteLine("YOUR CHARACTER \n" +
                        "Name: " + character.name + "\n" +
                        "Race: " + character.race + "\n" +
                        "Class: " + character.characterClass + "\n" +
                        "Alignment: " + character.alignment + "\n" +
                        "\n" +
                        "Strength: " + character.strength + "\n" +
                        "Dexterity: " + character.dexterity + "\n" +
                        "Constitution: " + character.constitution + "\n" +
                        "Intelligence: " + character.intelligence + "\n" +
                        "Wisdom: " + character.wisdom + "\n" +
                        "Charisma: " + character.charisma + "\n");
                        Console.ReadKey();

                        character.WriteToTxtDocument(character);
                        Console.Clear();

                        character.AddToDatabase(character);
                        break;

                    case "3":
                        Console.Clear();
                        dice.DieRollingMenu();
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("I'm sorry I don't understand");
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;
                }
            }
        }
    }

        public enum Alignment
        {
            Lawful_Good,
            Neutral_Good,
            Chaotic_Good,
            Lawful_Neutral,
            True_Neutral,
            Chaotic_Neutral,
            Lawful_Evil,
            Neutral_Evil,
            Chaotic_Evil

        }

        public enum Race
        {
            Dwarf,
            Elf,
            Gnome,
            Halfling,
            HalfElf,
            HalfOrc,
            Human
        }

        public enum Class
        {
            Barbarian,
            Bard,
            Cleric,
            Druid,
            Fighter,
            Monk,
            Paladin,
            Ranger,
            Rogue,
            Sorcerer,
            Wizard
        }

        class Character
        {
            //Variable declaration
            public Class characterClass;
            public Race race;
            public Alignment alignment;
            public string name;
            public int strength = 0;
            public int dexterity = 0;
            public int constitution = 0;
            public int intelligence = 0;
            public int wisdom = 0;
            public int charisma = 0;

            /// <summary>
            /// The menu for rolling characters, will send along to auto rolls and also gives an option to roll manually
            /// </summary>
            /// <param name="character">Character to roll stats for</param>
            public void RollingMenu(Character character)
            {
                bool isRunning = true;

                while (isRunning)
                {
                    Console.Clear();
                    Console.WriteLine("What do you want to do for your stats? \n" +
                            "1. Insert them manually after rolling in real life (I'm sure you wouldn't cheat, right?) \n" +
                            "2. Let the program roll for you");

                    string input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            bool manualStatPicking = true;

                            while (manualStatPicking)
                            {
                                Console.Clear();
                                Console.WriteLine("First choose which stat you want to put your roll in to, or write exit when you're done \n" +
                                    "1. Strength: " + character.strength + "\n" +
                                    "2. Dexterity: " + character.dexterity + "\n" +
                                    "3. Constitution: " + character.constitution + "\n" +
                                    "4. Intelligence: " + character.intelligence + "\n" +
                                    "5. Wisdom: " + character.wisdom + "\n" +
                                    "6. Charisma: " + character.charisma + "\n");

                                input = Console.ReadLine();

                                if (input.ToLower() == "exit")
                                    return;

                                Console.WriteLine("What is your roll?");

                                string rollInput = Console.ReadLine();
                                int roll = 0;

                                try
                                {
                                    roll = Convert.ToInt32(rollInput);
                                }
                                catch
                                {
                                    Console.WriteLine("Did you write a number? Try again \n" +
                                        "Press enter");
                                    Console.ReadKey();
                                }

                                if (roll > 6)
                                    switch (input.ToLower())
                                    {
                                        case "1":
                                            character.strength = character.strength + roll;
                                            break;
                                        case "2":
                                            character.dexterity = character.dexterity + roll;
                                            break;
                                        case "3":
                                            character.constitution = character.constitution + roll;
                                            break;
                                        case "4":
                                            character.intelligence = character.intelligence + roll;
                                            break;
                                        case "5":
                                            character.wisdom = character.wisdom + roll;
                                            break;
                                        case "6":
                                            character.charisma = character.charisma + roll;
                                            break;
                                        default:
                                            Console.WriteLine("Sorry I don't understand");
                                            break;
                                    }
                            }

                            break;
                        case "2":
                            character.StatSelectorAndRolling(character);
                            return;
                    }
                }

            }

            /// <summary>
            /// Writing the character sheet to a text document placed on the desktop
            /// </summary>
            /// <param name="character">Character to print to character sheet</param>
            public void WriteToTxtDocument(Character character)
            {
                //Loop to run it through
                bool isRunning = true;
                while (isRunning)
                {
                    //Giving the choice of either yes or no
                    Console.WriteLine("Would you like to save your character in a text document? (Yes/No)");
                    string input = Console.ReadLine();

                    //If they said yes
                    if (input.ToLower() == "yes")
                    {
                        //Setting the path to the desktop, and the file name as "characternameCharacterSheet" so that it will be easy to figure out which one it is
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                        desktopPath += "\\";
                        string path = desktopPath + character.name + "CharacterSheet.txt";

                        //What to write to the file \r\n is to switch to the next line
                        string text = "YOUR CHARACTER \r\n" +
                            "Name: " + character.name + "\r\n" +
                            "Race: " + character.race + "\r\n" +
                            "Class: " + character.characterClass + "\r\n" +
                            "Alignment: " + character.alignment + "\r\n" +
                            "\r\n" +
                            "Strength: " + character.strength + "\r\n" +
                            "Dexterity: " + character.dexterity + "\r\n" +
                            "Constitution: " + character.constitution + "\r\n" +
                            "Intelligence: " + character.intelligence + "\r\n" +
                            "Wisdom: " + character.wisdom + "\r\n" +
                            "Charisma: " + character.charisma;

                        // Writing the text to the file, if the file doesn't exist, it will create it with the name given in the path variable
                        File.WriteAllText(path, text);


                        Console.WriteLine("Your file should be saved on your desktop now, thank you");
                        Thread.Sleep(1000);
                        Console.Clear();

                        //Leaving the loop and the method in general
                        return;
                    }

                    //If the user wrote no
                    else if (input.ToLower() == "no")
                    {
                        Console.WriteLine("Okay that's fine!");
                        Thread.Sleep(1000);
                        Console.Clear();

                        //Leaving the loop and the method in general
                        return;
                    }

                    //If anything else was written
                    else
                    {
                        Console.WriteLine("I'm sorry I didn't understand that, please write either yes or no to make sure choose");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }

            /// <summary>
            /// Function to name the character
            /// </summary>
            /// <param name="character">Character to name</param>
            public void CharacterNaming(Character character)
            {
                //Asking and recieving the name
                Console.WriteLine("Please enter your characters name");
                string name = Console.ReadLine();
                Console.Clear();

                //Loop to make sure they get the right name
                bool namePicking = true;
                while (namePicking)
                {

                    //Making sure they spelled it correctly, or that they really want that to be the name
                    Console.WriteLine("{0}, are you sure you want {0} to be your characters name? (y/n)", name);
                    string input = Console.ReadLine();

                    //If they say yes, the name will get set and the loop will stop
                    if (input.ToLower() == "y" || input.ToLower() == "yes")
                    {
                        character.name = name;
                        Console.WriteLine("Your character has been named {0}", name);
                        namePicking = false;
                    }
                    //If they say no, they will be asked to re-enter a name and it will get changed to their new input, before going back to the confirmation
                    else if (input.ToLower() == "n" || input.ToLower() == "no")
                    {
                        Console.WriteLine("Please enter your characters name");
                        name = Console.ReadLine();
                        Console.Clear();
                    }

                    //If they wrote anything other than yes or no the loop will restart
                    else
                    {
                        Console.WriteLine("I'm sorry I didn't understand try again \n" +
                            "Press enter to continue");
                        Console.ReadKey();
                        Console.Clear();
                    }

                    Console.Clear();
                }
            }

            /// <summary>
            /// Rolls four random sixsided die and puts them in a stat
            /// </summary>
            /// <param name="character">Character to roll them for</param>
            public void StatSelectorAndRolling(Character character)
            {
                //Variable declaration
                int rollTotal = 0;
                int die1;
                int die2;
                int die3;
                int die4;

                //Loop running 6 times in order to place all 6 stats
                for (int b = 0; b < 6; b += 1)
                {
                    //bools for while loops
                    bool roll = true;
                    bool statPick = true;

                    while (roll)
                    {
                        //The instance of random being used, only one instance should ever be in your code
                        Random d6 = new Random();

                        //Loop for shit and giggles, just a visual effect of a rolling die, no functionality
                        for (int i = 0; i < 3; i += 1)
                        {
                            for (int a = 1; a <= 6; a += 1)
                            {
                                Console.WriteLine(a);
                                Thread.Sleep(10);
                                Console.Clear();
                            }
                        }
                        //Setting the first die as the first random between 1 and 6 (Random starts at the first number specified and ends at the number before the last number specified)
                        die1 = d6.Next(1, 7);

                        //See above
                        for (int i = 0; i < 3; i += 1)
                        {
                            for (int a = 1; a <= 6; a += 1)
                            {
                                Console.WriteLine(a);
                                Thread.Sleep(10);
                                Console.Clear();
                            }
                        }

                        die2 = d6.Next(1, 7);

                        //See above
                        for (int i = 0; i < 3; i += 1)
                        {
                            for (int a = 1; a <= 6; a += 1)
                            {
                                Console.WriteLine(a);
                                Thread.Sleep(10);
                                Console.Clear();
                            }
                        }

                        die3 = d6.Next(1, 7);

                        //See above
                        for (int i = 0; i < 3; i += 1)
                        {
                            for (int a = 1; a <= 6; a += 1)
                            {
                                Console.WriteLine(a);
                                Thread.Sleep(10);
                                Console.Clear();
                            }
                        }

                        die4 = d6.Next(1, 7);

                        //Sending the four dies down to the a different function to remove the smallest roll from the batch
                        rollTotal = DieSelect(die1, die2, die3, die4);

                        //If the sum of the dies are less than 6 you will need to reroll, this is what happens here
                        if (rollTotal < 6)
                        {
                            Console.WriteLine("You rolled a {0}, {1}, {2}, {3}, and removing the smallest number you have a total of {4}", die1, die2, die3, die4, rollTotal);
                            Console.WriteLine("Unfortunately that's not enough to put into a stat, press enter to reroll");
                            Thread.Sleep(1000);
                        }

                        //If the roll is above 6 the roll loop will stop
                        else
                        {
                            Console.WriteLine("You rolled a {0}, {1}, {2}, {3} and removing the smallest number you have a total of {4}", die1, die2, die3, die4, rollTotal);
                            Thread.Sleep(1000);
                            roll = false;
                        }

                    }
                    //The loop that will start when the roll loop has ended
                    while (statPick)
                    {
                        //Listing the different stats and their current values, asking for which one the user would like to set as their roll
                        Console.Clear();
                        Console.WriteLine("Write the number for the stat you want to place your roll ({6}) do you want to put it? \n" +
                            "1. Strength {0} \n" +
                            "2. Dexterity {1} \n" +
                            "3. Constitution {2} \n" +
                            "4. Intelligence {3} \n" +
                            "5. Wisdom {4} \n" +
                            "6. Charisma {5}", character.strength, character.dexterity, character.constitution, character.intelligence, character.wisdom, character.charisma, rollTotal);

                        string input = Console.ReadLine();

                        //Checking the input, if the input is one of the cases the loop will stop and go all the way back to the first loop which will run 6 times in total
                        switch (input)
                        {
                            //Setting the roll as the strength stat, if there was a race modifier it will also be added through this
                            case "1":

                                //a test of whether the stat has been picked or not, if it hasn't the loop will stop, if it has the loop will start over, the same goes for the other cases
                                if (character.strength < 6)
                                {
                                    character.strength = rollTotal + character.strength;
                                    statPick = false;
                                }
                                else
                                {
                                    Console.WriteLine("That has already been picked, pick a different stat");
                                    Console.ReadKey();
                                }
                                break;
                            case "2":
                                if (character.dexterity < 6)
                                {
                                    character.dexterity = rollTotal + character.dexterity;
                                    statPick = false;
                                }
                                else
                                {
                                    Console.WriteLine("That has already been picked, pick a different stat");
                                    Console.ReadKey();
                                }
                                break;
                            case "3":
                                if (character.constitution < 6)
                                {
                                    character.constitution = rollTotal + character.constitution;
                                    statPick = false;
                                }
                                else
                                {
                                    Console.WriteLine("That has already been picked, pick a different stat");
                                    Console.ReadKey();
                                }
                                break;
                            case "4":
                                if (character.intelligence < 6)
                                {
                                    character.intelligence = rollTotal + character.intelligence;
                                    statPick = false;
                                }
                                else
                                {
                                    Console.WriteLine("That has already been picked, pick a different stat");
                                    Console.ReadKey();
                                }
                                break;
                            case "5":
                                if (character.wisdom < 6)
                                {
                                    character.wisdom = rollTotal + character.wisdom;
                                    statPick = false;
                                }
                                else
                                {
                                    Console.WriteLine("That has already been picked, pick a different stat");
                                    Console.ReadKey();
                                }
                                break;
                            case "6":
                                if (character.charisma < 6)
                                {
                                    character.charisma = rollTotal + character.charisma;
                                    statPick = false;
                                }
                                else
                                {
                                    Console.WriteLine("That has already been picked, pick a different stat");
                                    Console.ReadKey();
                                }
                                break;
                            //If the user writes something invalid
                            default:
                                Console.WriteLine("I'm sorry I don't understand");
                                Thread.Sleep(1000);
                                break;
                        }
                    }

                }
            }

            /// <summary>
            /// Selecting which die to use for the total
            /// </summary>
            /// <param name="a">die 1</param>
            /// <param name="b">die 2</param>
            /// <param name="c">die 3</param>
            /// <param name="d">die 4</param>
            /// <returns>The roll total</returns>
            public int DieSelect(int a, int b, int c, int d)
            {
                //Variable declaration
                int total = 0;
                int lowest = a;

                //Checking which die is the smallest die, if there is two of the same die, it wont change anything, only one will get removed
                if (b < lowest)
                    lowest = b;
                if (c < lowest)
                    lowest = c;
                if (d < lowest)
                    lowest = d;

                //Setting the total to the die which are not the lowest
                if (lowest == a)
                    total = b + c + d;
                if (lowest == b)
                    total = a + c + d;
                if (lowest == c)
                    total = b + a + d;
                if (lowest == d)
                    total = b + c + a;
                //Returning the total
                return total;
            }

            /// <summary>
            /// Selecting race for the character, also adds the race modifiers
            /// </summary>
            /// <param name="character">Character to select race for</param>
            public void RaceSelector(Character character)
            {
                bool isRunning = true;
                //Loop while selecting race
                while (isRunning)
                {
                    //Menu
                    Console.WriteLine("Please select your characters race \n" +
                        "1. Dwarf \n" +
                        "2. Elf \n" +
                        "3. Gnome \n" +
                        "4. Halfling \n" +
                        "5. Half-Elf \n" +
                        "6. Half-Orc \n" +
                        "7. Human");
                    string input = Console.ReadLine();

                    //switch case on the input, if the race selected has race modifiers they will also be set
                    switch (input)
                    {
                        case "1":
                            character.race = Race.Dwarf;
                            character.constitution = character.constitution + 2;
                            character.charisma = character.charisma - 2;
                            isRunning = false;
                            break;

                        case "2":
                            character.race = Race.Elf;
                            character.dexterity = character.dexterity + 2;
                            character.constitution = character.constitution - 2;
                            isRunning = false;
                            break;

                        case "3":
                            character.race = Race.Gnome;
                            character.constitution = character.constitution + 2;
                            character.strength = character.strength - 2;
                            isRunning = false;
                            break;

                        case "4":
                            character.race = Race.Halfling;
                            character.dexterity = character.dexterity + 2;
                            character.strength = character.strength - 2;
                            isRunning = false;
                            break;

                        case "5":
                            character.race = Race.HalfElf;
                            isRunning = false;
                            break;

                        case "6":
                            character.race = Race.HalfOrc;
                            character.strength = character.strength + 2;
                            character.intelligence = character.intelligence - 2;
                            character.charisma = character.charisma - 2;
                            isRunning = false;
                            break;

                        case "7":
                            character.race = Race.Human;
                            isRunning = false;
                            break;

                        default:
                            Console.WriteLine("I'm sorry I don't understand. Did you write a number corrosponding to the race?");
                            break;
                    }
                    Console.Clear();
                }
            }

            /// <summary>
            /// Selecting class for your character, requires stats to be set to work as intended
            /// </summary>
            /// <param name="character">Character to select class for</param>
            public void ClassSelector(Character character)
            {
                bool isRunning = true;
                //Class picker loop
                while (isRunning)
                {
                    //Menu
                    Console.WriteLine("Now that you have your stats, please select your class \n" +
                        "1. Barbarian                          Strength         {0}\n" +
                        "2. Bard                               Dexterity        {1}\n" +
                        "3. Cleric                             Constitution     {2}\n" +
                        "4. Druid                              Intelligence     {3}\n" +
                        "5. Fighter                            Wisdom           {4}\n" +
                        "6. Monk                               Charisma         {5}\n" +
                        "7. Paladin \n" +
                        "8. Ranger \n" +
                        "9. Rogue \n" +
                        "10. Sorcerer \n" +
                        "11. Wizard", character.strength, character.dexterity, character.constitution, character.intelligence, character.wisdom, character.charisma);

                    string input = Console.ReadLine();

                    //Switch on the input, sets the class as the selected one
                    switch (input)
                    {
                        case "1":
                            character.characterClass = Class.Barbarian;
                            isRunning = false;
                            break;
                        case "2":
                            character.characterClass = Class.Bard;
                            isRunning = false;
                            break;
                        case "3":
                            character.characterClass = Class.Cleric;
                            isRunning = false;
                            break;
                        case "4":
                            character.characterClass = Class.Druid;
                            isRunning = false;
                            break;
                        case "5":
                            character.characterClass = Class.Fighter;
                            isRunning = false;
                            break;
                        case "6":
                            character.characterClass = Class.Monk;
                            isRunning = false;
                            break;
                        case "7":
                            character.characterClass = Class.Paladin;
                            isRunning = false;
                            break;
                        case "8":
                            character.characterClass = Class.Ranger;
                            isRunning = false;
                            break;
                        case "9":
                            character.characterClass = Class.Rogue;
                            isRunning = false;
                            break;
                        case "10":
                            character.characterClass = Class.Sorcerer;
                            isRunning = false;
                            break;
                        case "11":
                            character.characterClass = Class.Wizard;
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("I'm sorry I didn't understand that, please try again");
                            break;

                    }
                    Console.Clear();
                }
            }

            /// <summary>
            /// Selecting alignment for your character
            /// </summary>
            /// <param name="character"></param>
            public void AlignmentSelector(Character character)
            {
                //Variable declaration
                bool isRunning = true;

                //If the class picked is paladin, you do not get a choice of alignment and as such it will automatically get set as Lawful Good
                if (character.characterClass == Class.Paladin)
                {
                    Console.WriteLine("Since you're playing Paladin, your alignment has been set as Lawful Good");
                    Thread.Sleep(1000);
                    Console.Clear();
                    character.alignment = Alignment.Lawful_Good;
                }

                //If the class is monk you can only choose between the lawful alignments
                else if (character.characterClass == Class.Monk)
                {
                    //Loop to make sure they choose one of them
                    while (isRunning)
                    {

                        Console.WriteLine("Since you're a monk you can only play as a Lawful aligned character \n" +
                            "1. LAWFUL GOOD \n" +
                            "2. LAWFUL NEUTRAL \n" +
                            "3. LAWFUL EVIL");
                        string input = Console.ReadLine();
                        //Switch case on the input, it sets the alignment as the one picked
                        switch (input)
                        {
                            case "1":
                                character.alignment = Alignment.Lawful_Good;
                                isRunning = false;
                                break;
                            case "2":
                                character.alignment = Alignment.Lawful_Neutral;
                                isRunning = false;
                                break;
                            case "3":
                                character.alignment = Alignment.Lawful_Evil;
                                isRunning = false;
                                break;
                            default:
                                Console.WriteLine("I'm sorry I don't understand");
                                break;
                        }
                    }
                }

                //There is no such restrictions on other classes and as such this gives all of the alignment as options, otherwise it works like the monk switch
                else
                {
                    while (isRunning)
                    {
                        Console.WriteLine("Which alignment does {0} have? \n" +
                            "1. LAWFUL GOOD           2. NEUTRAL GOOD          3. CHAOTIC GOOD \n\n" +
                            "4. LAWFUL NEUTRAL        5. TRUE NEUTRAL          6. CHAOTIC NEUTRAL \n\n" +
                            "7. LAWFUL EVIL           8. NEUTRAL EVIL          9. CHAOTIC EVIL", character.name);
                        string input = Console.ReadLine();

                        switch (input)
                        {
                            case "1":
                                character.alignment = Alignment.Lawful_Good;
                                isRunning = false;
                                break;
                            case "2":
                                character.alignment = Alignment.Neutral_Good;
                                isRunning = false;
                                break;
                            case "3":
                                character.alignment = Alignment.Chaotic_Good;
                                isRunning = false;
                                break;
                            case "4":
                                character.alignment = Alignment.Lawful_Neutral;
                                isRunning = false;
                                break;
                            case "5":
                                character.alignment = Alignment.True_Neutral;
                                isRunning = false;
                                break;
                            case "6":
                                character.alignment = Alignment.Chaotic_Neutral;
                                isRunning = false;
                                break;
                            case "7":
                                character.alignment = Alignment.Lawful_Evil;
                                isRunning = false;
                                break;
                            case "8":
                                character.alignment = Alignment.Neutral_Evil;
                                isRunning = false;
                                break;
                            case "9":
                                character.alignment = Alignment.Chaotic_Evil;
                                isRunning = false;
                                break;
                            default:
                                Console.WriteLine("I don't understand, try again");
                                break;
                        }

                        //Confirmation of what has been chosen as alignment
                        Console.WriteLine("You have selected {0} as your alignment", character.alignment);
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }

            /// <summary>
            /// Adding the created character to the database through MySQL and in my case XAAMP
            /// </summary>
            /// <param name="character">Character to add</param>
            public void AddToDatabase(Character character)
            {
                //Since this is the first time I have used these commands I will explain every line, the comments might get a bit much but bear with me

                //This string is the string used to connect to the database, the first part is the IP to connect to, the second is the port to connect over,
                //the third is the username on the database, in my case it is root by standard, the fourth is the password which is empty by standard 
                //and the last part is which database to use
                string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=characterdatabase;";

                //This is preparing for the connection, it uses the string we just created to create an object containing the connection information
                //I assume it splits it from the semi-colons in order to put the different things into the variables in the object, and later on these variables will be used in functions
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                //The query which we will prompt the database to run, it is written in SQL and in my case the parts where I have put "+" is where I need to use a variable
                //In order to get the correct data from the character created
                string query = "INSERT INTO characters (Name, Class, Race, Alignment, Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma) VALUES" +
                 " ('" + character.name + "', '" + character.characterClass + "', '" + character.race + "', '" + character.alignment + "', '" + character.strength + "','" + character.dexterity + "','" + character.constitution + "','" + character.intelligence + "','" + character.wisdom + "','" + character.charisma + "')";

                // This is where we combine the connection to the database and the query which we will run in a new object which has, amongst others, the function for executing
                //The query in the database
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

                //This is the command to open the connection to the database, there can only be one query per connection so it is important to open and close the connection
                //Every time you have a query to run
                databaseConnection.Open();

                //This is the command that executes the query, it is required to have the connection opened in order to run this
                commandDatabase.ExecuteReader();

                //And this is where we close the connection so that we can execute a different query later on
                databaseConnection.Close();
            }

            /// <summary>
            /// Shows the names from the database and will also give you the entire character sheet
            /// </summary>
            public void ShowNamesFromDataBase()
            {
                //Variable declaration
                bool inputRun = true;
                int IDNumber = 0;
                bool characterExists = false;

                //The same start configuration as in the function "AddToDatabase" 
                string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=characterdatabase;";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);

                string query = "SELECT * FROM characters";
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

                //This is needed if your query is for you to be able to display some rows from the database
                MySqlDataReader reader;

                //Menu loop
                while (inputRun)
                {
                    //Closing the connection in case the user wrote something incorrectly
                    databaseConnection.Close();

                    Console.WriteLine("CHARACTERS FROM THE DATABASE");

                    databaseConnection.Open();

                    //This line puts the rows from the execution into the reader
                    reader = commandDatabase.ExecuteReader();

                    //This is a loop to display all of the information needed, in the "GetString()" line the int is the column number that you want information displayed from
                    //It starts from a zero index position, in this case it will display the ID and the name from the database
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0) + ". " + reader.GetString(1));
                    }

                    databaseConnection.Close();

                    //Opening for the next query
                    databaseConnection.Open();


                    Console.WriteLine("Write the number of the character you want more information about");

                    string input = Console.ReadLine();
                    //Trying to convert the input to an int
                    try
                    {
                        IDNumber = Convert.ToInt32(input);
                        inputRun = false;
                    }
                    //Catching everything else
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }

                Console.Clear();

                //The new specified query
                query = "SELECT * FROM characters WHERE ID = '" + IDNumber + "'";
                MySqlCommand commandDatabase2 = new MySqlCommand(query, databaseConnection);

                reader = commandDatabase2.ExecuteReader();

                //Same as the above one except with more information
                while (reader.Read())
                {
                    Console.WriteLine("Name: " + reader.GetString(1) + "\n" +
                        "Class: " + reader.GetString(2) + "\n" +
                        "Race: " + reader.GetString(3) + "\n" +
                        "Alignment: " + reader.GetString(4) + "\n" +
                        "\n" +
                        "Strength: " + reader.GetString(5) + "\n" +
                        "Dexterity: " + reader.GetString(6) + "\n" +
                        "Constitution: " + reader.GetString(7) + "\n" +
                        "Intelligence: " + reader.GetString(8) + "\n" +
                        "Wisdom: " + reader.GetString(9) + "\n" +
                        "Charisma: " + reader.GetString(10) + "\n");
                    Console.ReadKey();
                    characterExists = true;
                }

                databaseConnection.Close();

                if (characterExists == false)
                {
                    Console.WriteLine("Did you pick a real character?");
                    Console.ReadKey();
                }

            }
        }

        class Dice
        {
            public Random die = new Random();

            /// <summary>
            /// A function for rolling dice
            /// </summary>
            public void DieRollingMenu()
            {
                //Variable declaration
                bool isRunning = true;
                bool numberOfDie = true;
                bool inputCheck = true;
                int rollResult = 0;
                int totalRoll = 0;
                int numberOfRolls = 0;
                string input = "";

                while (isRunning)
                {
                    //Dice selecting
                    while (inputCheck)
                    {
                        Console.WriteLine("Which die do you want to roll? \n" +
                        "1. D4 \n" +
                        "2. D6 \n" +
                        "3. D8 \n" +
                        "4. D10 \n" +
                        "5. D12 \n" +
                        "6. D20 \n" +
                        "7. D% \n" +
                        "8. Exit");

                        //Taking only the first char of input
                        input = Console.ReadLine();
                        char firstCharInput = input[0];
                        input = Convert.ToString(firstCharInput);

                        //Test on input
                        try
                        {
                            int dieSelection = Convert.ToInt32(input);
                            if (dieSelection > 8 || dieSelection <= 0)
                            {
                                Console.WriteLine("Something went wrong try again");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                inputCheck = false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("You made a grave mistake, moron");
                            Console.ReadKey();
                            Console.Clear();
                        }

                        if (input == "8")
                        {
                            Console.Clear();
                            return;
                        }
                    }

                    //Number of rolls to make
                    while (numberOfDie)
                    {
                        Console.WriteLine("How many dice do you want to roll?");
                        string stringNumberOfRolls = Console.ReadLine();

                        //Test on input
                        try
                        {
                            numberOfRolls = Convert.ToInt32(stringNumberOfRolls);
                            numberOfDie = false;
                            Console.Clear();
                        }

                        catch
                        {
                            Console.WriteLine("I asked for a number you god damn cockgobbler");
                        }
                    }

                    Console.Clear();

                    //Rolling
                    for (int i = 0; i < numberOfRolls; i++)
                    {
                        switch (input)
                        {
                            case "1":
                                rollResult = die.Next(1, 5);
                                break;
                            case "2":
                                rollResult = die.Next(1, 7);
                                break;
                            case "3":
                                rollResult = die.Next(1, 9);
                                break;
                            case "4":
                                rollResult = die.Next(1, 11);
                                break;
                            case "5":
                                rollResult = die.Next(1, 13);
                                break;
                            case "6":
                                rollResult = die.Next(1, 21);
                                break;
                            case "7":
                                rollResult = die.Next(1, 101);
                                break;
                            default:
                                Console.WriteLine("Did you write the correct number?");
                                break;
                        }
                        Console.WriteLine(rollResult);
                        totalRoll += rollResult;
                    }

                    isRunning = false;
                }
                //End result
                Console.WriteLine("Your total is " + totalRoll);
                Console.ReadKey();
                Console.Clear();
            }
        }
}

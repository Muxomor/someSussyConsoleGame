using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shamaev
{
    internal class Program
    {
        public static string[] dungeonMaps = new string[10];
        public static int health = 100;
        public static int potionsQnt = 3;
        public static int gold;
        public static int arrows = 5;
        public static bool sword = true;
        public static bool bow = true;
        public static Random random = new Random();
        public static string[] inventory;
        static void Main(string[] args)
        {
            InitializeDungeon();
            EnterDungeon();
            Console.ReadKey();
        }
        static void InitializeDungeon()
        {
            for (int i = 0; i < dungeonMaps.Length - 1; i++)
            {
                Random localRandom = new Random(i);
                int eventType = localRandom.Next(0, 4);
                switch (eventType)
                {
                    case 0:
                        dungeonMaps[i] = "Монстр";
                        break;
                    case 1:
                        dungeonMaps[i] = "Ловушка";
                        break;
                    case 2:
                        dungeonMaps[i] = "Сундук";
                        break;
                    case 3:
                        dungeonMaps[i] = "Торговец";
                        break;
                    default:
                        dungeonMaps[i] = "Комната пуста";
                        break;
                }
            }
            dungeonMaps[9] = "boss";
        }
        public static void EnterDungeon()
        {
            for (int i = 0; i < dungeonMaps.Length; i++)
            {
                Console.WriteLine($"Вы входите в подземелье №{i} емае");
                switch (dungeonMaps[i])
                {
                    case "Монстр":
                        FightMonster();
                        break;
                    case "Ловушка":
                        EnterTrapRoom();
                        break;
                    case "Сундук":
                        Console.WriteLine("Вы вошли в комнату с сундуком. Я пока не придумал что тут будет так что вы прошли мимо!");
                        break;
                    case "Торговец":
                        EnterMerchantRoom();
                        break;
                    default:
                        Console.WriteLine("Комната пуста");
                        Console.WriteLine("Вы входите в новую комнату, но она оказывается пустой. Вы просто прошли мимо.");
                        break;
                }
                if (health < 1)
                {
                    Console.WriteLine("Игра окончена! Вы проиграли!");
                    return;
                }
                Console.WriteLine($"Нажмите Enter для продолжения, или впишите Зелье чтобы выпить зелье. У вас {potionsQnt}!");
                string userChoose = Console.ReadLine();
                if (userChoose == "Зелье" || userChoose.ToLower() == "зелье")
                {
                    DrinkPotion();
                }
                else
                {
                    continue;
                }

            }
        }
        public static void FightMonster()
        {
            int monsterHealth = random.Next(20, 52);
            Console.WriteLine($"При входе в комнату вы встретили монстра с {monsterHealth} HP");
            Console.WriteLine($"Ваше здоровье: {health} HP\nВыберите оружие для сражения!");
            while (monsterHealth > 0 && health > 0)
            {

                Console.WriteLine($"Выберите оружие: 1 - Меч (10-20 урона), 2 - Лук (5-15 урона, требует стрелы. Кол-во стрел в инвентаре: {arrows}) ");
                string choice = Console.ReadLine();

                int damage = 0;
                if (choice == "1")
                {
                    damage = random.Next(10, 21);
                    Console.WriteLine($"Вы нанесли {damage} урона мечом!");
                }
                else if (choice == "2" && arrows > 0)
                {
                    damage = random.Next(5, 16);
                    arrows--;
                    Console.WriteLine($"Вы нанесли {damage} урона из лука! Осталось стрел: {arrows}");
                }
                else
                {
                    Console.WriteLine("Нельзя использовать лук!");
                    continue;
                }

                monsterHealth -= damage;
                if (monsterHealth > 0)
                {
                    int monsterDamage = random.Next(5, 16);
                    health -= monsterDamage;
                    Console.WriteLine($"Монстр нанес вам {monsterDamage} урона!");
                }
            }

            if (health > 0)
            {
                Console.WriteLine("Монстр побежден!");
            }
            else
            {
                Console.WriteLine("Вы погибли...");
            }
        }

        public static void EnterTrapRoom()
        {
            Console.WriteLine($"Вы вошли в комнату с ловушкой! Ваше здоровье:{health}");
            int trapDamage = random.Next(10, 20);
            health -= trapDamage;
            Console.WriteLine($"Вы случайно наступили на ловушку и упали в выгребную яму. Ловушка нанесла {trapDamage} урона");
        }
        public static void EnterMerchantRoom()
        {
            Console.WriteLine($"Вы вошли в комнату торговца. Он предлагает купить зелье за 30 монет. Ваши монеты: {gold}");
            if (potionsQnt >= 5 || gold < 30)
            {
                Console.WriteLine("У вас недостаточно золота на покупку или инвентарь переполнен");
            }
            else
            {
                potionsQnt++;
                gold -= 30;
            }
        }

        public static void DrinkPotion()
        {
            if (potionsQnt > 0)
            {
                int randomDigit = random.Next(10, 30);
                potionsQnt -= 1;
                health += randomDigit;
                Console.WriteLine($"Здоровье востановленно на: {randomDigit}\nЗелий осталось: {potionsQnt}");
            }
            else
            {
                Console.WriteLine("Зелья закончились!");
            }
        }
    }

}
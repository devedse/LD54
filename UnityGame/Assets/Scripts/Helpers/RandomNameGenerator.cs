using System.Collections.Generic;
using UnityEngine;

public static class RandomNameGenerator
{
    private static List<string> adjectives = new List<string>
    {
        "Fuzzy", "Sharp", "Mighty", "Swift", "Shiny", "Rusty", "Sly", "Brave", "Sneaky", "Lucky",
        "Ancient", "Bold", "Charming", "Daring", "Eager", "Fierce", "Gentle", "Humble", "Intrepid", "Jovial",
        "Keen", "Lively", "Majestic", "Noble", "Optimistic", "Proud", "Quiet", "Radiant", "Stalwart", "Tenacious"
    };

    private static List<string> nouns = new List<string>
    {
        "Tiger", "Blade", "Warrior", "Flash", "Diamond", "Nail", "Fox", "Knight", "Shadow", "Star",
        "Eagle", "Dragon", "Phoenix", "Griffin", "Sorcerer", "Wizard", "Bear", "Jaguar", "Paladin", "Mercenary",
        "Valkyrie", "Samurai", "Assassin", "Monk", "Priest", "Ranger", "Elf", "Dwarf", "Giant", "Orc"
    };

    public static string GeneratePlayerName()
    {
        return adjectives[Random.Range(0, adjectives.Count)] + " " + nouns[Random.Range(0, nouns.Count)];
    }
}
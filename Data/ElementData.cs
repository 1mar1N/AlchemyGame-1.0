using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AlchemyGame.Models;

namespace AlchemyGame.Data;


public static class ElementData
{
    public static readonly Dictionary<string, Element> Elements = new()
    {
        ["fire"]    = new Element { Id = "fire",    Name = "Огонь",      Category = "базовый",   Emoji = "🔥" },
        ["water"]   = new Element { Id = "water",   Name = "Вода",       Category = "базовый",   Emoji = "💧" },
        ["earth"]   = new Element { Id = "earth",   Name = "Земля",      Category = "базовый",   Emoji = "🌍" },
        ["air"]     = new Element { Id = "air",     Name = "Воздух",     Category = "базовый",   Emoji = "🌬" },
        ["steam"]   = new Element { Id = "steam",   Name = "Пар",        Category = "вода",      Emoji = "♨" },
        ["mud"]     = new Element { Id = "mud",     Name = "Грязь",      Category = "земля",     Emoji = "🟫" },
        ["lava"]    = new Element { Id = "lava",    Name = "Лава",       Category = "огонь",     Emoji = "🌋" },
        ["dust"]    = new Element { Id = "dust",    Name = "Пыль",       Category = "земля",     Emoji = "🌫" },
        ["rain"]    = new Element { Id = "rain",    Name = "Дождь",      Category = "вода",      Emoji = "🌧" },
        ["stone"]   = new Element { Id = "stone",   Name = "Камень",     Category = "земля",     Emoji = "🪨" },
        ["sand"]    = new Element { Id = "sand",    Name = "Песок",      Category = "земля",     Emoji = "⏳" },
        ["metal"]   = new Element { Id = "metal",   Name = "Металл",     Category = "земля",     Emoji = "⚙" },
        ["ice"]     = new Element { Id = "ice",     Name = "Лёд",        Category = "вода",      Emoji = "🧊" },
        ["cloud"]   = new Element { Id = "cloud",   Name = "Облако",     Category = "воздух",    Emoji = "☁" },
        ["plant"]   = new Element { Id = "plant",   Name = "Растение",   Category = "природа",   Emoji = "🌿" },
        ["tree"]    = new Element { Id = "tree",    Name = "Дерево",     Category = "природа",   Emoji = "🌳" },
        ["wood"]    = new Element { Id = "wood",    Name = "Брёвна",     Category = "природа",   Emoji = "🪵" },
        ["coal"]    = new Element { Id = "coal",    Name = "Уголь",      Category = "огонь",     Emoji = "⬛" },
        ["glass"]   = new Element { Id = "glass",   Name = "Стекло",     Category = "земля",     Emoji = "🔷" },
        ["smoke"]   = new Element { Id = "smoke",   Name = "Дым",        Category = "воздух",    Emoji = "💨" },
        ["volcano"] = new Element { Id = "volcano", Name = "Вулкан",     Category = "огонь",     Emoji = "🏔" },
        ["ocean"]   = new Element { Id = "ocean",   Name = "Океан",      Category = "вода",      Emoji = "🌊" },
        ["storm"]   = new Element { Id = "storm",   Name = "Гроза",      Category = "воздух",    Emoji = "⛈" },
        ["desert"]  = new Element { Id = "desert",  Name = "Пустыня",    Category = "земля",     Emoji = "🏜" },
        ["swamp"]   = new Element { Id = "swamp",   Name = "Болото",     Category = "природа",   Emoji = "🐸" },
        ["life"]    = new Element { Id = "life",    Name = "Жизнь",      Category = "особый",    Emoji = "✨" },
        ["human"]   = new Element { Id = "human",   Name = "Человек",    Category = "особый",    Emoji = "🧑" },
        ["tool"]    = new Element { Id = "tool",    Name = "Инструмент", Category = "особый",    Emoji = "🔨" },
        ["energy"]  = new Element { Id = "energy",  Name = "Энергия",    Category = "особый",    Emoji = "⚡" },
        ["light"]   = new Element { Id = "light",   Name = "Свет",       Category = "особый",    Emoji = "💡" },
    };

    public static readonly List<Recipe> Recipes = new()
    {
        new Recipe { InputA = "fire",   InputB = "water",  ResultId = "steam"   },
        new Recipe { InputA = "water",  InputB = "earth",  ResultId = "mud"     },
        new Recipe { InputA = "fire",   InputB = "earth",  ResultId = "lava"    },
        new Recipe { InputA = "earth",  InputB = "air",    ResultId = "dust"    },
        new Recipe { InputA = "air",    InputB = "water",  ResultId = "rain"    },
        new Recipe { InputA = "rain",   InputB = "earth",  ResultId = "plant"   },
        new Recipe { InputA = "stone",  InputB = "fire",   ResultId = "metal"   },
        new Recipe { InputA = "water",  InputB = "air",    ResultId = "cloud"   },
        new Recipe { InputA = "cloud",  InputB = "air",    ResultId = "storm"   },
        new Recipe { InputA = "sand",   InputB = "fire",   ResultId = "glass"   },
        new Recipe { InputA = "mud",    InputB = "fire",   ResultId = "stone"   },
        new Recipe { InputA = "lava",   InputB = "water",  ResultId = "stone"   },
        new Recipe { InputA = "fire",   InputB = "air",    ResultId = "smoke"   },
        new Recipe { InputA = "plant",  InputB = "earth",  ResultId = "tree"    },
        new Recipe { InputA = "tree",   InputB = "fire",   ResultId = "coal"    },
        new Recipe { InputA = "coal",   InputB = "fire",   ResultId = "energy"  },
        new Recipe { InputA = "stone",  InputB = "stone",  ResultId = "sand"    },
        new Recipe { InputA = "lava",   InputB = "earth",  ResultId = "volcano" },
        new Recipe { InputA = "water",  InputB = "water",  ResultId = "ocean"   },
        new Recipe { InputA = "sand",   InputB = "earth",  ResultId = "desert"  },
        new Recipe { InputA = "mud",    InputB = "plant",  ResultId = "swamp"   },
        new Recipe { InputA = "metal",  InputB = "fire",   ResultId = "energy"  },
        new Recipe { InputA = "swamp",  InputB = "energy", ResultId = "life"    },
        new Recipe { InputA = "life",   InputB = "earth",  ResultId = "plant"   },
        new Recipe { InputA = "metal",  InputB = "wood",   ResultId = "tool"    },
        new Recipe { InputA = "energy", InputB = "glass",  ResultId = "light"   },
        new Recipe { InputA = "water",  InputB = "ice",    ResultId = "ocean"   },
        new Recipe { InputA = "earth",  InputB = "earth",  ResultId = "stone"   },
        new Recipe { InputA = "air",    InputB = "fire",   ResultId = "energy"  },
        new Recipe { InputA = "water",  InputB = "stone",  ResultId = "sand"    },
        new Recipe { InputA = "tree",   InputB = "air",    ResultId = "wood"    },
        new Recipe { InputA = "water",  InputB = "cold",   ResultId = "ice"     },
        new Recipe { InputA = "ocean",  InputB = "fire",   ResultId = "steam"   },
        new Recipe { InputA = "life",   InputB = "human",  ResultId = "human"   },
    };

    public static readonly HashSet<string> StarterElements = new()
        { "fire", "water", "earth", "air" };
}

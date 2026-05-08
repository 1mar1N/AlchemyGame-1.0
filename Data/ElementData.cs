using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using AlchemyGame.Models;
namespace AlchemyGame.Data;

public static class ElementData
{
    public static readonly Dictionary<string, Element> Elements = new()
    {
        // Базовые (4)
        ["fire"]      = new() { Id="fire",      Name="Огонь",          Category="Базовые",       Emoji="🔥", Color="#FF6B35" },
        ["water"]     = new() { Id="water",     Name="Вода",           Category="Базовые",       Emoji="💧", Color="#29B6F6" },
        ["earth"]     = new() { Id="earth",     Name="Земля",          Category="Базовые",       Emoji="🌍", Color="#8D6E63" },
        ["air"]       = new() { Id="air",       Name="Воздух",         Category="Базовые",       Emoji="🌬", Color="#90A4AE" },

        // Природа — вещества (15)
        ["steam"]     = new() { Id="steam",     Name="Пар",            Category="Природа",       Emoji="♨",  Color="#B2EBF2" },
        ["mud"]       = new() { Id="mud",       Name="Грязь",          Category="Природа",       Emoji="🟫", Color="#6D4C41" },
        ["lava"]      = new() { Id="lava",      Name="Лава",           Category="Природа",       Emoji="🌋", Color="#FF5722" },
        ["dust"]      = new() { Id="dust",      Name="Пыль",           Category="Природа",       Emoji="🌫", Color="#BDBDBD" },
        ["stone"]     = new() { Id="stone",     Name="Камень",         Category="Природа",       Emoji="🪨", Color="#9E9E9E" },
        ["sand"]      = new() { Id="sand",      Name="Песок",          Category="Природа",       Emoji="⏳", Color="#FFD54F" },
        ["metal"]     = new() { Id="metal",     Name="Металл",         Category="Природа",       Emoji="⚙",  Color="#78909C" },
        ["ice"]       = new() { Id="ice",       Name="Лёд",            Category="Природа",       Emoji="🧊", Color="#81D4FA" },
        ["coal"]      = new() { Id="coal",      Name="Уголь",          Category="Природа",       Emoji="⬛", Color="#424242" },
        ["glass"]     = new() { Id="glass",     Name="Стекло",         Category="Природа",       Emoji="🔷", Color="#80DEEA" },
        ["salt"]      = new() { Id="salt",      Name="Соль",           Category="Природа",       Emoji="🧂", Color="#ECEFF1" },
        ["gold"]      = new() { Id="gold",      Name="Золото",         Category="Природа",       Emoji="🪙", Color="#FFD700" },
        ["diamond"]   = new() { Id="diamond",   Name="Алмаз",          Category="Природа",       Emoji="💎", Color="#80D8FF" },
        ["obsidian"]  = new() { Id="obsidian",  Name="Обсидиан",       Category="Природа",       Emoji="🖤", Color="#283593" },
        ["clay"]      = new() { Id="clay",      Name="Глина",          Category="Природа",       Emoji="🏺", Color="#A1887F" },

        // Ландшафт (5)
        ["ocean"]     = new() { Id="ocean",     Name="Океан",          Category="Ландшафт",      Emoji="🌊", Color="#1565C0" },
        ["volcano"]   = new() { Id="volcano",   Name="Вулкан",         Category="Ландшафт",      Emoji="🏔", Color="#BF360C" },
        ["desert"]    = new() { Id="desert",    Name="Пустыня",        Category="Ландшафт",      Emoji="🏜", Color="#F9A825" },
        ["swamp"]     = new() { Id="swamp",     Name="Болото",         Category="Ландшафт",      Emoji="🐸", Color="#558B2F" },
        ["smoke"]     = new() { Id="smoke",     Name="Дым",            Category="Ландшафт",      Emoji="💨", Color="#B0BEC5" },

        // Погода (8)
        ["rain"]      = new() { Id="rain",      Name="Дождь",          Category="Погода",        Emoji="🌧", Color="#5C6BC0" },
        ["cloud"]     = new() { Id="cloud",     Name="Облако",         Category="Погода",        Emoji="☁",  Color="#ECEFF1" },
        ["storm"]     = new() { Id="storm",     Name="Гроза",          Category="Погода",        Emoji="⛈",  Color="#455A64" },
        ["snow"]      = new() { Id="snow",      Name="Снег",           Category="Погода",        Emoji="❄",  Color="#E3F2FD" },
        ["fog"]       = new() { Id="fog",       Name="Туман",          Category="Погода",        Emoji="🌁", Color="#B0BEC5" },
        ["rainbow"]   = new() { Id="rainbow",   Name="Радуга",         Category="Погода",        Emoji="🌈", Color="#FF4081" },
        ["lightning"] = new() { Id="lightning", Name="Молния",         Category="Погода",        Emoji="⚡", Color="#FFEE58" },
        ["wind"]      = new() { Id="wind",      Name="Ветер",          Category="Погода",        Emoji="💨", Color="#80CBC4" },

        // Растения (8)
        ["plant"]     = new() { Id="plant",     Name="Растение",       Category="Живое",         Emoji="🌿", Color="#4CAF50" },
        ["tree"]      = new() { Id="tree",      Name="Дерево",         Category="Живое",         Emoji="🌳", Color="#2E7D32" },
        ["wood"]      = new() { Id="wood",      Name="Брёвна",         Category="Живое",         Emoji="🪵", Color="#795548" },
        ["flower"]    = new() { Id="flower",    Name="Цветок",         Category="Живое",         Emoji="🌸", Color="#E91E63" },
        ["mushroom"]  = new() { Id="mushroom",  Name="Гриб",           Category="Живое",         Emoji="🍄", Color="#EF5350" },
        ["seaweed"]   = new() { Id="seaweed",   Name="Водоросли",      Category="Живое",         Emoji="🌿", Color="#00897B" },
        ["cactus"]    = new() { Id="cactus",    Name="Кактус",         Category="Живое",         Emoji="🌵", Color="#388E3C" },
        ["wheat"]     = new() { Id="wheat",     Name="Пшеница",        Category="Живое",         Emoji="🌾", Color="#F9A825" },

        // Животные (7)
        ["fish"]      = new() { Id="fish",      Name="Рыба",           Category="Живое",         Emoji="🐟", Color="#0288D1" },
        ["bird"]      = new() { Id="bird",      Name="Птица",          Category="Живое",         Emoji="🐦", Color="#1976D2" },
        ["beast"]     = new() { Id="beast",     Name="Зверь",          Category="Живое",         Emoji="🐺", Color="#5D4037" },
        ["insect"]    = new() { Id="insect",    Name="Насекомое",      Category="Живое",         Emoji="🐛", Color="#558B2F" },
        ["dragon"]    = new() { Id="dragon",    Name="Дракон",         Category="Мифология",     Emoji="🐉", Color="#C62828" },
        ["phoenix"]   = new() { Id="phoenix",   Name="Феникс",         Category="Мифология",     Emoji="🦅", Color="#E65100" },
        ["life"]      = new() { Id="life",      Name="Жизнь",          Category="Особое",        Emoji="✨", Color="#8E24AA" },

        // Человек и цивилизация (10)
        ["human"]     = new() { Id="human",     Name="Человек",        Category="Цивилизация",   Emoji="🧑", Color="#F9A825" },
        ["tool"]      = new() { Id="tool",      Name="Инструмент",     Category="Цивилизация",   Emoji="🔨", Color="#546E7A" },
        ["house"]     = new() { Id="house",     Name="Дом",            Category="Цивилизация",   Emoji="🏠", Color="#FF7043" },
        ["city"]      = new() { Id="city",      Name="Город",          Category="Цивилизация",   Emoji="🏙", Color="#607D8B" },
        ["fire_pit"]  = new() { Id="fire_pit",  Name="Костёр",         Category="Цивилизация",   Emoji="🔥", Color="#FF8F00" },
        ["paper"]     = new() { Id="paper",     Name="Бумага",         Category="Цивилизация",   Emoji="📄", Color="#FFFDE7" },
        ["book"]      = new() { Id="book",      Name="Книга",          Category="Цивилизация",   Emoji="📚", Color="#3949AB" },
        ["weapon"]    = new() { Id="weapon",    Name="Оружие",         Category="Цивилизация",   Emoji="⚔",  Color="#546E7A" },
        ["wheel"]     = new() { Id="wheel",     Name="Колесо",         Category="Цивилизация",   Emoji="⚙",  Color="#6D4C41" },
        ["ash"]       = new() { Id="ash",       Name="Пепел",          Category="Цивилизация",   Emoji="🌫", Color="#757575" },

        // Еда (8)
        ["bread"]     = new() { Id="bread",     Name="Хлеб",           Category="Еда",           Emoji="🍞", Color="#FF8A65" },
        ["meat"]      = new() { Id="meat",      Name="Мясо",           Category="Еда",           Emoji="🥩", Color="#EF5350" },
        ["soup"]      = new() { Id="soup",      Name="Суп",            Category="Еда",           Emoji="🍲", Color="#FF7043" },
        ["beer"]      = new() { Id="beer",      Name="Пиво",           Category="Еда",           Emoji="🍺", Color="#FFB300" },
        ["cheese"]    = new() { Id="cheese",    Name="Сыр",            Category="Еда",           Emoji="🧀", Color="#FFD740" },
        ["milk"]      = new() { Id="milk",      Name="Молоко",         Category="Еда",           Emoji="🥛", Color="#ECEFF1" },
        ["honey"]     = new() { Id="honey",     Name="Мёд",            Category="Еда",           Emoji="🍯", Color="#FF8F00" },
        ["fish_dish"] = new() { Id="fish_dish", Name="Жареная рыба",   Category="Еда",           Emoji="🍤", Color="#FF6D00" },

        // Технологии (6)
        ["energy"]       = new() { Id="energy",       Name="Энергия",       Category="Технологии",    Emoji="⚡", Color="#FDD835" },
        ["light"]        = new() { Id="light",        Name="Свет",          Category="Технологии",    Emoji="💡", Color="#FFEE58" },
        ["electricity"]  = new() { Id="electricity",  Name="Электричество", Category="Технологии",    Emoji="🔌", Color="#FFCA28" },
        ["computer"]     = new() { Id="computer",     Name="Компьютер",     Category="Технологии",    Emoji="💻", Color="#1E88E5" },
        ["internet"]     = new() { Id="internet",     Name="Интернет",      Category="Технологии",    Emoji="🌐", Color="#00ACC1" },
        ["robot"]        = new() { Id="robot",        Name="Робот",         Category="Технологии",    Emoji="🤖", Color="#78909C" },

        // Космос (5)
        ["sun"]       = new() { Id="sun",       Name="Солнце",         Category="Космос",        Emoji="☀",  Color="#FF6F00" },
        ["moon"]      = new() { Id="moon",      Name="Луна",           Category="Космос",        Emoji="🌙", Color="#9E9E9E" },
        ["star"]      = new() { Id="star",      Name="Звезда",         Category="Космос",        Emoji="⭐", Color="#FFF176" },
        ["planet"]    = new() { Id="planet",    Name="Планета",        Category="Космос",        Emoji="🪐", Color="#AB47BC" },
        ["universe"]  = new() { Id="universe",  Name="Вселенная",      Category="Космос",        Emoji="🌌", Color="#1A237E" },
    };

    // ── 92 рецепта (уникальные пары) 
    public static readonly List<Recipe> Recipes = new()
    {
        // ── БАЗОВЫЕ (10) 
        new() { InputA="fire",      InputB="water",      ResultId="steam"       },
        new() { InputA="water",     InputB="earth",      ResultId="mud"         },
        new() { InputA="fire",      InputB="earth",      ResultId="lava"        },
        new() { InputA="earth",     InputB="air",        ResultId="dust"        },
        new() { InputA="water",     InputB="air",        ResultId="cloud"       },
        new() { InputA="fire",      InputB="air",        ResultId="smoke"       },
        new() { InputA="earth",     InputB="earth",      ResultId="stone"       },
        new() { InputA="water",     InputB="water",      ResultId="ocean"       },
        new() { InputA="air",       InputB="air",        ResultId="wind"        },
        new() { InputA="fire",      InputB="fire",       ResultId="energy"      },

        // ── ПОГОДА (8) 
        new() { InputA="cloud",     InputB="water",      ResultId="rain"        },
        new() { InputA="cloud",     InputB="air",        ResultId="storm"       },
        new() { InputA="steam",     InputB="air",        ResultId="fog"         },
        new() { InputA="cloud",     InputB="ice",        ResultId="snow"        },
        new() { InputA="rain",      InputB="sun",        ResultId="rainbow"     },
        new() { InputA="storm",     InputB="energy",     ResultId="lightning"   },
        new() { InputA="snow",      InputB="sun",        ResultId="water"       },
        new() { InputA="wind",      InputB="water",      ResultId="ice"         },

        // ── ПРИРОДА — МИНЕРАЛЫ (12) 
        new() { InputA="mud",       InputB="fire",       ResultId="stone"       },
        new() { InputA="lava",      InputB="water",      ResultId="stone"       },
        new() { InputA="lava",      InputB="earth",      ResultId="volcano"     },
        new() { InputA="stone",     InputB="stone",      ResultId="sand"        },
        new() { InputA="sand",      InputB="earth",      ResultId="desert"      },
        new() { InputA="stone",     InputB="fire",       ResultId="metal"       },
        new() { InputA="sand",      InputB="fire",       ResultId="glass"       },
        new() { InputA="mud",       InputB="stone",      ResultId="clay"        },
        new() { InputA="lava",      InputB="stone",      ResultId="obsidian"    },
        new() { InputA="metal",     InputB="stone",      ResultId="gold"        },
        new() { InputA="glass",     InputB="stone",      ResultId="diamond"     },
        new() { InputA="ocean",     InputB="sun",        ResultId="salt"        },

        // ── ЛАНДШАФТ (5) 
        new() { InputA="ocean",     InputB="fire",       ResultId="steam"       },
        new() { InputA="ocean",     InputB="earth",      ResultId="swamp"       },
        new() { InputA="mud",       InputB="plant",      ResultId="swamp"       },
        new() { InputA="lava",      InputB="air",        ResultId="smoke"       },
        new() { InputA="volcano",   InputB="water",      ResultId="stone"       },

        // ── РАСТЕНИЯ (10) 
        new() { InputA="rain",      InputB="earth",      ResultId="plant"       },
        new() { InputA="plant",     InputB="earth",      ResultId="tree"        },
        new() { InputA="tree",      InputB="fire",       ResultId="coal"        },
        new() { InputA="tree",      InputB="air",        ResultId="wood"        },
        new() { InputA="plant",     InputB="water",      ResultId="flower"      },
        new() { InputA="plant",     InputB="swamp",      ResultId="mushroom"    },
        new() { InputA="water",     InputB="plant",      ResultId="seaweed"     },
        new() { InputA="plant",     InputB="desert",     ResultId="cactus"      },
        new() { InputA="plant",     InputB="sun",        ResultId="wheat"       },
        new() { InputA="wood",      InputB="fire",       ResultId="coal"        },

        // ── ЖИВОТНЫЕ И ЖИЗНЬ (9) 
        new() { InputA="swamp",     InputB="energy",     ResultId="life"        },
        new() { InputA="life",      InputB="ocean",      ResultId="fish"        },
        new() { InputA="life",      InputB="air",        ResultId="bird"        },
        new() { InputA="life",      InputB="earth",      ResultId="beast"       },
        new() { InputA="life",      InputB="plant",      ResultId="insect"      },
        new() { InputA="beast",     InputB="fire",       ResultId="dragon"      },
        new() { InputA="bird",      InputB="fire",       ResultId="phoenix"     },
        new() { InputA="insect",    InputB="flower",     ResultId="honey"       },
        new() { InputA="beast",     InputB="water",      ResultId="milk"        },

        // ── ЧЕЛОВЕК И ЦИВИЛИЗАЦИЯ (13)
        new() { InputA="life",      InputB="beast",      ResultId="human"       },
        new() { InputA="human",     InputB="wood",       ResultId="tool"        },
        new() { InputA="human",     InputB="stone",      ResultId="tool"        },
        new() { InputA="human",     InputB="fire",       ResultId="fire_pit"    },
        new() { InputA="tool",      InputB="stone",      ResultId="house"       },
        new() { InputA="tool",      InputB="wood",       ResultId="house"       },
        new() { InputA="clay",      InputB="fire",       ResultId="house"       },
        new() { InputA="house",     InputB="human",      ResultId="city"        },
        new() { InputA="tool",      InputB="metal",      ResultId="weapon"      },
        new() { InputA="wood",      InputB="stone",      ResultId="wheel"       },
        new() { InputA="wood",      InputB="water",      ResultId="paper"       },
        new() { InputA="paper",     InputB="fire",       ResultId="ash"         },
        new() { InputA="paper",     InputB="human",      ResultId="book"        },

        // ── ЕДА (8)
        new() { InputA="wheat",     InputB="fire",       ResultId="bread"       },
        new() { InputA="beast",     InputB="tool",       ResultId="meat"        },
        new() { InputA="fish",      InputB="fire",       ResultId="fish_dish"   },
        new() { InputA="meat",      InputB="water",      ResultId="soup"        },
        new() { InputA="fire_pit",  InputB="meat",       ResultId="soup"        },
        new() { InputA="wheat",     InputB="water",      ResultId="beer"        },
        new() { InputA="milk",      InputB="fire",       ResultId="cheese"      },
        new() { InputA="salt",      InputB="fish",       ResultId="fish_dish"   },

        // ── ТЕХНОЛОГИИ (7)
        new() { InputA="energy",    InputB="glass",      ResultId="light"       },
        new() { InputA="lightning", InputB="metal",      ResultId="electricity" },
        new() { InputA="electricity",InputB="metal",     ResultId="computer"    },
        new() { InputA="computer",  InputB="human",      ResultId="internet"    },
        new() { InputA="electricity",InputB="human",     ResultId="robot"       },
        new() { InputA="tool",      InputB="electricity",ResultId="robot"       },
        new() { InputA="electricity",InputB="glass",     ResultId="light"       },

        // ── КОСМОС (6)
        new() { InputA="fire",      InputB="energy",     ResultId="sun"         },
        new() { InputA="stone",     InputB="ice",        ResultId="moon"        },
        new() { InputA="energy",    InputB="stone",      ResultId="star"        },
        new() { InputA="star",      InputB="earth",      ResultId="planet"      },
        new() { InputA="planet",    InputB="star",       ResultId="universe"    },
        new() { InputA="sun",       InputB="planet",     ResultId="universe"    },

        // ── ДОПОЛНИТЕЛЬНЫЕ РЕЦЕПТЫ (4)
        new() { InputA="rain",      InputB="stone",      ResultId="clay"        },
        new() { InputA="fog",       InputB="sun",        ResultId="rainbow"     },
        new() { InputA="ice",       InputB="fire",       ResultId="water"       },
        new() { InputA="coal",      InputB="stone",      ResultId="diamond"     },
    };

    public static readonly HashSet<string> StarterElements = new()
        { "fire", "water", "earth", "air" };
}

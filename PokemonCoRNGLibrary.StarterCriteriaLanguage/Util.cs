using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using PokemonStandardLibrary;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    static class ConvertExt
    {
        private static readonly Dictionary<string, Nature> dict_nature = new Dictionary<string, Nature>()
        {
            { "がんばりや", Nature.Hardy },
            { "さみしがり", Nature.Lonely },
            { "ゆうかん", Nature.Brave },
            { "いじっぱり", Nature.Adamant },
            { "やんちゃ", Nature.Naughty },
            { "ずぶとい", Nature.Bold },
            { "すなお", Nature.Docile },
            { "のんき", Nature.Relaxed },
            { "わんぱく", Nature.Impish },
            { "のうてんき", Nature.Lax },
            { "おくびょう", Nature.Timid },
            { "せっかち", Nature.Hasty },
            { "まじめ", Nature.Serious },
            { "ようき", Nature.Jolly },
            { "むじゃき", Nature.Naive },
            { "ひかえめ", Nature.Modest },
            { "おっとり", Nature.Mild },
            { "れいせい", Nature.Quiet },
            { "てれや", Nature.Bashful },
            { "うっかりや", Nature.Rash },
            { "おだやか", Nature.Calm },
            { "おとなしい", Nature.Gentle },
            { "なまいき", Nature.Sassy },
            { "しんちょう", Nature.Careful },
            { "きまぐれ", Nature.Quirky },

            { "Hardy", Nature.Hardy },
            { "Lonely", Nature.Lonely },
            { "Brave", Nature.Brave },
            { "Adamant", Nature.Adamant },
            { "Naughty", Nature.Naughty },
            { "Bold", Nature.Bold },
            { "Docile", Nature.Docile },
            { "Relaxed", Nature.Relaxed },
            { "Impish", Nature.Impish },
            { "Lax", Nature.Lax },
            { "Timid", Nature.Timid },
            { "Hasty", Nature.Hasty },
            { "Serious", Nature.Serious },
            { "Jolly", Nature.Jolly },
            { "Naive", Nature.Naive },
            { "Modest", Nature.Modest },
            { "Mild", Nature.Mild },
            { "Quiet", Nature.Quiet },
            { "Bashful", Nature.Bashful },
            { "Rash", Nature.Rash },
            { "Calm", Nature.Calm },
            { "Gentle", Nature.Gentle },
            { "Sassy", Nature.Sassy },
            { "Careful", Nature.Careful },
            { "Quirky", Nature.Quirky },

            { "hardy", Nature.Hardy },
            { "lonely", Nature.Lonely },
            { "brave", Nature.Brave },
            { "adamant", Nature.Adamant },
            { "naughty", Nature.Naughty },
            { "bold", Nature.Bold },
            { "docile", Nature.Docile },
            { "relaxed", Nature.Relaxed },
            { "impish", Nature.Impish },
            { "lax", Nature.Lax },
            { "timid", Nature.Timid },
            { "hasty", Nature.Hasty },
            { "serious", Nature.Serious },
            { "jolly", Nature.Jolly },
            { "naive", Nature.Naive },
            { "modest", Nature.Modest },
            { "mild", Nature.Mild },
            { "quiet", Nature.Quiet },
            { "bashful", Nature.Bashful },
            { "rash", Nature.Rash },
            { "calm", Nature.Calm },
            { "gentle", Nature.Gentle },
            { "sassy", Nature.Sassy },
            { "careful", Nature.Careful },
            { "quirky", Nature.Quirky },

            { "HARDY", Nature.Hardy },
            { "LONELY", Nature.Lonely },
            { "BRAVE", Nature.Brave },
            { "ADAMANT", Nature.Adamant },
            { "NAUGHTY", Nature.Naughty },
            { "BOLD", Nature.Bold },
            { "DOCILE", Nature.Docile },
            { "RELAXED", Nature.Relaxed },
            { "IMPISH", Nature.Impish },
            { "LAX", Nature.Lax },
            { "TIMID", Nature.Timid },
            { "HASTY", Nature.Hasty },
            { "SERIOUS", Nature.Serious },
            { "JOLLY", Nature.Jolly },
            { "NAIVE", Nature.Naive },
            { "MODEST", Nature.Modest },
            { "MILD", Nature.Mild },
            { "QUIET", Nature.Quiet },
            { "BASHFUL", Nature.Bashful },
            { "RASH", Nature.Rash },
            { "CALM", Nature.Calm },
            { "GENTLE", Nature.Gentle },
            { "SASSY", Nature.Sassy },
            { "CAREFUL", Nature.Careful },
            { "QUIRKY", Nature.Quirky },
        };
        private static readonly Dictionary<string, PokeType> dict_type = new Dictionary<string, PokeType>()
        {
            { "Fire", PokeType.Fire },
            { "Water", PokeType.Water },
            { "Grass", PokeType.Grass },
            { "Electric", PokeType.Electric },
            { "Ice", PokeType.Ice },
            { "Fighting", PokeType.Fighting },
            { "Poison", PokeType.Poison },
            { "Ground", PokeType.Ground },
            { "Flying", PokeType.Flying },
            { "Psychic", PokeType.Psychic },
            { "Bug", PokeType.Bug },
            { "Rock", PokeType.Rock },
            { "Ghost", PokeType.Ghost },
            { "Dragon", PokeType.Dragon },
            { "Dark", PokeType.Dark },
            { "Steel", PokeType.Steel },

            { "FIRE", PokeType.Fire },
            { "WATER", PokeType.Water },
            { "GRASS", PokeType.Grass },
            { "ELECTRIC", PokeType.Electric },
            { "ICE", PokeType.Ice },
            { "FIGHTING", PokeType.Fighting },
            { "POISON", PokeType.Poison },
            { "GROUND", PokeType.Ground },
            { "FLYING", PokeType.Flying },
            { "PSYCHIC", PokeType.Psychic },
            { "BUG", PokeType.Bug },
            { "ROCK", PokeType.Rock },
            { "GHOST", PokeType.Ghost },
            { "DRAGON", PokeType.Dragon },
            { "DARK", PokeType.Dark },
            { "STEEL", PokeType.Steel },

            { "fire", PokeType.Fire },
            { "water", PokeType.Water },
            { "grass", PokeType.Grass },
            { "electric", PokeType.Electric },
            { "ice", PokeType.Ice },
            { "fighting", PokeType.Fighting },
            { "poison", PokeType.Poison },
            { "ground", PokeType.Ground },
            { "flying", PokeType.Flying },
            { "psychic", PokeType.Psychic },
            { "bug", PokeType.Bug },
            { "rock", PokeType.Rock },
            { "ghost", PokeType.Ghost },
            { "dragon", PokeType.Dragon },
            { "dark", PokeType.Dark },
            { "steel", PokeType.Steel },

            { "ほのお", PokeType.Fire },
            { "みず", PokeType.Water },
            { "くさ", PokeType.Grass },
            { "でんき", PokeType.Electric },
            { "こおり", PokeType.Ice },
            { "かくとう", PokeType.Fighting },
            { "どく", PokeType.Poison },
            { "じめん", PokeType.Ground },
            { "ひこう", PokeType.Flying },
            { "エスパー", PokeType.Psychic },
            { "むし", PokeType.Bug },
            { "いわ", PokeType.Rock },
            { "ゴースト", PokeType.Ghost },
            { "ドラゴン", PokeType.Dragon },
            { "あく", PokeType.Dark },
            { "はがね", PokeType.Steel },

            { "炎", PokeType.Fire },
            { "水", PokeType.Water },
            { "草", PokeType.Grass },
            { "電", PokeType.Electric },
            { "氷", PokeType.Ice },
            { "闘", PokeType.Fighting },
            { "毒", PokeType.Poison },
            { "地", PokeType.Ground },
            { "飛", PokeType.Flying },
            { "超", PokeType.Psychic },
            { "虫", PokeType.Bug },
            { "岩", PokeType.Rock },
            { "霊", PokeType.Ghost },
            { "竜", PokeType.Dragon },
            { "龍", PokeType.Dragon },
            { "悪", PokeType.Dark },
            { "鋼", PokeType.Steel },

        };

        public static Nature TryConvertToNature(this string maybeNatureStr)
        {
            if (!dict_nature.TryGetValue(maybeNatureStr, out var nature)) return Nature.other;

            return nature;
        }

        public static PokeType TryConvertToPokeType(this string maybeTypeStr)
        {
            if (!dict_type.TryGetValue(maybeTypeStr, out var type)) return PokeType.None;

            return type;
        }

        public static bool TryConvertToTID(this string maybeTIDStr, out uint tid)
        {
            if (!uint.TryParse(maybeTIDStr, out tid)) return false;

            if (tid >= 0x10000) return false;

            return true;
        }

        public static bool TryConvertToPID(this string maybePIDStr, out uint pid)
        {
            pid = 0;
            if (Regex.IsMatch(maybePIDStr, "[^0-9A-F]")) return false;
            pid = Convert.ToUInt32(maybePIDStr, 16);

            return true;
        }
    }

    public static class StarterCriteriaLanguage
    {
        private readonly static HashSet<char> symbols = new HashSet<char>() { ':', ',', '[', ']', '{', '}' };
        private readonly static HashSet<char> ignore = new HashSet<char>() { ' ', '\t', '\r', '\n' };

        private static IEnumerable<string> ConvertToTokens(string source)
        {
            using (var reader = new StringReader(source))
            {
                int n;
                string token = "";
                while ((n = reader.Read()) != -1)
                {
                    var c = (char)n;

                    if (symbols.Contains(c))
                    {
                        if (token.Length > 0)
                            yield return token;
                        token = "";

                        yield return c.ToString();
                    }
                    else if (ignore.Contains(c))
                    {
                        if (token.Length > 0)
                            yield return token;
                        token = "";
                    }
                    else
                        token += c;
                }
                if (token.Length > 0) yield return token;
            }
        }
        private static IEnumerable<Symbol> ConvertToSymbols(this IEnumerable<string> tokens)
        {
            var hold = "";
            foreach (var token in tokens)
            {
                if (token == ":")
                {
                    if (hold == "") throw new Exception("unexpected ':'");

                    yield return new PropertyName(hold);
                    hold = "";
                    continue;
                }

                if (hold != "")
                    yield return new Value(hold);

                var maybeMono = TryConvertMono(token);
                if (maybeMono != null) yield return maybeMono;

                hold = maybeMono == null ? token : "";
            }

            if (hold != "")
                yield return new Value(hold);
        }

        private static Symbol TryConvertMono(string mono)
        {
            if (mono == "[") return ListOpen.Instance;
            if (mono == "]") return ListClose.Instance;
            if (mono == "{") return ObjectOpen.Instance;
            if (mono == "}") return ObjectClose.Instance;
            if (mono == ",") return Comma.Instance;

            return null;
        }

        public static ICriteriaNode<CoStarterResult> TryParse(string source)
        {
            var it = new SymbolIterator(ConvertToTokens(source).ConvertToSymbols().GetEnumerator());
            var parser = new ListParser<CoStarterResult>(new CoStarterCriteriaParser());

            try
            {
                var node = new OrCriteriaNode<CoStarterResult>(parser.Parse(it));
                if (it.HasValue) throw new UnexpectedSymbolException(it.GetAndNext());
                return node;
            }
            catch(Exception e)
            {
                throw new Exception($"{e.Message}{Environment.NewLine}{it.GetUsedSymbols()}<-");
            }
        }
    }
}

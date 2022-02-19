using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
    interface Symbol { }

    class ListOpen : Symbol
    {
        public static readonly ListOpen Instance = new ListOpen();
        public override string ToString() => "[";
    }

    class ListClose : Symbol
    {
        public static readonly ListClose Instance = new ListClose();
        public override string ToString() => "]";
    }

    class ObjectOpen : Symbol
    {
        public static readonly ObjectOpen Instance = new ObjectOpen();
        public override string ToString() => "{";
    }

    class ObjectClose : Symbol
    {
        public static readonly ObjectClose Instance = new ObjectClose();
        public override string ToString() => "}";
    }

    class PropertyName : Symbol
    {
        public string Name { get; }
        public PropertyName(string name) => Name = name;
        public override string ToString() => $"{Name}:";
    }

    class Value : Symbol
    {
        public string Name { get; }
        public Value(string name) => Name = name;
        public override string ToString() => Name;
    }

    class Comma : Symbol
    {
        public static readonly Comma Instance = new Comma();
        public override string ToString() => ",";
    }
}

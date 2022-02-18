using System;

namespace PokemonCoRNGLibrary.StarterCriteriaLanguage
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名スタイル", Justification = "<保留中>")]
    interface Symbol { }

    class ListOpen : Symbol
    {
        public static readonly ListOpen Instance = new ListOpen();
    }

    class ListClose : Symbol
    {
        public static readonly ListClose Instance = new ListClose();
    }

    class ObjectOpen : Symbol
    {
        public static readonly ObjectOpen Instance = new ObjectOpen();
    }

    class ObjectClose : Symbol
    {
        public static readonly ObjectClose Instance = new ObjectClose();
    }

    class PropertyName : Symbol
    {
        public string Name { get; }
        public PropertyName(string name) => Name = name;
    }

    class Value : Symbol
    {
        public string Name { get; }
        public Value(string name) => Name = name;
    }

    class Comma : Symbol
    {
        public static readonly Comma Instance = new Comma();
    }
}

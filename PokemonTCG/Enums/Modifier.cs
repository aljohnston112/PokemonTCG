namespace PokemonTCG.Enums
{

    internal class Modifier
    {

        internal readonly ModifierType ModifierType;
        internal readonly int Value;

        internal Modifier(ModifierType modifierType, int value)
        {
            ModifierType = modifierType;
            Value = value;
        }

    }

}

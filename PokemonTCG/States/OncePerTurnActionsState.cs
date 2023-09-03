namespace PokemonTCG.States
{

    internal class OncePerTurnActionsState
    {

        internal readonly bool HasAttachedEnergy;
        internal readonly bool HasUsedSupporter;
        internal readonly bool HasUsedStadium;
        internal readonly bool HasRetreated;

        internal OncePerTurnActionsState(
            bool hasAttachedEnergy,
            bool hasUsedSupporter,
            bool hasUsedStadium,
            bool hasRetreated
            )
        {
            HasAttachedEnergy = hasAttachedEnergy;
            HasUsedSupporter = hasUsedSupporter;
            HasUsedStadium = hasUsedStadium;
            HasRetreated = hasRetreated;
        }

        internal OncePerTurnActionsState AfterAttachingEnergy()
        {
            return new OncePerTurnActionsState(
                hasAttachedEnergy: true,
                hasUsedSupporter: HasUsedSupporter,
                hasUsedStadium: HasUsedStadium,
                hasRetreated: HasRetreated
                );
        }

    }

}
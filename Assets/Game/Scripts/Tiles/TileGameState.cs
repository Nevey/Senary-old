namespace CCore.Senary.Tiles
{
    public enum TileGameState
    {
        NotAvailable,
        AvailableForReinforcement,
        AvailableForTakeOver,
        AvailableAsTarget,
        AvailableAsAttacker,
        SelectedAsTarget,
        SelectedAsAttacker,
        InvadingFrom,
        InvadingTo,
        Disconnected
    }
}
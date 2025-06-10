using System;

namespace gLibrary.Core.Saving;

public interface ISaveableGame
{
    GridState ToGameState();
    void FromGameState(GridState state);
}

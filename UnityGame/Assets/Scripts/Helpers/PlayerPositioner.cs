using System.Collections.Generic;
using UnityEngine;

public static class PlayerPositioner
{
    public static (List<float> positions, float scale) DistributePlayers(int totalPlayers, float startRange, float endRange, float playerWidthNormal, float gapNormal)
    {
        List<float> positions = new List<float>();

        // Calculate total space required at normal scale for all players and gaps
        float totalWidthRequiredNormal = totalPlayers * playerWidthNormal + (totalPlayers - 1) * gapNormal;

        // Calculate the scale factor if required
        float spaceAvailable = (endRange - startRange);
        float scale = Mathf.Min(1, spaceAvailable / totalWidthRequiredNormal);

        playerWidthNormal *= scale;
        gapNormal *= scale;

        // Adjusted space required after scaling
        float adjustedTotalWidthRequired = totalPlayers * playerWidthNormal + (totalPlayers - 1) * gapNormal;

        // Calculate the starting position to ensure players are centered
        float startPos = startRange + ((spaceAvailable - adjustedTotalWidthRequired) / 2) + playerWidthNormal / 2;

        for (int i = 0; i < totalPlayers; i++)
        {
            positions.Add(startPos + i * (playerWidthNormal + gapNormal));
        }

        return (positions, scale);
    }
}
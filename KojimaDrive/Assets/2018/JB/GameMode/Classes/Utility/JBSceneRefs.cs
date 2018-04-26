using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class JBSceneRefs
{
    public static DuckHuntBoat boat
    {
        get
        {
            if (boat_ == null)
                boat_ = Object.FindObjectOfType<DuckHuntBoat>();

            return boat_;
        }
    }

    public static GeneralCanvas generalCanvas
    {
        get
        {
            if (generalCanvas_ == null)
                generalCanvas_ = Object.FindObjectOfType<GeneralCanvas>();

            return generalCanvas_;
        }
    }

    public static PlayerScore playerScore
    {
        get
        {
            if (playerScore_ == null)
                playerScore_ = Object.FindObjectOfType<PlayerScore>();

            return playerScore_;
        }
    }

    public static ObjectSpawner objectSpawner
    {
        get
        {
            if (objectSpawner_ == null)
                objectSpawner_ = Object.FindObjectOfType<ObjectSpawner>();

            return objectSpawner_;
        }
    }

    private static DuckHuntBoat boat_;
    private static GeneralCanvas generalCanvas_;
    private static PlayerScore playerScore_;
    private static ObjectSpawner objectSpawner_;
}

} // namespace JB

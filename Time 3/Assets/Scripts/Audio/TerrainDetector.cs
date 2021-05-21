using UnityEngine;

public class TerrainDetector
{
    private Terrain GetClosestCurrentTerrain(Vector3 playerPos)
    {
        //Get the closest one to the player
        Terrain[] _terrains = Terrain.activeTerrains;
        var center = new Vector3(_terrains[0].transform.position.x + _terrains[0].terrainData.size.x / 2, playerPos.y, _terrains[0].transform.position.z + _terrains[0].terrainData.size.z / 2);
    
        float lowDist = (center - playerPos).sqrMagnitude;
        var terrainIndex = 0;

        for (int i = 0; i < _terrains.Length; i++)
        {
            center = new Vector3(_terrains[i].transform.position.x + _terrains[i].terrainData.size.x / 2, playerPos.y, _terrains[i].transform.position.z + _terrains[i].terrainData.size.z / 2);

            //Find the distance and check if it is lower than the last one then store it
            var dist = (center - playerPos).sqrMagnitude;
            if (dist < lowDist)
            {
                lowDist = dist;
                terrainIndex = i;
            }
        }
        return _terrains[terrainIndex];
    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition, Terrain terrain)
    {
        Vector3 splatPosition = new Vector3();
        Vector3 terPosition = terrain.transform.position;//GetPosition()?
        splatPosition.x = ((worldPosition.x - terPosition.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth;
        splatPosition.z = ((worldPosition.z - terPosition.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight;
        return splatPosition;
    }

    public int GetActiveTerrainTextureIdx(Vector3 position)
    {
        Terrain currentTerrain = GetClosestCurrentTerrain(position);

        TerrainData terrainData = currentTerrain.terrainData;
        int alphamapWidth = terrainData.alphamapWidth;
        int alphamapHeight = terrainData.alphamapHeight;

        float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        int numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
        
        Vector3 terrainCord = ConvertToSplatMapCoordinate(position, currentTerrain);
        int activeTerrainTextureIndex = 0;
        float largestOpacity = 0f;

        for (int i = 0; i < numTextures; i++)
        {
            if (largestOpacity < splatmapData[(int)terrainCord.z, (int)terrainCord.x, i])
            {
                activeTerrainTextureIndex = i;
                largestOpacity = splatmapData[(int)terrainCord.z, (int)terrainCord.x, i];
            }
        }

        return activeTerrainTextureIndex;
    }
}
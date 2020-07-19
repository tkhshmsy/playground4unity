using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Soukoban : MonoBehaviour
{
    private enum TileType {
        NONE = 0,
        GROUND,
        TARGET,
        PLAYER,
        BLOCK,
        PLAYER_ON_TAREGT,
        BLOCK_ON_TARGET,
    };

    public TextAsset stageFile;
    private int rows;
    private int cols;
    private TileType[,] tileList;

    private void LoadTileFile() {
        var lines = stageFile.text.Split(
            new[]{'\r', '\n'},
            System.StringSplitOptions.RemoveEmptyEntries
        );
        var nums = lines[0].Split( new[]{','} );
        rows = lines.Length;
        cols = nums.Length;
        tileList = new TileType[ cols, rows ];
        for(int y = 0; y < rows; y++) {
            var st = lines[y];
            nums = st.Split( new[]{ ',' });
            for (int x = 0; x < cols; x++) {
                tileList[ x, y ] = (TileType)int.Parse( nums[x]);
            }
        }
    }

    public float tileSize;
    public Sprite groundSprite;
    public Sprite targetSprite;
    public Sprite playerSprite;
    public Sprite blockSprite;
    private GameObject player;
    private Vector2 middleOffset;
    private int blockCount;
    private Dictionary<GameObject, Vector2Int> gameObjectPositionTable = new Dictionary<GameObject, Vector2Int>();

    private void CreateStage() {
        middleOffset.x = cols * tileSize * 0.5f - tileSize * 0.5f;
        middleOffset.y = rows * tileSize * 0.5f - tileSize * 0.5f;

        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < cols; x++) {
                var val = tileList[ x, y ];
                if (val == TileType.NONE) {
                    continue;
                }
                var name = "tile" + y + "_" + x;
                var tile = new GameObject( name );
                var sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = groundSprite;
                tile.transform.position = GetDisplayPosition( x, y );
                if ( val == TileType.TARGET) {
                    var destination = new GameObject("destination");
                    sr = destination.AddComponent<SpriteRenderer>();
                    sr.sprite = targetSprite;
                    sr.sortingOrder = 1;
                    destination.transform.position = GetDisplayPosition( x, y );
                }
                if (val == TileType.PLAYER) {
                    player = new GameObject( "player" );
                    sr = player.AddComponent<SpriteRenderer>();
                    sr.sprite = playerSprite;
                    sr.sortingOrder = 2;
                    player.transform.position = GetDisplayPosition( x, y );
                    gameObjectPositionTable.Add( player, new Vector2Int(x, y));
                } else if (val == TileType.BLOCK) {
                    blockCount++;
                    var block = new GameObject("block" + blockCount);
                    sr = block.AddComponent<SpriteRenderer>();
                    sr.sprite = blockSprite;
                    sr.sortingOrder = 2;
                    block.transform.position = GetDisplayPosition(x, y);
                    gameObjectPositionTable.Add(block, new Vector2Int(x, y));
                }
            }
        }
    }
    private Vector2 GetDisplayPosition(int x, int y)
    {
        return new Vector2(
             x * tileSize - middleOffset.x,
             -y *  tileSize + middleOffset.y
        );
    }

    private void Start() {
        LoadTileFile();
        CreateStage();
    }

    private GameObject GetGameObjectAtPosition(Vector2Int pos)
    {
        foreach(var pair in gameObjectPositionTable) {
            if (pair.Value == pos) {
                return pair.Key;
            }
        }
        return null;
    }

    private bool IsValidPosition(Vector2Int pos)
    {
        if ( 0 <= pos.x 
        && pos.x < cols
        && 0 <= pos.y
        && pos.y < rows) {
            return tileList[ pos.x, pos.y ] != TileType.NONE;
        }
        return false;
    }

    private bool IsBlock(Vector2Int pos)
    {
        var cell = tileList[ pos.x, pos.y ];
        return cell == TileType.BLOCK || cell == TileType.BLOCK_ON_TARGET;
    }

    private void Update()
    {
        if (isClear) {
            return;
        }
        if (Input.GetKeyDown( KeyCode.UpArrow )) {
            TryMovePlayer(DirectionType.Up);
        }
        if (Input.GetKeyDown( KeyCode.DownArrow )) {
            TryMovePlayer(DirectionType.Down);
        }
        if (Input.GetKeyDown( KeyCode.LeftArrow )) {
            TryMovePlayer(DirectionType.Left);
        }
        if (Input.GetKeyDown( KeyCode.RightArrow )) {
            TryMovePlayer(DirectionType.Right);
        }
    }

    private bool isClear = false;
    private enum DirectionType {
        Up,
        Down,
        Left,
        Right,
    }

    private void TryMovePlayer(DirectionType direction)
    {
        var currentPlayerPos = gameObjectPositionTable[ player ];
        var nextPlayerPos = GetNextPositionAlong( currentPlayerPos, direction );
        if (!IsValidPosition(nextPlayerPos)) {
            return;
        }
        if (IsBlock(nextPlayerPos)) {
            var nextBlockPos = GetNextPositionAlong( nextPlayerPos, direction );
            if (IsValidPosition(nextBlockPos) && !IsBlock(nextBlockPos)) {
                var block = GetGameObjectAtPosition(nextPlayerPos);
                UpdateGameObjectPosition(nextPlayerPos);
                block.transform.position = GetDisplayPosition(nextBlockPos.x, nextBlockPos.y);
                gameObjectPositionTable[block] = nextBlockPos;
                if (tileList[nextBlockPos.x, nextBlockPos.y] == TileType.GROUND) {
                    tileList[nextBlockPos.x, nextBlockPos.y] = TileType.BLOCK;
                } else if (tileList[nextBlockPos.x, nextBlockPos.y] == TileType.TARGET) {
                    tileList[nextBlockPos.x, nextBlockPos.y] = TileType.BLOCK_ON_TARGET;
                }
                UpdateGameObjectPosition(currentPlayerPos);
                player.transform.position = GetDisplayPosition(nextPlayerPos.x, nextPlayerPos.y);
                gameObjectPositionTable[ player ] = nextPlayerPos;
                if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.GROUND) {
                    tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER;
                } else if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.TARGET) {
                    tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER_ON_TAREGT;
                }
            }
        } else {
            UpdateGameObjectPosition(currentPlayerPos);
            player.transform.position = GetDisplayPosition(nextPlayerPos.x, nextPlayerPos.y);
            gameObjectPositionTable[ player ] = nextPlayerPos;
            if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.GROUND) {
                tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER;
            } else if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.TARGET) {
                tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER_ON_TAREGT;
            }
        }
        CheckCompletion();
    }

    private Vector2Int GetNextPositionAlong( Vector2Int pos, DirectionType direction)
    {
        switch (direction) {
            case DirectionType.Up:
                pos.y -= 1;
                break;
            case DirectionType.Down:
                pos.y += 1;
                break;
            case DirectionType.Left:
                pos.x -= 1;
                break;
            case DirectionType.Right:
                pos.x += 1;
                break;
        }
        return pos;
    }

    private void UpdateGameObjectPosition( Vector2Int pos )
    {
        var cell = tileList[pos.x, pos.y];
        if (cell == TileType.PLAYER || cell == TileType.BLOCK) {
            tileList[pos.x, pos.y] = TileType.GROUND;
        } else if (cell == TileType.PLAYER_ON_TAREGT || cell == TileType.BLOCK_ON_TARGET) {
            tileList[pos.x, pos.y] = TileType.TARGET;
        }
    }

    private void CheckCompletion()
    {
        int blockOnTargetCount = 0;
        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < cols; x++) {
                if (tileList[x, y] == TileType.BLOCK_ON_TARGET) {
                    blockOnTargetCount++;
                }
            }
        }
        if (blockCount == blockOnTargetCount) {
            isClear = true;
        }
    }
}

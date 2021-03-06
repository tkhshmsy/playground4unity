# memo for studying Unity

## Unityでprojectを作ったら最初にやること
- Edit>Project>Settings>Editor
  - Visual Control
    - Visual Meta Files
  - Asset Serialization
    - Force Text
- .gitignore
```
/[Ll]ibrary/
/[Tt]emp/
/[Oo]bj/
/[Bb]uild/
/[Bb]uilds/
/Assets/AssetStoreTools*

# Autogenerated VS/MD solution and project files
ExportedObj/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd

# Unity3D generated meta files
*.pidb.meta

# Unity3D Generated File On Crash Reports
sysinfo.txt

# Builds
*.apk
*.unitypackage
```

## about Soukoban

- Soukoban
  - https://baba-s.hatenablog.com/entry/2018/03/30/085000
  - いわゆる倉庫番サンプル
- MainCameraに所属するスクリプトに全部書くスタイル
  - ファイルの読み込み
    - publicなTextAsset型インスタンスで読める
  - 画面全体の描画戦略
    - フィールドの有効なコマすべてに対してGroundSpriteを生成表示しておく
    - ブロック、プレイヤ、ターゲットは重ねて表示
      - レンダラ(を持ってるGameObject)を移動させる
  - リソース固定のスプライト
    - Spriteインスタンスをpublicに
      - Assetから画像をInspectorへD&Dで適用
      - 画像サイズと指定サイズの関係に注意
    - 表示はGameObjectにSpriteRendererをAddComponentする
      - 表示位置はVector2型gameObject.transform.position
  - キー入力はInput.GetKeyDown(KeyCode.～)で取得
- 疑問点
  - ルーチンはSceneに書くべきでは？
  - アニメーションスプライトはどうする？
  - (アニメーション)スプライトの切り替えは？
  - シーン切り替えやったことない
    - 共有リソースの持ち込みは？
  - JSONが読みたい
    - ないしはもっと効率的にCSVが読めないものか

## about tutorial: Roll-a-Ball

- My First Unity 玉転がしゲーム
  - https://github.com/unity3d-jp/FirstTutorial/wiki
- Chapter 0
  - Unity起動
  - Project作成
  - save scene
- Chapter 1
  - Ground
    - make planes
    - make GroundMaterial
  - Wall
    - make cube
    - make WallMaterial
    - set position and scale
    - duplicate with rotate
  - refactoring
    - Asset>Material folder
    - Hierarchy>Stage object
    - make stageObject to static
- Chapter 2
  - make Player
  - play the game
  - add component Physics>Rigidbody
  - add component PlayerController.cs
  - edit PlayerController.cs
    - Input
    - add force to rigidbody
    - make parameter for Inspector
- update Unity 2019.4.2f1 -> 4.3f1
- Chapter 3
  - move camera with player
  - main camera follows player
    - set position
    - add offset
- Chapter 4
  - make Item
    - adjust scale
  - prefab
    - set
    - onTriggerEnter
      - with Tag
    - Player has Tag "Player"
    - destroy GameObject
    - apply all
- Chapter 5
  - make Canvas
    - scale with screen size
  - make ScoreLabel
  - make GameController
    - set tag "Item" to all Items
    - GameController script
      - counts of Items
- Chapter 6
  - make winnerLabel
  - set active winnerLabel after clearing the game
- Chapter 7
  - make DangerWall
    - into prefab
    - make DangerWallMaterial
      - apply for DangerWall and apply all
    - make DangerWalls from prefab
    - set Static
  - adjust environment light
  - make restart with collision

## make environment

- installed
  - Unity Hub
    - Unity 2019.4.2f1
    - Unity 2019.4.3f1
  - GitHub Desktop
  - Visual Studio Code
    - UnityでVisual Studio Codeを使用できるようにするまでの手順
      - https://qiita.com/riekure/items/c45868f37a187f8e1d69
    - (dot)gitignoreで.vscode/以下を取り込まないようにすること


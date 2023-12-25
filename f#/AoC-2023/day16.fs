module day16

open System

type Point =
    { X: int
      Y: int }

    static member (+)(left, right) =
        { X = left.X + right.X
          Y = left.Y + right.Y }


type MirrorAngle =
    | Forward = '/'
    | Back = '\\'

type SplitAxis =
    | Horizontal = '-'
    | Vertical = '|'

type SplitResult =
    | Continue = 0
    | Split = 1



type Direction =
    | Up
    | Down
    | Left
    | Right

    member this.Point =
        match this with
        | Up -> { X = 0; Y = 1 }
        | Down -> { X = 0; Y = -1 }
        | Left -> { X = -1; Y = 0 }
        | Right -> { X = 1; Y = 0 }

    member this.ReflectedPoint angle =
        let isForward = angle = MirrorAngle.Forward

        match this with
        | Up -> if isForward then Right else Left
        | Down -> if isForward then Left else Right
        | Left -> if isForward then Down else Up
        | Right -> if isForward then Up else Down

    member private this.SplitResult axis =
        match this with
        | Up
        | Down ->
            if axis = SplitAxis.Vertical then
                SplitResult.Continue
            else
                SplitResult.Split
        | Left
        | Right ->
            if axis = SplitAxis.Vertical then
                SplitResult.Split
            else
                SplitResult.Continue

    member this.SplitPoints axis =
        let splitResult = this.SplitResult axis

        match this with
        | Up
        | Down ->
            if splitResult = SplitResult.Continue then
                [| this |]
            else
                [| Left; Right |]
        | Left
        | Right ->
            if splitResult = SplitResult.Continue then
                [| this |]
            else
                [| Up; Down |]

type SingleBeamExit =
    { EntryDirection: Direction
      Target: Point }

type DoubleBeamExit =
    { First: SingleBeamExit
      Second: SingleBeamExit }

    member this.DoublePoints = this.First.Target, this.Second.Target

type BeamExit =
    | Ignore
    | Single of SingleBeamExit
    | Double of DoubleBeamExit

type Beams =
    { Beams: Option<BeamExit> array }

    member this.NoBeams =
        not (this.Beams |> Array.exists Option.isSome)
        
    member this.GetExistingBeams =
        this.Beams
        |> Array.map (fun option ->
            match option with
            | None -> Ignore
            | Some value -> value)
        |> Array.filter (fun beamExit ->
            match beamExit with
            | Ignore -> false
            | _ -> true)



type ITile =
    abstract member Next: Direction -> Option<BeamExit>
    abstract member GridPosition: Point
    abstract member Energized: bool

type EmptyTile =
    { Position: Point
      mutable IsEnergized: bool
      mutable BeamPassedHorizontal: bool
      mutable BeamPassedVertical: bool }

    interface ITile with
        member this.Next(direction: Direction) =
            this.IsEnergized <- true

            let horizontal =
                match direction with
                | Up
                | Down -> false
                | Left
                | Right -> true

            if horizontal && this.BeamPassedHorizontal then
                None
            else if not (horizontal) && this.BeamPassedVertical then
                None
            else
                if horizontal then
                    this.BeamPassedHorizontal <- true
                else
                    this.BeamPassedVertical <- true

                let beamExit =
                    { EntryDirection = direction
                      Target = this.Position + direction.Point }

                Some(Single(beamExit))

        member this.GridPosition = this.Position
        member this.Energized = this.IsEnergized




type Mirror =
    { Position: Point
      Angle: MirrorAngle
      mutable IsEnergized: bool
      mutable ExitedUp: bool
      mutable ExitedRight: bool
      mutable ExitedDown: bool
      mutable ExitedLeft: bool }

    member this.HasExited =
        function
        | Up -> this.ExitedUp
        | Down -> this.ExitedDown
        | Left -> this.ExitedLeft
        | Right -> this.ExitedRight

    member this.Exit =
        function
        | Up -> this.ExitedUp <- true
        | Down -> this.ExitedDown <- true
        | Left -> this.ExitedLeft <- true
        | Right -> this.ExitedRight <- true

    interface ITile with
        member this.Next(direction: Direction) =
            this.IsEnergized <- true
            let exitDirection = this.Angle |> direction.ReflectedPoint

            if this.HasExited exitDirection then
                None
            else
                this.Exit exitDirection

                let beamExit =
                    { EntryDirection = exitDirection
                      Target = this.Position + exitDirection.Point }

                Some(Single(beamExit))

        member this.GridPosition = this.Position
        member this.Energized = this.IsEnergized


type Splitter =
    { Position: Point
      Axis: SplitAxis
      mutable IsEnergized: bool
      mutable BeamContinued: bool
      mutable BeamSplit: bool }

    interface ITile with
        member this.Next(direction: Direction) =
            this.IsEnergized <- true
            let exitPoints = direction.SplitPoints this.Axis

            if exitPoints.Length = 2 && this.BeamSplit then
                None
            else if exitPoints.Length = 1 && this.BeamContinued then
                None
            else if exitPoints.Length = 2 then
                this.BeamSplit <- true

                Some(
                    Double(
                        { First =
                            { EntryDirection = exitPoints[0]
                              Target = this.Position + exitPoints[0].Point }
                          Second =
                            { EntryDirection = exitPoints[1]
                              Target = this.Position + exitPoints[1].Point } }
                    )
                )
            else
                this.BeamContinued <- true

                Some(
                    Single(
                        { EntryDirection = exitPoints[0]
                          Target = this.Position + exitPoints[0].Point }
                    )
                )

        member this.GridPosition = this.Position
        member this.Energized = this.IsEnergized

type Grid =
    { TileMap: Map<Point, ITile>
      Size: Point }

    member this.Update(tile: ITile) =
        { TileMap = this.TileMap |> Map.add tile.GridPosition tile
          Size = this.Size }
        
    member this.UpdateAll(tiles: ITile array) =
        let mutable updatedMap = this.TileMap
        tiles |> Array.iter (fun tile -> updatedMap <- updatedMap |> Map.add tile.GridPosition tile)
        { TileMap = updatedMap
          Size = this.Size }

    member this.PointOutOfBounds point = not (this.TileMap.ContainsKey point)

    member this.DoubleWithinBounds(double: Point * Point) =
        this.TileMap.ContainsKey(double |> fst)
        && this.TileMap.ContainsKey(double |> snd)

    member this.Item(key: Point) : ITile = this.TileMap[key]




let createTile y x chr : ITile =
    let point = { X = x; Y = y }

    match chr with
    | '.' ->
        { EmptyTile.Position = point
          IsEnergized = false
          BeamPassedHorizontal = false
          BeamPassedVertical = false }
    | '-' ->
        { Position = point
          Axis = SplitAxis.Horizontal
          IsEnergized = false
          BeamSplit = false
          BeamContinued = false }
    | '|' ->
        { Position = point
          Axis = SplitAxis.Vertical
          IsEnergized = false
          BeamSplit = false
          BeamContinued = false }
    | '/' ->
        { Position = point
          Angle = MirrorAngle.Forward
          IsEnergized = false
          ExitedDown = false
          ExitedLeft = false
          ExitedRight = false
          ExitedUp = false }
    | '\\' ->
        { Position = point
          Angle = MirrorAngle.Back
          IsEnergized = false
          ExitedDown = false
          ExitedLeft = false
          ExitedRight = false
          ExitedUp = false }
    | _ -> failwith "Invalid input character"


let rec passBeam (grid: Grid) (beams: Beams) : Grid =
    let nextTiles =
        beams.GetExistingBeams
        |> Array.map (fun beam ->
            match beam with
            | Single singleBeamExit ->
                if grid.PointOutOfBounds singleBeamExit.Target then
                    [| None; None |]
                else
                    [| Some(grid[singleBeamExit.Target], singleBeamExit.EntryDirection); None |]
            | Double doubleBeamExit ->
                if grid.DoubleWithinBounds doubleBeamExit.DoublePoints then
                    [| Some(grid[doubleBeamExit.First.Target], doubleBeamExit.First.EntryDirection); Some(grid[doubleBeamExit.Second.Target], doubleBeamExit.Second.EntryDirection)|]                       
                else
                    [| (if grid.PointOutOfBounds doubleBeamExit.First.Target then
                            None
                        else
                            Some(grid[doubleBeamExit.First.Target], doubleBeamExit.First.EntryDirection));
                    (if grid.PointOutOfBounds doubleBeamExit.Second.Target then
                         None
                     else
                         Some(grid[doubleBeamExit.Second.Target], doubleBeamExit.Second.EntryDirection))|])
        |> Array.collect id
        |> Array.filter Option.isSome
        |> Array.map Option.get
        |> Array.map (fun (tile, direction) ->
            tile.Next direction, tile)
    let newGrid =
        nextTiles
        |> Array.map snd
        |> grid.UpdateAll
    let nextBeams =
        { Beams = nextTiles |> Array.map fst }
    if nextBeams.NoBeams then
        newGrid
    else
        nextBeams |> passBeam newGrid
        

let part1 (lines: string array) : string =
    let size =
        { X = lines[0].Length - 1
          Y = lines.Length - 1 }

    let startBeams =
        let beam = { EntryDirection = Direction.Right; Target = { X = 0; Y = size.Y } }
        let single = Single(beam)
        { Beams = [| Some(single) |] }
        

    let tileMap =
        lines
        |> Array.rev
        |> Array.mapi (fun y line -> line.ToCharArray() |> Array.mapi (createTile y))
        |> Array.collect id
        |> Array.map (fun tile -> tile.GridPosition, tile)
        |> Map.ofArray

    let engergizedGrid = startBeams |> passBeam ({ TileMap = tileMap; Size = size })
    engergizedGrid.TileMap.Values |> Seq.sumBy (fun tile -> if tile.Energized then 1 else 0) |> string
        
       

let part2 (lines: string array) : string =
    let size =
        { X = lines[0].Length - 1
          Y = lines.Length - 1 }

    let startBeams =
        let topEdge =
            [|0..size.X|]
            |> Array.map (fun x ->
                Single({Target = { X = x; Y = size.Y }; EntryDirection = Direction.Down }))
        let bottomEdge =
            [|0..size.X|]
            |> Array.map (fun x ->
                Single({Target = { X = x; Y = 0 }; EntryDirection = Direction.Up }))
        let leftEdge =
            [|0..size.Y|]
            |> Array.map (fun y ->
                Single({Target = { X = 0; Y = y }; EntryDirection = Direction.Right }))
        let rightEdge =
            [|0..size.Y|]
            |> Array.map (fun y ->
                Single({Target = { X = size.X; Y = y }; EntryDirection = Direction.Left }))
        topEdge |> Array.append bottomEdge |> Array.append leftEdge |> Array.append rightEdge |> Array.map (fun beam -> { Beams = [| Some(beam) |] })
       
            
        

    let tileMap =
        lines
        |> Array.rev
        |> Array.mapi (fun y line -> line.ToCharArray() |> Array.mapi (createTile y))
        |> Array.collect id
        |> Array.map (fun tile -> tile.GridPosition, tile)
        |> Map.ofArray

    let engergizedGrids = startBeams |> Array.map (passBeam ({ TileMap = tileMap; Size = size }))
    engergizedGrids |> Array.map (fun energizedGrid -> energizedGrid.TileMap.Values |> Seq.sumBy (fun tile -> if tile.Energized then 1 else 0)) |> Array.max |> string 

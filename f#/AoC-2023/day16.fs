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

type SingleBeamExit = { ExitDirection: Direction; Target: Point }
type DoubleBeamExit = { First: SingleBeamExit; Second: SingleBeamExit }

type BeamExit =
    | Single of SingleBeamExit
    | Double of DoubleBeamExit
    
type Beams = BeamExit array

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
                | Up | Down -> false
                | Left | Right -> true
            if horizontal && this.BeamPassedHorizontal then
                None
            else if not (horizontal) && this.BeamPassedVertical then
                None
            else
                if horizontal then
                    this.BeamPassedHorizontal <- true
                else this.BeamPassedVertical <- true
                let beamExit = { ExitDirection = direction; Target = this.Position + direction.Point }
                Some (Single(beamExit))
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
                let beamExit = { ExitDirection = exitDirection; Target = this.Position + exitDirection.Point }
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
            else
                if exitPoints.Length = 2 then
                    this.BeamSplit <- true
                    Some(
                        Double({
                            First = { ExitDirection = exitPoints[0]; Target = this.Position + exitPoints[0].Point }
                            Second = { ExitDirection = exitPoints[1]; Target = this.Position + exitPoints[1].Point } 
                            }))
                else
                    this.BeamContinued <- true
                    Some(Single({ ExitDirection = exitPoints[0]; Target = this.Position + exitPoints[0].Point }))
        member this.GridPosition = this.Position
        member this.Energized = this.IsEnergized

type Grid = {
    Map: Map<Point, ITile>; Size: Point
} with
    member this.Update (tile: ITile) =
        { Map = this.Map |> Map.add tile.GridPosition tile; Size = this.Size }
        
            
let createTile y x chr : ITile =
    let point = { X = x; Y = y }

    match chr with
    | '.' -> { EmptyTile.Position = point
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
    
    
let rec passBeam (beams: Beams) ()

let part1 (lines: string array) : string =
    let size = { X = lines[0].Length - 1; Y = lines.Length - 1}
    let startBeam = { ExitDirection = Direction.Right; Target = { X = 0; Y = size.Y } }
    let grid =
        lines
        |> Array.rev
        |> Array.mapi (fun y line ->
            line.ToCharArray()
            |> Array.mapi (createTile y))
        |> Array.collect id
        |> Array.map (fun tile -> tile.GridPosition, tile)
        |> Map.ofArray
    passBeam startBeam size grid 0 |> string

let part2 (lines: string array) : string = failwithf "Not implemented yet"

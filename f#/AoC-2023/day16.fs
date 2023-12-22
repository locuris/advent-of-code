module day16

open System
open day10

type Point = { x: int; y: int }

type Points =
    | Single of Point
    | Double of Point * Point
    
let getPointFromSingle = function
    | Single point -> point
    | _ -> ArgumentException() |> raise

type Direction =
    | Up = 0
    | Down = 1
    | Left = 2
    | Right = 3
    | SplitHorizontal = 4
    | SplitVertical = 5
    
    
let rec directionToPoints = function
    | Direction.Up -> Single {x=0;y = 1}
    | Direction.Down -> Single {x=0; y = -1}
    | Direction.Left -> Single {x= -1; y=0}
    | Direction.Right -> Single {x= 1; y=0}
    | Direction.SplitHorizontal -> Double (getPointFromSingle (directionToPoints Direction.Left), getPointFromSingle (directionToPoints Direction.Right))
    | Direction.SplitVertical -> Double (getPointFromSingle (directionToPoints Direction.Up), getPointFromSingle (directionToPoints Direction.Down))
    | _ -> ArgumentOutOfRangeException() |> raise


type TileType =
    | Empty = '.'
    | RightMirror = '/'
    | LeftMirror = '\\'
    | VerticalSplitter = '|'
    | HorizontalSplitter = '-'
    
let rec tileTypeToPoints = function
    | TileType.Empty -> Single {x=0;y = 1}
    | TileType. -> Single {x=0; y = -1}
    | TileType. -> Single {x= -1; y=0}
    | TileType. -> Single {x= 1; y=0}
    | TileType. -> Double (getPointFromSingle (directionToPoints Direction.Left), getPointFromSingle (directionToPoints Direction.Right))
    | TileType. -> Double (getPointFromSingle (directionToPoints Direction.Up), getPointFromSingle (directionToPoints Direction.Down))
    | _ -> ArgumentOutOfRangeException() |> raise

type Beam =
    | Reflect of Point
    | Continue of Point
    | Split of Point * Point    

let move direction targetTile =
    match direction with

type MirrorType =
    | Right = '/'
    | Left = '\\'

type Mirror = {
    Type: MirrorType
} with
    member this.encounter(inDirection: Direction) : Direction =
        match inDirection with
        | Direction.Up -> if this.Type = MirrorType.Left then Direction.Left else Direction.Right
        | Direction.Down -> if this.Type = MirrorType.Left then Direction.Right else Direction.Left
        | Direction.Right -> if this.Type = MirrorType.Left then Direction.Down else Direction.Up
        | Direction.Left -> if this.Type = MirrorType.Left then Direction.Up else Direction.Down
        | _ -> failwithf $"Invalid direction argument! {inDirection}"
        
        
type SplitterType =
    | Horizontal = '-'
    | Vertical = '|'
    

type Splitter = {
    Type: SplitterType
} with
    member this.encounter(inDirection): Direction =
        match inDirection with
        | Direction.Up -> if this.Type = SplitterType.Horizontal then Direction.Split else inDirection
        | 
    
    




let part1 (lines: string array) : string = failwithf "Not implemented yet"

let part2 (lines: string array) : string = failwithf "Not implemented yet"

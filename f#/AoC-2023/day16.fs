module day16

open System
open day10

type Point = { x: int; y: int }
    with static member (+) (left, right) = { x = left.x + right.x; y = left.y + right.y }

type Direction =
    | Up of Point
    | Down of Point
    | Left of Point
    | Right of Point
    
    member this.Point =
        match this with
        | Up point -> { x = 0; y = 1 }
        | Down point -> { x = 0; y = -1}
        | Left point -> { x = -1; y = 0}
        | Right point -> { x = 1; y = 0}
        
        
type Tile = 
    
type TileEffect =
    | None
    | Reflect of Direction
    | Split of Direction * Direction
    


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

module day16

open System


type Point = { x: int; y: int }
    with static member (+) (left, right) = { x = left.x + right.x; y = left.y + right.y }
    
    
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
        | Up -> { x = 0; y = 1 }
        | Down -> { x = 0; y = -1}
        | Left -> { x = -1; y = 0}
        | Right -> { x = 1; y = 0}
        
    member this.ReflectedPoint angle =
        let isForward = angle = MirrorAngle.Forward
        match this with
        | Up -> if isForward then Right else Left
        | Down -> if isForward then Left else Right
        | Left -> if isForward then Down else Up
        | Right -> if isForward then Up else Down
        
    member this.SplitResult axis =
        match this with
        | Up | Down -> if axis = SplitAxis.Vertical then SplitResult.Continue else SplitResult.Split
        | Left | Right -> if axis = SplitAxis.Vertical then SplitResult.Split else SplitResult.Continue
        
        
        
    member this.SplitPoint axis =
        let isHorizontal = axis = SplitAxis.Horizontal
        let splitResult = 
        match this with
        | 
        

type ITile =
    abstract member Next : Direction -> Point

type EmptyTile = {
    Position: Point
} with
    interface ITile with
        member this.Next (direction: Direction) =
            this.Position + direction.Point
        

        
type Mirror = {
    Position: Point
    Angle: MirrorAngle
} with
    interface ITile with
        member this.Next (direction: Direction) =
            this.Position + (this.Angle |> direction.ReflectedPoint).Point
        
        
        
        
type Tile =
    | Empty
    | RightMirror of 
    

    


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

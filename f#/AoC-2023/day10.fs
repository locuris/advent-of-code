module day10

open System
open System.Net
open Common.Data
open Common

type Point = int * int

let PointAdd ((xA, yA): Point) ((xB, yB): Point) : Point =
    xA + xB, yA + yB

type Direction =
    | Up = 0
    | Down = 1 
    | Left = 2
    | Right = 3
    
let OppositeDirection (direction: Direction) =
    match direction with
    | Direction.Up -> Direction.Down
    | Direction.Down -> Direction.Up
    | Direction.Left -> Direction.Right
    | Direction.Right -> Direction.Left
    
    
let DirectionPoint (direction: Direction) =
    match direction with
    | Direction.Up -> Point(0, -1)
    | Direction.Down -> Point(0, 1)
    | Direction.Left -> Point(-1, 0)
    | Direction.Right -> Point(1, 0)
    | _ -> failwith "Invalid direction"
    
type PipeCharacter =
    | Pipe = '|'
    | Dash = '-'
    | L = 'L'
    | J = 'J'
    | F = 'F'
    | Seven = '7'
    | S = 'S'
    | Dot = '.'
    

type PipeType =
    | RegularPipe of Direction * Direction
    | StartPipe of Direction
    | EmptyPipe
    
let CharToPipe chr =
    match Core.LanguagePrimitives.EnumOfValue<char, PipeCharacter>(chr) with
    | PipeCharacter.Pipe -> RegularPipe(Direction.Up, Direction.Down)
    | PipeCharacter.Dash -> RegularPipe(Direction.Left, Direction.Right)
    | PipeCharacter.L -> RegularPipe(Direction.Up, Direction.Right)
    | PipeCharacter.J -> RegularPipe(Direction.Up, Direction.Left)
    | PipeCharacter.F -> RegularPipe(Direction.Down, Direction.Right)
    | PipeCharacter.Seven -> RegularPipe(Direction.Down, Direction.Left)
    | PipeCharacter.Dot -> EmptyPipe
    | PipeCharacter.S -> StartPipe(Direction.Up)
    | e -> failwith $"Invalid pipe character {e}"
    
    
type PipeBase =
    | None of Point
    | Start of Point * Point
    | Pipe of Point * PipeType * Point * Point

let ToPipeBase (point: Point) (pipe: PipeType) : PipeBase =
    match pipe with
    | RegularPipe (d1, d2) -> Pipe(point, pipe, PointAdd point  (DirectionPoint d1), PointAdd point  (DirectionPoint d2))
    | EmptyPipe -> None(point)
    | StartPipe dir -> Start(point, PointAdd point (DirectionPoint dir))
    
let CanJoin (pipes: PipeBase * PipeBase) : bool =
    match pipes with
    | Pipe (point1, _, point1A, point1B), Pipe (point2, _, point2A, point2B) -> (point1 = point2A || point1 = point2B) && (point2 = point1A || point2 = point1B)
    | Pipe _, Start _ -> true
    | Start (point1, out), Pipe (point, _, inA, inB) -> out = point && (inA = point1 || inB = point1)
    | _ -> false
    
    
type PipePath =
    | Start of PipeBase * PipePath
    | None
    | Pipe of PipeBase * PipePath
    
type PipeTree =
    | End
    | PipeNode of PipeBase * PipeTree
    
    

let rec buildPipePath (outDirection: Direction) (point: Point) (pipes: Map<(int * int), PipeBase>) =
    let nextPoint = PointAdd point (DirectionPoint outDirection)
    if pipes.ContainsKey(nextPoint) then
        let nextPipe = pipes[nextPoint]
        match nextPipe with
        | PipeBase.None _ | PipeBase.Start _ -> End
        | PipeBase.Pipe (_, pipe, _, _) ->
            match pipe with
            | EmptyPipe | StartPipe _ -> End
            | RegularPipe (d1, d2) ->
                let inDirection = OppositeDirection outDirection
                if d1 = inDirection then
                    PipeNode(nextPipe, buildPipePath d2 nextPoint pipes)
                elif d2 = inDirection then
                    PipeNode(nextPipe, buildPipePath d1 nextPoint pipes)
                else End
    else
        PipeTree.End
    
let rec TraversePath (path: PipeTree) (length: int) =
    match path with
    | PipeNode (_, next) -> TraversePath next (length + 1)
    | _ -> length
    
let BuildPipes (lines: string array) : Map<Point, PipeBase> =
    lines |> Input.InputAsCharArray2D |> fst |> Array2D.map CharToPipe |> MapOfArray2D |> Map.map ToPipeBase
    
let GetStart (pipes: Map<Point, PipeBase>) =
    pipes |> Map.filter (fun _ pipe ->
                         match pipe with
                         | PipeBase.Start _ -> true
                         | _ -> false) |> Map.keys |> Seq.head

let part1(lines: string array) : string =
    let pipes = BuildPipes lines
    let start = GetStart pipes
    
    let pipeSystem = buildPipePath Direction.Down start pipes
    
    let path = TraversePath pipeSystem 0
    let half = (path / 2) + (path % 2)
    half |> string
    
let part2(lines: string array) : string =
    let pipes = BuildPipes lines
    let start = GetStart pipes
    let pipeSystem = buildPipePath Direction.Down start pipes
    let pipesByRow = pipes |> Map.toArray |> Array.groupBy (fun ((_, y), _) -> y)
    let pipesByColumn = pipes |> Map.toArray |> Array.groupBy (fun ((x, _), _) -> x)
    
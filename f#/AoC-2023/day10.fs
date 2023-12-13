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

let ToPipeBase (point: Point) (pipe: PipeType) =
    match pipe with
    | RegularPipe (d1, d2) -> Pipe(point, pipe, PointAdd point  (DirectionPoint d1), PointAdd point  (DirectionPoint d2))
    | EmptyPipe -> None(point)
    | StartPipe dir -> Start(point, PointAdd point (DirectionPoint dir))
    | a -> failwith $"Invalid argument {a}"
    
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
        
    
let rec ToPipePath (pipe: PipeBase) (pipeBases: Map<(Point), PipeBase>) (previousPoint: Point) (hasStart: bool) (step: int) =
    printfn $"Step {step}"
    match pipe with
    | PipeBase.None _ -> None
    | PipeBase.Start (point, next) ->
        if not (pipeBases.ContainsKey next) then None else
        let nextPipeBase = pipeBases[next]
        if hasStart then
            None
            else Start(pipe, ToPipePath nextPipeBase pipeBases point true (step + 1))
    | PipeBase.Pipe (point, _, point1, point2) ->
        if previousPoint <> point1 && previousPoint <> point2 then None else
        let nextPoint = (if point1 = previousPoint then point2 else point1)
        match (pipeBases |> Map.tryFind nextPoint) with
        | Some nextPipe ->
            if (CanJoin (pipe, nextPipe)) then
                Pipe(pipe, ToPipePath nextPipe pipeBases point hasStart (step + 1))
            else None
        | _ -> None                

(*
type Pipes =
    struct
        val private _pipes: Map<Point, Pipe>
        val private _size: Size
        new (lines: char[,], size: Size) = {
            _pipes = lines |> Array2D.map Core.LanguagePrimitives.EnumOfValue<char, PipeType> |> mapOfArray2D |> Map.map (fun point pipe -> point, pipe)
            _size = size
        }
        
        member this.GetPipe (point: Point) : Pipe =
            this._pipes[point]
            
        member this.ContainsPoint (point: Point) =
            this._pipes.ContainsKey point            
            
    end
    *)
    
let rec TraversePath (path: PipeTree) (length: int) =
    match path with
    | PipeNode (_, next) -> TraversePath next (length + 1)
    | _ -> length
            

let part1(lines: string array) : string =
    let pipes = lines |> Input.InputAsCharArray2D |> fst |> Array2D.map CharToPipe |> MapOfArray2D |> Map.map ToPipeBase
    let start = pipes |> Map.filter (fun _ pipe ->
                                                match pipe with
                                                | PipeBase.Start _ -> true
                                                | _ -> false) |> Map.keys |> Seq.head
    
    let pipeSystem = buildPipePath Direction.Down start pipes
    //6896
    let path = TraversePath pipeSystem 0
    let half = (path / 2) + (path % 2)
    half |> string
    
let part2(lines: string array) : string =
    ""
﻿module day10

open System
open Common.Data
open Common
    

type Direction =
    | Up = 0
    | Down = 1 
    | Left = 2
    | Right = 3
    
let DirectionPoint (direction: Direction) =
    match direction with
    | Direction.Up -> Point(0, 1)
    | Direction.Down -> Point(0, -1)
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
    | Start
    | Empty    
    
let CharToPipe chr =
    match Core.LanguagePrimitives.EnumOfValue<char, PipeCharacter>(chr) with
    | PipeCharacter.Pipe -> RegularPipe(Direction.Up, Direction.Down)
    | PipeCharacter.Dash -> RegularPipe(Direction.Left, Direction.Right)
    | PipeCharacter.L -> RegularPipe(Direction.Up, Direction.Right)
    | PipeCharacter.J -> RegularPipe(Direction.Up, Direction.Left)
    | PipeCharacter.F -> RegularPipe(Direction.Down, Direction.Right)
    | PipeCharacter.Seven -> RegularPipe(Direction.Down, Direction.Left)
    | PipeCharacter.Dot -> Empty
    | PipeCharacter.S -> Start
    | e -> failwith $"Invalid pipe character {e}"
    
    
type PipeBase =
    | NoneP of Point
    | StartPipe of Point
    | RealPipe of Point * PipeType * Point * Point

let ToPipeBase (point: Point) (pipe: PipeType) =
    match pipe with
    | RegularPipe (d1, d2) -> RealPipe(point, pipe, point + (DirectionPoint d1), point + (DirectionPoint d2))
    | Empty -> NoneP(point)
    | Start -> StartPipe(point)
    | a -> failwith $"Invalid argument {a}"
    
let CanJoin (pipes: PipeBase * PipeBase) : bool =
    match pipes with
    | RealPipe (point1, _, point1A, point1B), RealPipe (point2, _, point2A, point2B) -> (point1 = point2A || point1 = point2B) && (point2 = point1A || point2 = point1B)
    | _ -> false
    
    
type Pipes =
    | Start of PipeBase * Pipes
    | None
    | Pipe of PipeBase * Pipes * Pipes
    
type SimplePipe = PipeBase * PipeBase * PipeBase

let toPipes (pipeBase: PipeBase) (pipes: Map<Point, PipeBase>) =
    match pipeBase with
    | RealPipe point, pipe, pointA, pointB ->
        let pipe = RealPipe(point, pipe, pointA, pointB)
        let pipeA = if pipes.ContainsKey pointA then pipes[pointA] else NoneP(pointA)
        let pipeB = if pipes.ContainsKey pointB then pipes[pointB] else NoneP(pointB)
        SimplePipe(pipeA, pipe, pibeB)
    | _ -> NoneSP

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
            

let part1(lines: string array) : string =
    lines |> Input.InputAsCharArray2D |> fst |> Array2D.map CharToPipe |> MapOfArray2D |> Map.map ToPipeBase
    |> ignore
    ""
    
let part2(lines: string array) : string =
    ""
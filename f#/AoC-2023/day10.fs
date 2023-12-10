module day10

open System
open Common.Data
open Common
    

type Direction =
    | Up = 0
    | Down = 1 
    | Left = 2
    | Right = 3
    
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
    | None    
    
let CharToPipe chr =
    match Core.LanguagePrimitives.EnumOfValue<char, PipeCharacter>(chr) with
    | PipeCharacter.Pipe -> RegularPipe(Direction.Up, Direction.Down)
    | PipeCharacter.Dash -> RegularPipe(Direction.Left, Direction.Right)
    | PipeCharacter.L -> RegularPipe(Direction.Up, Direction.Right)
    | PipeCharacter.J -> RegularPipe(Direction.Up, Direction.Left)
    | PipeCharacter.F -> RegularPipe(Direction.Down, Direction.Right)
    | PipeCharacter.Seven -> RegularPipe(Direction.Down, Direction.Left)
    | PipeCharacter.Dot -> None
    | PipeCharacter.S -> Start
    | e -> failwith $"Invalid pipe character {e}"
    
    
type PipeBase = PipeType * Point

    
type Pipes =
    | Start of PipeBase * Pipes
    | None
    | Pipe of PipeBase * Pipes * Pipes



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
    lines |> Input.InputAsCharArray |> 
    |> ignore*)
    ""
    
let part2(lines: string array) : string =
    ""
module day8

type Operation =
    | Nop
    | Acc
    | Jmp

type Instruction = { op: Operation; arg: int }

let parseInstruction (line: string) : Instruction =
    let parts = line.Split(' ')

    let op =
        match parts.[0] with
        | "nop" -> Nop
        | "acc" -> Acc
        | "jmp" -> Jmp
        | _ -> failwith "Unknown operation"

    let arg = int parts.[1]
    { op = op; arg = arg }


let rec run (instructions: Instruction array) (instructionState: bool array) (acc: int) (pc: int) : int =
    let instruction = instructions.[pc]
    if instructionState.[pc] then
        acc
    else
        instructionState.[pc] <- true
        match instruction.op with
        | Nop -> run instructions instructionState acc (pc + 1)
        | Acc -> run instructions instructionState (acc + instruction.arg) (pc + 1)
        | Jmp -> run instructions instructionState acc (pc + instruction.arg)
        
let day8part1 (lines: string array) : string =
    let instructions = lines
                       |> Array.map parseInstruction
               
    let instructionState = instructions |> Array.map (fun _ -> false)
                       
    (run instructions instructionState 0 0) |> string 
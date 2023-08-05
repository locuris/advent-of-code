module day8

open System.Collections.Generic
open Microsoft.Win32

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
    if pc >= instructions.Length then
        printfn $"Accumulator at end is {acc}"
        -1
    else
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
    
let day8part2 (lines: string array) : string =
    let instructions = lines
                       |> Array.map parseInstruction
           
    let alteredInstructions = Dictionary<Instruction array, int>()
    
    for i in 0 .. instructions.Length - 1 do
        let instruction = instructions.[i]
        match instruction.op with
        | Nop -> 
            alteredInstructions.Add(instructions |> Array.map (fun inst -> 
                if inst = instruction then
                    { op = Jmp; arg = instruction.arg }
                else
                    inst), i
            )       
        | Jmp -> 
            alteredInstructions.Add(instructions |> Array.map (fun inst -> 
                if inst = instruction then
                    { op = Nop; arg = instruction.arg }
                else
                    inst), i
            )
        | _ -> ()
        
    for KeyValue(alteredInstruction, changedOp) in alteredInstructions do
        if run alteredInstruction (alteredInstruction |> Array.map (fun _ -> false)) 0 0 = -1 then
            printfn $"Changed instruction {changedOp} from {instructions.[changedOp]} to {alteredInstruction.[changedOp]}"
        else ()   
    
    "Done!"
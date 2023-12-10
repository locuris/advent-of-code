module day8

open System
open Common.Input


type InstructionSet =
    struct
        val instructions: int array
        new (instructionSet: int array) = {
            instructions = instructionSet
        }
        new (instructionSet: string) = {
            instructions = instructionSet.ToCharArray() |> Array.map (fun c -> if c = 'R' then 1 else 0)
        }
        
        member this.nextInstruction(stepCount: int64) : int =
            let index = stepCount % int64(this.instructions.Length) |> int            
            this.instructions[index]
    end


let rec traverseTree (tree: Map<string, string list>) (steps: int64) (instructionSet: InstructionSet) (endCondition: string -> bool) (node: string) : int64 =
    if endCondition(node)
    then steps
    else traverseTree tree (steps + 1L) instructionSet endCondition (tree[node][instructionSet.nextInstruction(steps)])                                                       
    
let rec traverseTrees (tree: Map<string, string list>) (nodes: string array) (step: int64) (instructionSet: InstructionSet) : int64 =    
    if nodes |> Array.forall (_.EndsWith('Z'))
    then step
    else
        traverseTrees tree (nodes |> Array.map (fun node -> tree[node][instructionSet.nextInstruction(step)])) (step + 1L) instructionSet
    

let getInstructionsAndNodes (lines: string array) : InstructionSet * Map<string, string list> =
    let instructionInput, nodesInput = lines |> GetLinesGroupedByNewLine |> fun groups -> groups[0][0], groups[1]
    let instructions = instructionInput |> InstructionSet
    let nodes = nodesInput |> Array.map (GetMatchesAsStringArray @"\w\w\w") |> Array.map (fun node -> (node[0], [node[1];node[2]])) |> Map.ofArray
    instructions, nodes
    
let part1EndCondition (node: string) : bool =
    node = "AAA"

let part1 (lines: string array) : string =
    let instructions, nodes = lines |> getInstructionsAndNodes
    traverseTree nodes 0 instructions part1EndCondition "AAA" |> string
    
let rec gcd a b =
    if b = 0L then a else gcd b (a % b)

let lcm a b = (a * b) / (gcd a b)

let findLCM (list: int64 list) =
    match list with
    | [] -> 0L
    | h::t -> List.fold lcm h t


let part2EndCondition (node: string) : bool =
    node.EndsWith('Z')    
    
let part2 (lines: string array) : string =
    let instructions, nodes = lines |> getInstructionsAndNodes
    let pathLengths = nodes.Keys |> Seq.filter (_.EndsWith('A')) |> List.ofSeq |> List.map (traverseTree nodes 0L instructions part2EndCondition) |> List.sort
    pathLengths |> findLCM |> string
    
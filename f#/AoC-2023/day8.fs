module day8

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
        
        member this.nextInstruction(stepCount: int) : int =
            this.instructions[stepCount % this.instructions.Length]
    end


let rec traverseTree (tree: Map<string, string list>) (node: string) (steps: int) (instructionSet: InstructionSet) : int =
    if node = "ZZZ"
    then steps
    else traverseTree tree (tree[node][instructionSet.nextInstruction(steps)]) (steps + 1) instructionSet
    

    
let rec traverseTrees (tree: Map<string, string list>) (nodes: string array) (step: int) (instructionSet: InstructionSet) (debug: Map<int, int>) : int =
    let newDebug =
        if nodes |> Array.exists (_.EndsWith('Z')) then
            let endPaths = nodes |> Array.sumBy (fun node -> if node.EndsWith('Z') then 1 else 0)                                              
            let n = debug |> Map.map (fun path count -> if path = endPaths then count + 1 else count)
            let concat = n.Values |> Seq.map (_.ToString()) |> String.concat "."
            printfn $"{concat}"
            n
            else debug
    if nodes |> Array.forall (_.EndsWith('Z'))
    then step
    else
        traverseTrees tree (nodes |> Array.map (fun node -> tree[node][instructionSet.nextInstruction(step)])) (step + 1) instructionSet newDebug
    

let getInstructionsAndNodes (lines: string array) : InstructionSet * Map<string, string list> =
    let instructionInput, nodesInput = lines |> getLinesGroupedByNewLine |> fun groups -> groups[0][0], groups[1]
    let instructions = instructionInput |> InstructionSet
    let nodes = nodesInput |> Array.map (getMatchesAsStringArray @"\w\w\w") |> Array.map (fun node -> (node[0], [node[1];node[2]])) |> Map.ofArray
    instructions, nodes

let part1 (lines: string array) : string =
    let instructions, nodes = lines |> getInstructionsAndNodes
    traverseTree nodes "AAA" 0 instructions |> string
    
let part2 (lines: string array) : string =
    let instructions, nodes = lines |> getInstructionsAndNodes
    let startNodes = nodes.Keys |> Seq.filter (_.EndsWith('A')) |> Array.ofSeq
    let debug = startNodes |> Array.mapi (fun i _ -> (i, 0)) |> Map.ofArray
    traverseTrees nodes startNodes 0 instructions debug |> string
    
module day15

open System
open System.Text
open Microsoft.FSharp.Collections

type Operator =
    | Add = '='
    | Remove = '-'

type LabelString = string
type BoxId = int
type FocalLength = int

// Text before operator * operator * hash value
type Label = LabelString * Operator * BoxId

// Label * focal Length
type Lens = Label * FocalLength

type BoxedLens = int * Lens

// Box number * Map: KEY=LabelString VALUE= position * Lens
type Box = BoxId * Map<LabelString, BoxedLens>

let createBox id =
    Box(id, Map.empty)

let InitialBoxes : Box array =
    [|0..255|] |> Array.map createBox 

let hashAlgorithm (code: int) (chr: char) : int =
    (((chr |> int) + code) * 17) % 256

let hash (code: string) : BoxId =    
    let mutable hashCode = 0
    code.ToCharArray()
    |> Array.iter (fun c ->
        hashCode <- hashAlgorithm hashCode c)
    hashCode

let createLens (input: string) : Lens =
    let operation = if input.Contains('-') then Operator.Remove else Operator.Add
    let (label, focalLength) =
        input.Split(LanguagePrimitives.EnumToValue operation) |> (fun split -> (split[0], if split[1] = "" then 0 else int split[1]))
    Lens(Label(label, operation, hash label), focalLength)









let part1 (lines: string array) : string =
    let inputSet = lines[0].Split(',') |> Array.map (fun chr -> chr.ToCharArray())    
    let answer =
        inputSet
        |> Array.sumBy (fun input ->
            let mutable value = 0
            input
            |> Array.iter (fun chr ->
                let codeAsInt = chr |> int
                value <- value + codeAsInt
                value <- value * 17
                value <- value % 256)
            value)
    answer |> string


let addToBox (lens: Lens) (box: Box) : Box =
    let mutable (_, boxContents) = box
    let ((label, _, _), _) = lens
    if boxContents.ContainsKey(label) then
        let (pos, _) = boxContents[label]
        boxContents <- boxContents |> Map.add label (BoxedLens(pos, lens))
        else
            let pos = boxContents.Values.Count + 1
            boxContents <- boxContents |> Map.add label (BoxedLens(pos, lens))
            
    Box(box |> fst, boxContents)
    

let removeFromBox (lens: Lens) (box: Box) : Box =
    let mutable (_, boxContents) = box
    let ((label, _, _), _) = lens
    if boxContents.ContainsKey(label) then
        let (pos, _) = boxContents[label]
        boxContents <- boxContents |> Map.remove label
        let mutable x = -1
        boxContents.Values |> Seq.sortBy fst |> Seq.iter ( fun (old, lens) ->
            let label = lens |> fst |> (fun (label, _, _) -> label)
            if old > pos then
                x <- x + 1
                boxContents <- boxContents |> Map.change label (fun lens ->
                                                                match lens with
                                                                | Some (_, l) -> Some(BoxedLens(pos + x, l))))
        Box(box |> fst, boxContents)
    else box
    

let processLens (boxes: Box array) (lens: Lens)  : Box array =
    let (label, operator, boxId), focalLength = lens
    let box = boxes[boxId]
    let newBox =
        match operator with
        | Operator.Add -> addToBox lens box
        | Operator.Remove -> removeFromBox lens box
    boxes |> Array.map (fun (id, map) -> if id = boxId then newBox else Box(id, map))
    
    
    


let part2 (lines: string array) : string =
    let mutable boxes = InitialBoxes
    let lenses = lines[0].Split(',') |> Array.map createLens
    for lens in lenses do
        boxes <- processLens boxes lens
        
    boxes |> Array.filter (fun (_, content) -> not content.IsEmpty ) |> Array.sumBy (fun box ->
        let (boxId, contents) = box
        let sum = contents.Values |> Seq.sumBy (fun (pos, (_, f)) ->
            (1 + boxId) * pos * f)
        printfn $"{boxId} = {sum}"
        sum
        )
    |> string

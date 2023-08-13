module day14

open System
open System.Collections.Generic

let day14part1 (lines: string array) : string =
    let memory = Dictionary<int64, int64>()

    let mutable mask = Map.empty

    for line in lines do
        if line.Substring(0, 4) = "mask" then
            mask <-
                line.Substring(7)
                |> Seq.rev
                |> Seq.mapi (fun i v -> i, v)
                |> Seq.filter (fun (_, v) -> v <> 'X')
                |> Seq.map (fun (i, v) -> i, v = '1')
                |> Map.ofSeq
        else
            let memAddress = line.Substring(4).Split(']').[0] |> Int64.Parse
            let value = line.Split('=').[1].Trim() |> Int64.Parse
            let valueAsBinary = Convert.ToString(value, 2).PadLeft(36, '0')

            let newValue =
                Convert.ToInt64(
                    valueAsBinary
                    |> Seq.rev
                    |> Seq.mapi (fun i v -> i, v)
                    |> Seq.map (fun (i, v) ->
                        if mask.ContainsKey i then
                            if mask.[i] then '1' else '0'
                        else
                            v)
                    |> Seq.rev
                    |> Seq.toArray
                    |> String.Concat,
                    2
                )

            if memory.ContainsKey memAddress then
                memory.[memAddress] <- newValue
            else
                memory.Add(memAddress, newValue)

    memory.Values |> Seq.sum |> string

let day14part2 (lines: string array) : string = "NOT DONE"

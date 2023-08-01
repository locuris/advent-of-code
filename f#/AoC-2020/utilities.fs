module utilities

open System.IO

let readFile (fileName: string) = File.ReadLines(fileName) |> Array.ofSeq

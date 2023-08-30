module day5

open System
open System.Collections.Generic

let getSeatIds (lines: string array): Dictionary<int, int * int> =
    let seatIds: Dictionary<int, int * int> = Dictionary()
    
    for line in lines do
        let row = line.Substring(0, 7)
        let seat = line.Substring(7, 3)
        
        let rowBinary = row.Replace("F", "0").Replace("B", "1")
        let seatBinary = seat.Replace("L", "0").Replace("R", "1")
        
        let rowNumber = Convert.ToInt32(rowBinary, 2)
        let seatNumber = Convert.ToInt32(seatBinary, 2)
        
        seatIds.Add(rowNumber * 8 + seatNumber, (rowNumber, seatNumber))
        
    seatIds

let day5part1 (lines: string array) =
    getSeatIds(lines).Keys |> Seq.max |> string
    
let day5part2 (lines: string array) =
    let seatIdMap = getSeatIds lines
    let seatIds = seatIdMap.Keys |> Seq.toList |> Seq.sortDescending |> Seq.rev
    
    let mutable lastSeatId = 0
    let mutable mySeatId = 0
    let mutable found = false
    for seatId in seatIds do
        if lastSeatId = 0 then
            lastSeatId <- seatId
        elif seatId - lastSeatId > 1 && not found then
            mySeatId <- seatId - 1
            found <- true
        else
            lastSeatId <- seatId
            
    mySeatId |> string
    
module day5

open System

let seatIds (lines: string array): ResizeArray<int> =
    let seatIds = ResizeArray<int>()
    
    for line in lines do
        let row = line.Substring(0, 7)
        let seat = line.Substring(7, 3)
        
        let rowBinary = row.Replace("F", "0").Replace("B", "1")
        let seatBinary = seat.Replace("L", "0").Replace("R", "1")
        
        let rowNumber = Convert.ToInt32(rowBinary, 2)
        let seatNumber = Convert.ToInt32(seatBinary, 2)
        
        seatIds.Add(rowNumber * 8 + seatNumber)
        
    seatIds

let day5part1 (lines: string array) =
    seatIds lines |> Seq.max |> string